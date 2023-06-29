using System.Collections.Generic;
using UnityEngine;

namespace Game.Services.Audio
{
    public enum AudioClipName
    {
        Click
    }

    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        private class Sound
        {
            public AudioClipName AudioClipName;
            public AudioClip AudioClip;
        }

        [SerializeField]
        private List<Sound> sounds;

        private static AudioController audioController;
        private static IAudioController iac;
        private static IAudioPlayer iap;
        private static IMusicPlayer imp;

        private void Awake()
        {
            audioController = new AudioController(transform);
            iac = audioController;
            iap = audioController;
            imp = audioController;
        }

        private void OnDestroy()
        {
            iac?.Dispose();
        }

        public static void SoundEnabled(bool value)
        {
            iac.SoundEnabled = value;
        }

        public static bool IsSoundEnabled()
        {
            return iac == null || iac.SoundEnabled;
        }

        public static void PlayClip2D(AudioClip clip)
        {
            iap.PlayAudioClip2D(clip);
        }

        public void PlayClip2D(AudioClipName audioClipName)
        {
            var clip = GetAudioClip(audioClipName);

            if (clip == null)
            {
                return;
            }

            iap.PlayAudioClip2D(clip);
        }

        private AudioClip GetAudioClip(AudioClipName audioClipName)
        {
            var soundCount = sounds.Count;

            for (var i = 0; i < soundCount; i++)
            {
                if (sounds[i].AudioClipName == audioClipName)
                {
                    return sounds[i].AudioClip;
                }
            }

            return null;
        }
    }
}