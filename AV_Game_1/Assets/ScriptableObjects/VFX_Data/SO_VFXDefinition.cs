using UnityEngine;

[CreateAssetMenu(menuName = "AVSim/VFX/VFX Definition")]
public class SO_VFXDefinition : ScriptableObject
{
    [Header("Identification")]
    public string vfxID;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Lifetime")]
    public float lifetime = 2f;

    [Header("Transform")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 scale = Vector3.one;
}