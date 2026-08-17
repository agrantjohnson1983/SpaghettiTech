using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_DeptData",
    menuName = "ScriptableObjects/Gig/Department Data")]
public class SO_DeptData : ScriptableObject
{
    public eGigDept departmentID;
    public string departmentName;

    public List<SO_ObjectiveData> objectives = new();
}

[Serializable]
public class SO_ObjectiveData
{
    public string objectiveID;
    public string objectiveName;

    [TextArea]
    public string description;

    [Min(0)]
    public int totalItems;

    public float weight = 1f;
}