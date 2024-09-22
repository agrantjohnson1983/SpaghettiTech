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
        Debug.Log("Button available spawnned");

        connectionsAvailablePanel = GetComponentInParent<uConnectionsAvailablePanel>();

        backgroundImage = GetComponent<Image>();

        backgroundImage.color = Color.green;
    }

    public void SetConnection(Sprite _spriteImage, string _connectionNumber, int _index)
    {
        connectionImage.sprite = _spriteImage;
        connectionNumberText.text = _connectionNumber;
        index = _index;
    }

    // This gets called when a click release happens on an available channel
    public void OnChannelClick()
    {
        Debug.Log("Connection available button clicked");
        //connectionsAvailablePanel.OnConnectionClick(index);
        //Destroy(this.gameObject);
        isConnected = true;

        backgroundImage.color = Color.yellow;
    }

    public void OnClick()
    {
        if(isConnected)
        {
            Debug.Log("Disconnecting Connection Available");

            isConnected = false;

            backgroundImage.color = Color.yellow;
        }
    }


    public void OnPointerDown(PointerEventData _eventData)
    {
        Debug.Log(name + " clicked");
        connectionsAvailablePanel.OnConnectionClick(index);      
    }

    //public void OnPointerClick(PointerEventData _eventData)
    //{
    //    Debug.Log(name + "clicked");
    //}

    public bool ReturnIsConnected()
    {
        return isConnected;
    }

    public int ReturnIndex()
    {
        return index;
    }    
}
