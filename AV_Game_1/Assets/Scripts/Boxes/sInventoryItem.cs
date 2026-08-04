using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class sInventoryItem : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    sInventory inventory;

    //public string itemName;

    public Image itemImage;

    GameObject pItem;

    public TMP_Text textItemName;

    SO_ItemData itemData;

    public GameObject toolTip;
    public TMP_Text toolText;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<sInventory>();
    }

    public void SetItem(SO_ItemData _itemData)
    {
        itemData = _itemData;
        textItemName.text = _itemData.itemName;
        itemImage.sprite = _itemData.itemSprite;
        pItem = _itemData.prefabItem;
        toolText.text = _itemData.description;
    }


    public void OnClick()
    {
        //Debug.Log("Inventory Item " + textItemName.ToString() + " button was clicked") ;

        inventory?.ItemPicked(itemData);

        //Instantiate(pItem, inventory.ReturnBox().transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Inventory Item pointer enter");

        //bg.color = hoverColor;
        toolTip?.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Inventory Item pointer exit");

        //bg.color = normalColor;
        toolTip?.SetActive(false);
    }
}
