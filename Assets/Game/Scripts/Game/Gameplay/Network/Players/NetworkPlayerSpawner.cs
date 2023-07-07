using Fusion;
using UnityEngine;

namespace Game.Gameplay
{
    public class NetworkPlayerSpawner : SimulationBehaviour, ISpawned
    {
        [SerializeField]
        private NetworkPrefabRef networkPlayerPrefab;

        private NetworkGameController networkGameController;

        public void Spawned()
        {
            if (FindObjectOfType<NetworkGameController>().GameIsRunning)
            {
                SpawnPlayer(Runner.LocalPlayer);
            }
        }

        public void SpawnPlayer(PlayerRef player)
        {
            var playerObject = Runner.Spawn(networkPlayerPrefab, Vector3.zero, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObject);

            if (!networkGameController)
            {
                networkGameController = FindObjectOfType<NetworkGameController>();
            }

            networkGameController.RPC_TrackNewPlayer(playerObject.GetComponent<NetworkPlayerData>().Id);
        }
    }
}