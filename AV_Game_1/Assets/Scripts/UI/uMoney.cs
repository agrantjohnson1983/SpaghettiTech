using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uMoney : MonoBehaviour
{
    public static uMoney moneyGlobal;

    public GameObject moneyUI;

    public float moneyStarting = 100000;
    float moneyCurrent;

    public TextMeshProUGUI textMoney;

    public SO_EventsUI soUI;

    private void Awake()
    {
        if (moneyGlobal == null)
            moneyGlobal = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        moneyCurrent  = moneyStarting;
        textMoney.text = "$" + moneyCurrent;
    }

    private void OnEnable()
    {
        soUI.crewHire.AddListener(CrewHire);
    }

    private void OnDisable()
    {
        soUI.crewHire.RemoveListener(CrewHire);
    }

    void CrewHire(SO_CrewProfile _crew)
    {
        MoneyChange(-_crew.hireCost);
    }

    void MoneyChange(float _amount)
    {
        if(moneyUI.activeInHierarchy)
        {
            moneyCurrent += _amount;
            textMoney.text = "$" + moneyCurrent;
        }

        else
        {
            Debug.LogWarning("Money change was triggered but money UI is not active");
        }
    }

    public void ToggleUI(bool _isOn)
    {
        moneyUI.SetActive(_isOn);
    }

}
