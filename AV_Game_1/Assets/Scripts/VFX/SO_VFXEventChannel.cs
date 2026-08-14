using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "AVSim/Events/VFX Event Channel")]
public class SO_VFXEventChannel : ScriptableObject
{
    public UnityAction<string, Vector3, Quaternion> OnEventRaised;

    public void Raise(
        string vfxID,
        Vector3 position,
        Quaternion rotation)
    {
        OnEventRaised?.Invoke(
            vfxID,
            position,
            rotation);
    }
}