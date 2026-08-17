using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    /*private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoad;
    }
*/
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

    /*private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        switch (GameManager.gm.GetGameMode())
        {
            case eGameMode.frontEnd:

                break;

            case eGameMode.warehouse:

                break;

            case eGameMode.gig:

                //AddObjectives();
                //SpawnSetupObjects();

                break;
        }
    }*/

    /*void AddObjectives()
    {
        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Truss",
            name = "Setup Speakers"
        });

        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Motors",
            name = "Setup Microphones"
        });

        Status.objectives.Add(new ObjectiveStatus()
        {
            //id = "Raise",
            name = "Setup Mixer"
        });
    }*/

    public void RegiseterAudioSetup(eAudioType _type, GameObject _object)
    {
        Debug.Log("Registering audio setup spot for " + _type);

        switch (_type)
        {
            case eAudioType.speaker:

                setupSpeaker.Add(_object);

                //status.objectives[0].totalItems++;

                break;

            case eAudioType.sub:

                setupSub.Add(_object);

                //status.objectives[0].totalItems++;

                break;

            case eAudioType.mixer:

                setupMixer.Add(_object);

                //status.objectives[2].totalItems++;

                break;

            case eAudioType.micStand:

                setupMicStand.Add(_object);

                //status.objectives[1].totalItems++;

                break;

            // NO MIC SETUP CURRENTLY - it just snaps to stand
            case eAudioType.microphone:

                setupMicrophone.Add(_object);

                //status.objectives[0].totalItems++;

                break;
        }
    }

    public void AudioSet(eAudioType _type)
    {
        Debug.Log("Setting audio for type " + _type);


        switch(_type)
        {
            case eAudioType.speaker:
            case eAudioType.sub:

                ObjectiveItemComplete("Set Speakers");

                //ObjectiveStatus speakers = Status.objectives[0];

                //speakers.completedItems++;

                //Debug.Log("Adding to objective status: " + speakers);

                break;

            case eAudioType.mixer:

                //ObjectiveStatus mixer = Status.objectives[2];
                //mixer.completedItems++;

                ObjectiveItemComplete("Set Mixer");

                break;

            case eAudioType.micStand:

                //ObjectiveStatus micStand = Status.objectives[1];
                //micStand.completedItems++;

                ObjectiveItemComplete("Set Mic Stands");

                break;

            case eAudioType.microphone:

                ObjectiveItemComplete("Set Microphone");

                //ObjectiveStatus microphone = Status.objectives[1];
                //microphone.completedItems++;

                //Debug.Log("Adding to objective status: " + microphone);

                break;
        }

        RefreshProgress();

        UpdateDepartmentUI();
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
