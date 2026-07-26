using System.Collections.Generic;

public interface IJobProvider
{
    List<sCrewJob> GetAvailableJobs();

    void OnJobAccepted(sCrewJob job);

    void OnJobCompleted(sCrewJob job);
}