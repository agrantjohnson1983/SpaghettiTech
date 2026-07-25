using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sCrewStatusUI : MonoBehaviour
{
    public sCrewMember crew;

    public TMP_Text nameText;
    public TMP_Text stateText;
    public TMP_Text jobText;
    public TMP_Text roleText;
    public TMP_Text skillText;

    public Image bg;


    void Update()
    {
        if (crew == null)
            return;


        nameText.text = crew.profile.crewName;

        roleText.text =
            crew.profile.role +
             " Lv." +
            crew.profile.level;

        skillText.text =
            "Rigging: " +
            crew.profile.riggingSkill +
            "\nAudio: " +
            crew.profile.audioSkill;

        stateText.text =
            GetStateText();

        bg.color = GetBGColor();


        if (crew.currentJob != null)
        {
            jobText.text =
                "Job: " +
                crew.currentJob.jobType;
        }
        else
        {
            jobText.text =
                "No Job";
        }

        
    }

    Color GetBGColor()
    {
        switch (crew.state)
        {
            case CrewState.Idle:

                return Color.green;

            case CrewState.Working:

                return Color.red;

            case CrewState.Moving:

                return Color.yellow;

            case CrewState.Waiting:

                return Color.blue;
        }

        return Color.gray;
    }

    string GetStateText()
    {
        switch (crew.state)
        {
            case CrewState.Idle:
                return "Available";

            case CrewState.Moving:
                return "Walking";

            case CrewState.Working:
                return "Working";

            case CrewState.Carrying:
                return "Carrying";

            default:
                return "";
        }
    }

    void LateUpdate()
    {
        transform.forward =
            Camera.main.transform.forward;
    }
}