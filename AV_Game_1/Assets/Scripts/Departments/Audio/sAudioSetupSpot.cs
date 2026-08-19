using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sAudioSetupSpot : sSetupSpotBASE
{
    //public SO_AudioEventChannel soAudio;

    public eAudioType typeAudio;

    public Vector3 setupOffset;

    private void Start()
    {
        if (sAudioManager.audioManagerGlobal)
            sAudioManager.audioManagerGlobal.RegiseterAudioSetup(typeAudio, this.gameObject);
        else
            Debug.LogWarning("Audio setup spot did not register for " + this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (HasAction)
        //    ToolCheck(other.gameObject);
        if (hasBeenSet)
            return;

        if (other.TryGetComponent(out sAudioGear _audioGear))
        {
            // Checks that collided rig type is same as setup type
            if (_audioGear.typeAudio == typeAudio && _audioGear.enabled && !_audioGear.isSet)
            {
                hasBeenSet = true;

                // Resets the grab in case player is still holding the gear
                sPlayerCharacter.playerCharacterGlobal.ReturnGrabController().GrabReset();

                // this keeps the grab UI from turning on and prevents player from grabbing
                    _audioGear.CanBeGrabbed = false;

                _audioGear.SetGear(typeAudio);

                switch (typeAudio)
                {
                    case eAudioType.speaker:
                        {

                        //Debug.Log("Audio setup Collision with speaker Spot");

                        StartCoroutine(ReleaseThenMove(other.gameObject, this.transform.position + offset, this.transform.rotation));

                        sAudioManager.audioManagerGlobal.AudioSet(typeAudio);

                        break;
                        }

                            

                    case eAudioType.sub:
                        {

                        //Debug.Log("Audio setup Collision with Sub Spot");

                        StartCoroutine(ReleaseThenMove(other.gameObject, this.transform.position + offset, this.transform.rotation));                         

                        sAudioManager.audioManagerGlobal.AudioSet(typeAudio);

                        break;
                        }

                    case eAudioType.mixer:
                        {

                        //Debug.Log("Audio setup Collision with Mixer Spot");

                        StartCoroutine(ReleaseThenMove(other.gameObject, this.transform.position + offset, this.transform.rotation));

                        sAudioManager.audioManagerGlobal.AudioSet(typeAudio);

                        break;
                        }

                    case eAudioType.micStand:
                        {


                        //Debug.Log("Audio setup Collision with Mic Stand Spot");

                        StartCoroutine(ReleaseThenMove(other.gameObject, this.transform.position + offset, this.transform.rotation));

                        sAudioManager.audioManagerGlobal.AudioSet(typeAudio);

                        break;
                        }
                }

            soAudio.TriggerSFX("AudioGearSet");

            //soText.SpawnTextPopup(this.transform, "SETUP!", 12);

            //Destroy(this.gameObject, 0.75f);
        }

            else
            {
                Debug.Log("Wrong setup spot type");
            }

        }
        
    }
}
