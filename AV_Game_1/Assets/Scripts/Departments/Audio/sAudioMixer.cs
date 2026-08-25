using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sCameraTrigger))]
public class sAudioMixer : sAudioGear
{
    public GameObject mixerMiniGame;

    bool isPlayingMinigame = false;

    public GameObject canvasMixer, firstSelected;

    public static bool isMixing = false;

    sCameraTrigger camTrigger;

    sPlayerUIController controller;

    public override void Start()
    {
        base.Start();

        canvasMixer.SetActive(false);

        camTrigger = GetComponent<sCameraTrigger>();

        
    }

    // TO DO - add in a world canvas controller with single button "START MIXING"
    // START MIXING will toggle on mini game and lock player movement
    // There should be an "END" button in the mixer game

    public override void SetGear(eAudioType _typeAudio)
    {
        base.SetGear(_typeAudio);

        camTrigger.canTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerUIController>(out sPlayerUIController _UI_Controller) && !isPlayingMinigame && isSet)
        {
            isPlayingMinigame = true;

            controller = _UI_Controller;

            if(!canvasMixer || !firstSelected)
            {
                Debug.LogWarning("canvas mixer or first selected was null for audio mixer");
                return;
            }

            controller.OpenPopup(canvasMixer, firstSelected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayingMinigame && controller != null)
        {
            isPlayingMinigame = false;

            controller.ClosePopup();

            controller = null;
        }
    }

}
