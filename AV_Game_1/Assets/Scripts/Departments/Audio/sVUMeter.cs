using UnityEngine;
using UnityEngine.UI;

public class sVUMeter : MonoBehaviour
{
    public Image fill;

    [Range(0f, 1f)]
    public float level;

    float noiseOffset;


    void Start()
    {
        noiseOffset = Random.Range(0f, 100f);
    }


    void Update()
    {
        float bounce =
            Mathf.Sin(Time.time * 8f + noiseOffset) * 0.05f;

        float value =
            Mathf.Clamp01(level + bounce);

        fill.fillAmount = value;
    }


    public void SetLevel(float value)
    {
        level = value;
    }
}