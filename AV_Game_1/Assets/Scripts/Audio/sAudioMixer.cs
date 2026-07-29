using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sAudioMixer : sAudioGear
{
    public GameObject mixerMiniGame;

    bool isPlayingMinigame = false;

    // TO DO - add in a world canvas controller with single button "START MIXING"
    // START MIXING will toggle on mini game and lock player movement
    // There should be an "END" button in the mixer game

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isPlayingMinigame)
        {
            isPlayingMinigame = true;
            mixerMiniGame.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayingMinigame)
        {
            isPlayingMinigame = false;
            mixerMiniGame.SetActive(false);
        }
    }

}
