using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sVolumeChannelUI : MonoBehaviour
{
    public TMP_Text channelName;

    public Image targetFill;
    public Image liveFill;

    public Slider slider;

    public sVUMeter vuMeter;

    public AudioSource _audioSource;

    private void OnEnable()
    {


    }

    public void Setup(sVolumeChannel channel)
    {
        Debug.Log(
            channel.channelName +
            " target = " +
            channel.targetVolume);

        channelName.text = channel.channelName;

        targetFill.fillAmount =
            channel.targetVolume;

        slider.value =
            channel.playerVolume;
    }


    public void UpdateLive(float value)
    {
        liveFill.fillAmount = value;

        if (vuMeter != null)
        {
            vuMeter.SetLevel(value);
        }

        if(_audioSource != null)
        {
            _audioSource.volume = value;
        }
    }
}