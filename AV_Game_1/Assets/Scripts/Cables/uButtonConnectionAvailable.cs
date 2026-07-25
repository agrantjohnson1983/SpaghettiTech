using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uButtonConnectionAvailable : MonoBehaviour, IPointerDownHandler
{
    public Image connectionImage;
    public Text connectionNumberText;

    //public GameObject backGround;

    Image backgroundImage;

    int index;

    uConnectionsAvailablePanel connectionsAvailablePanel;

    bool isConnected = false;

    // Start is called before the first frame update



    void Start()
    {
        //Debug.Log("Button available spawnned");

        connectionsAvailablePanel = GetComponentInParent<uConnectionsAvailablePanel>();

        backgroundImage = GetComponent<Image>();

        backgroundImage.color = Color.green;
    }

    // This will set a connections image, the channel number and also give it the index associated with the pluggable lists
    public void SetConnection(Sprite _spriteImage, string _connectionNumber, int _index)
    {
        connectionImage.sprite = _spriteImage;
        connectionNumberText.text = _connectionNumber;
        index = _index;
    }

    // This gets called by the connection available panel.  Changes the isConnected bool and switches the color
    public void OnChannelClick()
    {
        if(!isConnected)
        {
            //Debug.Log("Connection available button clicked");

            isConnected = true;

            backgroundImage.color = Color.yellow;
        }   
    }

    // Use this for disconnecting a connection
    public void OnClick()
    {
        if(isConnected)
        {
            //Debug.Log("Disconnecting Connection Available");

            isConnected = false;

            backgroundImage.color = Color.green;

            //connectionsAvailablePanel.ReturnConnectionPlate().

            // Sends a disconnnect to connections available which removes the connected line renderer
            connectionsAvailablePanel.DisconnectLine(index);

            // Sends a disconnect to the channel on the plate the connection available is connected to
            connectionsAvailablePanel.ReturnConnectionPlate().DisconnectChannel(index);
        }
    }

    // Use this for connecting a connection - DO NOT TOGGLE isConnected
    public void OnPointerDown(PointerEventData _eventData)
    {
        if(!isConnected)
        {
            //Debug.Log(name + " clicked");
            
            connectionsAvailablePanel.OnConnectionClick(index);
        }      
    }

    // Returns if the button is connected to anything
    public bool ReturnIsConnected()
    {
        return isConnected;
    }

    // Returns the index
    public int ReturnIndex()
    {
        return index;
    }    
}
