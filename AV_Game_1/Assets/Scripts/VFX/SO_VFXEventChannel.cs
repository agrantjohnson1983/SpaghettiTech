using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "AVSim/Events/VFX Event Channel")]
public class SO_VFXEventChannel : ScriptableObject
{
    public UnityAction<VFXType, Vector3, Quaternion> OnEventRaised;

    public void Raise(
        VFXType type,
        Vector3 position,
        Quaternion rotation)
    {
        OnEventRaised?.Invoke(type, position, rotation);
    }
}