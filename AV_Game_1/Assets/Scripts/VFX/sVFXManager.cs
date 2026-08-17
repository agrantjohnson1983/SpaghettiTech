using UnityEngine;

public class sVFXManager : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private SO_VFXDatabase database;

    [Header("Events")]
    [SerializeField] private SO_VFXEventChannel channel;


    private void OnEnable()
    {
        if (database != null)
            database.Initialize();

        if (channel != null)
            channel.OnEventRaised += Spawn;
    }

    private void OnDisable()
    {
        if (channel != null)
            channel.OnEventRaised -= Spawn;
    }


    private void Spawn(
        string vfxID,
        Vector3 position,
        Quaternion rotation)
    {
        if (database == null)
        {
            Debug.LogWarning("VFX Manager has no VFX Database assigned.");
            return;
        }

        SO_VFXDefinition definition = database.Get(vfxID);

        if (definition == null)
            return;

        if (definition.prefab == null)
        {
            Debug.LogWarning(
                $"VFX '{vfxID}' has no prefab assigned.");

            return;
        }

        Debug.Log("Spwaning vfx: " + vfxID);

        GameObject effect = Instantiate(
            definition.prefab,
            position + definition.positionOffset,
            rotation * Quaternion.Euler(definition.rotationOffset));

        effect.transform.localScale = definition.scale;

        Destroy(effect, definition.lifetime);
    }
}