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

    // AUDIO EVENTS
    public SO_AudioEventChannel soAudio;

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

    // TOOLBELT STUFF
    public GameObject toolbelt;//, toolbeltGrid, toolbeltCloseArrow;

    public Image toolHeldImage;
    public Text tooldHeldText;

    //public Transform toolbeltPanel;
    //public GameObject pToolbeltToolButton;

    //public List<GameObject> toolButtonsList;

    // START SCREENa
    public GameObject startScreen, startButton;
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

        soUI.toolHeldImage.AddListener(SetToolHeld);

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

        soUI.toolHeldImage.RemoveListener(SetToolHeld);

        soUI.messageEvent.RemoveListener(MessageSend);
    }

    // Start is called before the first frame update
    void Start()
    {
        taskGauge.fillAmount = 0;

        popupDictionary = new Dictionary<string, GameObject>();

        //toolbeltGrid.SetActive(false);
        //toolbeltCloseArrow.SetActive(false);
        tooldHeldText.text = "";

        //toolHeldImage.gameObject.SetActive(false);

        //toolbelt.SetActive(false);

        // turns off message at start
        MessageSend("", 0f);

        GameManager.gm.SetFirstSelected(startButton);
    }

    // GETS CALLED WHEN PLAYER PRESSES START
    public void OnStart()
    {
        startScreen.SetActive(false);

        soUI.TriggerMessage("GAME START!", 3f);

        GameManager.gm.StartGameplay();
    }

    // MESSAGES

    private void MessageSend(string _message, float _time)
    {
        // turns object on
        textMessageMain.gameObject.SetActive(true);

        // turn alpha on
        textMessageMain.CrossFadeAlpha(1, 0f, false);

        // set message
        textMessageMain.text = _message;

        // slowly fade alpha out
        textMessageMain.CrossFadeAlpha(0, _time, false);

        //textMessageMain.CrossFadeColor()

        // turns off message in time
        Invoke("TurnOffMessage", _time + 0.5f);
    }

    void TurnOffMessage()
    {
        textMessageMain.gameObject.SetActive(false);
    }

    // POPUPS UI

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
/*
    // This gets called when a player clicks a item in the hands UI
    public void OnItemClick(int _index)
    {
        Debug.Log("Item image " + _index + " triggered");
    }*/

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

    // MOTORS

    void ToggleMotorController(bool _isOn)
    {
        motorController.SetActive(_isOn);
    }
    
    public void MotorControllerUp()
    {
        soUI.TriggerMotorControls(true);

        if (soAudio != null)
            soAudio.TriggerSFX("RigMotorControlPressed");
    }

    public void MotorControllerDown()
    {
        soUI.TriggerMotorControls(false); ;

        if (soAudio != null)
            soAudio.TriggerSFX("RigMotorControlPressed");
    }

    

    // HIRING AND CREW

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

        else
        {

        }
    }

    // TOOLBELT

    void SetToolHeld(SO_ToolData _itemData)
    {
        //toolHeldImage.gameObject.SetActive(true);

        toolHeldImage.sprite = _itemData.itemSprite;

        tooldHeldText.text = _itemData.itemName;
    }

    private void OnDestroy()
    {
        //Debug.Log($"{name} WAS DESTROYED");
    }
}
