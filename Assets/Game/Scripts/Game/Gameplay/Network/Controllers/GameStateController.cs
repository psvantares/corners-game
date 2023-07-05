using System;
using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameStateController : NetworkBehaviour
    {
        private enum GameState
        {
            Starting,
            Running,
            Ending
        }

        [SerializeField]
        private float startDelay = 1.0f;

        [SerializeField]
        private float endDelay = 1.0f;

        [SerializeField]
        private float gameSessionLength = 180.0f;

        [Networked]
        private TickTimer Timer { get; set; }

        [Networked]
        private GameState CurrentGameState { get; set; }

        [Networked]
        private NetworkBehaviourId Winner { get; set; }

        [Networked, Capacity(2)]
        private NetworkLinkedList<NetworkBehaviourId> PlayerDataNetworkedIds => default;

        public bool GameIsRunning => CurrentGameState == GameState.Running;
        public int PlayersCount => PlayerDataNetworkedIds.Count;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (CurrentGameState != GameState.Starting)
            {
                foreach (var player in Runner.ActivePlayers)
                {
                    if (Runner.TryGetPlayerObject(player, out var playerObject) == false)
                    {
                        continue;
                    }

                    RPC_TrackNewPlayer(playerObject.GetComponent<NetworkPlayerData>().Id);
                }
            }

            CurrentGameState = GameState.Starting;
            Timer = TickTimer.CreateFromSeconds(Runner, startDelay);
        }

        public override void FixedUpdateNetwork()
        {
            switch (CurrentGameState)
            {
                case GameState.Starting:
                    UpdateStartingDisplay();
                    break;
                case GameState.Running:
                    UpdateRunningDisplay();
                    if (Timer.ExpiredOrNotRunning(Runner) && Object.HasStateAuthority)
                    {
                        GameHasEnded();
                    }

                    break;
                case GameState.Ending:
                    UpdateEndingDisplay();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateStartingDisplay()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (Timer.ExpiredOrNotRunning(Runner) == false)
            {
                return;
            }

            FindObjectOfType<PlayerSpawner>().StartPlayerSpawner(this);

            RPC_SpawnReadyPlayers();

            CurrentGameState = GameState.Running;
            Timer = TickTimer.CreateFromSeconds(Runner, gameSessionLength);
        }

        private void UpdateRunningDisplay()
        {
            var source = Timer.RemainingTime(Runner);
            var tick = source ?? 0;
            EventBus.RemainingTime.OnNext(tick);
        }

        private void UpdateEndingDisplay()
        {
            if (Runner.TryFindBehaviour(Winner, out NetworkPlayerData playerData) == false)
            {
                return;
            }

            if (Timer.ExpiredOrNotRunning(Runner) == false)
            {
                return;
            }

            Runner.Shutdown();
        }

        // Called from the ShipController when it hits an asteroid
        public void CheckIfGameHasEnded()
        {
            // if (Object.HasStateAuthority == false) return;
            //
            // int playersAlive = 0;
            //
            // for (int i = 0; i < PlayerDataNetworkedIds.Count; i++)
            // {
            //     if (Runner.TryFindBehaviour(PlayerDataNetworkedIds[i], out NetworkPlayerData playerDataNetworkedComponent) == false)
            //     {
            //         PlayerDataNetworkedIds.Remove(PlayerDataNetworkedIds[i]);
            //         i--;
            //         continue;
            //     }
            //
            //     if (playerDataNetworkedComponent.Lives > 0) playersAlive++;
            // }
            //
            // // If more than 1 player is left alive, the game continues.
            // // If only 1 player is left, the game ends immediately.
            // if (playersAlive > 1) return;
            //
            // foreach (var playerDataNetworkedId in PlayerDataNetworkedIds)
            // {
            //     if (Runner.TryFindBehaviour(playerDataNetworkedId,
            //             out PlayerDataNetworked playerDataNetworkedComponent) ==
            //         false) continue;
            //
            //     if (playerDataNetworkedComponent.Lives > 0 == false) continue;
            //
            //     Winner = playerDataNetworkedId;
            // }
            //
            // GameHasEnded();
        }

        private void GameHasEnded()
        {
            Timer = TickTimer.CreateFromSeconds(Runner, endDelay);
            CurrentGameState = GameState.Ending;
        }

        // RPC

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_CheckIfGameHasEnded()
        {
            CheckIfGameHasEnded();
        }

        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        private void RPC_SpawnReadyPlayers()
        {
            FindObjectOfType<PlayerSpawner>().SpawnSpaceship(Runner.LocalPlayer);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
        {
            PlayerDataNetworkedIds.Add(playerDataNetworkedId);
        }
    }
}