using System;
using Fusion;
using Game.Data;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class NetworkGameController : NetworkBehaviour
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
        public PlayerType CurrentPlayer { get; private set; }

        [Networked]
        private GameState CurrentGameState { get; set; }

        [Networked]
        private NetworkBehaviourId Winner { get; set; }

        [Networked, Capacity(2)]
        private NetworkLinkedList<NetworkBehaviourId> PlayerDataNetworkedIds => default;

        private readonly ISubject<PlayerType> switchPlayerEvent = new Subject<PlayerType>();
        private readonly ISubject<string> remainingEvent = new Subject<string>();
        private readonly ISubject<string> disconnectEvent = new Subject<string>();

        public bool GameIsRunning => CurrentGameState == GameState.Running;

        public IObservable<PlayerType> SwitchPlayerEvent => switchPlayerEvent;
        public IObservable<string> RemainingEvent => remainingEvent;
        public IObservable<string> DisconnectEvent => disconnectEvent;

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

            RPC_SwitchActivePlayer(PlayerType.White);

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

            RPC_SpawnReadyPlayers();

            CurrentGameState = GameState.Running;
            Timer = TickTimer.CreateFromSeconds(Runner, gameSessionLength);
        }

        private void UpdateRunningDisplay()
        {
            var source = Timer.RemainingTime(Runner);
            var tick = source ?? 0;
            var time = TimeSpan.FromSeconds(tick);
            var timeText = time.ToString(@"hh\:mm\:ss");

            remainingEvent.OnNext(timeText);
        }

        private void UpdateEndingDisplay()
        {
            if (Runner.TryFindBehaviour(Winner, out NetworkPlayerData playerData) == false)
            {
                return;
            }

            var source = Timer.RemainingTime(Runner);
            var tick = source ?? 0;
            var time = TimeSpan.FromSeconds(tick);
            var timeText = time.ToString(@"hh\:mm\:ss");
            var text = $"{playerData.NickName}: disconnecting in {timeText}";

            disconnectEvent.OnNext(text);

            if (Timer.ExpiredOrNotRunning(Runner) == false)
            {
                return;
            }

            Runner.Shutdown();
        }

        private void GameHasEnded()
        {
            Timer = TickTimer.CreateFromSeconds(Runner, endDelay);
            CurrentGameState = GameState.Ending;
        }

        // RPC

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
        {
            PlayerDataNetworkedIds.Add(playerDataNetworkedId);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SwitchActivePlayer(PlayerType playerType)
        {
            CurrentPlayer = playerType;
            switchPlayerEvent.OnNext(playerType);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SpawnReadyPlayers()
        {
            FindObjectOfType<NetworkPlayerSpawner>().SpawnPlayer(Runner.LocalPlayer);
        }
    }
}