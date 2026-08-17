using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uProgressMeters : MonoBehaviour
{
    public SO_EventsUI soUI;

    public Image progBarOverall, progBarRigging, progBarAudio, progBarVideo, progBarLighting, progBarPower;

    public TMP_Text textOverall, textRigging, textAudio, textVideo, textLighting, textPower;

    public bool SetToZeroOnStart = true;

    private void OnEnable()
    {
        soUI.progOverall.AddListener(UpdateOverallProgress);
        soUI.progRigging.AddListener(UpdateRiggingProgress);
        soUI.progAudio.AddListener(UpdateAudioProgress);
        soUI.progVideo.AddListener(UpdateVideoProgress);
        soUI.progLighting.AddListener(UpdateLightingProgress);
        soUI.progPower.AddListener(UpdatePowerProgress);
    }

    private void OnDisable()
    {
        soUI.progOverall.RemoveListener(UpdateOverallProgress);
        soUI.progRigging.RemoveListener(UpdateRiggingProgress);
        soUI.progAudio.RemoveListener(UpdateAudioProgress);
        soUI.progVideo.RemoveListener(UpdateVideoProgress);
        soUI.progLighting.RemoveListener(UpdateLightingProgress);
        soUI.progPower.RemoveListener(UpdatePowerProgress);
    }

    private void Start()
    {
        if(SetToZeroOnStart)
        {
            UpdateOverallProgress(0f);
            UpdateRiggingProgress(0f);
            UpdateAudioProgress(0f);
            UpdateVideoProgress(0f);
            UpdateLightingProgress(0f);
            UpdatePowerProgress(0f);
        }

        else
        {
            UpdateAll();
        }
    }

    void UpdateAll()
    {
        Debug.Log("Setting all progress for results");

        if (sGigManager.gigManagerGlobal != null)
        {
            //sGigManager.gigManagerGlobal.

            if (sRiggingManager.riggingMangerGlobal != null)
            {
                float completion = sRiggingManager.riggingMangerGlobal.CalculateProgress();

                //sGigManager.gigManagerGlobal.

                Debug.Log("Setting rigging progress to " + completion);

                UpdateRiggingProgress(completion);
            }

            else
            {
                Debug.LogWarning("Rigging Mgr was null!");
            }

            if (sAudioManager.audioManagerGlobal != null)
            {
                float completion = sAudioManager.audioManagerGlobal.Status.Completion;

                UpdateAudioProgress(completion);
            }

            else
            {
                Debug.LogWarning("Audio Mgr was null!");
            }

            if(sPowerManager.powerManagerGlobal != null)
            {
                float completion = sPowerManager.powerManagerGlobal.Status.Completion;

                UpdatePowerProgress(completion);
            }

            else
            {
                Debug.LogWarning("Power Mgr was null!");
            }

            UpdateVideoProgress(0f);

            UpdateLightingProgress(0f);

            UpdateOverallProgress(sGigManager.gigManagerGlobal.GetOverallProgress());
        }

        else
        {
            Debug.LogWarning("Gig Mgr was null!");
        }
    }

    void UpdateOverallProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarOverall, progBarOverall.fillAmount, _amount));

        //progBarOverall.fillAmount = _amount;

        int total = (int)(_amount * 100f);

        textOverall.text = total.ToString() + "% \n[TOTAL]";
         
        SetColor(_amount, progBarOverall);
    }

    void UpdateAudioProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarAudio, progBarAudio.fillAmount, _amount));

        //progBarAudio.fillAmount = _amount;

        textAudio.text = ((int)(_amount * 100f)).ToString() + "% \n[AUDIO]";

        SetColor(_amount, progBarAudio);
    }

    void UpdateRiggingProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarRigging, progBarRigging.fillAmount, _amount));

        //progBarRigging.fillAmount = _amount;

        textRigging.text = ((int)(_amount * 100f)).ToString() + "% \n[RIGGING]";

        SetColor(_amount, progBarRigging);

    }

    void UpdateVideoProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarVideo, progBarVideo.fillAmount, _amount));

        //progBarVideo.fillAmount = _amount;

        textVideo.text = ((int)(_amount * 100f)).ToString() + "% \n[VIDEO]";

        SetColor(_amount, progBarVideo);
    }

    void UpdateLightingProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarLighting, progBarLighting.fillAmount, _amount));
        
        //progBarLighting.fillAmount = _amount;
        
        textLighting.text = ((int)(_amount * 100f)).ToString() + "% \n[LIGHTING]";

        SetColor(_amount, progBarLighting);
    }

    void UpdatePowerProgress(float _amount)
    {
        StartCoroutine(BarAnimation(progBarPower, progBarPower.fillAmount, _amount));

        textPower.text = ((int)(_amount * 100f)).ToString() + "% \n[POWER]";

        SetColor(_amount, progBarPower);
    }

    IEnumerator BarAnimation(Image _image, float _startingAmount, float _endAmount)
    {
        float counter = 0f;

        while (counter < 0.5f)
        {
            _image.fillAmount = Mathf.Lerp(_startingAmount, _endAmount, (counter / 0.5f));

            counter += Time.deltaTime;

            yield return null;
        }
    }

    void SetColor(float _amount, Image _image)
    {
        if(_amount < 0.75f)
        {
            _image.color = Color.yellow;
        }

        if(_amount >= 1f)
        {
            _image.color = Color.green;
        }
    }
}
