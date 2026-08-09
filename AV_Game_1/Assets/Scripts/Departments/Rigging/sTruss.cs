using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTruss : sRigGear
{
    private void Start()
    {
        sRiggingManager.riggingMangerGlobal.trussList.Add(this);
    }
}
