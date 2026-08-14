using UnityEngine;
using UnityEngine.EventSystems;

public enum eBoltPartType
{
    Bolt,
    Nut
}

// Attach to any bolt or nut UI element that the player should be able to
// drag from a tray onto a sBoltHoleSlot. Requires a RectTransform and
// CanvasGroup (CanvasGroup lets us disable raycasts while dragging so the
// item does not block itself from detecting drop targets).
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class sDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public eBoltPartType partType;
    public float snapDistance = 60f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private Vector2 originalAnchoredPosition;
    private Transform originalParent;
    private bool isPlaced;

    public SO_AudioEventChannel soAudio;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced)
        {
            return;
        }

        originalAnchoredPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        // Reparent to canvas root so the item renders above trays and slots
        // while being dragged.
        transform.SetParent(parentCanvas.transform, true);
        transform.SetAsLastSibling();

        switch (partType)
        {
            case eBoltPartType.Bolt:

                if (soAudio != null)
                    soAudio.TriggerSFX("RigBoltSelect");

                break;

            case eBoltPartType.Nut:

                if (soAudio != null)
                    soAudio.TriggerSFX("RigNutSelect");

                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced)
        {
            return;
        }

        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;

        sBoltHoleSlot nearestSlot = FindNearestValidSlot();

        if (nearestSlot != null)
        {
            SnapToSlot(nearestSlot);
        }
        else
        {
            ReturnToStart();
        }
    }

    private sBoltHoleSlot FindNearestValidSlot()
    {
        // FindObjectsOfType is fine for a small, one-off minigame with only
        // a handful of slots. Swap for a cached list if this gets reused
        // with many slots at once.
        sBoltHoleSlot[] allSlots = FindObjectsOfType<sBoltHoleSlot>();
        sBoltHoleSlot best = null;
        float bestDist = snapDistance;

        foreach (sBoltHoleSlot slot in allSlots)
        {
            if (!slot.CanAccept(partType))
            {
                continue;
            }

            float dist = Vector2.Distance(rectTransform.position, slot.SnapPoint.position);

            if (dist <= bestDist)
            {
                bestDist = dist;
                best = slot;
            }
        }

        return best;
    }

    private void SnapToSlot(sBoltHoleSlot slot)
    {
        isPlaced = true;

        transform.SetParent(slot.SnapPoint, false);
        rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
        rectTransform.localPosition = Vector2.zero;

        slot.PlaceItem(partType, gameObject);

        switch(partType)
        {
            case eBoltPartType.Bolt:

                if (soAudio != null)
                    soAudio.TriggerSFX("RigBoltPlaced");

                break;

            case eBoltPartType.Nut:

                if (soAudio != null)
                    soAudio.TriggerSFX("RigNutPlaced");

                break;
        }
    }

    private void ReturnToStart()
    {
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}
