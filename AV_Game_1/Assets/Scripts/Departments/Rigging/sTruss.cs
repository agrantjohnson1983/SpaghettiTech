using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTruss : sRigGear
{
    public override void Start()
    {
        base.Start();
        sRiggingManager.riggingMangerGlobal.trussList.Add(this);
    }
}
