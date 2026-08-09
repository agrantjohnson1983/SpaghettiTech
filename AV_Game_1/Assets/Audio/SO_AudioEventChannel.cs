using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "SO_AudioEventChannel",
    menuName = "Audio/Audio Event Channel"
)]
public class SO_AudioEventChannel : ScriptableObject
{
    public UnityEvent<string> OnAudioEventRaised;

    private void OnEnable()
    {
        if(OnAudioEventRaised== null)
        {
            OnAudioEventRaised = new UnityEvent<string>();
        }
    }

    public void Raise(string eventName)
    {
        OnAudioEventRaised?.Invoke(eventName);
    }
}