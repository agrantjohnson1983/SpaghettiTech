using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraTrigger : MonoBehaviour
{
    bool isTriggered = false;

    public bool canTrigger = true;

    private void Start()
    {
        //if (disableOnStart)
        //    this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<sCameraController>(out sCameraController _cameraController) && !isTriggered && canTrigger)
        {
            isTriggered = true;

            _cameraController.ToggleForwardCamera(true, 0.5f);

            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<sCameraController>(out sCameraController _cameraController) && isTriggered)
        {
            //Debug.Log("Triggering player exiting truck");

            isTriggered = false;

            _cameraController.ToggleForwardCamera(false, 0.5f);
        }
    }
}
