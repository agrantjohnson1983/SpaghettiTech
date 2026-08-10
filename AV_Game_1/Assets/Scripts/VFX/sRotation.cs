using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sRotation : MonoBehaviour
{
    public float speed = 10f;


    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 360, 0) * speed * Time.deltaTime);
    }
}
