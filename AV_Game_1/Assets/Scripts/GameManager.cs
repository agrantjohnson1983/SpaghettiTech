using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eGameMode { none, frontEnd, warehouse, gig }

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public SO_EventsUI soUI;

    public GameObject canvasGameplayObject;
    public canvasGameplay canvasGameplay;

    public GameObject canvasWorldSpaceObject;
    Canvas canvasWorldSpace;

    public GameObject canvasWarehouseObject;
    public CanvasWarehouse canvasWarehouse;

    sPlayerCharacter currentPlayer;

    List<sPlayerCharacter> playerCharacters;

    int activePlayerIndex = 0;

    public GameObject cameraOrbit, cameraBlueprint;

    public eGameMode startingGameMode;

    eGameMode currentGameMode;

    public bool isDoingTut = false;
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
            gm.canvasGameplayObject = this.canvasGameplayObject;
            gm.canvasWorldSpaceObject = this.canvasWorldSpaceObject;
            gm.canvasWarehouseObject = this.canvasWarehouseObject;

            gm.RefreshCanvasReferences();

            Destroy(this.gameObject);
            return;
        }

        SetGameMode(startingGameMode);

        playerCharacters = new List<sPlayerCharacter>();
    }

    // Re-caches the canvas component references from whatever GameObjects
    // canvasGameplayObject / canvasWorldSpaceObject currently point at.
    // Called from Start() the first time, and again from Awake() above
    // whenever a duplicate GameManager hands off a new scene's canvas refs.
    public void RefreshCanvasReferences()
    {
        canvasGameplay = canvasGameplayObject.GetComponent<canvasGameplay>();

        canvasWorldSpace = canvasWorldSpaceObject.GetComponent<Canvas>();
        canvasWorldSpace.worldCamera = Camera.main;

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
        switch (currentGameMode)
        {
            case eGameMode.gig:

                ToggleBlueprintCamera(true);

                break;

            case eGameMode.warehouse:

                break;

            case eGameMode.frontEnd:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Tab))
        //{
        //    SwitchActivePlayer(1);
        //}
    }
    public eGameMode GetGameMode()
    {
        return currentGameMode;
    }

    public void SetGameMode(eGameMode _gameMode)
    {
        currentGameMode = _gameMode;

        switch (currentGameMode)
        {
            case eGameMode.frontEnd:

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

                //cameraBlueprint.SetActive(true);
                //canvasGameplay.characterPanel.SetActive(true);
                //canvasGameplay.moneyUI.SetActive(true);
                //canvasGameplay.timeUI.SetActive(true);
                //canvasGameplay.toolbelt.SetActive(true);
                //canvasGameplay.blueprintsButton.SetActive(true);

                break;

        }
    }

    public void StartGig()
    {
        SetGameMode(eGameMode.gig);

        //ToggleOrbitCamera(true);

        SceneManager.LoadScene("TestLevel");
    }

    // This is used to change the index manually outside of the TAB button or arrow buttons - mostly when a player gets clicked
    //public void SwitchActivePlayerIndex(int _index)
    //{
    //    activePlayerIndex = _index;
    //}

    //public void SwitchActivePlayer(int _increase)
    //{

    //    Debug.Log("Switching Player");

    //    //currentPlayer.ReturnGrabController().GrabReset();

    //    // turns off the current players controls
    //    playerCharacters[activePlayerIndex].CharacterControlsToggle(false);

    //    // increments or decrements the player index based on the argument given
    //    activePlayerIndex+=_increase;

    //    // if the index is too high then it resets at 0
    //    if(activePlayerIndex > playerCharacters.Count-1)
    //    {
    //        activePlayerIndex = 0;
    //    }

    //    // if the index is too low then it resets to the count minus 1 (matches array)
    //    if(activePlayerIndex < 0)
    //    {
    //        activePlayerIndex = playerCharacters.Count - 1;
    //    }

    //    // turns back the character controls with the correct character
    //    playerCharacters[activePlayerIndex].CharacterControlsToggle(true);

    //    // sets the camera to the new character
    //    canvasWorldSpace.worldCamera = Camera.main;

    //    // turns off any popups currently over a character
    //    soUI.ToggleControlsPopup(null);



    //}

    /*
    public void ToggleActivePlayer(bool _isOn)
    {
        playerCharacters[activePlayerIndex].CharacterControlsToggle(_isOn);

        ToggleHiringCamera(!_isOn);
    }

    */


    public void ToggleOrbitCamera(bool _isOn)
    {
        cameraOrbit.SetActive(_isOn);

        sPlayerCharacter.playerCharacterGlobal.CharacterControlsToggle(!_isOn);

        //if(playerCharacters.Count > 0)
        //{
        //    //SetCurrentPlayer(playerCharacters[activePlayerIndex]);
        //    playerCharacters[activePlayerIndex].SetToCurrentPlayer();
        //    //playerCharacters[activePlayerIndex].CharacterControlsToggle(!_isOn);
        //}

        //else
        //{
        //    Debug.LogWarning("Player characters are null!");
        //}

    }

    public void ToggleBlueprintCamera(bool _isOn)
    {
        cameraBlueprint.SetActive(_isOn);

        sPlayerCharacter.playerCharacterGlobal.CharacterControlsToggle(!_isOn);

        //playerCharacters[activePlayerIndex].CharacterControlsToggle(!_isOn);
    }

    //public void AddCharacterToList(sPlayerCharacter _player)
    //{
    //    Debug.Log("Adding player character to GM list");
    //    playerCharacters.Add(_player);
    //}

    //public List<sPlayerCharacter> ReturnPlayerList()
    //{
    //    return playerCharacters;
    //}

    //public void SetCurrentPlayer(sPlayerCharacter _playerCharacter)
    //{
    //    currentPlayer = _playerCharacter;
    //}

    //public sPlayerCharacter ReturnCurrentPlayer()
    //{
    //    return sPlayerCharacter.playerCharacterGlobal;
    //}

    //public Canvas ReturnCanvasWorldSpace()
    //{
    //    return canvasWorldSpace;
    //}

    //public void KillPlayers()
    //{
    //    for (int i = 0; i < playerCharacters.Count; i++)
    //    {
    //        Destroy(playerCharacters[i]);
    //    }
    //}
}