using System.Collections.Generic;
using UnityEngine;

public class sJobManager : MonoBehaviour
{
    public static sJobManager instance;

    public List<sCrewJob> availableJobs = new();

    public Transform testTarget;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        availableJobs.Add(
    new sCrewJob(
        CrewJobType.BuildTruss,
        testTarget.transform
    )
);
    }

    public void AddJob(sCrewJob job)
    {
        if (!availableJobs.Contains(job))
            availableJobs.Add(job);
    }

    public sCrewJob GetJob(CrewJobType type)
    {
        foreach (sCrewJob job in availableJobs)
        {
            if (job.jobType != type)
                continue;

            if (job.assigned)
                continue;

            if (job.completed)
                continue;

            job.assigned = true;

            return job;
        }

        return null;
    }

    public void RemoveJob(sCrewJob job)
    {
        availableJobs.Remove(job);
    }
}