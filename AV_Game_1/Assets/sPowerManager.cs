using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPowerManager : sDepartmentManager
{
    public static sPowerManager powerManagerGlobal;

    private void Awake()
    {
        if(powerManagerGlobal == null)
        {
            powerManagerGlobal = this;
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }



}
