using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_JobCompletedEventChannel",
    menuName = "AVSim/Event Channels/Job Completed"
)]
public class SO_JobCompletedEventChannel : ScriptableObject
{
    public event Action<JobEventData> OnEventRaised;

    public void Raise(JobEventData data)
    {
        OnEventRaised?.Invoke(data);
    }
}