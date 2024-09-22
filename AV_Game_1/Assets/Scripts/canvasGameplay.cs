using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasGameplay : MonoBehaviour
{
    // MOVE THIS TO SPAWN OVER ITEMS THAT HAVE TASK GAUGES
    public Image taskGauge;

    public SO_EventsUI soUI;

    // CHARACTER STUFF
    public GameObject characterPanel;
    public Image characterImage;
    public Image characterSymbolImage;
    public Text characterName;

    //HAND HELD STUFF
    public Sprite[] handImagesEmpty;
    public Image[] handBGImages;
    public Image[] handImages;
    //public GameObject handRight, handLeft;
    //public Sprite spriteHandRight, spriteHandLeft;
    //public Transform connectionPlatesTransform;

    // INSTRUCTIONS STUFF

    public Text textInstructionsRigging, textInstructionsAudio, textInstructionsVideo, textInstructionsLighting;
    public string startingInstructionsRigging, startingInstructionsAudio, startingInstructionsVideo, startingInstructionsLighting;

    // BLUEPRINTS STUFF
    public GameObject blueprintRiggingInstructions, blueprintRiggingInstructinsMinimized;
    public GameObject blueprintAudioInstructions, blueprintAudioInstructinsMinimized;
    public GameObject blueprintVideoInstructions, blueprintVideoInstructinsMinimized;
    public GameObject blueprintLightingInstructions, blueprintLightingInstructinsMinimized;

    // BLUEPRINTS STUFF
    public Text blueprintRiggingButtonText, blueprintAudioButtonText, blueprintVideoButtonText, blueprintLightingButtonText;
    bool blueprintRiggingOn, blueprintAudioOn, blueprintVideoOn, blueprintLightingOn = true;
    public GameObject blueprintsButton, blueprintsTut, blueprintsSetupSwitcher;
    public Text blueprintSetupText;
    int activeBlueprintIndex = 0;
    public string[] blueprintTextName;
    bool blueprintsOpen = false;

    // This is the controls for a Motor Controller that spawns when the motor controller is set
    public GameObject motorController;

    // CONTROLS POPUP - This is used mostly for the grabbable and actionable interfaces.
    public GameObject popupControls;
    public Text popupControlsText;
    public Vector3 popupControlsOffset;

    // HIRING STUFF
    public GameObject hiringPanel, hiringButton;
    bool isHiring = false;

    // MONEY STUFF
    public float startingMoney = 1000;
    float currentMoney;
    public Text currentMoneyText;

    // TOOLBELT STUFF

    public GameObject toolbelt, toolbeltGrid, toolbeltArrow;
    bool isHoldingTool = false;
    public Image toolHeld;
    public Text tooldHeldText;

    public Transform toolbeltPanel;
    public GameObject toolbeltToolButton;

    public List<GameObject> toolButtonsList;

    private void OnEnable()
    {
        soUI.instructionsRiggingEvent.AddListener(ChangeRiggingInstructions);
        soUI.instructionsAudioEvent.AddListener(ChangeAudioInstructions);
        soUI.instructionsVideoEvent.AddListener(ChangeVideoInstructions);
        soUI.instructionsLightingEvent.AddListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.AddListener(GoTaskGauge);

        soUI.characterChangeEvent.AddListener(ChangeCharacter);

        soUI.itemHeldImage.AddListener(ChangeHandImage);

        soUI.motorControlDisplay.AddListener(ToggleMotorController);

        soUI.controlsPopup.AddListener(TogglePopup);

        soUI.characterHire.AddListener(ChangeMoney);

        soUI.toolHeldImage.AddListener(AddToolToBelt);
    }

    private void OnDisable()
    {
        soUI.instructionsRiggingEvent.RemoveListener(ChangeRiggingInstructions);
        soUI.instructionsAudioEvent.RemoveListener(ChangeAudioInstructions);
        soUI.instructionsVideoEvent.RemoveListener(ChangeVideoInstructions);
        soUI.instructionsLightingEvent.RemoveListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.RemoveListener(GoTaskGauge);
        soUI.characterChangeEvent.RemoveListener(ChangeCharacter);
        soUI.itemHeldImage.RemoveListener(ChangeHandImage);
        soUI.motorControlDisplay.RemoveListener(ToggleMotorController);

        soUI.controlsPopup.RemoveListener(TogglePopup);

        soUI.characterHire.RemoveListener(ChangeMoney);

        soUI.toolHeldImage.RemoveListener(AddToolToBelt);
    }

    // Start is called before the first frame update
    void Start()
    {
        taskGauge.fillAmount = 0;

        //ChangeHandImage(null, true);
        //ChangeHandImage(null, false);

        TogglePopup(null);
        //ChangeHandImage(null, null);

        //hiringPanel.SetActive(false);

        blueprintSetupText.text = blueprintTextName[activeBlueprintIndex];

        currentMoney = startingMoney;
        currentMoneyText.text = currentMoney.ToString();

        toolbeltGrid.SetActive(false);
        toolbeltArrow.SetActive(false);
        tooldHeldText.text = "";
    }

    void TogglePopup(string _popupText)
    {
        Vector3 playerPos;

        if(_popupText == null)
        {
            popupControls.SetActive(false);
        }

        else
        {
            playerPos = GameManager.gm.ReturnCurrentPlayer().transform.position;
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

    // THis is used to change the image of the hand to reprsent what the hand is doing
    void ChangeHandImage(Sprite _itemImage, int[] _indexArray)
    {
        //Debug.Log("Canvas Hand Image changing at index of " + _indexArray[i].ToString() + " with sprite named " + _itemImage.name.ToString());

        for (int i = 0; i < _indexArray.Length-1; i++)
        {
            if (_itemImage != null)
            {
                handImages[_indexArray[i]].sprite = _itemImage;
                handBGImages[_indexArray[i]].color = Color.yellow;
            }
                

            else
            {
                handImages[_indexArray[i]].sprite = handImagesEmpty[_indexArray[i]];
                handBGImages[_indexArray[i]].color = Color.green;
            }
                
        }  
    }

    // This gets called when a player clicks a item in the hands UI
    public void OnItemClick(int _index)
    {
        Debug.Log("Item image " + _index + " triggered");
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

        toolbelt.SetActive(!isHiring);

        GameManager.gm.ToggleHiringCamera(isHiring);

        if (!isHiring)
        {
            
        }
    }

    // This turns on and off the blueprints
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
        toolbelt.SetActive(!blueprintsOpen);

    }

    // This changes between the blueprint setups when the blueprints are open
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

    // This takes in an amount and adds it to the money
    void ChangeMoney(float _amount)
    {
        currentMoney += _amount;

        currentMoneyText.text = currentMoney.ToString();
    }

    public void ToggleToolbelt(bool _isOpen)
    {
        Debug.Log("Toggling the toolbelt");

        toolbeltGrid.SetActive(_isOpen);
        toolbeltArrow.SetActive(_isOpen);
    }
 
    public void ChangeToolImage(Sprite _sprite)
    {
        toolHeld.sprite = _sprite;
    }

    public void OnToolClick(SO_ItemData _itemData, int _index)
    {
        Debug.Log("Tool Item Clicked");
    }

    public void AddToolToBelt(SO_ItemData _itemData)
    {
        // if no tool is held then it changes the held tool;
        if(!isHoldingTool)
        {
            isHoldingTool = true;
            toolHeld.sprite = _itemData.itemSprite;
            tooldHeldText.text = _itemData.itemName;
        }

        // Spawns toolbelt button and adds it to list
        GameObject tempObject = Instantiate(toolbeltToolButton, toolbeltPanel);

        uButtonTool buttonTool;

        buttonTool = tempObject.GetComponent<uButtonTool>();

        buttonTool.SetButton(_itemData);

        toolButtonsList.Add(tempObject);

        buttonTool.SetIndex(toolButtonsList.Count);

        Debug.Log("Setting Tool to index " + toolButtonsList.Count);
    }
}
