using Fusion;
using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerSpawner : SimulationBehaviour, ISpawned
    {
        [SerializeField]
        private NetworkPrefabRef networkPlayerPrefab = NetworkPrefabRef.Empty;

        private GameStateController gameStateController;

        public void Spawned()
        {
            if (FindObjectOfType<GameStateController>().GameIsRunning)
            {
                SpawnPlayer(Runner.LocalPlayer);
            }
        }

        public void StartPlayerSpawner(GameStateController gameStateController)
        {
            this.gameStateController = gameStateController;
        }

        public void SpawnPlayer(PlayerRef player)
        {
            var playerObject = Runner.Spawn(networkPlayerPrefab, Vector3.zero, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObject);

            if (!gameStateController)
            {
                gameStateController = FindObjectOfType<GameStateController>();
            }

            gameStateController.RPC_TrackNewPlayer(playerObject.GetComponent<NetworkPlayerData>().Id);
        }
    }
}