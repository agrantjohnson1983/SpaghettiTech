using UnityEngine;

public class sVFXManager : MonoBehaviour
{
    [SerializeField]
    SO_VFXDatabase database;

    [SerializeField]
    SO_VFXEventChannel channel;

    void OnEnable()
    {
        database.Initialize();
        channel.OnEventRaised += Spawn;
    }

    void OnDisable()
    {
        channel.OnEventRaised -= Spawn;
    }

    void Spawn(
    VFXType type,
    Vector3 position,
    Quaternion rotation)
    {
        var definition = database.Get(type);

        if (definition == null)
        {
            Debug.LogWarning($"No VFX Definition for {type}");
            return;
        }

        GameObject effect = Instantiate(
            definition.prefab,
            position + definition.positionOffset,
            rotation * Quaternion.Euler(definition.rotationOffset));

        effect.transform.localScale = definition.scale;

        Destroy(effect, definition.lifetime);
    }
}