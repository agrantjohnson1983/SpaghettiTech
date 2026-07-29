using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uPopupControls : MonoBehaviour
{
    public Image BG;
    public Text textControl, textAction;

    // Start is called before the first frame update
    public void SetPopup(string _textControl, string _textAction)
    {
        Debug.Log("setting controls popup for " + _textAction);

        textControl.text = _textControl;
        textAction.text = _textAction;
    }
}
