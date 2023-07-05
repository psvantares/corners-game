using System;

namespace Game.Services
{
    public interface IAudioController : IDisposable
    {
        bool SoundEnabled { get; set; }
        bool MusicEnabled { get; set; }
        float SoundVolume { get; set; }
        float MusicVolume { get; set; }
    }
}