using UnityEngine;

public class sAudioPulseParticles : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem particles;

    [Range(64, 8192)]
    public int qSamples = 1024;
    public float threshold = 0.1f;
    public float multiplier = 50f;

    private float[] spectrumData;
    private float lastTime;

    void Start()
    {
        spectrumData = new float[qSamples];
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float maxV = 0;
        int maxIndex = 0;
        for (int i = 0; i < qSamples; i++)
        {
            if (spectrumData[i] > maxV && spectrumData[i] > 0)
            {
                maxV = spectrumData[i];
                maxIndex = i;
            }
        }

        // If a beat or loud pulse is detected
        if (maxV > threshold && Time.time - lastTime > 0.1f)
        {
            TriggerPulse(maxV);
            lastTime = Time.time;
        }
    }

    void TriggerPulse(float intensity)
    {
        var emission = particles.emission;

        // Dynamically change emission rate based on the bass/beat peak
        int emitCount = Mathf.RoundToInt(intensity * multiplier);
        particles.Emit(emitCount);

        // Optionally adjust the size of newly spawned particles dynamically
        var main = particles.main;
        main.startSize = new ParticleSystem.MinMaxCurve(intensity * 2, intensity * 4);
    }
}