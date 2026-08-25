using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraController : MonoBehaviour
{

    bool isInTruckMode = false;

    CinemachineVirtualCamera vCam;

    private Vector3 camOffset, camStart;

    Camera cam;

    Vector3 camPos;
    Quaternion camRot;

    private void Awake()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camStart = vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        cam = GameManager.gm.ReturnCameraGameplay().GetComponentInChildren<Camera>();

        if (cam == null)
            cam = GameManager.gm.ReturnCameraGameplay().GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Coroutine truckCamRoutine;

    public void ToggleForwardCamera(Vector3 _camOffset, bool _isOn, float _moveTime)
    {
        // Ignore duplicate calls - already in the requested state
        if (_isOn == isInTruckMode)
        {
            Debug.LogWarning($"ToggleTruckCamera({_isOn}) ignored - already in that state.");
            return;
        }

        isInTruckMode = _isOn;

        if (truckCamRoutine != null)
            StopCoroutine(truckCamRoutine);

        camOffset = _camOffset;

        if (_isOn)
        {
            camPos = cam.transform.localPosition;
            camRot = cam.transform.localRotation;

            cam.transform.SetParent(null);

            truckCamRoutine = StartCoroutine(CamMovementWorld(0.5f, true));
        }

        else
        {
            cam.transform.SetParent(this.gameObject.transform);
            cam.transform.localRotation = camRot;

            truckCamRoutine = StartCoroutine(CamMovementLocal(_moveTime, false));
        }
    }

    // Used only while unparented (entry) — world space lerp is correct here
    IEnumerator CamMovementWorld(float _time, bool _isOn)
    {
        Debug.Log("Starting truck movement");

        float _counter = 0f;

        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        if (_isOn)
        {
            startPoint = camStart;
            endPoint = camOffset;
        }

        else
        {
            startPoint = camOffset;
            endPoint = camStart;
        }

        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        while (_counter < _time)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startPoint, endPoint, _counter / _time);
            _counter += Time.deltaTime;
            yield return null;
        }

        //cam.transform.position = _endPos; // snap, same pattern as sCharacterMover fix

        //if (_isOn)
        //    cam.transform.LookAt(this.transform);
    }

    // Used only while parented (exit) — local space lerp tracks the moving player
    IEnumerator CamMovementLocal(float _time, bool _isOn)
    {
        float _counter = 0f;

        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        if (_isOn)
        {
            startPoint = camStart;
            endPoint = camOffset;
        }

        else
        {
            startPoint = camOffset;
            endPoint = camStart;
        }

        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();

        while (_counter < _time)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startPoint, endPoint, _counter / _time);
            _counter += Time.deltaTime;
            yield return null;
        }

        //Debug.Log($"Reset complete. localPosition = {cam.transform.localPosition}, expected = {_endLocalPos}");

        // camera.transform.localPosition = _endLocalPos; // snap to guarantee exact reset
    }


    public void ToggleCameraMain(bool _isOn)
    {
        cam.gameObject.SetActive(_isOn);
    }
}
