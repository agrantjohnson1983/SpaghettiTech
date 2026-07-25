using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class sGigManager : MonoBehaviour
{
    public static sGigManager gigManagerGlobal;

    //public List<SO_BoxData> boxDataList;

    public List<GameObject> itemsLoadedObjectList, workersHiredList;

    //public List<SO_ItemData> itemsNeededDataList, itemsLoadedDataList;

    SO_GigData currentGig;

    public TextMeshProUGUI textCurrentGig;

    bool hasAGig = false;

    UnityEvent<SO_GigData> gigEvent;

    //string gigSceneToLoad = null;


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

    public void SetCurrentGig(SO_GigData _gigData)
    {
        if (_gigData == null)
            return;

        hasAGig = true;

        currentGig = _gigData;

        //SetItemsNeededForGig(currentGig.itemsNeededForGigList);

        textCurrentGig.text = "Current Gig: " + currentGig.gigName;

        sComputer.computerGlobal.SetText("Current Gig: " + currentGig.gigName);
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

        foreach (var item in currentGig.itemsNeededForGigList)
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

    public bool CheckIfHasAGig()
    {
        return hasAGig;
    }
}
