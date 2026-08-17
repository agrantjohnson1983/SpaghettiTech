using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sAudioGear : sInteractive
{
    public eAudioType typeAudio;

    public bool isSet = false;

    public virtual void SetGear(eAudioType _typeAudio)
    {
        isSet = true;


    }
}
