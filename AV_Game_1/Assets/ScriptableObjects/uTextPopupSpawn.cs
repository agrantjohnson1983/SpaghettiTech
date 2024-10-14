using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class uTextPopupSpawn : ScriptableObject
{

    public GameObject pTextPopup;

    uTextPopup textPopup;

    public UnityEvent<string> canvasMessageEvent;

    private void OnEnable()
    {
        if(canvasMessageEvent == null)
        {
            canvasMessageEvent = new UnityEvent<string>();
        }
    }

    public void SpawnTextPopup(Transform _transform, string _text, int _size)
    {
        textPopup = Instantiate(pTextPopup, _transform.position, Quaternion.identity).GetComponent<uTextPopup>();
        textPopup.SetText(_text, _size);
    }

    public void SpawnCanvasMessage(string _message)
    {
        canvasMessageEvent.Invoke(_message);
    }

}
