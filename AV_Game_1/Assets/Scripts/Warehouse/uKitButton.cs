using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uKitButton : MonoBehaviour
{
    sKitBuilder kitBuilder;

    public TextMeshProUGUI itemName;
    public Image itemImage;

    SO_ItemData itemData;

    private void Start()
    {
        kitBuilder = GetComponentInParent<sKitBuilder>();
    }

    public void SetButton(SO_ItemData _ItemData)
    {
        if (_ItemData == null)
        {
            Debug.LogWarning("Setting button with null item data");
            return;
        }

        itemData = _ItemData;

        itemName.text = itemData.itemName;

        itemImage.sprite = itemData.itemSprite;
    }

    public void OnClick()
    {
        if(kitBuilder != null && itemData != null)
        {
            kitBuilder.AddToBox(itemData);
        }

        else
        {
            Debug.LogWarning("Kit Builder or item data is null!");
        }
    }
}
