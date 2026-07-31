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
    SO_ToolData toolData;

    

    private void Start()
    {
        CanvasGameplay = GetComponentInParent<canvasGameplay>();
    }

    public void SetButton(SO_ToolData _toolData)
    {
        toolData = _toolData;

        toolImage.sprite = _toolData.itemSprite;

        toolText.text = _toolData.itemName;
    }

    public void OnClick()
    {
        CanvasGameplay.OnToolClick(toolData);
        
        CanvasGameplay.ToggleToolbelt(false);

        // Sets 

        //CanvasGameplay.
    }
}
