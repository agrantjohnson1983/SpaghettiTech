using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMicrophone : sAudioGear
{
    public bool isWireless = false;

    public bool isConnected = false;

    public override void SetGear(eAudioType _typeAudio)
    {
        base.SetGear(_typeAudio);

        sAudioManager.audioManagerGlobal.AudioSet(eAudioType.microphone);
    }

    public void SetWireless(bool _isConnected, sAudioWirelessReceiver receiver)
    {
        Debug.Log("Wireless mic is now conneted to receiver");

        isConnected = _isConnected;

        sAudioManager.audioManagerGlobal.AudioSet(eAudioType.wireless);
    }
}
