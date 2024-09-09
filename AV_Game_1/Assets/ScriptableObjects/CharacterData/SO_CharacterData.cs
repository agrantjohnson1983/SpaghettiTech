using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTypeOfCharacter { stagehand, rigger, audioOp, videoOp, lightingOp, carpenter}

[CreateAssetMenuAttribute(fileName = "SO_CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class SO_CharacterData : ScriptableObject
{
    public GameObject pCharacter;

    public string characterName;

    public string characterStats;

    public int costToHire;

    public Sprite characterSprite;

    public Sprite characterSymbolSprite;

    public eTypeOfCharacter characterType;

    public SO_ItemData[] startingItems;
    //public SO_InteractiveItemData
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
