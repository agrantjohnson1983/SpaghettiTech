using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class uGigButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SO_GigData gigData;

    public GameObject gigDescription;

    public Image gigImage;

    public TextMeshProUGUI textGigName, textGigDescription, textGigPay;

    // Start is called before the first frame update
    void Start()
    {
        SetButton();

        gigDescription.SetActive(false);
    }

    public void SetButton()
    {
        textGigName.text = gigData.gigName;
        textGigDescription.text = gigData.gigDescription;
        textGigPay.text = "$"+ gigData.basePay.ToString();
        gigImage.sprite = gigData.gigSprite;
    }

    public void OnClick()
    {
        Debug.Log("Gig Button was clicked");

        sGigManager.gigManagerGlobal.SetCurrentGig(gigData);

        this.transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerEnter)
            gigDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.fullyExited)
            gigDescription.SetActive(false);
    }
}
