using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class sMaterialAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Graphic targetGraphic;

    [Header("Property")]
    [SerializeField] private string propertyName = "_Progress";

    [Header("Animation")]
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private float duration = 1f;

    [Header("Curve")]
    [SerializeField]
    private AnimationCurve curve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Material runtimeMaterial;
    private Coroutine animationRoutine;

    private void Awake()
    {
        if (targetGraphic == null)
        {
            Debug.LogError(
                $"{name}: No target Graphic assigned."
            );

            return;
        }

        if (targetGraphic.material == null)
        {
            Debug.LogError(
                $"{name}: Target Graphic has no material."
            );

            return;
        }

        runtimeMaterial = targetGraphic.material;

        if (!runtimeMaterial.HasProperty(propertyName))
        {
            Debug.LogError(
                $"{name}: Material does not have property '{propertyName}'."
            );
        }
    }

    private void Start()
    {
        PlayBackward();
    }

    [ContextMenu("Play Forward")]
    public void PlayForward()
    {
        Play(startValue, endValue);
    }

    [ContextMenu("Play Backward")]
    public void PlayBackward()
    {
        Play(endValue, startValue);
    }

    public void Play(float from, float to)
    {
        if (runtimeMaterial == null)
        {
            Debug.LogError(
                $"{name}: No runtime material available."
            );

            return;
        }

        if (!runtimeMaterial.HasProperty(propertyName))
        {
            Debug.LogError(
                $"{name}: Property '{propertyName}' was not found."
            );

            return;
        }

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        animationRoutine = StartCoroutine(
            AnimateRoutine(from, to)
        );
    }

    private IEnumerator AnimateRoutine(float from, float to)
    {
        float timer = 0f;

        runtimeMaterial.SetFloat(propertyName, from);

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float normalizedTime =
                Mathf.Clamp01(timer / duration);

            float curveValue =
                curve.Evaluate(normalizedTime);

            float value =
                Mathf.Lerp(from, to, curveValue);

            runtimeMaterial.SetFloat(
                propertyName,
                value
            );

            yield return null;
        }

        runtimeMaterial.SetFloat(propertyName, to);

        animationRoutine = null;
    }
}