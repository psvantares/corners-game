using System;

namespace Game.Services.Vibration
{
    public interface IVibrationController : IDisposable
    {
        bool VibrationEnabled { get; set; }
    }
}