using UnityEngine;

namespace Game.Services
{
    public interface IMusicPlayer
    {
        int PlayMusicClip(AudioClip clip, float volumeProportion = 1f);
        void StopPlayingMusicClip(int audioCode);
        void PausePlayingClip(int audioCode);
        void ResumeClipIfInPause(int audioCode);
        bool IsMusicClipCodePlaying(int audioCode);
    }
}