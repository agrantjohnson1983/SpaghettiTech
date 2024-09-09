using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sInventory : MonoBehaviour
{
    //public int numberOfSlots;

    sBox box;

    public GameObject pInventorySlot;

    //SO_ItemData[] itemData;

    public Transform panelInventory;

    List<GameObject> inventoryItemList;

    // Start is called before the first frame update
    void Start()
    {
        inventoryItemList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInventory(SO_ItemData[] _itemData)
    {
        for (int i = 0; i < _itemData.Length; i++)
        {
            sInventoryItem tempItem;
            GameObject tempSlot;

            tempSlot = Instantiate(pInventorySlot, panelInventory);

            tempItem = tempSlot.GetComponent<sInventoryItem>();

            tempItem.SetItem(_itemData[i], i);

            //inventoryItemList.Add(tempSlot);
        }
    }

    public void ItemPick(int _index)
    {
        box.InventoryItemPick(_index);

        // Removes item then re-indexes the inventory
        //inventoryItemList.RemoveAt(_index);

        //for (int i = 0; i < inventoryItemList.Count; i++)
        //{
        //    inventoryItemList[i].GetComponent<sInventoryItem>().SetIndex(i);
        //}
        
        //box.RemoveItemData(_index);

        if (box.ReturnNumberOfInventorySlots() <= 0)
        {
            box.EmptyBox();

            //box.RemoveItemData(_index);

            Destroy(this.gameObject);
        }
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

    public sBox ReturnBox()
    {
        return box;
    }
}
