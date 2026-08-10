using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class sScreenTransitionManager : MonoBehaviour
{
    public static sScreenTransitionManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image transitionImage;

    private Material runtimeMaterial;
    private Coroutine currentTransition;

    public bool IsTransitioning => currentTransition != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        //transitionImage.enabled = false;
    }

    public void Play(SO_ScreenTransition transition)
    {
        if (transition == null)
        {
            Debug.LogWarning("Screen Transition is null.");
            return;
        }

        Debug.Log($"Playing transition: {transition.name}");

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(
            PlayRoutine(transition)
        );
    }

    private IEnumerator PlayRoutine(
        SO_ScreenTransition transition)
    {
        // Create a runtime copy of the material
        if (runtimeMaterial != null)
            Destroy(runtimeMaterial);

        runtimeMaterial = new Material(transition.material);

        transitionImage.material = runtimeMaterial;
        transitionImage.color = transition.color;

        // Make sure the Image is visible
        transitionImage.enabled = true;

        float timer = 0f;

        while (timer < transition.duration)
        {
            timer += Time.unscaledDeltaTime;

            float normalizedTime =
                Mathf.Clamp01(timer / transition.duration);

            float progress =
                transition.curve.Evaluate(normalizedTime);

            runtimeMaterial.SetFloat(
                "_Progress",
                progress
            );

            yield return null;
        }

        // Make absolutely sure we end at 1
        runtimeMaterial.SetFloat("_Progress", 1f);

        Debug.Log("Transition finished.");

        currentTransition = null;
    }
}