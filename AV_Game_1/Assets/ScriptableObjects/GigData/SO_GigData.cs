using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GigData_", menuName = "Gigs/Gig Data")]
public class SO_GigData : ScriptableObject
{
    [Header("Identity")]
    public string gigID;
    public string gigName;
    public Sprite gigSprite;

    [TextArea]
    public string gigDescription;

    [Header("Scene")]
    public string sceneNameToLoad;

    [Header("Economy")]
    public float basePay;
    public int difficulty = 1;

    [Header("Requirements")]
    public int minimumCrew = 1;
    public List<SO_ItemData> requiredItems = new();

    [Header("Phases")]
    public List<GigPhaseData> phases = new();
}

[System.Serializable]
public class GigPhaseData
{
    public string phaseName;

    [TextArea]
    public string description;

    public float duration = 300f;

    public List<GigTaskData> tasks = new();
}

[System.Serializable]
public class GigTaskData
{
    public string taskID;

    public string displayName;

    [TextArea]
    public string description;

    public bool required = true;

    public int scoreValue = 100;
}