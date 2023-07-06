﻿using Fusion;

namespace Game.Gameplay
{
    public class NetworkPlayerController : NetworkBehaviour
    {
        private NetworkGameController networkGameController;
        private GameplayEntry gameplayEntry;
        private GameplayView gameplayView;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            networkGameController = FindObjectOfType<NetworkGameController>();
            gameplayEntry = FindObjectOfType<GameplayEntry>();
            gameplayView = FindObjectOfType<GameplayView>();

            gameplayView.RunText(networkGameController.CurrentPlayerType.ToString());
            gameplayEntry.Initialize();
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (networkGameController.GameIsRunning)
            {
            }
        }
    }
}