using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sHiringScreen : MonoBehaviour
{
    public Transform hiringScreenPanel;

    public GameObject characterHireButton;

    public int numberOfHiresOnScreen = 6;

    public SO_CharacterData[] characterData;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < numberOfHiresOnScreen; i++)
        {
            GameObject tempObj;
            sPlayerCharacter tempPlayer;

            tempObj = Instantiate(characterHireButton, hiringScreenPanel);

            //tempPlayer = tempObj.GetComponent<sPlayerCharacter>();
        }
    }

    public SO_CharacterData HireSelect()
    {
        int randomIndex;

        randomIndex = Random.Range(0, characterData.Length);

        return characterData[randomIndex];
    }
}
