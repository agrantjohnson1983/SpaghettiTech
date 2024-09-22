using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCamLookAt : MonoBehaviour
{
    Camera cam;

    public Vector3 positionOffset;
    //public bool invert;

    //public bool flipRot;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        //transform.LookAt(cam.transform, );

        if (cam == null)
            cam = Camera.main;

        else
            transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        //transform.rotation = transform.rotation * Quaternion.Euler(-1,-1, -1);
        //transform.position = transform.forward + positionOffset;

    }
}
