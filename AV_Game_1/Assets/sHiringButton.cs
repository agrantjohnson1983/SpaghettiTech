using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class sHiringButton : MonoBehaviour
{
    sHiringScreen hiringScreen;

    public Image characterImage;
    public Text characterNameText;
    public Text characterStats;
    public Text costText;
    public float costToHire;

    //GameObject pCharacter;

    SO_CharacterData characterData;

    public SO_EventsUI soUI;

    // Start is called before the first frame update
    void Start()
    {
        hiringScreen = GetComponentInParent<sHiringScreen>();

        characterData = hiringScreen.HireSelect();

        SetCharacter(characterData);

        //SetCharacter(characterData.characterSprite, characterData.characterName, characterData.characterStats, characterData.costToHire);

        //pCharacter = characterData.pCharacter;
    }

    public void SetCharacter(SO_CharacterData _data)
    {
        characterImage.sprite = _data.characterSprite;

        characterNameText.text = _data.characterName;

        characterStats.text = _data.characterStats;

        costText.text = "$"+_data.costToHire.ToString();

        costToHire = _data.costToHire;
    }

    public void OnClick()
    {
        //Debug.Log("Character Button Clicked - spawning character");

        GameObject tempObj;
        sPlayerCharacter tempPlayer;

        tempObj = Instantiate(characterData.pCharacter);

        soUI.TriggerCharacterHire(-costToHire);
        //tempPlayer = tempObj.GetComponent<sPlayerCharacter>();

        //tempPlayer.ToggleCameraMain(false);
        //tempPlayer.CharacterSwitch(false);

        Destroy(this.gameObject);
    }
}
