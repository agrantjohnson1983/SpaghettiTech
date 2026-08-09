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

    private void Start()
    {
        sPlayerCharacter.playerCharacterGlobal.ToggleMovement(false);

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

        if (tightenedCount >= boltSlots.Count && !hasCompleted)
        {
            hasCompleted = true;
            OnAllBoltsSecured?.Invoke();
        }
    }
}
