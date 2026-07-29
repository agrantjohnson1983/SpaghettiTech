using UnityEngine;

[System.Serializable]
public class sVolumeChannel
{
    public string channelName;

    [Range(0f, 1f)]
    public float targetVolume;

    [Range(0f, 1f)]
    public float playerVolume;
}