using System;
using UnityEngine;
using UnityEngine.UI;

// One of these lives on each of the 4 bolt holes on the truss. Tracks
// whether a bolt and nut are present, and - now that tightening is
// done by a single shared wrench that can be dragged between holes -
// tracks each hole's own tightening progress directly, so it survives
// the wrench being detached and reattached, or the player leaving and
// coming back later.
public class sBoltHoleSlot : MonoBehaviour
{
    public int slotIndex;

    // Empty child transform positioned exactly on the hole. Draggable
    // items (and the shared wrench) snap here.
    public Transform SnapPoint;

    public bool HasBolt { get; private set; }
    public bool HasNut { get; private set; }
    public bool IsTightened { get; private set; }

    [Header("Tightening Progress")]
    public int clicksToTighten = 3;
    public float wrenchAnglePerClick = -40f;
    public float initialWrenchAngleDegrees = 0f;
    public float nutRotationPerClick = 60f;

    public int CurrentClicks { get; private set; }
    public float CurrentWrenchAngle { get; private set; }

    private Material mat;
    MaterialPropertyBlock block;

    // True only when this hole is actually ready for the shared
    // wrench to attach to - both parts present, not already done.
    public bool IsReadyForWrench
    {
        get
        {
            return HasBolt && HasNut && !IsTightened;
        }
    }

    public event Action<sBoltHoleSlot> OnSlotTightened;

    GameObject placedBolt;
    GameObject placedNut;

    void Awake()
    {
        CurrentWrenchAngle = initialWrenchAngleDegrees;

        mat = new Material(GetComponent<Image>().material);

        GetComponent<Image>().material = mat;

        block = new MaterialPropertyBlock();
    }

    public bool CanAccept(eBoltPartType partType)
    {
        if (partType == eBoltPartType.Bolt)
        {
            return !HasBolt;
        }

        if (partType == eBoltPartType.Nut)
        {
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

            // No longer auto-spawns a wrench here - the single shared
            // wrench (sSharedWrench) is what the player drags onto
            // this slot once it is ready.
        }
    }

    // Called by sSharedWrench each time it registers a successful
    // swing while attached to this slot.
    public void RegisterRatchetClick()
    {
        if (IsTightened)
        {
            return;
        }

        CurrentClicks++;

        //MaterialPropertyBlock propertyBlock -new MaterialPropertyBlock();

        //GetComponent<Image>().material.Get

        mat.SetFloat("_Progress", CurrentClicks / clicksToTighten);

        if (placedNut != null)
        {
            placedNut.transform.Rotate(0f, 0f, -nutRotationPerClick);
        }

        CurrentWrenchAngle += wrenchAnglePerClick;

        if (CurrentClicks >= clicksToTighten)
        {
            IsTightened = true;
            OnSlotTightened?.Invoke(this);
        }

        
    }
}