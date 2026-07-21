using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sTruck : MonoBehaviour
{
    public Vector3 camOffset;

    public Transform camMoveTransform;

    public static bool isLoaded = false;

    public Animator animator;

    public GameObject canvasLoaded;

    public GameObject pTruckItemUI;

    public Transform transformItemNeeded, transformItemLoaded;

    List<SO_ItemData> itemNeededDataList, itemLoadedDataList;

    // Start is called before the first frame update
    void Start()
    {
        HandleTruckUI(false);

        if(GameManager.gm.GetGameMode() == eGameMode.gig)
        {
            List<GameObject> _tempItemObjectList = sGigManager.gigManagerGlobal.itemsLoadedObjectList;

            for (int i = 0; i < _tempItemObjectList.Count; i++)
            {
                _tempItemObjectList[i].transform.position = this.transform.position + Random.insideUnitSphere;
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
        GameManager.gm.StartGig();
    }

    void HandleTruckUI(bool _isOn)
    {
        canvasLoaded.SetActive(_isOn);

        if(_isOn)
        {
            Dictionary<SO_ItemData, int> needed =
            sGigManager.gigManagerGlobal.GetNeededItemCounts();

            Dictionary<SO_ItemData, int> loaded =
                sGigManager.gigManagerGlobal.GetLoadedItemCounts();

            foreach (var pair in needed)
            {
                GameObject obj = Instantiate(pTruckItemUI, transformItemNeeded);

                instantiatedNeededUI.Add(obj);

                uTruckItem ui = obj.GetComponent<uTruckItem>();

                ui.SetTruckItemUI(pair.Key);

                int loadedCount = loaded.TryGetValue(pair.Key, out int count)
                    ? count
                    : 0;

                ui.SetQuantity(count, pair.Value);
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
    }

    List<GameObject> instantiatedNeededUI = new List<GameObject>();
    List<GameObject> instantiatedLoadedUI = new List<GameObject>();

    void ClearUI()
    {
        for (int i = 0; i < instantiatedNeededUI.Count; i++)
            Destroy(instantiatedNeededUI[i]);
        instantiatedNeededUI.Clear();

        for (int i = 0; i < instantiatedLoadedUI.Count; i++)
            Destroy(instantiatedLoadedUI[i]);
        instantiatedLoadedUI.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            Debug.Log("Triggering player entering truck");
            _player.ToggleTruckCamera(true, this.transform, camMoveTransform, 0.5f, camOffset);
            HandleTruckUI(true);
            return;
        }

        else
        {
            Debug.Log("Adding " + other.gameObject + " to gig mgr");
            sGigManager.gigManagerGlobal.AddItemToGig(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent<sPlayerCharacter>(out sPlayerCharacter _player))
        {
            Debug.Log("Triggering player exiting truck");
            _player.ToggleTruckCamera(false, this.transform, null, 0.5f, camOffset);
            HandleTruckUI(false);
        }
    }
}
