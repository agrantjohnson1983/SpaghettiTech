using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class sHiringButton : MonoBehaviour
{
    sHiringScreen hiringScreen;

    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterStats;
    public TextMeshProUGUI costText;
    public float costToHire;

    //GameObject pCharacter;

    //SO_CharacterData characterData;

    public SO_CrewProfile crewProfile;

    public SO_EventsUI soUI;

    public Transform spawnTransform;

    // Start is called before the first frame update
    void Start()
    {
        hiringScreen = GetComponentInParent<sHiringScreen>();

        //characterData = hiringScreen.HireSelect();

        SetCharacter(crewProfile);

        //SetCharacter(characterData.characterSprite, characterData.characterName, characterData.characterStats, characterData.costToHire);

        //pCharacter = characterData.pCharacter;
    }
/*
    public void SetCharacter(SO_CharacterData _data)
    {
        characterImage.sprite = _data.characterSprite;

        characterNameText.text = _data.characterName;

        characterStats.text = _data.characterStats;

        costText.text = "$"+_data.costToHire.ToString();

        costToHire = _data.costToHire;
    }*/

    void SetCharacter(SO_CrewProfile _crew)
    {
        crewProfile = _crew;

        //characterImage.sprite = _crew.characterSprite;

        characterNameText.text = _crew.crewName;

        //characterStats.text = _data.characterStats;

        costText.text = "$" + _crew.hireCost.ToString();

        costToHire = _crew.hireCost;
    }

    public void OnClick()
    {
        //Debug.Log("Character Button Clicked - spawning character");

        GameObject tempObj;
        //sPlayerCharacter tempPlayer;

        //tempObj = Instantiate(characterData.pCharacter);

        tempObj = Instantiate(crewProfile.pCrew, (spawnTransform.transform.position + Random.insideUnitSphere + Vector3.up), Quaternion.identity);

        //tempObj.transform.position = new Vector3(tempObj.transform.position.x, 1f, transform.position.z);

        if (GameManager.gm.GetGameMode() == eGameMode.warehouse)
            tempObj.SetActive(false);

        // TO - DO
        // - CrewMember objects get stored in CrewManager
        // - CrewManager turns on objects at gig scene start and sets the agents to move to a "crew meeting location"

        sGigManager.gigManagerGlobal.AddWorkerToGig(tempObj);

        soUI.TriggerCrewHire(crewProfile);

        Destroy(this.gameObject);
    }
}
