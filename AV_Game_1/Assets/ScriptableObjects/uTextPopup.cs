using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uTextPopup : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        // disconnects this from any parent game objects
        transform.parent = null;
    }

    // This sets the text and text size
    public void SetText(string _text, int _size)//, int _textSize)
    {
        textMeshPro.text = _text;
        textMeshPro.fontSize = _size;
    }

    // This gets called by an animation event in the text popup
    public void KillUI()
    {
        Destroy(this.gameObject);
    }

}
