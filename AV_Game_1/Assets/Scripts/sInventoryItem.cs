using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sInventoryItem : MonoBehaviour
{
    int index;

    sInventory inventory;

    //public string itemName;

    public Image itemImage;

    GameObject pItem;

    public Text textItemName;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<sInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(SO_ItemData _itemData, int _index)
    {
        textItemName.text = _itemData.itemName;
        itemImage.sprite = _itemData.itemSprite;
        pItem = _itemData.prefabItem;
        index = _index;
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }

    public void OnClick()
    {
        //Debug.Log("Inventory Item " + textItemName.ToString() + " button was clicked") ;

        inventory.ItemPick(index);

        Instantiate(pItem, inventory.ReturnBox().transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
