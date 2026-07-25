using UnityEngine;

public class sCrewJob
{
    public CrewJobType jobType;

    public Transform target;

    public bool completed;

    public bool assigned;

    public CrewJobState state;

    public sCrewJob(CrewJobType type, Transform targetObject)
    {
        jobType = type;
        target = targetObject;
        state = CrewJobState.Waiting;
        assigned = false;
        completed = false;
    }
}