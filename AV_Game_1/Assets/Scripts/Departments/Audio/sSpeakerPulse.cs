using UnityEngine;

public class sSpeakerPulse : MonoBehaviour
{
    public AudioSource audioSource;
    public float pulseIntensity = 15f;    // Intensity of the punch
    public float smoothSpeed = 15f;       // Lower = smoother, Higher = snappier
    public float restScale = 1.0f;         // Default scale of your speaker cone

    private float[] spectrumData = new float[512];
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        if (audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
        }
    }

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        // Collect 512 samples of spectrum data
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        // Average the first 8 samples (low bass frequencies)
        float bassAverage = 0f;
        for (int i = 0; i < 8; i++)
        {
            bassAverage += spectrumData[i];
        }
        bassAverage /= 8f;

        // Calculate target scale based on bass energy
        float targetScaleMultiplier = restScale + (bassAverage * pulseIntensity);
        Vector3 targetScale = initialScale * targetScaleMultiplier;

        // Apply smooth interpolation (Lerp) to simulate rubbery physical resistance
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }
}