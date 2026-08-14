using UnityEngine;
using UnityEngine.EventSystems;

// The single physical wrench. Only one exists, so only one bolt hole
// can be actively worked on at a time - matches the tool-belt design
// where the player carries one wrench, not one per hole.
//
// Drag it (like a bolt or nut) onto any slot where IsReadyForWrench is
// true to snap it into place there. While attached, the same drag
// gesture is instead interpreted as a ratchet swing - a swing past
// swingThreshold that reverses direction (or is released past
// threshold) registers one click on the attached slot's own progress.
// Call DetachAndReturnHome() (wire to a UI button) to explicitly let
// go of the current hole and free the wrench up for another one.
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class sSharedWrench : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float snapDistance = 60f;
    public float swingThreshold = 25f;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Canvas parentCanvas;

    Vector2 homeAnchoredPosition;
    Transform homeParent;

    sBoltHoleSlot attachedSlot;
    bool isAttached;

    Vector2 dragStartScreenPos;
    float accumulatedSwing;
    int swingDirection;

    public SO_AudioEventChannel soAudio;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();

        homeAnchoredPosition = rectTransform.anchoredPosition;
        homeParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAttached)
        {
            // Starting a turn attempt, not a move - just record where
            // the swing begins.
            dragStartScreenPos = eventData.position;
            accumulatedSwing = 0f;
            swingDirection = 0;
            return;
        }

        canvasGroup.blocksRaycasts = false;
        transform.SetParent(parentCanvas.transform, true);
        transform.SetAsLastSibling();

        if(soAudio != null)
        {
            soAudio.TriggerSFX("RigRatchetSelect");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAttached)
        {
            UpdateTurningDrag(eventData);
            return;
        }

        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAttached)
        {
            if (Mathf.Abs(accumulatedSwing) >= swingThreshold)
            {
                RegisterTurn();
            }

            return;
        }

        canvasGroup.blocksRaycasts = true;

        sBoltHoleSlot nearestSlot = FindNearestReadySlot();

        if (nearestSlot != null)
        {
            AttachToSlot(nearestSlot);
        }
        else
        {
            ReturnHome();
        }
    }

    // Explicit detach, meant for a UI button - avoids trying to guess
    // "is this drag a turn or an attempt to pick it up and move it"
    // from gesture shape alone, which is ambiguous.
    public void DetachAndReturnHome()
    {
        if (!isAttached)
        {
            return;
        }

        attachedSlot = null;
        isAttached = false;

        ReturnHome();
    }

    sBoltHoleSlot FindNearestReadySlot()
    {
        sBoltHoleSlot[] allSlots = FindObjectsOfType<sBoltHoleSlot>();
        sBoltHoleSlot best = null;
        float bestDist = snapDistance;

        foreach (sBoltHoleSlot slot in allSlots)
        {
            if (!slot.IsReadyForWrench)
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

    void AttachToSlot(sBoltHoleSlot slot)
    {
        attachedSlot = slot;
        isAttached = true;

        transform.SetParent(slot.SnapPoint, false);
        transform.SetAsLastSibling();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, slot.CurrentWrenchAngle);

        canvasGroup.blocksRaycasts = true;
    }

    void ReturnHome()
    {
        transform.SetParent(homeParent, true);
        rectTransform.anchoredPosition = homeAnchoredPosition;
        rectTransform.localRotation = Quaternion.identity;
    }

    void UpdateTurningDrag(PointerEventData eventData)
    {
        if (attachedSlot == null || attachedSlot.IsTightened)
        {
            return;
        }

        Vector2 delta = eventData.position - dragStartScreenPos;

        float rad = attachedSlot.CurrentWrenchAngle * Mathf.Deg2Rad;
        Vector2 handleDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        float projected = Vector2.Dot(delta, handleDir);
        int direction = projected > 0f ? 1 : -1;

        if (swingDirection == 0)
        {
            swingDirection = direction;
        }

        if (direction != swingDirection)
        {
            if (Mathf.Abs(accumulatedSwing) >= swingThreshold)
            {
                RegisterTurn();
            }

            swingDirection = direction;
            dragStartScreenPos = eventData.position;
            accumulatedSwing = 0f;
        }
        else
        {
            accumulatedSwing = projected;
        }
    }

    void RegisterTurn()
    {
        attachedSlot.RegisterRatchetClick();

        if (soAudio != null)
            soAudio.TriggerSFX("RigRatchetTurn");

        rectTransform.localRotation = Quaternion.Euler(0f, 0f, attachedSlot.CurrentWrenchAngle);

        accumulatedSwing = 0f;
        swingDirection = 0;
        dragStartScreenPos = Vector2.zero;

        if (attachedSlot.IsTightened)
        {
            if (uFasteners.instance != null)
            {
                uFasteners.instance.RemoveBolt();
                uFasteners.instance.RemoveNut();
            }

            DetachAndReturnHome();
        }
    }
}