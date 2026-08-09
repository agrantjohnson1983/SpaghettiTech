using UnityEngine;
using UnityEngine.Events;

// Overall controller for the crescent wrench minigame: jaw sizing
// phase first, then the turning phase. Wire OnWrenchTightened to the
// light's sRiggingSetupSpot.FinishSetup() in the inspector - same
// pattern as the bolt minigame's OnAllBoltsSecured wiring.
public class sCrescentWrenchMinigame : MonoBehaviour
{
    public sCrescentWrenchJawAdjust jawAdjust;
    public sCrescentWrenchTurnHandle turnHandle;

    public GameObject jawPhaseRoot;
    public GameObject turnPhaseRoot;

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
            turnHandle.OnFullyTightened += HandleFullyTightened;
        }
    }

    void OnDisable()
    {
        if (jawAdjust != null)
        {
            jawAdjust.OnJawMatched -= HandleJawMatched;
        }

        if (turnHandle != null)
        {
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
    }

    void HandleFullyTightened()
    {
        OnWrenchTightened?.Invoke();
    }
}