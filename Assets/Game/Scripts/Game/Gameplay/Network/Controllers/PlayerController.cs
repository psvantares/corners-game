using Fusion;

namespace Game.Gameplay
{
    public class PlayerController : NetworkBehaviour
    {
        [Networked(OnChanged = nameof(OnAliveStateChanged))]
        private NetworkBool IsAlive { get; set; }

        [Networked]
        private TickTimer RespawnTimer { get; set; }

        private GameStateController gameStateController;

        public bool AcceptInput => IsAlive && Object.IsValid;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            IsAlive = true;
            gameStateController = FindObjectOfType<GameStateController>();
        }

        private static void OnAliveStateChanged(Changed<PlayerController> spaceshipController)
        {
            // ignored
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (RespawnTimer.Expired(Runner) && gameStateController.GameIsRunning)
            {
                IsAlive = true;
                RespawnTimer = default;
            }

            if (IsAlive)
            {
                //
            }
        }
    }
}