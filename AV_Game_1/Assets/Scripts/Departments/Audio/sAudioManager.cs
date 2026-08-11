using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAudioType { speaker, sub, mixer, micStand, microphone }
public class sAudioManager : sDepartmentManager
{
    public static sAudioManager audioManagerGlobal;
    //public Transform[] speakerSetupLocations;
    //public Transform[] subSetupLocations;
    //public Transform[] mixerSetupLocations;
    //public Transform[] micStandLocations;

    //public GameObject pSpeakerSetup;
    //public GameObject pSubSetup;
    //public GameObject pMixerSetup;
    //public GameObject pMicStandSetup;

    [HideInInspector] public List<sAudioMixer> audioMixerList;
    [HideInInspector] public List<sAudioSpeakerMain> audioSpeakerMainList;

    List<GameObject> speakerList, subList, mixerList, micStandList, microphoneList;

    List<GameObject> setupSpeaker, setupSub, setupMixer, setupMicStand, setupMicrophone;

    private void Awake()
    {
        if (audioManagerGlobal == null)
            audioManagerGlobal = this;
        else
            Destroy(this.gameObject);

        setupSpeaker = new List<GameObject>();
        setupSub = new List<GameObject>();
        setupMixer = new List<GameObject>();
        setupMicStand = new List<GameObject>();
        setupMicrophone = new List<GameObject>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public void RegiseterAudioSetup(eAudioType _type, GameObject _object)
    {
        Debug.Log("Registering audio setup spot for " + _type);

        switch (_type)
        {
            case eAudioType.speaker:

                setupSpeaker.Add(_object);

                break;

            case eAudioType.sub:

                setupSub.Add(_object);

                break;

            case eAudioType.mixer:

                setupMixer.Add(_object);

                break;

            case eAudioType.micStand:

                setupMicStand.Add(_object);

                break;

            case eAudioType.microphone:

                setupMicrophone.Add(_object);

                break;
        }
    }

    /*public void RegisterAudioGear(eAudioType _type, GameObject _object)
    {
        switch(_type)
        {
            case eAudioType.speaker:

                break;

            case eAudioType.sub:

                break;

            case eAudioType.mixer:

                break;

            case eAudioType.micStand:

                break;

            case eAudioType.microphone:

                break;
        }
    }    */
}
