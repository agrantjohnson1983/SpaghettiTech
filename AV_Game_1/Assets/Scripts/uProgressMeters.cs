using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uProgressMeters : MonoBehaviour
{
    public SO_EventsUI soUI;

    public Image progBarOverall, progBarRigging, progBarAudio, progBarVideo, progBarLighting;

    public TMP_Text textOverall, textRigging, textAudio, textVideo, textLighting;

    private void OnEnable()
    {
        soUI.progOverall.AddListener(UpdateOverallProgress);
        soUI.progRigging.AddListener(UpdateRiggingProgress);
        soUI.progAudio.AddListener(UpdateAudioProgress);
        soUI.progVideo.AddListener(UpdateVideoProgress);
        soUI.progLighting.AddListener(UpdateLightingProgress);
    }

    private void OnDisable()
    {
        soUI.progOverall.RemoveListener(UpdateOverallProgress);
        soUI.progRigging.RemoveListener(UpdateRiggingProgress);
        soUI.progAudio.RemoveListener(UpdateAudioProgress);
        soUI.progVideo.RemoveListener(UpdateVideoProgress);
        soUI.progLighting.RemoveListener(UpdateLightingProgress);
    }

    private void Start()
    {
        UpdateOverallProgress(0f);
        UpdateRiggingProgress(0f);
        UpdateAudioProgress(0f);
        UpdateVideoProgress(0f);
        UpdateLightingProgress(0f);
    }

    void UpdateOverallProgress(float _amount)
    {
        progBarOverall.fillAmount = _amount;

        _amount *= 100f;

        int total = (int)_amount;

        Debug.Log("Overall progress updated to " + total);

        textOverall.text = total.ToString() + "% COMPLETE - OVERALL";
    }

    void UpdateAudioProgress(float _amount)
    {
        progBarAudio.fillAmount = _amount;
        textAudio.text = ((int)_amount * 100f).ToString() + "% COMPLETE - AUDIO";
    }

    void UpdateRiggingProgress(float _amount)
    {
        progBarRigging.fillAmount = _amount;
        textRigging.text = ((int)_amount * 100f).ToString() + "% COMPLETE - RIGGING";

    }

    void UpdateVideoProgress(float _amount)
    {
        progBarVideo.fillAmount = _amount;
        textVideo.text = ((int)_amount * 100f).ToString() + "% COMPLETE - VIDEO";
    }

    void UpdateLightingProgress(float _amount)
    {
        progBarLighting.fillAmount = _amount;
        textLighting.text = ((int)_amount * 100f).ToString() + "% COMPLETE - LIGHTING";
    }

}
