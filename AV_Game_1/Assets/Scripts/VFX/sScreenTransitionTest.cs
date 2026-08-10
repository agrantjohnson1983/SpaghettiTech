using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sScreenTransitionTest : MonoBehaviour
{
    public SO_ScreenTransition testTransition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sScreenTransitionManager.Instance.Play(
                testTransition
            );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
