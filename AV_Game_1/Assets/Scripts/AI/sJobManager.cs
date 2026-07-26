using System.Collections.Generic;
using UnityEngine;

public class sJobManager : MonoBehaviour
{
    public static sJobManager instance;

    List<IJobProvider> providers = new List<IJobProvider>();

    void Awake()
    {
        instance = this;
    }

    public void RegisterProvider(IJobProvider provider)
    {
        if (!providers.Contains(provider))
            providers.Add(provider);
    }

    public void UnregisterProvider(IJobProvider provider)
    {
        providers.Remove(provider);
    }

    public sCrewJob GetJob(CrewJobType type)
    {
        foreach (IJobProvider provider in providers)
        {
            foreach (sCrewJob job in provider.GetAvailableJobs())
            {
                if (!job.assigned &&
                    !job.completed &&
                    job.jobType == type)
                {
                    job.assigned = true;

                    provider.OnJobAccepted(job);

                    return job;
                }
            }
        }

        return null;
    }
}