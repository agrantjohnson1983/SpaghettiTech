using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sBlueprintsManager : MonoBehaviour
{
    public static sBlueprintsManager blueprintManagerGlobal;

    public SO_EventsUI soUI;

    public GameObject[] blueprintSetups;

    int activeBlueprintIndex = 0;

    float riggingTotal, audioTotal, videoTotal, lightingTotal;

    public GameObject gigStatus;

    public TMP_Text statusTotal, statusTruss, statusMotors;
    public Image imageTotal, imageTruss, imageMotors;

    // BLUEPRINTS STUFF
    //public Text blueprintRiggingButtonText, blueprintAudioButtonText, blueprintVideoButtonText, blueprintLightingButtonText;
    //bool blueprintRiggingOn, blueprintAudioOn, blueprintVideoOn, blueprintLightingOn = true;
    public GameObject blueprintsButton, blueprintsSetupSwitcher;
    //public Text blueprintSetupText;
    //public string[] blueprintTextName;
    bool blueprintsOpen = false;

    // BLUEPRINTS STUFF
    //public GameObject blueprintRiggingInstructions, blueprintRiggingInstructinsMinimized;
    //public GameObject blueprintAudioInstructions, blueprintAudioInstructinsMinimized;
    //public GameObject blueprintVideoInstructions, blueprintVideoInstructinsMinimized;
    //public GameObject blueprintLightingInstructions, blueprintLightingInstructinsMinimized;




    private void Awake()
    {
        blueprintManagerGlobal = this;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //blueprintSetupText.text = blueprintTextName[activeBlueprintIndex];

        for (int i = 0; i < blueprintSetups.Length; i++)
        {
            blueprintSetups[i].SetActive(false);
        }

        blueprintSetups[activeBlueprintIndex].SetActive(true);

        Invoke("SetBluePrints", 1f);
    }

    void UpdateRigging()
    {
        //truss
        ObjectiveStatus trussStatus = sRiggingManager.riggingMangerGlobal.Status.objectives[0];

        float trussProgress = 0f;

        if (trussStatus.totalItems > 0)
        {
            trussProgress = (float)trussStatus.completedItems / trussStatus.totalItems;
        }

        statusTruss.text = (int)(trussProgress*100) + "%";

        imageTruss.fillAmount = trussProgress;

        Debug.Log("truss progress is at: " + trussProgress);

        // motor
        ObjectiveStatus motorStatus = sRiggingManager.riggingMangerGlobal.Status.objectives[1];

        float motorProgress = 0f;

        if (motorStatus.totalItems > 0)
        {
            motorProgress = (float)motorStatus.completedItems / motorStatus.totalItems;
        }

        statusMotors.text = (int)(motorProgress*100) + "%";

        imageMotors.fillAmount = motorProgress;

        Debug.Log("motor progress is at: " + motorProgress);


        // overall
        float overallProgress = sRiggingManager.riggingMangerGlobal.Status.Completion;

        statusTotal.text = (int)(overallProgress*100f) + "%";

        imageTotal.fillAmount = overallProgress;
    }

    void UpdateAudio()
    {

    }

    void UpdateVideo()
    {

    }

    void UpdateLighting()
    {

    }

    void UpdateTotal()
    {

    }
    public void ToggleBlueprintOpen()
    {
        blueprintsOpen = !blueprintsOpen;

        GameManager.gm.ToggleOverheadCamera(blueprintsOpen);

        if (!GameManager.gm.isDoingTut)
        {
            blueprintsSetupSwitcher.SetActive(blueprintsOpen);
        }
        else
        {
            //blueprintsTut.SetActive(blueprintsOpen);
        }

        //hiringButton.SetActive(!blueprintsOpen);
        //characterPanel.SetActive(!blueprintsOpen);
        //toolbelt.SetActive(!blueprintsOpen);

        gigStatus.SetActive(blueprintsOpen);

        if (blueprintsOpen)
        {
            SetBluePrints();
        }
    }


    public void SetBluePrints()
    {
        //riggingTotal = sGigManager.gigManagerGlobal.

        UpdateRigging();
    }

    public void SwitchBlueprint(bool _isLeftButton)
    {
        blueprintSetups[activeBlueprintIndex].SetActive(false);

        if (_isLeftButton)
        {
            // setups -1
            if (activeBlueprintIndex <= 0)
            {
                activeBlueprintIndex = blueprintSetups.Length-1;
            }

            else
            {
                activeBlueprintIndex--;
            }

        }

        else
        {
            // setups +1
            if (activeBlueprintIndex >= blueprintSetups.Length)
            {
                activeBlueprintIndex = 0;
            }

            else
            {
                activeBlueprintIndex++;
                if(activeBlueprintIndex >= blueprintSetups.Length)
                {
                    activeBlueprintIndex = 0;
                }
            }
        }

        Debug.Log("Setting active with index of " + activeBlueprintIndex);
        blueprintSetups[activeBlueprintIndex].SetActive(true);
    }

    //public void ToggleBlueprintRiggingInstructions()
    //{
    //    blueprintRiggingOn = !blueprintRiggingOn;

    //    blueprintRiggingInstructions.SetActive(blueprintRiggingOn);
    //    blueprintRiggingInstructinsMinimized.SetActive(!blueprintRiggingOn);

    //    if (blueprintRiggingOn)
    //    {
    //        blueprintRiggingButtonText.text = "X";
    //    }

    //    else
    //    {
    //        blueprintRiggingButtonText.text = "+";
    //    }
    //}

    //public void ToggleBlueprintAudioInstructions()
    //{
    //    blueprintAudioOn = !blueprintAudioOn;

    //    blueprintAudioInstructions.SetActive(blueprintAudioOn);
    //    blueprintAudioInstructinsMinimized.SetActive(!blueprintAudioOn);

    //    if (blueprintAudioOn)
    //    {
    //        blueprintAudioButtonText.text = "X";
    //    }

    //    else
    //    {
    //        blueprintAudioButtonText.text = "+";
    //    }
    //}

    //public void ToggleBlueprintLightingInstructions()
    //{
    //    blueprintLightingOn = !blueprintLightingOn;

    //    blueprintLightingInstructions.SetActive(blueprintLightingOn);
    //    blueprintLightingInstructinsMinimized.SetActive(!blueprintLightingOn);

    //    if (blueprintLightingOn)
    //    {
    //        blueprintLightingButtonText.text = "X";
    //    }

    //    else
    //    {
    //        blueprintLightingButtonText.text = "+";
    //    }
    //}

    //public void ToggleBlueprintVideoInstructions()
    //{
    //    blueprintVideoOn = !blueprintVideoOn;

    //    blueprintVideoInstructions.SetActive(blueprintVideoOn);
    //    blueprintVideoInstructinsMinimized.SetActive(!blueprintVideoOn);

    //    if (blueprintVideoOn)
    //    {
    //        blueprintVideoButtonText.text = "X";
    //    }

    //    else
    //    {
    //        blueprintVideoButtonText.text = "+";
    //    }
    //}
}
