using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sInputChannel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    int index;

    int channelNumber;

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

    public void SetChannel(int _channelNumber, Sprite _connectedSprite, Sprite _disconnectedSprite, string _channelName, uConnectionsAvailablePanel _connectionsAvailablePanel)
    {
        channelNumber = _channelNumber;

        connectedImage = _connectedSprite;

        disconnectedImage = _disconnectedSprite;

        channelText.text = _channelName;

        channelImage.sprite = disconnectedImage;

        connectionAvailablePanel = _connectionsAvailablePanel;
    }

    public void SetIndex(int _index)
    {
        index = _index;
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

            if(!isConnected)
            {
                isConnected = true;

                channelImage.color = Color.yellow;

                connectionPlate.OnClick(connectionAvailablePanel.ReturnTempIndex());
            } 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter Triggered on channel ");

        // Sets a channel to green if a cursor is highlighted over it
        if(connectionAvailablePanel.ReturnIsClicking() == true && !isConnected)
        {
            channelImage.color = Color.green;
            isBeingSelected = true;
        }

        else
        {
            //Debug.Log("Not Clicking");
        }
    }

    

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit Triggered");
        // When cursor fully exits and the channel is not connected
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
            isConnected = false;

            channelImage.color = Color.white;

            // TODO 

            // Disconnect channel here
        }

        // if connections is already connected then it gets disconnected
        //if(isConnected)
        //{
        //    channelImage.color = Color.white;

        //    connectionPlate.OnClickDisconnect(index);
        //}

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
