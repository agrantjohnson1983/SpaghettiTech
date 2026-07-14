using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uTruckItem : MonoBehaviour
{
    public Image itemImage;

    public TextMeshProUGUI textTruckItem;

    public void SetTruckItemUI(SO_ItemData _itemData)
    {
        Debug.Log("Setting truck item with " + _itemData);

        itemImage.sprite = _itemData.itemSprite;

        textTruckItem.text = _itemData.itemName;
    }
}
