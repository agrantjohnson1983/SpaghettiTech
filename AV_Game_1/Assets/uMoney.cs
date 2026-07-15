using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uMoney : MonoBehaviour
{
    public static uMoney moneyGlobal;

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
        soUI.characterHire.AddListener(MoneyChange);
    }

    private void OnDisable()
    {
        soUI.characterHire.RemoveListener(MoneyChange);
    }

    void MoneyChange(float _amount)
    {
        moneyCurrent += _amount;
        textMoney.text = "$" + moneyCurrent;
    }

}
