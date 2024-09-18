using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uButtonTool : MonoBehaviour
{
    canvasGameplay CanvasGameplay;

    public Image toolImage;

    SO_EventsUI soUI;

    private void Start()
    {
        CanvasGameplay = GetComponentInParent<canvasGameplay>();
    }

    public void SetImage(Sprite _sprite)
    {
        toolImage.sprite = _sprite;
    }

    public void OnClick()
    {
        CanvasGameplay.ToggleToolbelt(false);

        // Sets 

        //CanvasGameplay.
    }
}
