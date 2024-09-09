using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uCanvasFrontEnd : MonoBehaviour
{
    public GameObject levelSelectCanvas;
    bool hasClicked = false;

    public GameObject clickToContinue, titleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickToContinue()
    {
        if(!hasClicked)
        {
            hasClicked = true;
            Instantiate(levelSelectCanvas, this.transform);
            clickToContinue.SetActive(false);
            titleText.SetActive(false);
        }
        
    }
}
