using UnityEngine;

[System.Serializable]
public class MixerChannel
{
    [Range(0f, 1f)]
    public float bass = 0.5f;

    [Range(0f, 1f)]
    public float mid = 0.5f;

    [Range(0f, 1f)]
    public float treble = 0.5f;

    [Range(0f, 1f)]
    public float gain = 0.5f;
}