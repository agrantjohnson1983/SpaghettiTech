using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SO_Text : ScriptableObject
{
    public GameObject pTextPopup;

    uTextPopup textPopup;

    public void SpawnTextPopup(Transform _transform, string _text, int _size)
    {
        textPopup = Instantiate(pTextPopup, _transform.position, Quaternion.identity).GetComponent<uTextPopup>();
        textPopup.SetText(_text, _size);
    }
}
