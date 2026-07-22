using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class uTruckItem : MonoBehaviour
{
    public string itemName;

    public Image itemImage, background;

    public TextMeshProUGUI textTruckItem;

    public TextMeshProUGUI textItemAmount;

    public void SetTruckItemUI(SO_ItemData _itemData)
    {
        Debug.Log("Setting truck item with " + _itemData);

        itemName = _itemData.itemName;

        itemImage.sprite = _itemData.itemSprite;

        textTruckItem.text = _itemData.itemName;
    }

    public void SetQuantity(int loaded, int needed)
    {
        Debug.Log("Setting quanity to " + loaded + "/" + needed + " for " + itemName);

        textItemAmount.text = $"{loaded}/{needed}";

        background.color = loaded >= needed
            ? Color.green
            : Color.white;
    }
}
