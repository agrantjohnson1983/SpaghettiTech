using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class canvasGameplay : MonoBehaviour
{
    // MOVE THIS TO SPAWN OVER ITEMS THAT HAVE TASK GAUGES
    public Image taskGauge;

    // EVENTS UI
    public SO_EventsUI soUI;

    //// CHARACTER STUFF
    //public GameObject characterPanel;
    //public Image characterImage;
    //public Image characterSymbolImage;
    //public Text characterName;

    //HAND HELD STUFF
    //public Sprite[] handImagesEmpty;
    //public Image[] handBGImages;
    //public Image[] handImages;
    //public GameObject handRight, handLeft;
    //public Sprite spriteHandRight, spriteHandLeft;
    //public Transform connectionPlatesTransform;

    // INSTRUCTIONS STUFF
    //public Text textInstructionsRigging, textInstructionsAudio, textInstructionsVideo, textInstructionsLighting;
    //public string startingInstructionsRigging, startingInstructionsAudio, startingInstructionsVideo, startingInstructionsLighting;

    // This is the controls for a Motor Controller that spawns when the motor controller is set
    public GameObject motorController;

    // CONTROLS POPUP - This is used mostly for the grabbable and actionable interfaces.
    public Transform popupControlsTransform;
    public GameObject pPopupControls;
    Dictionary<string, GameObject> popupDictionary;

    // HIRING STUFF
    public GameObject hiringPanel, hiringButton;
    bool isHiring = false;

    // TIME
    public GameObject timeUI;

    // MONEY STUFF
    public GameObject moneyUI;
    public float startingMoney = 1000;
    float currentMoney;
    public TextMeshProUGUI currentMoneyText;

    // TOOLBELT STUFF
    public GameObject toolbelt, toolbeltGrid, toolbeltArrow;
    bool isHoldingTool = false;
    public Image toolHeld;
    public Text tooldHeldText;

    public Transform toolbeltPanel;
    public GameObject toolbeltToolButton;

    public List<GameObject> toolButtonsList;

    // WAREHOUSE
    public GameObject startScreen;
    //public GameObject gigSelectScreen;

    public TMP_Text textMessageMain;

    // BLUEPRINTS
    public GameObject blueprintsButton;

    private void Awake()
    {
        //if (GameManager.gm.canvasGameplay.gameObject != this.gameObject)
        //    Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        //soUI.instructionsRiggingEvent.AddListener(ChangeRiggingInstructions);
        //soUI.instructionsAudioEvent.AddListener(ChangeAudioInstructions);
        //soUI.instructionsVideoEvent.AddListener(ChangeVideoInstructions);
        //soUI.instructionsLightingEvent.AddListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.AddListener(GoTaskGauge);

        soUI.motorControlDisplay.AddListener(ToggleMotorController);

        soUI.controlsPopup.AddListener(TogglePopup);

        soUI.crewHire.AddListener(HireCrew);

        soUI.toolHeldImage.AddListener(AddToolToBelt);

        soUI.messageEvent.AddListener(MessageSend);
    }

    private void OnDisable()
    {
        //soUI.instructionsRiggingEvent.RemoveListener(ChangeRiggingInstructions);
        //soUI.instructionsAudioEvent.RemoveListener(ChangeAudioInstructions);
        //soUI.instructionsVideoEvent.RemoveListener(ChangeVideoInstructions);
        //soUI.instructionsLightingEvent.RemoveListener(ChangeLightingInstructions);

        soUI.taskTimeEvent.RemoveListener(GoTaskGauge);

        soUI.motorControlDisplay.RemoveListener(ToggleMotorController);

        soUI.controlsPopup.RemoveListener(TogglePopup);

        soUI.crewHire.RemoveListener(HireCrew);

        soUI.toolHeldImage.RemoveListener(AddToolToBelt);

        soUI.messageEvent.RemoveListener(MessageSend);
    }

    private void MessageSend(string _message, float _time)
    {
        // turn alpha on
        textMessageMain.CrossFadeAlpha(1, 0f, false);

        // set message
        textMessageMain.text = _message;

        // slowly fade alpha out
        textMessageMain.CrossFadeAlpha(0, _time, false);

        //textMessageMain.CrossFadeColor()
    }

    IEnumerator MessageFade(float _time)
    {
        float counter = 0f;

        while (counter < _time)
        {
            
            counter += Time.deltaTime;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        taskGauge.fillAmount = 0;

        popupDictionary = new Dictionary<string, GameObject>();

        currentMoney = startingMoney;
        currentMoneyText.text = currentMoney.ToString();

        toolbeltGrid.SetActive(false);
        toolbeltArrow.SetActive(false);
        tooldHeldText.text = "";

        // turns off message at start
        MessageSend("", 0f);
    }

    void TogglePopup(string _controlText, string _actionText)
    {
        GameObject tempObj;

        // checks for "" which will destroy the object based on the action text sent
        if (_controlText == "")
        {
            if (popupDictionary.Count == 0)
                return;

            // checks dictionary for action text and outputs game object if found
            if(popupDictionary.TryGetValue(_actionText, out tempObj))
            {
                popupDictionary.Remove(_actionText);
                Destroy(tempObj);
            }

            else
            {
                //Debug.LogWarning("Destroy message was sent but no dictionary key found for " + _actionText);
            }

            return;
        }

        //Debug.Log("Toggle popup triggered for " + _actionText);

        if(popupDictionary.ContainsKey(_actionText))
        {
            //Debug.Log("Dictionary already contains " + _actionText + " this must be a duplicate");

            return;
        }

        else
        {
            // spawns object
            tempObj = Instantiate(pPopupControls, popupControlsTransform);

            // adds to dictionary
            popupDictionary.Add(_actionText, tempObj);

            // popup controls ref
            uPopupControls popupControls;

            // looks for the pupcontrols
            if (tempObj.TryGetComponent<uPopupControls>(out popupControls))
            {
                // sets popup
                popupControls.SetPopup(_controlText, _actionText);
            }

            else
            {
                Debug.LogWarning("No uPopup controls found on the spawned object");
            }
        }

        

        //Vector3 playerPos;

        //if(_popupText == null)
        //{
        //    popupControls.SetActive(false);
        //}

        //else
        //{
        //    playerPos = sPlayerCharacter.playerCharacterGlobal.transform.position;
        //    popupControls.SetActive(true);
        //    popupControlsText.text = _popupText;
        //    popupControlsText.gameObject.transform.position = Camera.main.WorldToScreenPoint(playerPos + popupControlsOffset);
        //}
    }

    // This gets called when a player clicks a item in the hands UI
    public void OnItemClick(int _index)
    {
        Debug.Log("Item image " + _index + " triggered");
    }

    //void ChangeRiggingInstructions(string _instructions)
    //{
    //    textInstructionsRigging.text = _instructions;
    //}

    //void ChangeAudioInstructions(string _instructions)
    //{
    //    textInstructionsAudio.text = _instructions;
    //}

    //void ChangeVideoInstructions(string _instructions)
    //{
    //    textInstructionsVideo.text = _instructions;
    //}

    //void ChangeLightingInstructions(string _instructions)
    //{
    //    textInstructionsLighting.text = _instructions;
    //}

    // This is used to trigger the UI
    void GoTaskGauge(float _time, Vector3 _pos)
    {

        //Debug.Log("Go Task Gauge Triggered on Canvas");

        taskGauge.transform.position = Camera.main.WorldToScreenPoint(_pos);

        if(_time == 0)
        {
            //Debug.Log("Stopping Task Gauge");

            //StopCoroutine(TaskGauge(0));

            StopAllCoroutines();

            taskGauge.fillAmount = 0;

            return;
        }

        else
        {
            //StopAllCoroutines();
            StartCoroutine(TaskGauge(_time));
        }
    }

    IEnumerator TaskGauge(float _time)
    {
        float counter = 0;

        while(counter < _time)
        {
            taskGauge.fillAmount = Mathf.Lerp(0, 1, (counter / _time));

            counter += Time.deltaTime;

            Debug.Log("Task Counter is at : " + counter / _time);

            yield return null;
        }

        //Debug.Log("Task Gauge has completed");

        taskGauge.fillAmount = 0f;
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
        //GameManager.gm.SwitchActivePlayer(-1);
    }

    public void OnArrowRight()
    {
        //GameManager.gm.SwitchActivePlayer(1);
    }

    public void OnStart()
    {
        startScreen.SetActive(false);

        soUI.TriggerMessage("GAME START!", 3f);

        GameManager.gm.StartGameplay();
    }

    public void ToggleHireScreen()
    {
        isHiring = !isHiring;

        hiringButton.SetActive(!isHiring);

        hiringPanel.SetActive(isHiring);

        blueprintsButton.SetActive(!isHiring);

        toolbelt.SetActive(!isHiring);

        GameManager.gm.ToggleOrbitCamera(isHiring);
        GameManager.gm.ToggleGameplayCamera(!isHiring);

        if (!isHiring)
        {
            
        }
    }

    void HireCrew (SO_CrewProfile _crew)
    {
        ChangeMoney(_crew.hireCost);
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

        // Checks if there are any tools
        if(toolButtonsList.Count > 0)
        {
            toolbeltGrid.SetActive(_isOpen);
            toolbeltArrow.SetActive(_isOpen);
        }      
    }
 
    public void ChangeToolImage(Sprite _sprite)
    {
        toolHeld.sprite = _sprite;
    }

    public void OnToolClick(SO_ToolData _toolData)
    {
        Debug.Log("Tool Item Clicked");

        sPlayerCharacter.playerCharacterGlobal.ReturnToolHandler().GoTool(_toolData);
    }

    void SetToolHeld(SO_ToolData _itemData)
    {
        isHoldingTool = true;
        toolHeld.sprite = _itemData.itemSprite;
        tooldHeldText.text = _itemData.itemName;
    }

    bool CheckIfInToolBelt(uButtonTool _tool)
    {
        bool isInBelt = false;

        for (int i = 0; i < toolButtonsList.Count; i++)
        {
            if(_tool == toolButtonsList[i])
            {
                isInBelt = true;
            }
        }

        return isInBelt;
    }

    public void AddToolToBelt(SO_ToolData _toolData)
    {
        // if no tool is held then it changes the held tool;
        if(!isHoldingTool)
        {
            
        }

        SetToolHeld(_toolData);

        // Spawns toolbelt button and adds it to list
        GameObject tempObject = Instantiate(toolbeltToolButton, toolbeltPanel);

        uButtonTool buttonTool;

        buttonTool = tempObject.GetComponent<uButtonTool>();

        buttonTool.SetButton(_toolData);

        toolButtonsList.Add(tempObject);

        //buttonTool.SetIndex(toolButtonsList.Count);

        Debug.Log("Setting Tool to index " + toolButtonsList.Count);
    }
}
