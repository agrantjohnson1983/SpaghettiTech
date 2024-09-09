using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sLight : MonoBehaviour
{
    public GameObject[] poweredObjects
    {
        get;
        set;
    }

    public bool hasPower
    {
        get;
        set;
    }
    // Start is called before the first frame update
    void Start()
    {
        //TogglePoweredObjects(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
