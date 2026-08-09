using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uCanvasFrontEnd : MonoBehaviour
{
    public GameObject levelSelectCanvas;
    bool hasClicked = false;

    public GameObject clickToContinue, titleText;

    public string sceneToLoad;

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
            //Instantiate(levelSelectCanvas, this.transform);
            clickToContinue.SetActive(false);
            titleText.SetActive(false);

            SceneManager.LoadScene(sceneToLoad);
        }
        
    }
}
