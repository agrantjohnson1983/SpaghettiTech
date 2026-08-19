using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AVSim/VFX/VFX Database")]
public class SO_VFXDatabase : ScriptableObject
{
    public List<SO_VFXDefinition> effects = new List<SO_VFXDefinition>();

    private Dictionary<string, SO_VFXDefinition> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, SO_VFXDefinition>();

        foreach (SO_VFXDefinition effect in effects)
        {
            if (effect == null)
                continue;

            if (string.IsNullOrWhiteSpace(effect.vfxID))
            {
                Debug.LogWarning(
                    $"VFX Definition '{effect.name}' has no VFX ID.");

                continue;
            }

            if (lookup.ContainsKey(effect.vfxID))
            {
                Debug.LogWarning(
                    $"Duplicate VFX ID found: '{effect.vfxID}'");

                continue;
            }

            lookup.Add(effect.vfxID, effect);
        }
    }

    public SO_VFXDefinition Get(string vfxID)
    {
        if (lookup == null)
            Initialize();

        if (lookup.TryGetValue(vfxID, out SO_VFXDefinition definition))
            return definition;

        Debug.LogWarning($"Could not find VFX ID: '{vfxID}'");

        return null;
    }
}