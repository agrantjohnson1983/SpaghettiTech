using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject pCanvasGameplay;
    public canvasGameplay canvasGameplay;

    public GameObject pCanvasWorldSpace;
    Canvas canvasWorldSpace;

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

    public void SwitchActivePlayer(int _increase)
    {
        Debug.Log("Switching Player");

        playerCharacters[activePlayerIndex].CharacterControlsToggle(false);

        activePlayerIndex+=_increase;

        if(activePlayerIndex > playerCharacters.Count-1)
        {
            activePlayerIndex = 0;
        }

        if(activePlayerIndex < 0)
        {
            activePlayerIndex = playerCharacters.Count - 1;
        }

        playerCharacters[activePlayerIndex].CharacterControlsToggle(true);

        canvasWorldSpace.worldCamera = Camera.main;

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
        playerCharacters[activePlayerIndex].CharacterControlsToggle(!_isOn);
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
}
