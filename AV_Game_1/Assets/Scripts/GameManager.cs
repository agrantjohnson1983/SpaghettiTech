using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum eGameMode { none, frontEnd, warehouse, gig, travel }

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public SO_EventsUI soUI;

    public SO_AudioEventChannel soAudio;

    public GameObject canvasGameplayObject;
    public canvasGameplay canvasGameplay;

    //public GameObject canvasWorldSpaceObject;
    //Canvas canvasWorldSpace;

    public GameObject canvasWarehouseObject;
    public CanvasWarehouse canvasWarehouse;

    sPlayerCharacter playerGlobal;

    List<sPlayerCharacter> playerCharacters;

    //int activePlayerIndex = 0;

    [SerializeField]private GameObject camOrbit, camOverhead, camGameplay;

    public eGameMode startingGameMode;

    eGameMode currentGameMode = eGameMode.none;

    public sCountdownSequence sequencer;

    //public bool isDoingTut = false;

    public EventSystem eventSystem;

    public GameObject Results;

    uMoney money;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;

            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            // This is a duplicate GameManager placed in a newly loaded scene
            // (e.g. for standalone scene testing). The canvas objects should
            // NOT be forced to persist across scenes - each scene has its
            // own local canvas, which is normal (a world-space canvas is
            // usually tied to that scene's own camera/layout anyway).
            // Instead, hand this scene's canvas references over to the
            // surviving persisted GameManager before this duplicate is
            // destroyed, so gm always ends up pointing at the CURRENT
            // scene's live canvas rather than a stale reference to
            // whatever scene it first spawned in.
            //gm.canvasGameplayObject = this.canvasGameplayObject;
            //gm.canvasWorldSpaceObject = this.canvasWorldSpaceObject;
            //gm.canvasWarehouseObject = this.canvasWarehouseObject;

            //gm.RefreshCanvasReferences();

            Destroy(this.gameObject);
            return;
        }

        playerCharacters = new List<sPlayerCharacter>();

        eventSystem = GetComponentInChildren<EventSystem>();

        money = GetComponentInChildren<uMoney>();

        SetGameMode(startingGameMode);
    }

    // Re-caches the canvas component references from whatever GameObjects
    // canvasGameplayObject / canvasWorldSpaceObject currently point at.
    // Called from Start() the first time, and again from Awake() above
    // whenever a duplicate GameManager hands off a new scene's canvas refs.
    public void RefreshCanvasReferences()
    {
        canvasGameplay = canvasGameplayObject.GetComponent<canvasGameplay>();

        //canvasWorldSpace = canvasWorldSpaceObject.GetComponent<Canvas>();
        //canvasWorldSpace.worldCamera = Camera.main;

        if (canvasWarehouseObject != null)
        {
            canvasWarehouse = canvasWarehouseObject.GetComponent<CanvasWarehouse>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Safety net matching the Awake() guard - if this somehow isn't the
        // surviving global instance, don't run any of the setup below
        if (gm != this)
        {
            return;
        }

        RefreshCanvasReferences();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene _scene, LoadSceneMode _loadMode)
    {
        if (currentGameMode == eGameMode.none)
            currentGameMode = startingGameMode;

        ToggleGameplayCamera(false);

        //Debug.Log("On scene load called for mode: " + currentGameMode);

        switch (currentGameMode)
        {
            case eGameMode.gig:

                //TogglePlayerControls(false);
                ToggleOverheadCamera(true);  
                ToggleOrbitCamera(false);

                soAudio.TriggerMUSIC("GameplayLoop");

                break;

            case eGameMode.warehouse:

                //TogglePlayerControls(false);
                ToggleOrbitCamera(true);
                ToggleOverheadCamera(false);

                soAudio.TriggerMUSIC("WarehouseLoop");

                break;

            case eGameMode.frontEnd:

                soAudio.TriggerMUSIC("FrontEndLoop");

                break;

            case eGameMode.travel:

                soAudio.TriggerMUSIC("TravelLoop");

                //Destroy(this.gameObject);



                break;
        }
    }

    public eGameMode GetGameMode()
    {
        return currentGameMode;
    }

    public void SetGameMode(eGameMode _gameMode)
    {
        //Debug.Log("Setting game mode to " + _gameMode);

        currentGameMode = _gameMode;

        camGameplay.SetActive(false);

        switch (currentGameMode)
        {
            case eGameMode.frontEnd:

                canvasGameplayObject.SetActive(false);
                canvasWarehouseObject.SetActive(false);

                break;

            case eGameMode.warehouse:

                canvasGameplayObject.SetActive(false);
                canvasWarehouseObject.SetActive(true);

                //cameraBlueprint.SetActive(false);
                //canvasGameplay.characterPanel.SetActive(false);
                //canvasGameplay.moneyUI.SetActive(false);
                //canvasGameplay.timeUI.SetActive(false);
                //canvasGameplay.toolbelt.SetActive(false);
                //canvasGameplay.blueprintsButton.SetActive(false);

                break;

            case eGameMode.gig:

                canvasGameplayObject.SetActive(true);
                canvasWarehouseObject.SetActive(false);
                //uMoney.moneyGlobal.ToggleUI(false);

                //ToggleOverheadCamera(true);

                //cameraBlueprint.SetActive(true);
                //canvasGameplay.characterPanel.SetActive(true);
                //canvasGameplay.moneyUI.SetActive(true);
                //canvasGameplay.timeUI.SetActive(true);
                //canvasGameplay.toolbelt.SetActive(true);
                //canvasGameplay.blueprintsButton.SetActive(true);

                break;

            case eGameMode.travel:

                canvasWarehouseObject.SetActive(false);

                break;

        }
    }

    public void StartTravel()
    {
        SetGameMode(eGameMode.travel);

        SceneManager.LoadScene("Travel");
    }

    public void ArriveAtGig()
    {
        SetGameMode(eGameMode.gig);

        ToggleOverheadCamera(true);
    }

    public void StartGameplay()
    {
        //TogglePlayerCharacter(true);

        ToggleOrbitCamera(false);

        ToggleOverheadCamera(false);

        ToggleGameplayCamera(true);

        if (sTruck.truckGlobal != null && playerGlobal != null)
            playerGlobal.transform.position = sTruck.truckGlobal.truckDriverTransform.position;
        else
            Debug.LogWarning("Truck or player global was null");

        TogglePlayerControls(true);

        if (sequencer != null && currentGameMode == eGameMode.gig)
            sequencer.StartSequence();
        //camGameplay.SetActive(true);
    }

    public void EndLevel()
    {
        TogglePlayerControls(false);

        ToggleOrbitCamera(true);

        soUI.TriggerMessage("Gig Over!", 2f);

        Invoke("BackToWarehouse", 5f);
    }

    void BackToWarehouse()
    {
        SetGameMode(eGameMode.warehouse);

        SceneManager.LoadScene("WarehouseHub");

        SetResults();

        //Invoke("SetResults", 4f);
    }

    void SetResults()
    {
        Results.SetActive(true);
    }

    public void ToggleOrbitCamera(bool _isOn)
    {
        camOrbit.SetActive(_isOn);
    }

    public void ToggleOverheadCamera(bool _isOn)
    {
        //Debug.Log("Setting overhead cam to: " + _isOn);
        camOverhead.SetActive(_isOn);
    }

    public void ToggleGameplayCamera(bool _isOn)
    {
        camGameplay.SetActive(_isOn);
    }

    void TogglePlayerControls(bool _isOn)
    {
        //playerGlobal.CharacterControlsToggle(_isOn);
    }

    void TogglePlayerCharacter(bool _isOn)
    {
        //playerGlobal.ToggleModelVisibility(_isOn);
    }

    public void SetPlayerGlobal(sPlayerCharacter _player)
    {
        //Debug.Log("Setting player global");
        playerGlobal = _player;
    }

    public GameObject ReturnCameraGameplay()
    {
        return camGameplay;
    }

    public void SetFirstSelected(GameObject _button)
    {
        Debug.Log("Setting first selected with: " + _button);
        eventSystem.firstSelectedGameObject = _button;
    }

    private void OnDestroy()
    {
        //Debug.Log($"{name} WAS DESTROYED");
    }
}