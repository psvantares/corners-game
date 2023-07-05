using System;
using Game.Utilities;

namespace Game.Services
{
    public class VibrationController : IVibrationController, IVibration
    {
        private readonly SavableValue<bool> vibrationEnabled = new("VibrationController.vibrationEnabled", true);

        bool IVibrationController.VibrationEnabled
        {
            get => vibrationEnabled.Value;
            set
            {
                if (vibrationEnabled.Value == value)
                {
                    return;
                }

                vibrationEnabled.Value = value;
            }
        }

        void IDisposable.Dispose()
        {
        }

        void IVibration.PlayVibration()
        {
            if (!vibrationEnabled.Value)
            {
                return;
            }
        }
    }
}