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
            //Debug.Log("Mic stand collision with audio gear");

            if(_gear.typeAudio == eAudioType.microphone)
            {
                sPlayerCharacter.playerCharacterGlobal.ReturnGrabController().GrabReset();

                GameObject micObject = other.gameObject;

                // turns off collision
                //other.enabled = false;
                micObject.GetComponent<Rigidbody>().detectCollisions = false;

                // moves object
                micObject.transform.parent = micConnectTransform;
                micObject.transform.localPosition = Vector3.zero + offset;
                micObject.transform.rotation = Quaternion.Euler(micRot);
                micObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                micObject.GetComponent<Rigidbody>().detectCollisions = false;

                _gear.SetGear(eAudioType.microphone);
            }
        }
    }

}
