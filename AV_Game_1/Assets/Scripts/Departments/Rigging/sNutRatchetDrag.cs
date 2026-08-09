using UnityEngine;
using UnityEngine.EventSystems;

// Added automatically to a nut once it snaps into a sBoltHoleSlot.
// A wrench icon spawns on the nut and must be dragged back and forth
// along its own current handle angle to register a ratchet click.
// Each click advances the wrench (and spins the nut) to a new angle,
// so the player has to track the wrench's orientation rather than
// always swinging flat left-right on screen.
public class sNutRatchetDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Wrench Visual")]
    public GameObject wrenchIconPrefab;
    public float initialWrenchAngleDegrees = 0f;
    public float wrenchAnglePerClick = 40f;

    [Header("Tightening")]
    public int clicksToTighten = 6;
    public float swingThreshold = 40f;
    public float rotationPerClick = 60f;

    private sBoltHoleSlot slot;
    private RectTransform rectTransform;
    private RectTransform wrenchIcon;
    private int currentClicks;
    private Vector2 dragStartScreenPos;
    private float accumulatedSwing;
    private int swingDirection;
    private float currentWrenchAngle;
    private bool isDragging;
    private bool isTightened;

    public void Initialize(sBoltHoleSlot ownerSlot)
    {
        slot = ownerSlot;
        rectTransform = GetComponent<RectTransform>();
        currentClicks = 0;
        isTightened = false;
        currentWrenchAngle = initialWrenchAngleDegrees;

        SpawnWrenchIcon();
    }

    private void SpawnWrenchIcon()
    {
        if (wrenchIconPrefab == null)
        {
            return;
        }

        GameObject iconInstance = Instantiate(wrenchIconPrefab, rectTransform);
        wrenchIcon = iconInstance.GetComponent<RectTransform>();

        if (wrenchIcon != null)
        {
            wrenchIcon.anchoredPosition = Vector2.zero;
            UpdateWrenchVisual();
        }
    }

    private void UpdateWrenchVisual()
    {
        if (wrenchIcon != null)
        {
            wrenchIcon.localRotation = Quaternion.Euler(0f, 0f, currentWrenchAngle);
        }
    }

    // Direction the wrench handle currently points in screen space.
    // The player has to drag roughly along this axis for the swing
    // to register.
    private Vector2 GetHandleDirection()
    {
        float rad = currentWrenchAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTightened)
        {
            return;
        }

        isDragging = true;
        dragStartScreenPos = eventData.position;
        accumulatedSwing = 0f;
        swingDirection = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isTightened || !isDragging)
        {
            return;
        }

        Vector2 delta = eventData.position - dragStartScreenPos;
        float projected = Vector2.Dot(delta, GetHandleDirection());
        int direction = projected > 0f ? 1 : -1;

        if (swingDirection == 0)
        {
            swingDirection = direction;
        }

        if (direction != swingDirection)
        {
            // Direction along the handle axis just reversed. Evaluate
            // the swing that finished.
            if (Mathf.Abs(accumulatedSwing) >= swingThreshold)
            {
                RegisterRatchetClick();
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

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isTightened)
        {
            return;
        }

        if (Mathf.Abs(accumulatedSwing) >= swingThreshold)
        {
            RegisterRatchetClick();
        }

        isDragging = false;
        accumulatedSwing = 0f;
        swingDirection = 0;
    }

    private void RegisterRatchetClick()
    {
        currentClicks++;

        // Visually spin the nut itself so it reads as tightening.
        rectTransform.Rotate(0f, 0f, -rotationPerClick);

        // Advance the wrench handle to a new angle, so the next swing
        // has to follow the new orientation, like resetting a ratchet
        // head before the next pull.
        currentWrenchAngle += wrenchAnglePerClick;
        UpdateWrenchVisual();

        if (currentClicks >= clicksToTighten)
        {
            isTightened = true;
            isDragging = false;

            if (wrenchIcon != null)
            {
                Destroy(wrenchIcon.gameObject);
            }

            //slot.NotifyTightened();
        }
    }
}
