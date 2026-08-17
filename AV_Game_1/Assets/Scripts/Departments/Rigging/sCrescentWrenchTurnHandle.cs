using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles the turning phase of the crescent wrench minigame. Unlike a
// ratchet, a crescent wrench cannot swing back and forth to accumulate
// turns - each turn requires a full grab, swing, and release/reset
// cycle. A single press-drag-release that sweeps far enough - in
// EITHER direction - counts as one turn. The handle's color changes
// live, mid-drag, the instant the swing has gone far enough, so the
// player gets real-time feedback that it is safe to release rather
// than only finding out after letting go.
public class sCrescentWrenchTurnHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int turnsToTighten = 4;
    public float degreesPerTurn = 45f;

    public Image feedbackImage;
    public Color readyColor = Color.green;
    public Color notReadyColor = Color.white;

    RectTransform rectTransform;
    Vector2 pivotScreenPos;
    float dragStartAngle;
    bool isDragging;
    int currentTurns;
    bool isComplete;

    float peakAbsoluteSweptAngle;

    public int CurrentTurns
    {
        get
        {
            return currentTurns;
        }
    }

    // (currentTurns, turnsToTighten) - fired after a successful turn.
    public event System.Action<int, int> OnTurnRegistered;

    // Fired on release when the swing did not reach the threshold.
    public event System.Action OnTurnFailed;

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

        if (feedbackImage != null)
        {
            feedbackImage.color = notReadyColor;
        }

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

        rectTransform.localRotation = Quaternion.Euler(0f, 0f, sweptAngle);

        // Live feedback: turns green the instant the swing is far
        // enough to count, so releasing now would register.
        if (feedbackImage != null)
        {
            feedbackImage.color = peakAbsoluteSweptAngle >= degreesPerTurn ? readyColor : notReadyColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isComplete || !isDragging)
        {
            return;
        }

        isDragging = false;

        rectTransform.localRotation = Quaternion.identity;

        if (feedbackImage != null)
        {
            feedbackImage.color = notReadyColor;
        }

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

            OnTurnFailed?.Invoke();
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

        OnTurnRegistered?.Invoke(currentTurns, turnsToTighten);

        if (currentTurns >= turnsToTighten)
        {
            isComplete = true;

            Debug.Log("[" + this.name + "] Fully tightened - all " + turnsToTighten + " turns done.");

            OnFullyTightened?.Invoke();
        }
    }
}