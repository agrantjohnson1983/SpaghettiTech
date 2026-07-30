using UnityEngine;

public class sAudioChannelSplitter : MonoBehaviour
{
    [Header("Input")]
    public AudioClip sourceStereoClip;

    [Header("Outputs for Verification")]
    public AudioSource leftChannelSource;
    public AudioSource rightChannelSource;

    void Start()
    {
        if (sourceStereoClip == null)
        {
            Debug.LogError("Please assign a Source Stereo Clip in the inspector.");
            return;
        }

        if (sourceStereoClip.channels != 2)
        {
            Debug.LogError("The assigned audio clip is not stereo (2 channels)!");
            return;
        }

        SplitStereoClip();
    }

    void SplitStereoClip()
    {
        // 1. Get properties from original clip
        int totalSamples = sourceStereoClip.samples;
        int totalChannels = sourceStereoClip.channels; // Always 2 here
        int frequency = sourceStereoClip.frequency;

        // Create container array for interleaved data: [L0, R0, L1, R1...]
        float[] interleavedData = new float[totalSamples * totalChannels];
        sourceStereoClip.GetData(interleavedData, 0);

        // 2. Prepare isolated arrays for single-channel data
        float[] leftData = new float[totalSamples];
        float[] rightData = new float[totalSamples];

        // 3. De-interleave the data
        int monoIndex = 0;
        for (int i = 0; i < interleavedData.Length; i += 2)
        {
            leftData[monoIndex] = interleavedData[i];       // Even elements -> Left
            rightData[monoIndex] = interleavedData[i + 1];   // Odd elements -> Right
            monoIndex++;
        }

        // 4. Create two distinct mono Unity AudioClips
        // Parameters: (Name, sampleLength, channelCount, frequency, stream)
        AudioClip leftClip = AudioClip.Create(sourceStereoClip.name + "_Left", totalSamples, 1, frequency, false);
        AudioClip rightClip = AudioClip.Create(sourceStereoClip.name + "_Right", totalSamples, 1, frequency, false);

        // 5. Populate clips with separated data
        leftClip.SetData(leftData, 0);
        rightClip.SetData(rightData, 0);

        // 6. Optional: Route to separate sources and route pan settings
        if (leftChannelSource != null)
        {
            leftChannelSource.clip = leftClip;
            leftChannelSource.panStereo = -1f; // Hard pan Left
            //leftChannelSource.Play();
        }

        if (rightChannelSource != null)
        {
            rightChannelSource.clip = rightClip;
            rightChannelSource.panStereo = 1f; // Hard pan Right
            //rightChannelSource.Play();
        }
    }
}