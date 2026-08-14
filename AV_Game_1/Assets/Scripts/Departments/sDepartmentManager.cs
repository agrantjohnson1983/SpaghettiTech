using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum eGigDept
{
    NONE,
    Rigging,
    Audio,
    Lighting,
    Video,
    Electricity,
}

public abstract class sDepartmentManager : MonoBehaviour
{
    public SO_EventsUI soUI;

    public SO_AudioEventChannel soAudio;

    [Header("Gig Task")]
    //[SerializeField] protected string taskID;

    [SerializeField] protected string departmentName;

    [Range(0f, 1f)]
    [SerializeField] protected float progress;

    [SerializeField]
    protected eGigDept deptID;

    List<GameObject> setupSpots;// = new List<GameObject>();

    public bool IsComplete => progress >= 1f;

    protected DepartmentStatus status = new DepartmentStatus();

    public DepartmentStatus Status => status;

    public virtual void Start()
    {
        setupSpots = new List<GameObject>();

        Status.departmentName = departmentName;

        sGigManager.gigManagerGlobal.RegisterDepartment(this);
    }

    public virtual void SetProgress(float value)
    {
        Debug.Log("Set progress called");

        progress = Mathf.Clamp01(value);

        UpdateGigManager();
    }

    protected virtual void UpdateGigManager()
    {
        Debug.Log("Updating Gig manager");

        if (sGigManager.gigManagerGlobal == null)
            return;

        //sGigManager.gigManagerGlobal.SetTaskProgress(taskID.ToString(), progress);
    }

    public virtual void Complete()
    {
        SetProgress(1f);
    }

    public virtual void ResetDepartment()
    {
        progress = 0f;
    }

    public virtual float CalculateProgress()
    {
        return progress;
    }

    public void RefreshProgress()
    {
        Debug.Log("Refresh Progress called");
        SetProgress(Status.Completion);
    }

    protected virtual void UpdateDepartmentUI()
    {
        if (soUI == null)
            return;

        Debug.Log("Updating " + deptID + " department progress to: " + progress);

        switch (deptID)
        {
            case eGigDept.Rigging:
                soUI.TriggerProgRigging(progress);
                break;

            case eGigDept.Audio:
                soUI.TriggerProgAudio(progress);
                break;

            case eGigDept.Lighting:
                soUI.TriggerProgLighting(progress);
                break;

            case eGigDept.Video:
                soUI.TriggerProgVideo(progress);
                break;
        }
    }
}

[System.Serializable]
public class DepartmentStatus
{
    public string departmentName;

    public List<ObjectiveStatus> objectives = new();

    public float Completion
    {
        get
        {
            if (objectives.Count == 0)
                return 0f;

            float total = 0f;

            foreach (ObjectiveStatus objective in objectives)
            {
                total += objective.Completion;
            }

            return total / objectives.Count;
        }
    }
}

[System.Serializable]
public class ObjectiveStatus
{
    public string name;

    public int completedItems;

    public int totalItems;

    public string statusText;

    public float weight = 1f;

    public float Completion
    {
        get
        {
            if (totalItems == 0)
                return 0f;

            return (float)completedItems / totalItems;
        }
    }

    public bool IsComplete => Completion >= 1f;
}