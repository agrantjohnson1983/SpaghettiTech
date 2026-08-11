using UnityEngine;

public class sAudioSystem : MonoBehaviour
{
    [Header("Event Channel")]
    [SerializeField]
    private SO_AudioEventChannel audioEventChannel;

    [Header("Audio Library")]
    [SerializeField]
    private SO_AudioLibrary audioLibrarySFX, audioLibraryMusic;

    [Header("Audio Source")]
    [SerializeField]
    private AudioSource audioSource_SFX, audioSource_MUSIC;

    private void OnEnable()
    {
        if (audioEventChannel != null)
        {
            audioEventChannel.audioEvent_SFX.AddListener(HandleSFXAudio);
            audioEventChannel.audioEvent_MUSIC.AddListener(HandleMusic);
        }
    }

    private void OnDisable()
    {
        if (audioEventChannel != null)
        {
            audioEventChannel.audioEvent_SFX.RemoveListener(HandleSFXAudio);
            audioEventChannel.audioEvent_MUSIC.RemoveListener(HandleMusic);
        }
    }

    private void HandleSFXAudio(string eventName)
    {
        if (audioLibrarySFX == null)
        {
            Debug.LogWarning("sAudioSystem has no Audio Library assigned for SFX.");
            return;
        }

        AudioEventEntry audioEvent = audioLibrarySFX.GetEvent(eventName);

        if (audioEvent == null)
        {
            Debug.LogWarning(
                $"No audio event found for: {eventName}"
            );

            return;
        }

        //Debug.Log("Playing audio event in AudioSystem");
        PlaySFX(audioEvent);
    }

    private void HandleMusic(string eventName)
    {
        if (audioLibraryMusic == null)
        {
            Debug.LogWarning("sAudioSystem has no Audio Library assigned for music.");
            return;
        }

        AudioEventEntry audioEvent = audioLibraryMusic.GetEvent(eventName);

        if (audioEvent == null)
        {
            Debug.LogWarning(
                $"No audio event found for: {eventName}"
            );

            return;
        }

        //Debug.Log("Playing audio event in AudioSystem");
        PlayMusic(audioEvent);
    }

    private void PlaySFX(
        AudioEventEntry audioEvent)
    {
        //Debug.Log("Playing audio event");

        if (audioEvent.clips == null ||
            audioEvent.clips.Count == 0)
        {
            Debug.LogWarning(
                $"Audio event '{audioEvent.eventName}' has no clips."
            );

            return;
        }

        // Cooldown
        if (Time.time < audioEvent.lastPlayedTime +
            audioEvent.cooldown)
        {
            return;
        }

        audioEvent.lastPlayedTime = Time.time;

        // Pick clip
        AudioClip clip;

        if (audioEvent.randomClip)
        {
            clip = audioEvent.clips[
                Random.Range(0, audioEvent.clips.Count)
            ];
        }
        else
        {
            clip = audioEvent.clips[0];
        }

        if (clip == null)
            return;

        Play2D(clip, audioEvent.volume, audioEvent.minPitch, audioEvent.maxPitch);
        // Position
        //if (position.HasValue && audioEvent.use3D)
        //{
        //    AudioSource.PlayClipAtPoint(
        //        clip,
        //        position.Value,
        //        audioEvent.volume
        //    );
        //}
        //else
        //{
        //    Play2D(
        //        clip,
        //        audioEvent.volume,
        //        audioEvent.minPitch,
        //        audioEvent.maxPitch
        //    );
        //}
    }

    private void PlayMusic(
        AudioEventEntry audioEvent)
    {
        //Debug.Log("Playing music");

        if (audioEvent.clips == null ||
            audioEvent.clips.Count == 0)
        {
            Debug.LogWarning(
                $"Audio event '{audioEvent.eventName}' has no clips."
            );

            return;
        }

        // Cooldown
        if (Time.time < audioEvent.lastPlayedTime +
            audioEvent.cooldown)
        {
            return;
        }

        audioEvent.lastPlayedTime = Time.time;

        // Pick clip
        AudioClip clip;

        if (audioEvent.randomClip)
        {
            clip = audioEvent.clips[
                Random.Range(0, audioEvent.clips.Count)
            ];
        }
        else
        {
            clip = audioEvent.clips[0];
        }

        if (clip == null)
            return;

        audioSource_MUSIC.clip = clip;
        audioSource_MUSIC.Play();

        //Debug.Log("Music was played with " + clip.name);

        //Play2D(clip, audioEvent.volume, audioEvent.minPitch, audioEvent.maxPitch);
        // Position
        //if (position.HasValue && audioEvent.use3D)
        //{
        //    AudioSource.PlayClipAtPoint(
        //        clip,
        //        position.Value,
        //        audioEvent.volume
        //    );
        //}
        //else
        //{
        //    Play2D(
        //        clip,
        //        audioEvent.volume,
        //        audioEvent.minPitch,
        //        audioEvent.maxPitch
        //    );
        //}
    }

    private void Play2D(
        AudioClip clip,
        float volume,
        float minPitch,
        float maxPitch)
    {
        Debug.Log("Playing 2d audio");

        if (audioSource_SFX == null)
        {
            Debug.LogWarning(
                "sAudioSystem has no AudioSource assigned."
            );

            return;
        }

        audioSource_SFX.pitch = Random.Range(
            minPitch,
            maxPitch
        );

        audioSource_SFX.volume = volume;

        audioSource_SFX.PlayOneShot(clip);

        // Reset pitch so other systems don't
        // accidentally inherit the randomized value.
        audioSource_SFX.pitch = 1f;
    }
}

public static class AudioEventNames
{
    public const string PowerConnected = "power_connected";
    public const string PowerDisconnected = "power_disconnected";

    public const string CableConnected = "cable_connected";
    public const string CableDisconnected = "cable_disconnected";
    public const string CableWrong = "cable_wrong";

    public const string TrussConnected = "truss_connected";
    public const string TrussRaised = "truss_raised";
    public const string TrussLowered = "truss_lowered";

    public const string GigComplete = "gig_complete";
    public const string GigFailed = "gig_failed";
}