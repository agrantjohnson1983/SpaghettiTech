using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sAudioSetupSpot : MonoBehaviour
{
    public eAudioType typeAudio;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

                            Debug.Log("Audio Speaker Collision with Setup Spot");


                            break;
                            }

                            

                        case eAudioType.sub:
                            {

                            Debug.Log("Audio Speaker Collision with Sub Spot");

                            break;
                            }

                        case eAudioType.mixer:
                            {

                            Debug.Log("Audio Speaker Collision with Mixer Spot");

                            break;
                            }

                        case eAudioType.micStand:
                            {

                            Debug.Log("Audio Speaker Collision with Mic Stand Spot");

                            break;
                            }
                    }
                }

                else
                {
                    Debug.Log("Wrong setup spot type");
                }

            }
        
    }
}
