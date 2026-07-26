using UnityEngine;
using UnityEngine.Events;

public class SO_JobEventChannel : ScriptableObject
{
    public UnityAction<sCrewJob> OnEventRaised;

    public void RaiseEvent(sCrewJob job)
    {
        OnEventRaised?.Invoke(job);
    }
}