using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAudioType { speaker, sub, mixer, micStand }
public class sAudioManager : MonoBehaviour
{
    public Transform[] speakerSetupLocations;
    public Transform[] subSetupLocations;
    public Transform[] mixerSetupLocations;
    public Transform[] micStandLocations;

    public GameObject pSpeakerSetup;
    public GameObject pSubSetup;
    public GameObject pMixerSetup;
    public GameObject pMicStandSetup;

    public List<sAudioMixer> audioMixerList;
    public List<sAudioSpeakerMain> audioSpeakerMainList;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < speakerSetupLocations.Length; i++)
        {
            GameObject tempObj;

            tempObj = Instantiate(pSpeakerSetup, speakerSetupLocations[i]);

            //audioSpeakerMainList.Add(tempObj);

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            //if (GameManager.gm.isDoingTut)
            //    tempObj.SetActive(false);

        }

        for (int i = 0; i < subSetupLocations.Length; i++)
        {
            GameObject tempObj;

            tempObj = Instantiate(pSubSetup, subSetupLocations[i]);

            //audioSpeakerMainList.Add(tempObj);

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            //if (GameManager.gm.isDoingTut)
            //    tempObj.SetActive(false);

        }

        for (int i = 0; i < mixerSetupLocations.Length; i++)
        {
            GameObject tempObj;

            tempObj = Instantiate(pMixerSetup, mixerSetupLocations[i]);

            //audioSpeakerMainList.Add(tempObj);

            // Turns off all objects during tutorial so you only do them one at a time vs all at once
            //if (GameManager.gm.isDoingTut)
            //    tempObj.SetActive(false);

        }
    }
}
