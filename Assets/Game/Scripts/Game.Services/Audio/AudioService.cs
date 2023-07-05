using Game.Core;
using UnityEngine;

namespace Game.Services
{
    public class AudioService : IGameService
    {
        private readonly AudioManager audioManager;

        public AudioService(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        public void SoundEnabled(bool value)
        {
            audioManager.SoundEnabled(value);
        }

        public bool IsSoundEnabled()
        {
            return audioManager.IsSoundEnabled();
        }

        public void PlayClip2D(AudioClip clip)
        {
            audioManager.PlayClip2D(clip);
        }

        public void PlayClip2D(AudioClipName audioClipName)
        {
            audioManager.PlayClip2D(audioClipName);
        }
    }
}