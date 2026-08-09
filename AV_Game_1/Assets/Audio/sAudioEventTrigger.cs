using UnityEngine;

public class sAudioEventTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private SO_AudioEventChannel audioEventChannel;

    [SerializeField]
    private string eventName;

    [Header("Options")]
    [SerializeField]
    private bool playOnStart = false;

    private void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (audioEventChannel == null)
        {
            Debug.LogWarning(
                $"{name}: No Audio Event Channel assigned."
            );

            return;
        }

        audioEventChannel.Raise(
            eventName
        );
    }
}