using UnityEngine;

public class sSelectionBoxUI : MonoBehaviour
{
    [SerializeField] RectTransform boxRect;

    public RectTransform canvasRect;

    Vector2 startPosition;


    void Awake()
    {
        //canvasRect = transform.parent as RectTransform;

        Hide();
    }


    public void Begin(Vector2 screenPosition)
    {
        Debug.Log(
    "Screen: " + screenPosition +
    " Canvas: " + startPosition
);

        startPosition = ScreenToCanvas(screenPosition);

        boxRect.gameObject.SetActive(true);

        boxRect.anchoredPosition = startPosition;
        boxRect.sizeDelta = Vector2.zero;
    }


    public void UpdateBox(Vector2 screenPosition)
    {
        Vector2 currentPosition = ScreenToCanvas(screenPosition);

        Vector2 min = Vector2.Min(
            startPosition,
            currentPosition
        );

        Vector2 max = Vector2.Max(
            startPosition,
            currentPosition
        );


        boxRect.anchoredPosition = min;

        boxRect.sizeDelta = max - min;
    }


    Vector2 ScreenToCanvas(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            null,
            out Vector2 localPoint
        );

        // Convert from canvas center origin to bottom-left origin
        localPoint += new Vector2(
            canvasRect.rect.width * 0.5f,
            canvasRect.rect.height * 0.5f
        );

        return localPoint;
    }


    public void Hide()
    {
        boxRect.gameObject.SetActive(false);
    }
}