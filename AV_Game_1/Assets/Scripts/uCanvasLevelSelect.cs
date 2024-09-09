using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uCanvasLevelSelect : MonoBehaviour
{
    public string testLevelSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTestLevelClicked()
    {
        SceneManager.LoadScene(testLevelSceneName, LoadSceneMode.Single);
    }
}
