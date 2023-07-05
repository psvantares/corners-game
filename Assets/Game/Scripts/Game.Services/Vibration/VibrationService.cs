using Game.Core;

namespace Game.Services
{
    public class VibrationService : IGameService
    {
        private readonly VibrationManager vibrationManager;

        public VibrationService(VibrationManager vibrationManager)
        {
            this.vibrationManager = vibrationManager;
        }
    }
}