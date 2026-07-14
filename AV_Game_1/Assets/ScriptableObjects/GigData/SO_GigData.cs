using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GigData_", menuName = "Gigs/Gig Data")]
public class SO_GigData : ScriptableObject
{
    public string gigName;

    public Sprite gigSprite;

    public string gigDescription;

    public List<SO_ItemData> itemsNeededForGigList;

    public string sceneNameToLoad;

    public float gigPay;
}
