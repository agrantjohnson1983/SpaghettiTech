using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sSpeakerSync : MonoBehaviour
{
    AudioSource _source;

    public AudioSource _syncSource;

    public static double timeSync;

    public static double startTime;

    int totalSamplesRead;
    int sampleRate;

    double perfectSyncTime;

    private void Start()
    {
        _source = GetComponent<AudioSource>();

        sampleRate = AudioSettings.outputSampleRate;

        if (_syncSource == null)
        {
            _source.Play();
            return;
        }

        if(_syncSource.isPlaying)
        {
            double delay = 0.05;

            int samplePos = _syncSource.timeSamples;
            samplePos += Mathf.RoundToInt((float)(delay * _source.clip.frequency));
            samplePos %= _source.clip.samples;

            double syncTime = AudioSettings.dspTime + delay;

            _source.timeSamples = samplePos;
            _source.PlayScheduled(syncTime);
            Debug.Log("syncing audio at: " + syncTime);
        }

        else
        {
            
            startTime = AudioSettings.dspTime;
            _source.PlayScheduled(startTime);
            Debug.Log("starting audio sync time at: " + startTime);

        }
    }

}
