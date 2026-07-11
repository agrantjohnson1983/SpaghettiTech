using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTruss : sRigGear
{
    private void Start()
    {
        sRiggingManager.riggingManger.trussList.Add(this);
    }
}
