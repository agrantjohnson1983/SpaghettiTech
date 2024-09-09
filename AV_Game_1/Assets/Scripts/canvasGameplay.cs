using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasGameplay : MonoBehaviour
{
    public Text textInstructionsRigging, textInstructionsAudio, textInstructionsVideo, textInstructionsLighting;

    public string startingInstructionsRigging, startingInstructionsAudio, startingInstructionsVideo, startingInstructionsLighting;

    public Image taskGauge;

    public SO_EventsUI soUI;

    public GameObject characterPanel;

    public Image characterImage;

    public Image characterSymbolImage;

    public Text characterName;

    //public Text characterType;

    public Image connectionTypeHeld, toolTypeHeld;

    public GameObject connectionTypeObj, toolTypeObj;

    public GameObject blueprintRiggingInstructions, blueprintRiggingInstructinsMinimized;
    public GameObject blueprintAudioInstructions, blueprintAudioInstructinsMinimized;
    public GameObject blueprintVideoInstructions, blueprintVideoInstructinsMinimized;
    public GameObject blueprintLightingInstructions, blueprintLightingInstructinsMinimized;

    public Text blueprintRiggingButtonText, blueprintAudioButtonText, blueprintVideoButtonText, blueprintLightingButtonText;

    bool blueprintRiggingOn, blueprintAudioOn, blueprintVideoOn, blueprintLightingOn = true;

    public GameObject blueprintsButton, blueprintsTut, blueprintsSetupSwitcher;

    public Text blueprintSetupText;

    int activeBlueprintIndex = 0;

    public string[] blueprintTextName;

    bool blueprintsOpen = false;

    public GameObject motorController;

    public GameObject popupControls;
    public Text popupControlsText;
    public Vector3 popupControlsOffset;

    public GameObject hiringPanel, hiringButton;

    bool isHiring = false;

    public Transform connectionPlatesTransform;

    public float startingMoney = 1000;
    float currentMoney;
    public Text currentMoneyText;

    private void OnEnable()
    {
        soUI.instructionsRiggingEvent.AddListener(ChangeRiggingInstructions);
        soUI.instructionsAudioEvent.AddListener(ChangeAudioInstructions);
        soUI.instructionsVideoEvent.AddListener(ChangeVideoInstructions);
        soUI.instructionsLightingEvent.AddListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.AddListener(GoTaskGauge);
        soUI.characterChangeEvent.AddListener(ChangeCharacter);
        soUI.connectionHeldImage.AddListener(ChangeConnectionHeldImage);
        soUI.toolHeldImage.AddListener(ChangeToolHeldImage);
        soUI.motorControlDisplay.AddListener(ToggleMotorController);

        soUI.controlsPopup.AddListener(TogglePopup);

        soUI.characterHire.AddListener(ChangeMoney);
    }

    private void OnDisable()
    {
        soUI.instructionsRiggingEvent.RemoveListener(ChangeRiggingInstructions);
        soUI.instructionsAudioEvent.RemoveListener(ChangeAudioInstructions);
        soUI.instructionsVideoEvent.RemoveListener(ChangeVideoInstructions);
        soUI.instructionsLightingEvent.RemoveListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.RemoveListener(GoTaskGauge);
        soUI.characterChangeEvent.RemoveListener(ChangeCharacter);
        soUI.connectionHeldImage.RemoveListener(ChangeConnectionHeldImage);
        soUI.toolHeldImage.RemoveListener(ChangeToolHeldImage);
        soUI.motorControlDisplay.RemoveListener(ToggleMotorController);

        soUI.controlsPopup.RemoveListener(TogglePopup);

        soUI.characterHire.RemoveListener(ChangeMoney);
    }

    // Start is called before the first frame update
    void Start()
    {
        taskGauge.fillAmount = 0;

        ChangeConnectionHeldImage(null);
        ChangeToolHeldImage(null);

        TogglePopup(null, Vector3.zero);

        //hiringPanel.SetActive(false);

        blueprintSetupText.text = blueprintTextName[activeBlueprintIndex];

        currentMoney = startingMoney;
        currentMoneyText.text = currentMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void TogglePopup(string _popupText, Vector3 _pos)
    {
        Vector3 playerPos;

        if(_popupText == null)
        {
            popupControls.SetActive(false);
        }

        else
        {
            playerPos = sPlayerCharacter.playerGlobal.transform.position;
            popupControls.SetActive(true);
            popupControlsText.text = _popupText;
            popupControlsText.gameObject.transform.position = Camera.main.WorldToScreenPoint(playerPos + popupControlsOffset);
        }
    }

    void ChangeCharacter(SO_CharacterData _characterData)
    {
        characterImage.sprite = _characterData.characterSprite;
        characterName.text = _characterData.characterName;
        //characterType.text = _characterData.characterType.ToString();
        characterSymbolImage.sprite = _characterData.characterSymbolSprite;
    }

    void ChangeConnectionHeldImage(Sprite _sprite)
    {
        if(_sprite != null)
        {
            //Debug.Log("Setting New Connection Sprite");

            connectionTypeObj.SetActive(true);
            connectionTypeHeld.sprite = _sprite;
        }

        else
        {
            connectionTypeObj.SetActive(false);
        }
    }

    void ChangeToolHeldImage(Sprite _sprite)
    {
        if(_sprite != null)
        {
            Debug.Log("Setting New Connection Sprite");

            toolTypeObj.SetActive(true);
            toolTypeHeld.sprite = _sprite;
        }

        else
        {
            toolTypeObj.SetActive(false);
        }
    }

    void ChangeRiggingInstructions(string _instructions)
    {
        textInstructionsRigging.text = _instructions;
    }

    void ChangeAudioInstructions(string _instructions)
    {
        textInstructionsAudio.text = _instructions;
    }

    void ChangeVideoInstructions(string _instructions)
    {
        textInstructionsVideo.text = _instructions;
    }

    void ChangeLightingInstructions(string _instructions)
    {
        textInstructionsLighting.text = _instructions;
    }

    void GoTaskGauge(float _time)
    {
        Debug.Log("Go Task Gauge Triggered on Canvas");

        if(_time == 0)
        {
            Debug.Log("Stopping Task Gauge");

            StopCoroutine(TaskGauge(0));

            taskGauge.fillAmount = 0;
        }

        StartCoroutine(TaskGauge(_time));
    }

    IEnumerator TaskGauge(float _time)
    {
        float counter = 0;

        while(counter < _time)
        {
            taskGauge.fillAmount = Mathf.Lerp(0, 1, (counter / _time));

            counter += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Task Gauge has completed");

        taskGauge.fillAmount = 0f;
    }


    public void ToggleBlueprintRiggingInstructions()
    {
        blueprintRiggingOn = !blueprintRiggingOn;

        blueprintRiggingInstructions.SetActive(blueprintRiggingOn);
        blueprintRiggingInstructinsMinimized.SetActive(!blueprintRiggingOn);

        if (blueprintRiggingOn)
        {
            blueprintRiggingButtonText.text = "X";
        }

        else
        {
            blueprintRiggingButtonText.text = "+";
        }
    }

    public void ToggleBlueprintAudioInstructions()
    {
        blueprintAudioOn = !blueprintAudioOn;

        blueprintAudioInstructions.SetActive(blueprintAudioOn);
        blueprintAudioInstructinsMinimized.SetActive(!blueprintAudioOn);

        if (blueprintAudioOn)
        {
            blueprintAudioButtonText.text = "X";
        }

        else
        {
            blueprintAudioButtonText.text = "+";
        }
    }

    public void ToggleBlueprintLightingInstructions()
    {
        blueprintLightingOn = !blueprintLightingOn;

        blueprintLightingInstructions.SetActive(blueprintLightingOn);
        blueprintLightingInstructinsMinimized.SetActive(!blueprintLightingOn);

        if (blueprintLightingOn)
        {
            blueprintLightingButtonText.text = "X";
        }

        else
        {
            blueprintLightingButtonText.text = "+";
        }
    }

    public void ToggleBlueprintVideoInstructions()
    {
        blueprintVideoOn = !blueprintVideoOn;

        blueprintVideoInstructions.SetActive(blueprintVideoOn);
        blueprintVideoInstructinsMinimized.SetActive(!blueprintVideoOn);

        if (blueprintVideoOn)
        {
            blueprintVideoButtonText.text = "X";
        }

        else
        {
            blueprintVideoButtonText.text = "+";
        }
    }

    void ToggleMotorController(bool _isOn)
    {
        motorController.SetActive(_isOn);
    }
    
    public void MotorControllerUp()
    {
        soUI.TriggerMotorControls(true);
    }

    public void MotorControllerDown()
    {
        soUI.TriggerMotorControls(false); ;
    }

    public void OnArrowLeft()
    {
        GameManager.gm.SwitchActivePlayer(-1);
    }

    public void OnArrowRight()
    {
        GameManager.gm.SwitchActivePlayer(1);
    }

    public void ToggleHireScreen()
    {
        isHiring = !isHiring;

        hiringButton.SetActive(!isHiring);

        hiringPanel.SetActive(isHiring);

        blueprintsButton.SetActive(!isHiring);
        characterPanel.SetActive(!isHiring);

        GameManager.gm.ToggleHiringCamera(isHiring);

        if (!isHiring)
        {
            
        }

        
    }

    public void ToggleBlueprintOpen()
    {
        blueprintsOpen = !blueprintsOpen;

        GameManager.gm.ToggleBlueprintCamera(blueprintsOpen);

        if(!GameManager.gm.isDoingTut)
        {
            blueprintsSetupSwitcher.SetActive(blueprintsOpen);
        }
        else
        {
            blueprintsTut.SetActive(blueprintsOpen);
        }

        hiringButton.SetActive(!blueprintsOpen);
        characterPanel.SetActive(!blueprintsOpen);
    }

    public void SwitchBlueprint(bool _isLeftButton)
    {
        sBlueprintsManager.blueprintManagerGlobal.SwitchBlueprint(_isLeftButton);

        if (_isLeftButton)
        {
            // setups -1
            if (activeBlueprintIndex <= 0)
            {
                activeBlueprintIndex = blueprintTextName.Length - 1;
            }

            else
            {
                activeBlueprintIndex--;
            }

        }

        else
        {
            // setups +1
            if (activeBlueprintIndex >= blueprintTextName.Length)
            {
                activeBlueprintIndex = 0;
            }

            else
            {
                activeBlueprintIndex++;
                if (activeBlueprintIndex >= blueprintTextName.Length)
                {
                    activeBlueprintIndex = 0;
                }
            }
        }

        blueprintSetupText.text = blueprintTextName[activeBlueprintIndex];
    }

    void ChangeMoney(float _amount)
    {
        currentMoney += _amount;

        currentMoneyText.text = currentMoney.ToString();
    }
}
