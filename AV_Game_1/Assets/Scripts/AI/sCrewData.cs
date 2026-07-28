using System;

[Serializable]
public class sCrewData
{
    public string crewID;

    public string crewName = "New Stagehand";

    public sCrewStats stats = new();

    // Runtime values
    public float currentEnergy = 100f;
}