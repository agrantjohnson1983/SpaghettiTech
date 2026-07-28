using UnityEngine;

public class sJobCompletedListener : MonoBehaviour
{
    public SO_JobCompletedEventChannel channel;

    private void OnEnable()
    {
        channel.OnEventRaised += JobCompleted;
    }

    private void OnDisable()
    {
        channel.OnEventRaised -= JobCompleted;
    }

    private void JobCompleted(JobEventData data)
    {
        Debug.Log(
            $"EVENT: {data.crewMember.name} completed {data.job.jobType}"
        );
    }
}