using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public SO_EventsUI soUI;

    public GameObject pCanvasGameplay;
    public canvasGameplay canvasGameplay;

    public GameObject pCanvasWorldSpace;
    Canvas canvasWorldSpace;

    sPlayerCharacter currentPlayer;

    List<sPlayerCharacter> playerCharacters;

    int activePlayerIndex = 0;

    public GameObject cameraHiring, cameraBlueprint;

    public bool isDoingTut = false;
    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasGameplay = pCanvasGameplay.GetComponent<canvasGameplay>();

        canvasWorldSpace = pCanvasWorldSpace.GetComponent<Canvas>();
        canvasWorldSpace.worldCamera = Camera.main;
       

        playerCharacters = new List<sPlayerCharacter>();

        cameraBlueprint.SetActive(false);

        canvasGameplay.ToggleHireScreen();    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchActivePlayer(1);
        }
    }

    // This is used to change the index manually outside of the TAB button or arrow buttons - mostly when a player gets clicked
    public void SwitchActivePlayerIndex(int _index)
    {
        activePlayerIndex = _index;
    }

    public void SwitchActivePlayer(int _increase)
    {

        Debug.Log("Switching Player");

        //currentPlayer.ReturnGrabController().GrabReset();

        // turns off the current players controls
        playerCharacters[activePlayerIndex].CharacterControlsToggle(false);

        // increments or decrements the player index based on the argument given
        activePlayerIndex+=_increase;

        // if the index is too high then it resets at 0
        if(activePlayerIndex > playerCharacters.Count-1)
        {
            activePlayerIndex = 0;
        }

        // if the index is too low then it resets to the count minus 1 (matches array)
        if(activePlayerIndex < 0)
        {
            activePlayerIndex = playerCharacters.Count - 1;
        }

        // turns back the character controls with the correct character
        playerCharacters[activePlayerIndex].CharacterControlsToggle(true);

        // sets the camera to the new character
        canvasWorldSpace.worldCamera = Camera.main;

        // turns off any popups currently over a character
        soUI.ToggleControlsPopup(null);


        
    }

    /*
    public void ToggleActivePlayer(bool _isOn)
    {
        playerCharacters[activePlayerIndex].CharacterControlsToggle(_isOn);

        ToggleHiringCamera(!_isOn);
    }

    */


    public void ToggleHiringCamera(bool _isOn)
    {
        cameraHiring.SetActive(_isOn);

        if(playerCharacters != null)
        {
            //SetCurrentPlayer(playerCharacters[activePlayerIndex]);
            playerCharacters[activePlayerIndex].SetToCurrentPlayer();
            //playerCharacters[activePlayerIndex].CharacterControlsToggle(!_isOn);
        }
        
    }

    public void ToggleBlueprintCamera(bool _isOn)
    {
        cameraBlueprint.SetActive(_isOn);

        playerCharacters[activePlayerIndex].CharacterControlsToggle(!_isOn);
    }

    public void AddCharacterToList(sPlayerCharacter _player)
    {
        playerCharacters.Add(_player);
    }

    public List<sPlayerCharacter> ReturnPlayerList()
    {
        return playerCharacters;
    }

    public void SetCurrentPlayer(sPlayerCharacter _playerCharacter)
    {
        currentPlayer = _playerCharacter;
    }

    public sPlayerCharacter ReturnCurrentPlayer()
    {
        return currentPlayer;
    }

    public Canvas ReturnCanvasWorldSpace()
    {
        return canvasWorldSpace;
    }
}
