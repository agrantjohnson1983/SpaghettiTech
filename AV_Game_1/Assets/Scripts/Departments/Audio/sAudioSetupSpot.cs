using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sAudioSetupSpot : MonoBehaviour
{
    public eAudioType typeAudio;

    public Vector3 setupOffset;

    private void OnTriggerEnter(Collider other)
    {       
            //if (HasAction)
            //    ToolCheck(other.gameObject);

            if (other.TryGetComponent(out sAudioGear _audioGear))
            {
                // Checks that collided rig type is same as setup type
                if (_audioGear.typeAudio == typeAudio && _audioGear.enabled)
                {
                    switch (typeAudio)
                    {
                        case eAudioType.speaker:
                            {

                            Debug.Log("Audio setup Collision with Setup Spot");


                            break;
                            }

                            

                        case eAudioType.sub:
                            {

                            Debug.Log("Audio setup Collision with Sub Spot");

                            break;
                            }

                        case eAudioType.mixer:
                            {

                            Debug.Log("Audio setup Collision with Mixer Spot");

                            break;
                            }

                        case eAudioType.micStand:
                            {

                            _audioGear.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                            _audioGear.gameObject.transform.position = _audioGear.gameObject.transform.position + setupOffset;
                            _audioGear.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            _audioGear.isSet = true;

                            Debug.Log("Audio setup Collision with Mic Stand Spot");

                            

                            break;
                            }
                    }

                Destroy(this.gameObject);
            }

                else
                {
                    Debug.Log("Wrong setup spot type");
                }

            }
        
    }
}
