using System.Collections.Generic;
using UnityEngine;

public class sJobManager : MonoBehaviour
{
    public static sJobManager instance;

    public List<sCrewJob> availableJobs = new List<sCrewJob>();

    public Transform testRiggingTarget;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        AddJob(
            new sCrewJob(
                CrewJobType.BuildTruss,
                testRiggingTarget
            )
        );
    }


    public void AddJob(sCrewJob job)
    {
        availableJobs.Add(job);

        Debug.Log(
            "New job added: " + job.jobType
        );
    }


    public sCrewJob GetJob(CrewJobType type)
    {
        foreach (var job in availableJobs)
        {
            if (job.jobType == type &&
               !job.completed &&
               !job.assigned)
            {
                job.assigned = true;

                return job;
            }
        }

        return null;
    }
}