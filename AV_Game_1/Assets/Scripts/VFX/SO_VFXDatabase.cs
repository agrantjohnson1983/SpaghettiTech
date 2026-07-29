using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AVSim/VFX/VFX Database")]
public class SO_VFXDatabase : ScriptableObject
{
    public List<SO_VFXDefinition> effects;

    Dictionary<VFXType, SO_VFXDefinition> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<VFXType, SO_VFXDefinition>();

        foreach (var effect in effects)
        {
            if (!lookup.ContainsKey(effect.type))
                lookup.Add(effect.type, effect);
        }
    }

    public SO_VFXDefinition Get(VFXType type)
    {
        if (lookup.TryGetValue(type, out var definition))
            return definition;

        return null;
    }
}