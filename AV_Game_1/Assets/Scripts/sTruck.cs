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

    public GameObject pItemNeededForGigUI, pItemLoadedUI;

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
                _tempItemObjectList[i].transform.position = this.transform.position * Random.insideUnitCircle;
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
            ClearUI();

            itemLoadedDataList = new List<SO_ItemData>();

            itemLoadedDataList = sGigManager.gigManagerGlobal.GetItemsLoadedDataList();

            itemNeededDataList = new List<SO_ItemData>();

            itemNeededDataList = sGigManager.gigManagerGlobal.GetItemsNeededDataList();

            for (int i = 0; i < itemNeededDataList.Count; i++)
            {
                uTruckItem _tempItem;
                _tempItem = Instantiate(pItemNeededForGigUI, transformItemNeeded).GetComponent<uTruckItem>();
                _tempItem.SetTruckItemUI(itemLoadedDataList[i]);
            }

            for (int i = 0; i < itemLoadedDataList.Count; i++)
            {
                uTruckItem _tempItem;
                _tempItem = Instantiate(pItemLoadedUI, transformItemLoaded).GetComponent<uTruckItem>();
                _tempItem.SetTruckItemUI(itemLoadedDataList[i]);
            }
        }
    }

    void ClearUI()
    {
        if(itemLoadedDataList != null)
            for (int i = itemLoadedDataList.Count; i >= 1; i--)
            {
                Debug.Log("Destorying item loaded data at index " + i);
                itemLoadedDataList.RemoveAt(i);
                Destroy(itemLoadedDataList[i]);
            }

        if(itemNeededDataList != null)
            for (int i = itemNeededDataList.Count; i >= 1; i--)
            {
                Debug.Log("Destorying item needed data at index " + i);
                itemNeededDataList.RemoveAt(i);
                Destroy(itemNeededDataList[i]);
            }
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
