using UnityEngine;

// Attach this to any grabbable object (alongside its iGrabbable implementation).
// sCharacterGrabController looks for this component and calls SetHighlight(true)
// when the object becomes the selected grabbable, and SetHighlight(false) when
// it is deselected, grabbed, or exits the trigger.
public class sObjectHighlighter : MonoBehaviour
{
    public enum HighlightMode
    {
        Tint,
        Outline,
        Both
    }

    [Header("Animation")]
    public float fadeSpeed = 10f;

    [Range(0f, 1f)]
    public float pulseAmount = 0.15f;

    public float pulseSpeed = 4f;

    float currentHighlight = 0f;
    float targetHighlight = 0f;

    Vector3 initialScale;

    [Header("Mode")]
    public HighlightMode mode = HighlightMode.Both;

    [Header("Renderer")]
    // Leave empty to auto grab the first Renderer found on this object or its children
    public Renderer targetRenderer;

    [Header("Tint Settings")]
    public Color tintColor = new Color(1f, 0.85f, 0.2f);
    [Range(0f, 5f)]
    public float tintEmissionIntensity = 1.5f;
    // Must match the emission color property name on your material's shader
    public string emissionColorProperty = "_EmissionColor";

    [Header("Outline Settings")]
    // Update these to match the property names on your URP outline shader
    public Color outlineColor = Color.yellow;
    [Range(0f, 0.1f)]
    public float outlineWidth = 0.02f;
    public string outlineWidthProperty = "_OutlineWidth";
    public string outlineColorProperty = "_OutlineColor";

    MaterialPropertyBlock _mpb;

    bool _isHighlighted = false;

    void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        _mpb = new MaterialPropertyBlock();

        initialScale = transform.localScale;
    }

    void Update()
    {
        if (targetRenderer == null)
            return;

        currentHighlight = Mathf.MoveTowards(
            currentHighlight,
            targetHighlight,
            fadeSpeed * Time.deltaTime);

        targetRenderer.GetPropertyBlock(_mpb);

        float pulse = 1f;

        if (currentHighlight > 0.001f)
        {
            pulse = 1f +
                Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        }

        if (mode == HighlightMode.Tint || mode == HighlightMode.Both)
        {
            Color emission =
                tintColor *
                tintEmissionIntensity *
                currentHighlight *
                pulse;

            _mpb.SetColor(emissionColorProperty, emission);
        }

        if (mode == HighlightMode.Outline || mode == HighlightMode.Both)
        {
            _mpb.SetFloat(
                outlineWidthProperty,
                outlineWidth * currentHighlight);

            _mpb.SetColor(
                outlineColorProperty,
                outlineColor);
        }

        targetRenderer.SetPropertyBlock(_mpb);

        float scalePulse =
    1f + currentHighlight * 0.02f;

        transform.localScale =
            initialScale * scalePulse;
    }

    public void SetHighlight(bool on)
    {
        _isHighlighted = on;
        targetHighlight = on ? 1f : 0f;
    }

    /*public void SetHighlight(bool _on)
    {
        if (targetRenderer == null)
        {
            return;
        }

        // Avoids redundant SetPropertyBlock calls every frame if something
        // calls this repeatedly while already in the desired state
        if (_isHighlighted == _on)
        {
            return;
        }

        _isHighlighted = _on;

        // Reads any existing overrides so we do not stomp other systems
        // that might also be using a MaterialPropertyBlock on this renderer
        targetRenderer.GetPropertyBlock(_mpb);

        if (_on)
        {
            if (mode == HighlightMode.Tint || mode == HighlightMode.Both)
            {
                Color _emission = tintColor * tintEmissionIntensity;
                _mpb.SetColor(emissionColorProperty, _emission);
            }

            if (mode == HighlightMode.Outline || mode == HighlightMode.Both)
            {
                _mpb.SetFloat(outlineWidthProperty, outlineWidth);
                _mpb.SetColor(outlineColorProperty, outlineColor);
            }
        }

        else
        {
            if (mode == HighlightMode.Tint || mode == HighlightMode.Both)
            {
                _mpb.SetColor(emissionColorProperty, Color.black);
            }

            if (mode == HighlightMode.Outline || mode == HighlightMode.Both)
            {
                _mpb.SetFloat(outlineWidthProperty, 0f);
            }
        }

        targetRenderer.SetPropertyBlock(_mpb);
    }*/

    public bool IsHighlighted()
    {
        return _isHighlighted;
    }
}
