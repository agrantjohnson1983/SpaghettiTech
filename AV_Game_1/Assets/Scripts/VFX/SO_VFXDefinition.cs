using UnityEngine;

public enum VFXType
{
    None,

    // Interaction
    Hover,
    Pickup,
    Place,

    // Construction
    BoltTighten,
    BoltComplete,
    TrussLock,

    // Electrical
    PowerOn,
    PowerOff,
    ElectricalSpark,
    CableConnect,
    CableDisconnect,

    // Audio
    AudioPulse,
    SpeakerBass,

    // Environment
    DustPuff,
    Footstep,

    // UI
    Success,
    Failure
}

[CreateAssetMenu(menuName = "AVSim/VFX/VFX Definition")]
public class SO_VFXDefinition : ScriptableObject
{
    public VFXType type;

    public GameObject prefab;

    public bool pool = true;

    public float lifetime = 2f;

    public Vector3 positionOffset;

    public Vector3 rotationOffset;

    public Vector3 scale = Vector3.one;
}