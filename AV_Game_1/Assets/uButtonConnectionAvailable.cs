using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uButtonConnectionAvailable : MonoBehaviour, IPointerDownHandler
{
    public Image connectionImage;
    public Text connectionNumberText;

    int index;

    uConnectionsAvailablePanel connectionsAvailablePanel;
    // Start is called before the first frame update
    void Start()
    {
        connectionsAvailablePanel = GetComponentInParent<uConnectionsAvailablePanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetConnection(Sprite _spriteImage, string _connectionNumber, int _index)
    {
        connectionImage.sprite = _spriteImage;
        connectionNumberText.text = _connectionNumber;
        index = _index;
    }

    public void OnClick()
    {
        //connectionsAvailablePanel.OnConnectionClick(index);
        //Destroy(this.gameObject);
    }

    public void ConnectionMade()
    {
        Destroy(this.gameObject);
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        Debug.Log(name + " clicked");
        connectionsAvailablePanel.OnConnectionClick(index);
    }

    public void OnPointerClick(PointerEventData _eventData)
    {
        Debug.Log(name + "clicked");
    }
}
