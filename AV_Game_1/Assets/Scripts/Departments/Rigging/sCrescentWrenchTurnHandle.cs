using UnityEngine;
using UnityEngine.EventSystems;

// Handles the turning phase of the crescent wrench minigame. Unlike a
// ratchet, a crescent wrench cannot swing back and forth to accumulate
// turns - each turn requires a full grab, swing, and release/reset
// cycle. A single press-drag-release that sweeps far enough - in
// EITHER direction, since there is no fixed "correct" swing direction
// here - counts as one turn; releasing too early registers nothing and
// the player has to press down again to retry. This is NOT a single
// continuous rotation - do a short swing, fully release, then press
// again for each subsequent turn (turnsToTighten times total).
public class sCrescentWrenchTurnHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int turnsToTighten = 4;
    public float degreesPerTurn = 45f;

    RectTransform rectTransform;
    Vector2 pivotScreenPos;
    float dragStartAngle;
    bool isDragging;
    int currentTurns;
    bool isComplete;

    // Tracks the largest swing reached during the current grab
    // (magnitude, either direction), not just whatever angle happens
    // to be showing at the exact moment of release - so a good swing
    // still counts even if the hand relaxes back toward center right
    // before letting go.
    float peakAbsoluteSweptAngle;

    public event System.Action OnTurnRegistered;
    public event System.Action OnFullyTightened;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isComplete)
        {
            Debug.Log("[" + this.name + "] Pointer down ignored - already fully tightened.");
            return;
        }

        isDragging = true;
        peakAbsoluteSweptAngle = 0f;
        pivotScreenPos = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);
        dragStartAngle = AngleFromPivot(eventData.position);

        Debug.Log("[" + this.name + "] Grab started - turn " + (currentTurns + 1) + " of " + turnsToTighten
            + ", need to swing at least " + degreesPerTurn + " degrees (either direction) before releasing.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isComplete || !isDragging)
        {
            return;
        }

        float currentAngle = AngleFromPivot(eventData.position);
        float sweptAngle = Mathf.DeltaAngle(dragStartAngle, currentAngle);

        peakAbsoluteSweptAngle = Mathf.Max(peakAbsoluteSweptAngle, Mathf.Abs(sweptAngle));

        // Visually rotate the handle to follow the drag while held, so
        // the player can see how far through the swing they are.
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, sweptAngle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isComplete || !isDragging)
        {
            return;
        }

        isDragging = false;

        // Reset the visual regardless of success - simulates lifting
        // the wrench off and resetting for the next grab, same as a
        // real crescent wrench with no ratchet mechanism.
        rectTransform.localRotation = Quaternion.identity;

        if (peakAbsoluteSweptAngle >= degreesPerTurn)
        {
            Debug.Log("[" + this.name + "] Released - peak swept angle " + peakAbsoluteSweptAngle
                + " (needed " + degreesPerTurn + ") - turn registered.");

            RegisterTurn();
        }
        else
        {
            Debug.Log("[" + this.name + "] Released - peak swept angle " + peakAbsoluteSweptAngle
                + " (needed " + degreesPerTurn + ") - not enough, no turn registered. Press down again to retry.");
        }
    }

    float AngleFromPivot(Vector2 screenPos)
    {
        Vector2 dir = screenPos - pivotScreenPos;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    void RegisterTurn()
    {
        currentTurns++;

        Debug.Log("[" + this.name + "] Turn " + currentTurns + " of " + turnsToTighten + " complete.");

        OnTurnRegistered?.Invoke();

        if (currentTurns >= turnsToTighten)
        {
            isComplete = true;

            Debug.Log("[" + this.name + "] Fully tightened - all " + turnsToTighten + " turns done.");

            OnFullyTightened?.Invoke();
        }
    }
}