using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sInputChannel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    int index;

    bool isConnected;

    public Image channelImage;

    public Text channelText;

    Sprite connectedImage, disconnectedImage;

    bool isBeingSelected = false;

    uConnectionPlate connectionPlate;
    uConnectionsAvailablePanel connectionAvailablePanel;

    // Start is called before the first frame update
    void Start()
    {
        isConnected = false;

        connectionPlate = GetComponentInParent<uConnectionPlate>();

        //connectionAvailablePanel = uConnectionsAvailablePanel.connectionAvailablePanel;
    }

    public void SetChannel(int _index, Sprite _connectedSprite, Sprite _disconnectedSprite, string _channelName, uConnectionsAvailablePanel _connectionsAvailablePanel)
    {
        index = _index;

        connectedImage = _connectedSprite;

        disconnectedImage = _disconnectedSprite;

        channelText.text = _channelName;

        channelImage.sprite = disconnectedImage;

        connectionAvailablePanel = _connectionsAvailablePanel;
    }

    public void OnClick()
    {
        //connectionPlate.OnClick(index);
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && isBeingSelected)
        {
            // TO DO : mouse up connects the input channel with connection available

            Debug.Log("Mouse Click Release When Selected");

            channelImage.color = Color.yellow;

            connectionPlate.OnClick(index);

            isConnected = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter Triggered");

        if(connectionAvailablePanel.ReturnIsClicking() == true)
        {
            channelImage.color = Color.green;
            isBeingSelected = true;
        }

        else
        {
            Debug.Log("Not Clicking");
        }
    }

    

    public void OnPointerExit(PointerEventData eventData)
    {
        if(eventData.fullyExited && !isConnected)
        {
            channelImage.color = Color.white;
            isBeingSelected = false;
        }
        
    }

    public void OnPointerDown(PointerEventData PointerEventData)
    {
        Debug.Log(name + " was down clicked");

        if(isConnected)
        {
            channelImage.color = Color.white;

            connectionPlate.OnClickDisconnect(index);
        }

        //channelImage.color = Color.green;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(name + " was upclicked");

        //channelImage.color = Color.red;
    }

    public void TogglePlug()
    {
        isConnected = !isConnected;

        if(isConnected)
        {
            channelImage.sprite = disconnectedImage;

            channelImage.color = Color.red;
        }

        else
        {
            channelImage.sprite = connectedImage;

            channelImage.color = Color.green;
        }
    }

    public bool ReturnIsConnected()
    {
        return isConnected;
    }
}
