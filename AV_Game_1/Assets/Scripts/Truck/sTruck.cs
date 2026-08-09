using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class sTruck : MonoBehaviour
{
    public Vector3 camOffset;

    public Transform camMoveTransform;

    public static bool isLoaded = false;

    public Animator animator;

    public GameObject canvasLoaded;

    public GameObject pTruckItemUI;

    public Transform transformItemUI;

    List<GameObject> instantiatedNeededUI = new List<GameObject>();

    bool isInTruck = false;

    //bool isUpdatingUI;

    public TextMeshProUGUI textLoaded;
    public string textLoadedMessage = "LOADED!";

    public GameObject textHatchClose;
    //List<SO_ItemData> itemNeededDataList, itemLoadedDataList;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedNeededUI = new List<GameObject>();

        HandleTruckUI(false);

        if(GameManager.gm.GetGameMode() == eGameMode.gig)
        {
            List<GameObject> _tempItemObjectList = sGigManager.gigManagerGlobal.itemsLoadedObjectList;

            for (int i = 0; i < _tempItemObjectList.Count; i++)
            {
                _tempItemObjectList[i].transform.position = this.transform.position + Random.insideUnitSphere + Vector3.up;
            }
        }
    }

    public void Drive()
    {
        animator.SetTrigger("Drive");

        sGigManager.gigManagerGlobal.TruckDrive();
    }

    // This is getting called by an animator event
    public void SceneChange()
    {
        GameManager.gm.StartTravel();

        
    }

    void HandleTruckUI(bool _isOn)
    {
        //isUpdatingUI = true;

        Debug.Log("Setting Truck UI to: " + _isOn);

        canvasLoaded.SetActive(_isOn);

        if(_isOn)
        {
            // checks to see if you have all items - will get toggled to false if not
            bool hasAll = true;

            Dictionary<SO_ItemData, int> needed =
                sGigManager.gigManagerGlobal.GetNeededItemCounts();
            

            Dictionary<SO_ItemData, int> loaded =
                sGigManager.gigManagerGlobal.GetLoadedItemCounts();

            foreach (var pair in needed)
            {
                //Debug.Log("Spawning truck UI");

                GameObject obj = Instantiate(pTruckItemUI, transformItemUI);

                instantiatedNeededUI.Add(obj);

                uTruckItem ui = obj.GetComponent<uTruckItem>();

                ui.SetTruckItemUI(pair.Key);

                int loadedCount = loaded.TryGetValue(pair.Key, out int count)
                    ? count
                    : 0;

                ui.SetQuantity(count, pair.Value);

                if((count/pair.Value) < 1)
                {
                    int numberNeeded = pair.Value - count;

                    //Debug.Log("Still need to get " + numberNeeded + " " + pair.Key + "s");
                    hasAll = false;
                }
            }

            if(hasAll)
            {
                Debug.Log("LET'S GO MOFO!");
                textLoaded.text = textLoadedMessage;
                textLoaded.color = Color.green;
                textHatchClose.SetActive(true);
            }

            //itemLoadedDataList = new List<SO_ItemData>();

            //itemLoadedDataList = sGigManager.gigManagerGlobal.GetItemsLoadedDataList();

            //itemNeededDataList = new List<SO_ItemData>();

            //itemNeededDataList = sGigManager.gigManagerGlobal.GetItemsNeededDataList();

            //for (int i = 0; i < itemNeededDataList.Count; i++)
            //{
            //    GameObject _obj = Instantiate(pItemNeededForGigUI, transformItemNeeded);
            //    instantiatedNeededUI.Add(_obj);
            //    _obj.GetComponent<uTruckItem>().SetTruckItemUI(itemNeededDataList[i]);
            //}

            //for (int i = 0; i < itemLoadedDataList.Count; i++)
            //{
            //    GameObject _obj = Instantiate(pItemLoadedUI, transformItemLoaded);
            //    instantiatedLoadedUI.Add(_obj);
            //    _obj.GetComponent<uTruckItem>().SetTruckItemUI(itemLoadedDataList[i]);
            //}
        }

        else
        {
            ClearUI();
        }

        //isUpdatingUI = false;
    }

    
    //List<GameObject> instantiatedLoadedUI = new List<GameObject>();

    void ClearUI()
    {
        //Debug.Log("Clearing UI");

        for (int i = 0; i < instantiatedNeededUI.Count; i++)
            Destroy(instantiatedNeededUI[i]);
        instantiatedNeededUI.Clear();

        /*for (int i = 0; i < instantiatedLoadedUI.Count; i++)
            Destroy(instantiatedLoadedUI[i]);
        instantiatedLoadedUI.Clear();*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player) && !isInTruck)
        {
            //Debug.Log("Triggering player entering truck");
            isInTruck = true;

            _player.ToggleTruckCamera(true, 0.5f);

            if (sGigManager.gigManagerGlobal.CheckIfHasAGig())
                HandleTruckUI(true);
            return;
        }

        // Checks boxes for SO_Items and adds them to gig list
        else if(other.TryGetComponent<sBox>(out sBox _box))
        {
            if(_box.boxedItemDataList != null)
            {
                sGigManager.gigManagerGlobal.AddItemToGig(_box.gameObject);
            }
        }

        else if(other.TryGetComponent<iLoadable>(out iLoadable _loadable))
            {
                sGigManager.gigManagerGlobal.AddItemToGig(other.gameObject);
                //sGigManager.gigManagerGlobal.AddItemToGig(other.gameObject);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player) && isInTruck)
        {
            //Debug.Log("Triggering player exiting truck");
            isInTruck = false;
            _player.ToggleTruckCamera(false, 0.5f);
            HandleTruckUI(false);
        }
    }
}
