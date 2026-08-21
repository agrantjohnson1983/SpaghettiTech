using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Drop this on a root object in the minigame scene/canvas and assign the
// 4 sBoltHoleSlot references in the inspector. Fires OnAllBoltsSecured
// once every slot reports itself tightened.
public class sTrussBoltGameManager : MonoBehaviour
{
    public List<sBoltHoleSlot> boltSlots = new List<sBoltHoleSlot>();
    public UnityEvent OnAllBoltsSecured;

    private int tightenedCount;
    private bool hasCompleted;

    public SO_AudioEventChannel soAudio;

    private void Start()
    {
        //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);

        GameManager.gm.canvasGameplayObject.SetActive(false);

        foreach (sBoltHoleSlot slot in boltSlots)
        {
            slot.OnSlotTightened += HandleSlotTightened;
        }
    }

    private void OnDestroy()
    {
        foreach (sBoltHoleSlot slot in boltSlots)
        {
            if (slot != null)
            {
                slot.OnSlotTightened -= HandleSlotTightened;
            }
        }
    }

    private void HandleSlotTightened(sBoltHoleSlot slot)
    {
        tightenedCount++;

        if (soAudio != null)
            soAudio.TriggerSFX("RigRatchetComplete");

        if (tightenedCount >= boltSlots.Count && !hasCompleted)
        {
            GameManager.gm.canvasGameplayObject.SetActive(true);
            //sPlayerCharacter.playerCharacterGlobal.ToggleMovement(true);
            hasCompleted = true;
            OnAllBoltsSecured?.Invoke();
        }
    }
}
