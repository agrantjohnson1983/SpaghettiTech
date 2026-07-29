using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class sCountdownTimer : MonoBehaviour
{
    [System.Serializable]
    public class WarningEvent
    {
        [Tooltip("Fire when remaining time is less than or equal to this value.")]
        public float triggerTime = 300f;

        public UnityEvent onReached;

        [HideInInspector]
        public bool fired;
    }

    [Header("Settings")]
    [SerializeField] private float duration = 600f;
    [SerializeField] private bool startOnEnable = true;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Events")]
    public UnityEvent OnTimerFinished;
    public List<WarningEvent> warningEvents = new();

    private float timeRemaining;
    private bool running;

    public float TimeRemaining => timeRemaining;
    public float Duration => duration;
    public float Normalized => Mathf.Clamp01(timeRemaining / duration);
    public bool IsRunning => running;

    private void OnEnable()
    {
        ResetTimer();

        if (startOnEnable)
            StartTimer();
    }

    private void Update()
    {
        if (!running)
            return;

        timeRemaining -= Time.deltaTime;

        foreach (var warning in warningEvents)
        {
            if (!warning.fired && timeRemaining <= warning.triggerTime)
            {
                warning.fired = true;
                warning.onReached?.Invoke();
            }
        }

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            running = false;

            UpdateUI();
            OnTimerFinished?.Invoke();
            return;
        }

        UpdateUI();
    }

    public void StartTimer()
    {
        running = true;
    }

    public void PauseTimer()
    {
        running = false;
    }

    public void ResumeTimer()
    {
        running = true;
    }

    public void StopTimer()
    {
        running = false;
        timeRemaining = 0f;
        UpdateUI();
    }

    public void ResetTimer()
    {
        timeRemaining = duration;
        running = false;

        foreach (var warning in warningEvents)
            warning.fired = false;

        UpdateUI();
    }

    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }

    public void SetDuration(float seconds)
    {
        duration = seconds;
        ResetTimer();
    }

    private void UpdateUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}