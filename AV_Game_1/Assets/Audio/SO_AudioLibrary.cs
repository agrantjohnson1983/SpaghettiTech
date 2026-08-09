using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_AudioLibrary",
    menuName = "Audio/Audio Library"
)]
public class SO_AudioLibrary : ScriptableObject
{
    public List<AudioEventEntry> events = new List<AudioEventEntry>();

    public AudioEventEntry GetEvent(string eventName)
    {
        foreach (var audioEvent in events)
        {
            if (audioEvent.eventName == eventName)
                return audioEvent;
        }

        return null;
    }
}

[Serializable]
public class AudioEventEntry
{
    [Header("Event")]
    public string eventName;

    [Header("Audio")]
    public List<AudioClip> clips = new List<AudioClip>();

    [Header("Playback")]
    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float minPitch = 1f;

    [Range(0.1f, 3f)]
    public float maxPitch = 1f;

    [Header("Cooldown")]
    public float cooldown = 0f;

    [Header("Spatial")]
    public bool use3D = false;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    [Header("Randomization")]
    public bool randomClip = true;

    [NonSerialized]
    public float lastPlayedTime = -Mathf.Infinity;
}