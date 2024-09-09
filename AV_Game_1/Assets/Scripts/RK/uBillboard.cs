using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uBillboard : MonoBehaviour
{
    public enum eBillboardType { LookAt, CameraForward}

    [SerializeField]
    public eBillboardType type;

    [SerializeField] private bool lockX, lockY, lockZ;

    Vector3 originalRot;

    public Vector3 lookAtOffset;

    private void Awake()
    {
        originalRot = transform.localRotation.eulerAngles;
    }

    private void LateUpdate()
    {
        if(GameManager.gm)
        {
            switch (type)
            {
                case eBillboardType.CameraForward:

                    transform.LookAt(Camera.main.transform.position, Vector3.up);
                    

                    break;

                case eBillboardType.LookAt:

                    transform.forward = Camera.main.transform.forward;

                    break;
            }

            

            /*
            Vector3 rot = transform.localRotation.eulerAngles;

            if (lockX) { rot.x = originalRot.x; }

            if (lockY) { rot.y = originalRot.y; }

            if(lockZ) { rot.z = originalRot.z; }

            transform.localRotation = Quaternion.Euler(rot);
            */
        }
    }
}
