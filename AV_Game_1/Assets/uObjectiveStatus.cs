using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uObjectiveStatus : MonoBehaviour
{
    public Image fillBarStatus;

    public TMP_Text textObjectiveName, textObjectiveStatus;

    public void SetObjective(ObjectiveStatus _status)
    {
        fillBarStatus.fillAmount = _status.Completion;
        textObjectiveName.text = _status.name;
        textObjectiveStatus.text = _status.completedItems + "/" + _status.totalItems;
    }
}
