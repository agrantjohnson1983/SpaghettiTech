using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class sCameraShake : MonoBehaviour
{
    public static sCameraShake instance;

    public bool isShaking = false;

    CinemachineVirtualCamera cam;

    float shakerTimer;

    private void Awake()
    {
        instance = this;

        cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakerTimer > 0)
        {
            shakerTimer -= Time.deltaTime;

            if(shakerTimer <= 0f)
            {
                // TIME OVER

                isShaking = false;

                CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                camPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void CameraShake(float _intensity, float _time)
    {
        //Debug.Log("Camera shook!");

        isShaking = true;

        CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        camPerlin.m_AmplitudeGain = _intensity;

        shakerTimer = _time;

    }
}
