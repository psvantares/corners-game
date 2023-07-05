using System;

namespace Game.Services
{
    public interface IVibrationController : IDisposable
    {
        bool VibrationEnabled { get; set; }
    }
}