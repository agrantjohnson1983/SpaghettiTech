using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sGigManager : MonoBehaviour
{
    public static sGigManager gigManagerGlobal;

    //public List<SO_BoxData> boxDataList;

    public List<GameObject> itemDataList, workerList;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
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
    }

    private void Start()
    {
        //boxDataList = new List<SO_BoxData>();
        itemDataList  = new List<GameObject>();

        workerList = new List<GameObject>();
    }

    public void AddItemToGig(GameObject _ItemData)
    {
        if (CheckForItemDupes(_ItemData))
            return;

        DontDestroyOnLoad(_ItemData);

        itemDataList.Add(_ItemData);
    }

    public void AddWorkerToGig(GameObject _WorkerData)
    {
        if (CheckForWorkerDupes(_WorkerData))
            return;

        DontDestroyOnLoad(_WorkerData);

        workerList.Add(_WorkerData);
    }

    bool CheckForWorkerDupes(GameObject _workerData)
    {
        bool isADupe = false;

        for (int i = 0; i < workerList.Count; i++)
        {
            if (_workerData == workerList[i])
            {
                isADupe = true;
            }
        }

        return isADupe;
    }

    bool CheckForItemDupes(GameObject _ItemData)
    {
        bool isADupe = false;

        for (int i = 0; i < itemDataList.Count; i++)
        {
            if(_ItemData == itemDataList[i])
            {
                isADupe = true;
            }
        }

        return isADupe;
    }

    public void TruckDrive()
    {
        for (int i = 0; i < itemDataList.Count; i++)
        {
            itemDataList[i].SetActive(false);
        }

        for (int i = 0; i < workerList.Count; i++)
        {
            workerList[i].SetActive(false);
        }
    }

    public List<GameObject>ReturnItems()
    {
        return itemDataList;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (this != gigManagerGlobal) return;

        if (GameManager.gm.GetGameMode() == eGameMode.gig)
        {
            StartGig();
        }
    }

    void StartGig()
    {
        for (int i = 0; i < itemDataList.Count; i++)
        {
            // TO DO - Set location to truck

            itemDataList[i].SetActive(true);
        }

        for (int i = 0; i < workerList.Count; i++)
        {
            workerList[i].SetActive(true);
        }
    }
}
