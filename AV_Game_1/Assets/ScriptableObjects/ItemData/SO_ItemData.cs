using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_ItemData", menuName = "ScriptableObjects/ItemData")]
public class SO_ItemData : ScriptableObject
{
    public int index;

    public int numberOfHandsNeeded;

    public string itemName;

    public Sprite itemSprite;

    public GameObject prefabItem;
    // item type?



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
