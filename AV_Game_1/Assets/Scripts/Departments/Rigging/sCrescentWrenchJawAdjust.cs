using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles the jaw-sizing phase of the crescent wrench minigame. The
// target position oscillates back and forth along the track rather
// than sitting still, so the player has to actively track and follow
// it with the handle - once aligned within tolerance, holding steady
// for requiredDwellTime locks it in. A visible target marker shows
// exactly where to aim at all times.
public class sCrescentWrenchJawAdjust : MonoBehaviour, IDragHandler
{
    public RectTransform track;
    public RectTransform handle;
    public RectTransform targetMarker;
    public Image feedbackImage;

    [Header("Target Movement")]
    public bool targetMoves = true;
    public float targetOscillationSpeed = 0.4f;
    public float targetOscillationRange = 0.3f;

    [Range(0f, 0.3f)]
    public float toleranceNormalized = 0.08f;

    [Header("Dwell Requirement")]
    // How long the handle must stay continuously within tolerance of
    // the (moving) target before it counts as matched.
    public float requiredDwellTime = 0.4f;

    public Color matchedColor = Color.green;
    public Color closeColor = Color.yellow;
    public Color unmatchedColor = Color.red;

    float currentNormalized = 0.5f;
    float targetNormalized = 0.5f;
    float dwellTimer;
    bool isMatched;

    public event System.Action OnJawMatched;

    // 0-1 progress toward the dwell lock-in, for a fill-bar or similar
    // UI element if you want one - fires every frame while active.
    public event System.Action<float> OnDwellProgress;

    void Start()
    {
        if (!targetMoves)
        {
            targetNormalized = 0.5f;
        }
    }

    void Update()
    {
        if (isMatched)
        {
            return;
        }

        if (targetMoves)
        {
            targetNormalized = 0.5f + Mathf.Sin(Time.time * targetOscillationSpeed * Mathf.PI * 2f) * targetOscillationRange;
        }

        if (targetMarker != null && track != null)
        {
            float trackWidth = track.rect.width;
            targetMarker.anchoredPosition = new Vector2((targetNormalized - 0.5f) * trackWidth, targetMarker.anchoredPosition.y);
        }

        UpdateFeedback(Time.deltaTime);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            track, eventData.position, eventData.pressEventCamera, out localPoint);

        float trackWidth = track.rect.width;
        currentNormalized = Mathf.Clamp01((localPoint.x + trackWidth * 0.5f) / trackWidth);

        handle.anchoredPosition = new Vector2(
            (currentNormalized - 0.5f) * trackWidth, handle.anchoredPosition.y);
    }

    void UpdateFeedback(float deltaTime)
    {
        float distance = Mathf.Abs(currentNormalized - targetNormalized);
        bool withinTolerance = distance <= toleranceNormalized;

        if (withinTolerance)
        {
            dwellTimer += deltaTime;
        }
        else
        {
            dwellTimer = 0f;
        }

        if (feedbackImage != null)
        {
            if (withinTolerance)
            {
                feedbackImage.color = matchedColor;
            }
            else if (distance <= toleranceNormalized * 2.5f)
            {
                feedbackImage.color = closeColor;
            }
            else
            {
                feedbackImage.color = unmatchedColor;
            }
        }

        float dwellProgress = requiredDwellTime > 0f
            ? Mathf.Clamp01(dwellTimer / requiredDwellTime)
            : (withinTolerance ? 1f : 0f);

        OnDwellProgress?.Invoke(dwellProgress);

        if (withinTolerance && dwellTimer >= requiredDwellTime && !isMatched)
        {
            isMatched = true;
            OnJawMatched?.Invoke();
        }
    }
}