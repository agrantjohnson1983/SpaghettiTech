using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uRingBehavior : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        this.transform.position = gameObject.transform.parent.transform.position;

        this.transform.rotation = Quaternion.Euler(-90, 0f, 0f);

    }
}
