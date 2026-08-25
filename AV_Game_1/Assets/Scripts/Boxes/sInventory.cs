using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class sInventory : MonoBehaviour

{
    //public int numberOfSlots;
    public SO_AudioEventChannel soAudio;

    sBox box;

    public GameObject pInventorySlot;

    //SO_ItemData[] itemData;

    public Transform panelInventory;

    List<GameObject> inventoryItemList;

    [Header("UI")]
    [SerializeField] RectTransform background;
    [SerializeField] GridLayoutGroup grid;

    [Header("Sizing")]
    [SerializeField] int maxColumns = 4;
    [SerializeField] float padding = 20f;

    sPlayerUIController controller;

    // Start is called before the first frame update
    void Start()
    {
        inventoryItemList = new List<GameObject>();
    }

    public void SetInventory(SO_ItemData[] _itemData)
    {

        for (int i = 0; i < _itemData.Length; i++)
        {
            sInventoryItem tempItem;

            GameObject tempSlot;

            tempSlot = Instantiate(pInventorySlot, panelInventory);

            tempItem = tempSlot.GetComponentInChildren<sInventoryItem>();

            tempItem.SetItem(_itemData[i]);

            //inventoryItemList.Add(tempSlot);

            //ResizeInventory();

            if (i == 0 && controller != null)
            {
                Debug.Log("Setting first inventory item to be first selected");

                if(controller != null)
                    controller.OpenPopup(null, tempSlot);
            }

            //GameManager.gm.SetFirstSelected(tempSlot);
        }
    }

    public void ResetInventory()
    {
        foreach(GameObject item in inventoryItemList)
        {
            Destroy(item);
            
        }

        inventoryItemList.Clear();
        inventoryItemList = new List<GameObject>();
    }

    void ResizeInventory()
    {
        int count = panelInventory.childCount;

        if (count == 0)
            return;

        int columns = Mathf.Min(maxColumns, count);
        int rows = Mathf.CeilToInt((float)count / columns);

        Vector2 cell = grid.cellSize;
        Vector2 spacing = grid.spacing;

        float width =
            padding * 2 +
            columns * cell.x +
            (columns - 1) * spacing.x;

        float height =
            padding * 2 +
            rows * cell.y +
            (rows - 1) * spacing.y;

        background.sizeDelta = new Vector2(width, height);
    }

    public void ItemPicked(SO_ItemData item)
    {
        Instantiate(item.prefabItem,
            box.transform.position,
            Quaternion.identity);

        box.RemoveItemData(item);

        if (soAudio != null)
            soAudio.TriggerSFX("BoxItemClick");

        box.CloseBox();
    }

    public void OnClickX()
    {
        box.CloseBox();
        //Destroy(this.gameObject);
    }

    public void SetBox(sBox _box)
    {
        box = _box;
    }

    public void SetController(sPlayerUIController _controller)
    {
        Debug.Log("Setting UI controller in inventory");
        controller = _controller;
    }

    public sBox ReturnBox()
    {
        return box;
    }
}
