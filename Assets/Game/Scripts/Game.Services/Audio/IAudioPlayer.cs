using UnityEngine;

namespace Game.Services.Audio
{
    public interface IAudioPlayer
    {
        int PlayAudioClip2D(AudioClip clip, float volumeProportion = 1f, bool looped = false);
        int PlayAudioClip3D(AudioClip clip, Vector3 position, float maxSoundDistance, float volumeProportion = 1f, bool looped = false);
        void StopPlayingClip(int audioCode);
        bool IsAudioClipCodePlaying(int audioCode);
        void SetAudioListenerToPosition(Vector3 position);
        void SetSourcePositionTo(int audioCode, Vector3 destinationPos);
    }
}