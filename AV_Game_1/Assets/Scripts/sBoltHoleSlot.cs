using System;
using UnityEngine;
using UnityEngine.UI;

// One of these lives on each of the 4 bolt holes on the truss. Tracks
// whether a bolt is present, whether a nut is present, and whether the
// nut has been fully ratcheted down.
public class sBoltHoleSlot : MonoBehaviour
{
    public int slotIndex;

    // Empty child transform positioned exactly on the hole. Draggable
    // items snap here when dropped.
    public Transform SnapPoint;

    public Image background;

    public bool HasBolt { get; private set; }
    public bool HasNut { get; private set; }
    public bool IsTightened { get; private set; }

    public event Action<sBoltHoleSlot> OnSlotTightened;

    private GameObject placedBolt;
    private GameObject placedNut;

    public bool CanAccept(eBoltPartType partType)
    {
        if (partType == eBoltPartType.Bolt)
        {
            return !HasBolt;
        }

        if (partType == eBoltPartType.Nut)
        {
            // Nut can only go on once the bolt is already seated.
            return HasBolt && !HasNut;
        }

        return false;
    }

    public void PlaceItem(eBoltPartType partType, GameObject item)
    {
        if (partType == eBoltPartType.Bolt)
        {
            placedBolt = item;
            HasBolt = true;
        }
        else if (partType == eBoltPartType.Nut)
        {
            placedNut = item;
            HasNut = true;

            // The nut becomes ratchetable the moment it is placed.
            sNutRatchetDrag ratchet = item.GetComponent<sNutRatchetDrag>();

            if (ratchet == null)
            {
                ratchet = item.AddComponent<sNutRatchetDrag>();
            }

            ratchet.Initialize(this);
        }
    }

    public void NotifyTightened()
    {
        IsTightened = true;
        background.color = Color.green;
        OnSlotTightened?.Invoke(this);
    }
}
