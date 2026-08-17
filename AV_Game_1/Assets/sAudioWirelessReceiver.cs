using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAudioWirelessReceiver : MonoBehaviour
{
    void SetMic(sMicrophone _mic)
    {
        if (_mic.isWireless && !_mic.isConnected)
        {
            _mic.SetWireless(true, this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sMicrophone>(out sMicrophone _mic))
        {
            SetMic(_mic);
        }
    }
}
