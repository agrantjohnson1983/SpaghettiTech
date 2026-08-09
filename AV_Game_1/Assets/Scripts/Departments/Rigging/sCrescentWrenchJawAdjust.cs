using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles the jaw-sizing phase of the crescent wrench minigame: the
// player drags a slider handle along a track to match the wrench's
// adjustable jaw width to the bolt/nut size before tightening can
// begin. This is the mechanic that makes a crescent wrench distinct
// from a fixed-size spanner.
public class sCrescentWrenchJawAdjust : MonoBehaviour, IDragHandler
{
    public RectTransform track;
    public RectTransform handle;
    public Image feedbackImage;

    [Range(0f, 1f)]
    public float targetNormalized = 0.5f;

    [Range(0f, 0.3f)]
    public float toleranceNormalized = 0.08f;

    public Color matchedColor = Color.green;
    public Color unmatchedColor = Color.red;

    float currentNormalized;
    bool isMatched;

    public event System.Action OnJawMatched;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            track, eventData.position, eventData.pressEventCamera, out localPoint);

        float trackWidth = track.rect.width;
        currentNormalized = Mathf.Clamp01((localPoint.x + trackWidth * 0.5f) / trackWidth);

        handle.anchoredPosition = new Vector2(
            (currentNormalized - 0.5f) * trackWidth, handle.anchoredPosition.y);

        UpdateFeedback();
    }

    void UpdateFeedback()
    {
        bool withinTolerance = Mathf.Abs(currentNormalized - targetNormalized) <= toleranceNormalized;

        if (feedbackImage != null)
        {
            feedbackImage.color = withinTolerance ? matchedColor : unmatchedColor;
        }

        if (withinTolerance && !isMatched)
        {
            isMatched = true;
            OnJawMatched?.Invoke();
        }
        else if (!withinTolerance)
        {
            isMatched = false;
        }
    }
}