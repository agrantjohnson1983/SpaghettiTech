using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uButtonTool : MonoBehaviour
{
    canvasGameplay CanvasGameplay;

    public Image toolImage;
    public Text toolText;

    public SO_EventsUI soUI;
    SO_ItemData itemData;

    

    private void Start()
    {
        CanvasGameplay = GetComponentInParent<canvasGameplay>();
    }

    public void SetButton(SO_ItemData _itemData)
    {
        itemData = _itemData;

        toolImage.sprite = _itemData.itemSprite;

        toolText.text = _itemData.itemName;
    }

    public void OnClick()
    {
        CanvasGameplay.OnToolClick(itemData);
        
        CanvasGameplay.ToggleToolbelt(false);

        // Sets 

        //CanvasGameplay.
    }
}
