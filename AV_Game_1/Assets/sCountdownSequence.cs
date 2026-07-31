using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class sCountdownSequence : MonoBehaviour
{
    [System.Serializable]
    public class TimerPhase
    {
        public string phaseName;

        [TextArea]
        public string statusText;

        public float duration = 300f;

        public UnityEvent OnPhaseStarted;
        public UnityEvent OnPhaseFinished;
    }

    [Header("References")]
    [SerializeField] private sCountdownTimer timer;
    [SerializeField] private TMP_Text phaseNameText;
    [SerializeField] private TMP_Text statusText;

    [Header("Sequence")]
    [SerializeField] private bool startOnEnable = true;
    [SerializeField] private List<TimerPhase> phases = new();

    public UnityEvent OnSequenceFinished;

    private int currentPhase = -1;

    private void Awake()
    {
        timer.OnTimerFinished.AddListener(NextPhase);
    }

    private void OnDestroy()
    {
        timer.OnTimerFinished.RemoveListener(NextPhase);
    }

    private void OnEnable()
    {
        if (startOnEnable)
            StartSequence();
    }

    public void StartSequence()
    {
        currentPhase = -1;
        NextPhase();
    }

    public void NextPhase()
    {
        if (currentPhase >= 0 && currentPhase < phases.Count)
            phases[currentPhase].OnPhaseFinished?.Invoke();

        currentPhase++;

        if (currentPhase >= phases.Count)
        {
            Debug.Log("Sequence Complete!");
            OnSequenceFinished?.Invoke();
            return;
        }

        TimerPhase phase = phases[currentPhase];

        if (phaseNameText)
            phaseNameText.text = phase.phaseName;

        if (statusText)
            statusText.text = phase.statusText;

        timer.StartTimer(phase.duration);

        phase.OnPhaseStarted?.Invoke();
    }

    public void PreviousPhase()
    {
        currentPhase = Mathf.Max(currentPhase - 2, -1);
        NextPhase();
    }

    public void RestartCurrentPhase()
    {
        if (currentPhase < 0 || currentPhase >= phases.Count)
            return;

        timer.StartTimer(phases[currentPhase].duration);
    }

    public void JumpToPhase(int index)
    {
        if (index < 0 || index >= phases.Count)
            return;

        currentPhase = index - 1;
        NextPhase();
    }
}