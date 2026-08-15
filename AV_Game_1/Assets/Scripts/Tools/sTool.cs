using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eToolType { NONE, ratchet, tape, bolt, nut, crescent,  }
public class sTool : MonoBehaviour
{
    Rigidbody rb;

    // This holds all the info for the tool
    public SO_ToolData toolData;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Ground")))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = Vector3.zero;
        }
    }
}
