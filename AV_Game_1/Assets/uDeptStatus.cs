using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uDeptStatus : MonoBehaviour
{
    public GameObject pObjectiveStatus;

    public Image overallStatusFill;
    public TMP_Text textDeptName, textOverallProgress;

    public Transform panelObjectives;

    public void SetObjectives(sDepartmentManager dm)
    {
        //Debug.Log("Setting objectives for " + dm);

        overallStatusFill.fillAmount = dm.Status.Completion;

        textDeptName.text = dm.Status.departmentName;

        textOverallProgress.text = ((int)dm.Status.Completion) * 100f + "%";

        foreach(KeyValuePair<string, ObjectiveStatus> kvp in dm.Status.objectives)
        {
            uObjectiveStatus oStatus;

            // spawns objective status panel
            oStatus = Instantiate(pObjectiveStatus, panelObjectives).GetComponent<uObjectiveStatus>();

            // sets objective status
            oStatus.SetObjective(kvp.Value);
        }
        
    }
}
