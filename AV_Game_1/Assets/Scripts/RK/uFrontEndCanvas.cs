using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum eScene {  mainMenu, testLevel }

public class uFrontEndCanvas : MonoBehaviour
{

    GameManager gm;

    float highScoreScreenSwitchTime = 10f;
    bool highScoreScreenOn = false;

    public GameObject highScoreObject;

    string highScoreName;

    string highScoreAmount;

    public Text tHighScoreName;

    public Text tHighScoreAmout;

    //float currentHighScore;

    public Button startButton;

    public string startScene = "testLevel";

    public GameObject loadingPanel;
    public Slider loadingBar;

    //public uHighScoreManager highScoreManager;

    // Start is called before the first frame update
    void Start()
    {

        gm = GameManager.gm;

        this.transform.parent = gm.transform;

        loadingBar.value = 0f;
        loadingPanel.SetActive(false);

        //highScoreName = gm.ReturnHighScoreName();

        //highScoreManager = GetComponentInChildren<uHighScoreManager>();

        highScoreObject.SetActive(highScoreScreenOn);

        StartCoroutine(HighScorePageSwitch());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighScore();
    }

    public void OnStartButton()
    {
        eScene scene = eScene.testLevel;

        StartCoroutine(LoadAsynchronously((int)scene));
    }

    IEnumerator LoadAsynchronously(int _sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneIndex);

        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress;
            progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.value = progress;

            yield return null;
        }
    }

    IEnumerator HighScorePageSwitch()
    {
        yield return new WaitForSeconds(highScoreScreenSwitchTime);

        highScoreScreenOn = !highScoreScreenOn;

        highScoreObject.SetActive(highScoreScreenOn);

        StartCoroutine(HighScorePageSwitch());
    }


    public void UpdateHighScore()
    {
        float highScore;

        //highScore = gm.ReturnHighScore();

        // CONVERTS TO MINUTES + SECONDS
        //highScoreAmount = ((int)highScore / 60).ToString() + ":" + (highScore % 60).ToString("f2");

        //tHighScoreAmout.text = highScore.ToString();

        //highScoreName = gm.ReturnHighScoreName();

        tHighScoreName.text = highScoreName;

    }

    public void NameInput(string _name)
    {

        Debug.Log("I'm a rooster named " + _name);

        //gm.ChangeCurrentName(_name);

        PlayerPrefs.SetString("CurrentName", _name);

    }

}
