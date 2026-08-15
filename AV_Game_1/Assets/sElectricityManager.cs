using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sElectricityManager : sDepartmentManager
{
    public static sElectricityManager electricityManagerGlobal;

    private void Awake()
    {
        if (electricityManagerGlobal == null)
            electricityManagerGlobal = this;
        else
            Destroy(this.gameObject);
    }
}
