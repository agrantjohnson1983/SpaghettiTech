using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMicStand : sAudioGear
{
    public Transform micConnectTransform;

    public Vector3 offset;
    public Vector3 micRot;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Mic stand collision with " + other);

        if(other.TryGetComponent<sAudioGear>(out sAudioGear _gear) && isSet)
        {
            Debug.Log("Mic stand collision with audio gear");

            if(_gear.typeAudio == eAudioType.microphone)
            {
                other.gameObject.transform.parent = micConnectTransform;
                other.gameObject.transform.localPosition = Vector3.zero + offset;
                other.gameObject.transform.rotation = Quaternion.Euler(micRot);
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                _gear.isSet = true;
            }
        }
    }

}
