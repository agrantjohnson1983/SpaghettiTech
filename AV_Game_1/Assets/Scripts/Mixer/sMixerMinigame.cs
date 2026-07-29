using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sMixerMinigame : MonoBehaviour
{
    public MixerChannel target = new MixerChannel();
    public MixerChannel player = new MixerChannel();

    [Header("UI")]
    public Slider bassSlider;
    public Slider midSlider;
    public Slider trebleSlider;
    public Slider gainSlider;


    [Header("Gameplay")]
    public float tolerance = 0.08f;
    public float holdTime = 2f;

    float timer;

    public TMP_Text statusText;


    void Start()
    {
        GenerateTarget();

        bassSlider.onValueChanged.AddListener(SetBass);
        midSlider.onValueChanged.AddListener(SetMid);
        trebleSlider.onValueChanged.AddListener(SetTreble);
        gainSlider.onValueChanged.AddListener(SetGain);
    }

    void GenerateTarget()
    {
        target.bass = Random.Range(.2f, .8f);
        target.mid = Random.Range(.2f, .8f);
        target.treble = Random.Range(.2f, .8f);
        target.gain = Random.Range(.3f, .7f);
    }


    public void SetBass(float value)
    {
        player.bass = value;
    }


    public void SetMid(float value)
    {
        player.mid = value;
    }


    public void SetTreble(float value)
    {
        player.treble = value;
    }


    public void SetGain(float value)
    {
        player.gain = value;
    }


    void Update()
    {
        UpdateStatus();

        if (IsCorrect())
        {
            timer += Time.deltaTime;

            if (timer >= holdTime)
            {
                Complete();
            }
        }
        else
        {
            timer = 0;
        }
    }


    bool IsCorrect()
    {
        return
        Mathf.Abs(target.bass - player.bass) < tolerance &&
        Mathf.Abs(target.mid - player.mid) < tolerance &&
        Mathf.Abs(target.treble - player.treble) < tolerance &&
        Mathf.Abs(target.gain - player.gain) < tolerance;
    }

    void UpdateStatus()
    {
        float quality = GetMixQuality();

        Debug.Log("Mix quality has score of: " + quality);

        if (quality < 85)
        {
            statusText.text = "Needs Work";
        }
        else if (quality < 90)
        {
            statusText.text = "Getting Better";
        }

        else
        {
            statusText.text = "Great Mix!";
        }
    }

    public float GetMixQuality()
    {
        float bassScore = 1 - Mathf.Abs(target.bass - player.bass);
        float midScore = 1 - Mathf.Abs(target.mid - player.mid);
        float trebleScore = 1 - Mathf.Abs(target.treble - player.treble);
        float gainScore = 1 - Mathf.Abs(target.gain - player.gain);

        float score =
            (bassScore +
            midScore +
            trebleScore +
            gainScore) / 4f;

        return score * 100f;
    }


    void Complete()
    {
        Debug.Log("MIX COMPLETE!");
        enabled = false;
    }
}