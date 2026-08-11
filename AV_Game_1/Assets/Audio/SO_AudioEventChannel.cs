using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "SO_AudioEventChannel",
    menuName = "Audio/Audio Event Channel"
)]
public class SO_AudioEventChannel : ScriptableObject
{
    public UnityEvent<string> audioEvent_SFX, audioEvent_MUSIC;

    private void OnEnable()
    {
        if(audioEvent_SFX == null)
        {
            audioEvent_SFX = new UnityEvent<string>();
        }

        if (audioEvent_MUSIC == null)
        {
            audioEvent_MUSIC = new UnityEvent<string>();
        }
    }

    public void TriggerSFX(string eventName)
    {
        audioEvent_SFX?.Invoke(eventName);
    }

    public void TriggerMUSIC(string eventName)
    {
        audioEvent_MUSIC?.Invoke(eventName);
    }
}