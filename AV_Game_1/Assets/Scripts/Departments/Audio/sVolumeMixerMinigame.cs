using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class sVolumeMixerMinigame : MonoBehaviour
{
    public sVolumeChannel[] channels;

    [Header("UI")]
    public Slider[] sliders;
    public TMP_Text qualityText;

    [Header("Gameplay")]
    public float tolerance = 0.08f;
    public float completeTime = 2f;

    float timer;

    public sVolumeChannelUI[] channelUI;

    public SO_AudioEventChannel soAudio;

    public AudioMixerGroup audioMixerMinigame;

    public Slider masterVolume;


    void Start()
    {
        GenerateTargets();

        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i;

            sliders[i].onValueChanged.AddListener(
                value =>
                {
                    SetVolume(index, value);
                    channelUI[index].UpdateLive(value);
                });


            channelUI[i].Setup(channels[i]);

            sliders[i].value = 0f;
        }

        
        masterVolume.onValueChanged.AddListener(value => {
            float clampedValue = Mathf.Max(masterVolume.value, 0.0001f); 
            float volume = Mathf.Log10(clampedValue) * 20f; 
            audioMixerMinigame.audioMixer.SetFloat("MinigameVolume", volume); });
    }


    void GenerateTargets()
    {
        foreach (var channel in channels)
        {
            channel.targetVolume =
                Random.Range(0.2f, 0.8f);

            channel.playerVolume = 0.5f;
        }
    }


    void SetVolume(int index, float value)
    {
        channels[index].playerVolume = value;
    }


    void Update()
    {
        float quality = GetQuality();

        if (qualityText)
        {
            qualityText.text =
                "Mix Quality: "
                + Mathf.RoundToInt(quality)
                + "%";
        }


        if (IsComplete())
        {
            timer += Time.deltaTime;

            if (timer >= completeTime)
            {
                Complete();
            }
        }
        else
        {
            timer = 0;
        }
    }


    float GetQuality()
    {
        float total = 0;

        foreach (var channel in channels)
        {
            float accuracy =
                1 - Mathf.Abs(
                    channel.targetVolume -
                    channel.playerVolume);

            total += accuracy;
        }

        return (total / channels.Length) * 100f;
    }


    bool IsComplete()
    {
        foreach (var channel in channels)
        {
            if (Mathf.Abs(
                channel.targetVolume -
                channel.playerVolume)
                > tolerance)
            {
                return false;
            }
        }

        return true;
    }


    void Complete()
    {
        Debug.Log("VOLUME MIX COMPLETE!");

        if (soAudio != null)
            soAudio.TriggerSFX("AudioMixerVolumeComplete");

        enabled = false;
    }
}