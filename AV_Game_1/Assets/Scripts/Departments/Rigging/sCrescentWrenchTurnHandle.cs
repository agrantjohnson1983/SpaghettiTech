using UnityEngine;
using UnityEngine.EventSystems;

// Handles the turning phase of the crescent wrench minigame. Unlike a
// ratchet, a crescent wrench cannot swing back and forth to accumulate
// turns - each turn requires a full grab, swing, and release/reset
// cycle. A single press-drag-release that sweeps far enough in the
// correct direction counts as one turn; releasing early, or not
// sweeping far enough, registers nothing and the player has to press
// down again to retry that turn.
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
            return;
        }

        isDragging = true;
        pivotScreenPos = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);
        dragStartAngle = AngleFromPivot(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isComplete || !isDragging)
        {
            return;
        }

        float currentAngle = AngleFromPivot(eventData.position);
        float sweptAngle = Mathf.DeltaAngle(dragStartAngle, currentAngle);

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

        float currentAngle = AngleFromPivot(eventData.position);
        float sweptAngle = Mathf.DeltaAngle(dragStartAngle, currentAngle);

        // Reset the visual regardless of success - simulates lifting
        // the wrench off and resetting for the next grab, same as a
        // real crescent wrench with no ratchet mechanism.
        rectTransform.localRotation = Quaternion.identity;

        if (sweptAngle >= degreesPerTurn)
        {
            RegisterTurn();
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

        OnTurnRegistered?.Invoke();

        if (currentTurns >= turnsToTighten)
        {
            isComplete = true;
            OnFullyTightened?.Invoke();
        }
    }
}