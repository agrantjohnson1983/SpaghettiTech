using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sTravelManager : MonoBehaviour
{
[Header("Gig Scene")]
string gigSceneName;


private AsyncOperation loadingOperation;

public bool GigLoaded { get; private set; }

private void Start()
{
    gigSceneName = sGigManager.gigManagerGlobal.GetGigScene();

    StartCoroutine(LoadGigScene());
}

private IEnumerator LoadGigScene()
{
    loadingOperation = SceneManager.LoadSceneAsync(
        gigSceneName,
        LoadSceneMode.Single
    );

    loadingOperation.allowSceneActivation = false;

    while (!loadingOperation.isDone)
    {
        float progress = Mathf.Clamp01(
            loadingOperation.progress / 0.9f
        );

        //Debug.Log("Gig Loading: " + progress);

        if (progress >= 1f)
        {
            GigLoaded = true;
            //Debug.Log("Gig scene loaded and waiting for activation.");
        }

        yield return null;
    }
}

public void ArriveAtGig()
{
    if (!GigLoaded)
    {
        Debug.Log("Arrived at gig, but scene is still loading.");
        return;
    }

    Debug.Log("Arrived at gig! Activating scene.");

    GameManager.gm.ArriveAtGig();
    
    loadingOperation.allowSceneActivation = true;

    //GameManager.gm.StartGig();
    }

}
