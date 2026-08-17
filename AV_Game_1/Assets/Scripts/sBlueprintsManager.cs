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

    public GameObject pDeptStatus;

    List<GameObject> deptStatusList;

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
        deptStatusList = new List<GameObject>();
    }


    public void ToggleBlueprintOpen()
    {
        blueprintsOpen = !blueprintsOpen;

        GameManager.gm.ToggleOverheadCamera(blueprintsOpen);
        GameManager.gm.ToggleGameplayCamera(!blueprintsOpen);

        if (blueprintsOpen)
        {
            SetBluePrints();
        }

        else
        {
            foreach(GameObject go in deptStatusList)
            {
                Destroy(go);
            }

            deptStatusList.Clear();
        }
    }


    public void SetBluePrints()
    {
        deptStatusList = new List<GameObject>();

        foreach(sDepartmentManager dm in sGigManager.gigManagerGlobal.departmentManagersList)
        {
            uDeptStatus dStatus = Instantiate(pDeptStatus, gigStatus.transform).GetComponent<uDeptStatus>();

            dStatus.SetObjectives(dm);

            deptStatusList.Add(dStatus.gameObject);
        }

    }

    /*public void SwitchBlueprint(bool _isLeftButton)
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
    }*/

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
