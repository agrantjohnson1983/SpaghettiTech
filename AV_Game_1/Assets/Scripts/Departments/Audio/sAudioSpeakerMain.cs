using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAudioSpeakerMain : sAudioGear
{
    public GameObject plugPower, plugAudio; 

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        plugPower.SetActive(false);
        plugAudio.SetActive(false);
    }

    public override void SetGear(eAudioType _typeAudio)
    {
        base.SetGear(_typeAudio);

        plugAudio.SetActive(true);
        plugPower.SetActive(true);
    }
}
