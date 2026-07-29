using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class sWaveformVisualizer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    // Must be a power of 2 (e.g., 64, 128, 256, 512)
    public int sampleSize = 256;

    [Header("Visual Settings")]
    public float width = 10f;
    public float heightMultiplier = 5f;

    private LineRenderer lineRenderer;
    private float[] audioSamples;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSamples = new float[sampleSize];

        // Initialize line renderer points
        lineRenderer.positionCount = sampleSize;
    }

    void Update()
    {
        if (audioSource == null) return;

        // 1. Grab raw audio output data (Time-Domain / Waveform)
        audioSource.GetOutputData(audioSamples, 0);

        // 2. Map data points to LineRenderer positions
        for (int i = 0; i < sampleSize; i++)
        {
            // Distribute points horizontally
            float x = ((float)i / sampleSize) * width - (width / 2f);

            // Scale vertically based on audio amplitude
            float y = audioSamples[i] * heightMultiplier;

            Vector3 position = new Vector3(x, y, 0f);
            lineRenderer.SetPosition(i, position);
        }
    }
}