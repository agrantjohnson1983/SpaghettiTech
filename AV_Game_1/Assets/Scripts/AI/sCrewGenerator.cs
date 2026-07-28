using System;
using UnityEngine;

public static class sCrewGenerator
{
    const int TotalStatPoints = 36;
    const int MinStat = 3;
    const int MaxStat = 9;

    static readonly string[] firstNames =
    {
        "Alex","Sam","Jordan","Casey","Taylor",
        "Morgan","Chris","Jamie","Drew","Cameron"
    };

    static readonly string[] lastNames =
    {
        "Johnson","Smith","Garcia","Lee","Brown",
        "Wilson","Walker","Young","Hall","Flores"
    };

    public static sCrewData GenerateCrew()
    {
        sCrewData crew = new sCrewData();

        crew.crewID = Guid.NewGuid().ToString();

        crew.crewName =
            firstNames[UnityEngine.Random.Range(0, firstNames.Length)]
            + " " +
            lastNames[UnityEngine.Random.Range(0, lastNames.Length)];

        GenerateBalancedStats(crew.stats);

        crew.currentEnergy = crew.stats.energy * 20f;

        return crew;
    }

    static void GenerateBalancedStats(sCrewStats stats)
    {
        int[] values =
        {
            MinStat,
            MinStat,
            MinStat,
            MinStat,
            MinStat,
            MinStat
        };

        int remaining =
            TotalStatPoints - (MinStat * values.Length);

        while (remaining > 0)
        {
            int index = UnityEngine.Random.Range(0, values.Length);

            if (values[index] >= MaxStat)
                continue;

            values[index]++;
            remaining--;
        }

        stats.strength = values[0];
        stats.speed = values[1];
        stats.attitude = values[2];
        stats.dexterity = values[3];
        stats.endurance = values[4];
        stats.energy = values[5];
    }
}