using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class sGigManager : MonoBehaviour
{
    public static sGigManager gigManagerGlobal;

    //public List<SO_BoxData> boxDataList;

    public List<GameObject> itemsLoadedObjectList, workersHiredList;

    public List<SO_ItemData> itemsLoadedDataList, itemsNeededDataList;

    SO_GigData currentGig;

    public TextMeshProUGUI textCurrentGig;

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
        itemsLoadedObjectList  = new List<GameObject>();

        workersHiredList = new List<GameObject>();

        itemsLoadedDataList = new List<SO_ItemData>();

        itemsNeededDataList = new List<SO_ItemData>();
    }

    public void SetCurrentGig(SO_GigData _gigData)
    {
        currentGig = _gigData;

        SetItemsNeededForGig(currentGig.itemsNeededForGigList);

        textCurrentGig.text = "Current Gig: " + currentGig.gigName;
    }

    public void SetItemsNeededForGig(List<SO_ItemData> itemData)
    {
        itemsNeededDataList = itemData;
    }

    public List<SO_ItemData> GetItemsNeededDataList()
    {
        return itemsNeededDataList;
    }

    public List<SO_ItemData> GetItemsLoadedDataList()
    {
        return itemsLoadedDataList;
    }

    public void AddItemToGig(GameObject _ItemObject)
    {
        if (CheckForItemDupes(_ItemObject))
            return;

        DontDestroyOnLoad(_ItemObject);

        itemsLoadedObjectList.Add(_ItemObject);

        if(_ItemObject.TryGetComponent<iLoadable>(out iLoadable _loadable))
        {
            itemsLoadedDataList.Add(_loadable.ItemData);
        }
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
}
