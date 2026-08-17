using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMicrophone : sAudioGear
{
    public override void SetGear(eAudioType _typeAudio)
    {
        base.SetGear(_typeAudio);

        sAudioManager.audioManagerGlobal.AudioSet(eAudioType.microphone);
    }
}
