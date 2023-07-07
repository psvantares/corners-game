using Fusion;

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

            gameplayView.PlayerText(networkGameController.CurrentPlayer.ToString());
            gameplayEntry.Initialize();
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }
        }
    }
}