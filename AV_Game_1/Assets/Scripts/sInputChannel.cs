using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class sInputChannel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    int index = -1;

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

    // This gets used to set a channel after it gets spawned
    public void SetChannel(int _channelNumber, Sprite _connectedSprite, Sprite _disconnectedSprite, string _channelName, uConnectionsAvailablePanel _connectionsAvailablePanel)
    {
        channelNumber = _channelNumber;

        connectedImage = _connectedSprite;

        disconnectedImage = _disconnectedSprite;

        channelText.text = _channelName;

        channelImage.sprite = disconnectedImage;

        connectionAvailablePanel = _connectionsAvailablePanel;
    }

    // This is used to set a channels index
    public void SetIndex(int _index)
    {
        Debug.Log("Setting Index of " + " to channel " + channelNumber);
        index = _index;
    }


    // This returns a channels index
    public int ReturnIndex()
    {
        return index;
    }

    // This gets called when a input channel button gets clicked directly
    public void OnClick()
    {
        if(isConnected)
        {
            TogglePlug();
        }
        //connectionPlate.OnClick(index);
    }

    private void Update()
    {
        // This is used to Detect Mouse up-click after the mouse is already held and line rendered.  It connects the connection and plug.
        
        if(Input.GetMouseButtonUp(0) && isBeingSelected)
        {
            // TO DO : mouse up connects the input channel with connection available

            Debug.Log("Mouse Click Release When a Pluge is Selected - Toggling Plug");

            TogglePlug();
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

    
    // This gets called when the mouse exits the button - Just used for toggling the isBeingSelected

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit Triggered");

        // When cursor fully exits and the channel is not connected
        if(eventData.fullyExited && !isConnected)
        {
            channelImage.color = Color.white;
            isBeingSelected = false;
            //TogglePlug();
        } 
    }

    
    public void OnPointerDown(PointerEventData PointerEventData)
    {
        //Debug.Log(name + " was down clicked");

        // if connections is already connected then it gets disconnected
        

        //channelImage.color = Color.green;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(name + " was upclicked");
    }

    

    public void TogglePlug()
    {
        isConnected = !isConnected;

        if (!isConnected)
        {
            Debug.Log("Toggling plug disconnect");

            // changes the image to disconnected image
            channelImage.sprite = disconnectedImage;

            // changes the image color to green to show it is available
            channelImage.color = Color.white;

            // This disconnects button from connection plate
            connectionPlate.OnClickDisconnect(index);

            // This disconnects the other button that is the connection available
            connectionPlate.ReturnConnectionAvailablePanel().DisconnectLine(index);
        }

        else
        {
            Debug.Log("Toggling plug connect");

            // changes the image to connected image
            channelImage.sprite = connectedImage;

            //changes the image color to green to show it is connected
            channelImage.color = Color.yellow;

            // This connects button from connection plate
            connectionPlate.OnClickConnect(connectionAvailablePanel.ReturnTempIndex());

            // This connects 
            connectionPlate.ReturnConnectionAvailablePanel().OnConnectionClick(index);
        }

        
    }

    public bool ReturnIsConnected()
    {
        return isConnected;
    }
}
