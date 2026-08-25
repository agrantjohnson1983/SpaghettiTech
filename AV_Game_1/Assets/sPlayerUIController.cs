using UnityEngine;
using UnityEngine.InputSystem;

public class sPlayerUIController : MonoBehaviour
{
    public PlayerInput playerInput;

    GameObject currentPopup;
    GameObject selectedObject;

    public void OpenPopup(GameObject popup, GameObject firstSelected)
    {
        if(currentPopup != null)
        {
            currentPopup = popup;
            currentPopup.SetActive(true);
        }

        if(firstSelected != null)
            Select(firstSelected);
    }

    public void Select(GameObject obj)
    {
        selectedObject = obj;
    }

    public void ClosePopup()
    {
        if (currentPopup != null)
            currentPopup.SetActive(false);

        currentPopup = null;
        selectedObject = null;
    }
}