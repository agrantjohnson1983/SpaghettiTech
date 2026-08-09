using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class GigStatus
{
    public string gigName;

    public float overallProgress;

    public List<DepartmentStatus> departments = new();

    public int currentPhase;

    public bool isComplete;
}

public enum GigPhase
{
    Warehouse,
    LoadTruck,
    Travel,
    LoadIn,
    Rigging,
    Audio,
    Lighting,
    Video,
    Showtime,
    LoadOut,
    ReturnHome,
    Results
}

public class sGigManager : MonoBehaviour
{
    public static sGigManager gigManagerGlobal;

    public List<GameObject> itemsLoadedObjectList, workersHiredList;

    public TextMeshProUGUI textCurrentGig;

    bool hasAGig = false;

    UnityEvent<SO_GigData> gigEvent;

    [SerializeField]
    private SO_GigData currentGig;
    public SO_GigData CurrentGig => currentGig;

    //private Dictionary<string, GigTaskData> taskLookup = new();

    //private Dictionary<string, float> taskProgress = new();

    private int currentPhaseIndex = 0;

    public SO_EventsUI soUI;

    public List<sDepartmentManager> departmentManagersList = new List<sDepartmentManager>();

    float progRigging, progAudio, progVideo, progLighting;

    GigStatus currentStatus = new GigStatus();

    public GigPhase CurrentPhase { get; private set; }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;

        soUI.progLighting.AddListener(UpdateLighting);
        soUI.progAudio.AddListener(UpdateAudio);
        soUI.progRigging.AddListener(UpdateRigging);
        soUI.progVideo.AddListener(UpdateVideo);

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;

        soUI.progLighting.RemoveListener(UpdateLighting);
        soUI.progAudio.RemoveListener(UpdateAudio);
        soUI.progRigging.RemoveListener(UpdateRigging);
        soUI.progVideo.RemoveListener(UpdateVideo);
    }

    public void Awake()
    {
        if (gigManagerGlobal != null && gigManagerGlobal != this)
        {
            Destroy(this.gameObject);
            return; // stop here — don't let the duplicate run OnEnable/Start
        }

        gigManagerGlobal = this;
        DontDestroyOnLoad(this.gameObject); // only the real singleton persists

        if (gigEvent == null)
            gigEvent = new UnityEvent<SO_GigData>();
    }

    private void Start()
    {
        itemsLoadedObjectList  = new List<GameObject>();

        workersHiredList = new List<GameObject>();

        //itemsLoadedDataList = new List<SO_ItemData>();

        //itemsNeededDataList = new List<SO_ItemData>();

        
    }

    public void RegisterDepartment(
    sDepartmentManager manager)
    {
        Debug.Log("registering dept " + manager);

        if (!departmentManagersList.Contains(manager))
        {
            departmentManagersList.Add(manager);
            
        }
            
    }

    public void SetPhase(GigPhase _phase)
    {
        CurrentPhase = _phase;

        switch(CurrentPhase)
        {
            
        }
    }

    void UpdateRigging(float _amount)
    {
        progRigging = _amount;

        UpdateTotalProgress();
    }

    void UpdateAudio(float _amount)
    {
        progAudio = _amount;

        UpdateTotalProgress();
    }

    void UpdateVideo(float _amount)
    {
        progVideo = _amount;

        UpdateTotalProgress();
    }

    void UpdateLighting(float _amount)
    {
        progLighting = _amount;

        UpdateTotalProgress();
    }

    void UpdateTotalProgress()
    {
        float total = progRigging + progAudio + progLighting + progVideo;

        float totalDepts = departmentManagersList.Count;

        total = total/totalDepts;

        //Debug.Log("total gig progress is " + total);

        soUI.TriggerProgOverall(total);
    }

    public void SetCurrentGig(SO_GigData _gigData)
    {
        currentGig = _gigData;

        if(currentGig == null)
        {
            Debug.LogWarning("Current Gig is null!");
            return;
        }

        hasAGig = true;

        currentStatus.gigName = currentGig.gigName;

        UpdateGigUI();


    }

    void FinishGig()
    {

    }

    public string GetGigScene()
    {
        if (currentGig != null)
            return currentGig.sceneNameToLoad;
        else
            return null;
    }

    public Dictionary<SO_ItemData, int> GetNeededItemCounts()
    {
        Dictionary<SO_ItemData, int> counts = new();

        foreach (var item in currentGig.requiredItems)
        {
            if (item == null)
                continue;

            counts.TryAdd(item, 0);
            counts[item]++;
        }

        return counts;
    }

    public Dictionary<SO_ItemData, int> GetLoadedItemCounts()
    {
        Dictionary<SO_ItemData, int> counts = new();

        foreach (GameObject obj in itemsLoadedObjectList)
        {
            if (obj == null)
                continue;

            //------------------------------------
            // BOX
            //------------------------------------

            if (obj.TryGetComponent<sBox>(out sBox box))
            {
                foreach (SO_ItemData item in box.boxedItemDataList)
                {
                    if (item == null)
                        continue;

                    counts.TryAdd(item, 0);
                    counts[item]++;
                }

                continue;
            }

            //------------------------------------
            // NORMAL ITEM
            //------------------------------------

            if (obj.TryGetComponent<iLoadable>(out iLoadable loadable))
            {
                if (loadable.ItemData == null)
                    continue;

                counts.TryAdd(loadable.ItemData, 0);
                counts[loadable.ItemData]++;
            }
        }

        return counts;
    }

    public void AddItemToGig(GameObject _itemObject )
    {
        if (CheckForItemDupes(_itemObject))
            return;

        itemsLoadedObjectList.Add(_itemObject);

        DontDestroyOnLoad(_itemObject);

        //itemsLoadedObjectList.Add(_ItemObject);

        //if(_ItemObject.TryGetComponent<iLoadable>(out iLoadable _loadable))
        //{
        //    itemsLoadedDataList.Add(_loadable.ItemData);
        //}
    }

    public void AddWorkerToGig(GameObject _WorkerData)
    {
        if (CheckForWorkerDupes(_WorkerData))
            return;

        DontDestroyOnLoad(_WorkerData);

        workersHiredList.Add(_WorkerData);
    }

    bool CheckForWorkerDupes(GameObject _workerData)
    {
        bool isADupe = false;

        for (int i = 0; i < workersHiredList.Count; i++)
        {
            if (_workerData == workersHiredList[i])
            {
                isADupe = true;
            }
        }

        return isADupe;
    }

    bool CheckForItemDupes(GameObject _ItemData)
    {
        bool isADupe = false;



        for (int i = 0; i < itemsLoadedObjectList.Count; i++)
        {
            if(_ItemData == itemsLoadedObjectList[i])
            {
                Debug.Log("Dupe found with " + _ItemData + " and " + itemsLoadedObjectList[i]);
                isADupe = true;
            }
        }

        return isADupe;
    }

    public void TruckDrive()
    {
        for (int i = 0; i < itemsLoadedObjectList.Count; i++)
        {
            itemsLoadedObjectList[i].SetActive(false);
        }

        for (int i = 0; i < workersHiredList.Count; i++)
        {
            workersHiredList[i].SetActive(false);
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (this != gigManagerGlobal) return;

        if (GameManager.gm.GetGameMode() == eGameMode.gig)
        {
            Debug.Log("Starting Gig");
            StartGig();
        }
    }

    
    void StartGig()
    {
        //RegisterTasks();

        currentPhaseIndex = 0;


        for (int i = 0; i < itemsLoadedObjectList.Count; i++)
        {
            // TO DO - Set location to truck

            itemsLoadedObjectList[i].SetActive(true);
        }

        for (int i = 0; i < workersHiredList.Count; i++)
        {
            workersHiredList[i].SetActive(true);
        }
    }

    //private void RegisterTasks()
    //{
    //    taskLookup.Clear();
    //    taskProgress.Clear();

    //    if (currentGig == null)
    //        return;

    //    foreach (var phase in currentGig.phases)
    //    {
    //        foreach (var task in phase.tasks)
    //        {
    //            if (taskLookup.ContainsKey(task.taskID))
    //            {
    //                Debug.LogWarning("Duplicate Task ID: " + task.taskID);
    //                continue;
    //            }

    //            taskLookup.Add(task.taskID, task);
    //            taskProgress.Add(task.taskID, 0f);
    //        }
    //    }
    //}


    //public void SetTaskProgress(string taskID, float progress)
    //{
    //    if (!taskProgress.ContainsKey(taskID))
    //    {
    //        Debug.LogWarning("Task not registered: " + taskID);
    //        return;
    //    }

    //    progress = Mathf.Clamp01(progress);

    //    taskProgress[taskID] = progress;
    //}

    //public float GetTaskProgress(string taskID)
    //{
    //    if (taskProgress.TryGetValue(taskID, out float progress))
    //        return progress;

    //    return 0f;
    //}

    //public bool IsTaskComplete(string taskID)
    //{
    //    return GetTaskProgress(taskID) >= 1f;
    //}

    //public float GetCurrentPhaseCompletion()
    //{
    //    if (currentGig == null)
    //        return 0;

    //    GigPhaseData phase = currentGig.phases[currentPhaseIndex];

    //    if (phase.tasks.Count == 0)
    //        return 1;

    //    float total = 0;

    //    foreach (var task in phase.tasks)
    //    {
    //        total += GetTaskProgress(task.taskID);
    //    }

    //    return total / phase.tasks.Count;
    //}

    //public float GetGigCompletion()
    //{
    //    if (taskProgress.Count == 0)
    //        return 0;

    //    float total = 0;

    //    foreach (var task in taskProgress)
    //    {
    //        total += task.Value;
    //    }

    //    return total / taskProgress.Count;
    //}

    public void NextPhase()
    {
        currentPhaseIndex++;

        if (currentPhaseIndex >= currentGig.phases.Count)
        {
            FinishGig();
        }
    }

    public bool CheckIfHasAGig()
    {
        return hasAGig;
    }

    void UpdateGigUI()
    {
        if (currentGig == null)
            return;

        textCurrentGig.text = $"Current Gig: {currentGig.gigName}";
        sComputer.computerGlobal.SetText($"Current Gig: {currentGig.gigName}");
    }

    public float GetOverallProgress()
    {
        if (departmentManagersList.Count == 0)
            return 0;

        float total = 0;

        foreach (var department in departmentManagersList)
            total += department.Status.Completion;

        return total / departmentManagersList.Count;
    }
}
