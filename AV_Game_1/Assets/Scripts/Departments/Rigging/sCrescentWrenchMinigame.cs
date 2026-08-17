using UnityEngine;
using UnityEngine.Events;
using TMPro;

// Overall controller for the crescent wrench minigame: jaw sizing
// phase first, then the turning phase. Also drives a single
// instructionText element with what to do and live status feedback,
// so the mechanic is understandable without external explanation.
// Wire OnWrenchTightened to the light's sRiggingSetupSpot.FinishSetup()
// in the inspector - same pattern as the bolt minigame's
// OnAllBoltsSecured wiring.
public class sCrescentWrenchMinigame : MonoBehaviour
{
    public sCrescentWrenchJawAdjust jawAdjust;
    public sCrescentWrenchTurnHandle turnHandle;

    public GameObject jawPhaseRoot;
    public GameObject turnPhaseRoot;

    public TMP_Text instructionText;

    public UnityEvent OnWrenchTightened;

    void OnEnable()
    {
        if (jawPhaseRoot != null)
        {
            jawPhaseRoot.SetActive(true);
        }

        if (turnPhaseRoot != null)
        {
            turnPhaseRoot.SetActive(false);
        }

        if (jawAdjust != null)
        {
            jawAdjust.OnJawMatched += HandleJawMatched;
        }

        if (turnHandle != null)
        {
            turnHandle.OnTurnRegistered += HandleTurnRegistered;
            turnHandle.OnTurnFailed += HandleTurnFailed;
            turnHandle.OnFullyTightened += HandleFullyTightened;
        }

        SetInstruction("Drag the jaw to match the moving target. Hold steady to lock it in.");
    }

    void OnDisable()
    {
        if (jawAdjust != null)
        {
            jawAdjust.OnJawMatched -= HandleJawMatched;
        }

        if (turnHandle != null)
        {
            turnHandle.OnTurnRegistered -= HandleTurnRegistered;
            turnHandle.OnTurnFailed -= HandleTurnFailed;
            turnHandle.OnFullyTightened -= HandleFullyTightened;
        }
    }

    void HandleJawMatched()
    {
        if (jawPhaseRoot != null)
        {
            jawPhaseRoot.SetActive(false);
        }

        if (turnPhaseRoot != null)
        {
            turnPhaseRoot.SetActive(true);
        }

        SetInstruction("Jaw sized. Swing the wrench, then fully RELEASE to lock in a turn. "
            + (turnHandle != null ? "Turn 0 of " + turnHandle.turnsToTighten + "." : ""));
    }

    void HandleTurnRegistered(int _current, int _total)
    {
        SetInstruction("Turn " + _current + " of " + _total + " done. Swing and release again.");
    }

    void HandleTurnFailed()
    {
        SetInstruction("Not quite far enough - swing a bit more before releasing. Try again.");
    }

    void HandleFullyTightened()
    {
        SetInstruction("Fully tightened!");

        OnWrenchTightened?.Invoke();
    }

    void SetInstruction(string _text)
    {
        if (instructionText != null)
        {
            instructionText.text = _text;
        }
    }
}