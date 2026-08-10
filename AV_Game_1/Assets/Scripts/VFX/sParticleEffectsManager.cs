using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sParticleEffectsManager : MonoBehaviour
{
    public static sParticleEffectsManager particleEffectsManager;

    [System.Serializable]
    public class ParticleEffect
    {
        public string effectName;
        public UnityEngine.GameObject particleSystemObject;

        [Header("Optional Settings")]
        //public bool resetOnTrigger = true;
        //public bool playAtPosition = false;
        public Vector3 customOffset = Vector3.zero;

        public bool stayOnParent = false;
    }

    [SerializeField]
    private ParticleEffect[] particleEffects;

    private Dictionary<string, ParticleEffect> particleMap;

    private void Awake()
    {
        // singleton
        particleEffectsManager = this;

        InitParticleMap();
    }

    void InitParticleMap()
    {
        particleMap = new Dictionary<string, ParticleEffect>();

        foreach(var effect in particleEffects)
        {
            if(!particleMap.ContainsKey(effect.effectName))
            {
                particleMap.Add(effect.effectName, effect);

                //Debug.Log("Adding in " + effect.effectName + " to particle map");
            }

            else
            {
                Debug.Log("Duplicate particle effect name");
            }
        }
    }

    public void TriggerParticle(string effectName, Transform transform)
    {
        //Debug.Log("Trigger Particle called");

        if(particleMap.TryGetValue(effectName, out ParticleEffect effect))
        {
            if(effect.particleSystemObject == null)
            {
                Debug.Log("Particle system is null");
                return;
            }

            //if(effect.resetOnTrigger)
            //{

            //}

            // Determine play pos
            //Vector3 playPosition = effect.playAtPosition
            //    ? position + effect.customOffset
            //    : transform.position + effect.customOffset;

            // Set system pos and play
            //effect.particleSystemObject.transform.position = playPosition;
            //effect.particleSystemObject.Play
            UnityEngine.GameObject tempParticleObj; 

            tempParticleObj = Instantiate(effect.particleSystemObject, transform);

            // adds in offset
            tempParticleObj.transform.position += effect.customOffset;

            if(effect.stayOnParent)
            {

            }

            // Removes from parent object
            else
            {
                tempParticleObj.gameObject.transform.parent = null;
            }
        }

        else
        {
            Debug.Log("Particle effect " + effectName + " not found");
        }
    }

    public void TriggerParticle(string effectName, Vector3 position)
    {
        //Debug.Log("Trigger Particle called");

        if (particleMap.TryGetValue(effectName, out ParticleEffect effect))
        {
            if (effect.particleSystemObject == null)
            {
                Debug.Log("Particle system is null");
                return;
            }

            //if(effect.resetOnTrigger)
            //{

            //}

            // Determine play pos
            //Vector3 playPosition = effect.playAtPosition
            //    ? position + effect.customOffset
            //    : transform.position + effect.customOffset;

            // Set system pos and play
            //effect.particleSystemObject.transform.position = playPosition;
            //effect.particleSystemObject.Play

            UnityEngine.GameObject tempParticleObj;

            tempParticleObj = Instantiate(effect.particleSystemObject, position + effect.customOffset, Quaternion.identity);

            if (effect.stayOnParent)
            {

            }

            // Removes from parent object
            else
            {
                tempParticleObj.gameObject.transform.parent = null;
            }
        }

        else
        {
            Debug.Log("Particle effect not found");
        }
    }
}
