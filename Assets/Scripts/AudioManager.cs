using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    /// <summary>
    /// Holds all sound clip types.
    /// </summary>
    public enum SoundClips
    {
        Jump,// Rename these accordingly
        Death,
        DimensionSwitchUp,
        DimensionSwitchDown,
        DimensionTimer
    }

    /// <summary>
    /// AudioClipToPlay holds all the values of
    /// the sounds and their clips.
    /// </summary>
    [System.Serializable]
    public class AudioClipToPlay
    {
        public SoundClips soundClips;
        public AudioMixerGroup audioMixerGroup;
        public AudioClip[] audioClip;
    }

    public AudioClipToPlay[] audioClipArray;


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        };
    }


    /// <summary>
    /// Store the currently playing audio sources for looped sounds.
    /// </summary>
    private static Dictionary<SoundClips, AudioSource> playingAudioSources = new Dictionary<SoundClips, AudioSource>();

    /// <summary>
    /// Takes in a sound audio clip, and assigns floats
    /// to the parameters of the instantiated audio source,
    /// to be played for 3D sounds.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="pitch"></param>
    /// <param name="volume"></param>
    /// <param name="spatialBlend"></param>
    /// <param name="spread"></param>
    /// <param name="loop"></param>
    public static void PlaySound3D(SoundClips sound, Transform positionTransform, float volume, float pitch, float spread, float fadeInTime, bool loop = false, bool fadeIn = false)
    {

        if (playingAudioSources.ContainsKey(sound) && playingAudioSources[sound].isPlaying)
        {
            return;
        }

        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioMixerGroup audioMixerGroup = GetMixer(sound);
        soundGameObject.transform.position = positionTransform.position;

        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = GetAudioClip(sound);
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spread = spread;
        audioSource.spatialBlend = 1f;
        audioSource.loop = loop;

        audioSource.Play();

        if (fadeIn == true)
        {
            Instance.StartCoroutine(Instance.FadeInSound(sound, fadeInTime));
        }
        else { }

        if (loop == true)
        {
            playingAudioSources[sound] = audioSource;
        }
        else if (loop == false)
        {
            Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    /// <summary>
    /// Same as 3D, without the transform of the game object,
    /// and spatial settings.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="positionTransform"></param>
    /// <param name="pitch"></param>
    /// <param name="volume"></param>
    /// <param name="spatialBlend"></param>
    /// <param name="spread"></param>
    /// <param name="loop"></param>
    public static void PlaySound2D(SoundClips sound, float volume, float pitch, float fadeInTime, bool loop = false, bool fadeIn = false)
    {

        if (playingAudioSources.ContainsKey(sound) && playingAudioSources[sound].isPlaying)
        {
            return;
        }

        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioMixerGroup audioMixerGroup = GetMixer(sound);

        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = GetAudioClip(sound);
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;

        audioSource.Play();

        if (fadeIn == true)
        {
            Instance.StartCoroutine(Instance.FadeInSound(sound, fadeInTime));
        }
        else { }

        if (loop == true)
        {
            playingAudioSources[sound] = audioSource;
        }
        else if (loop == false)
        {
            Destroy(soundGameObject, audioSource.clip.length);
        }
    }
    private AudioClip[] clip;


    /// <summary>
    /// Get the correct sound clip from the sound clip array.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static AudioClip GetAudioClip(SoundClips sound)
    {
        foreach (AudioClipToPlay audioClipToPlay in Instance.audioClipArray)
        {
            if (audioClipToPlay.soundClips == sound)
            {
                if (audioClipToPlay.audioClip != null)
                {
                    int index = UnityEngine.Random.Range(0, audioClipToPlay.audioClip.Length);
                    return audioClipToPlay.audioClip[index];
                }
                else
                {
                    Debug.LogError("Please assign an audio clip for" + sound + "sound!");
                    return null;
                }
            }
        }
        Debug.LogError("Sound" + sound + " not found!");
        return null;
    }


    /// <summary>
    /// Get mixer channel for the associated AudioClipToPlay enum audio source.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static AudioMixerGroup GetMixer(SoundClips sound)
    {
        foreach (AudioClipToPlay audioClipToPlay in Instance.audioClipArray)
        {
            if (audioClipToPlay.soundClips == sound)
            {
                if (audioClipToPlay.audioMixerGroup != null)
                {
                    return audioClipToPlay.audioMixerGroup;
                }
                else
                {
                    Debug.LogError("Please assign Audio Mixer Group for" + sound + "sound!");
                    return null;
                }
            }
        }
        return null;
    }


    /// <summary>
    /// Immediately stop the sound.
    /// </summary>
    /// <param name="sound"></param>
    public static void StopSound(SoundClips sound)
    {
        if (playingAudioSources.ContainsKey(sound))
        {
            AudioSource audioSource = playingAudioSources[sound];
            audioSource.loop = false;
            audioSource.Stop();
            playingAudioSources.Remove(sound);

            Destroy(audioSource.gameObject);
        }
    }

    /// <summary>
    /// Fade in sound in specified seconds.
    /// </summary>
    /// <param name="time"></param>
    public IEnumerator FadeInSound(SoundClips sound, float fadeInTime)
    {
        if (playingAudioSources.ContainsKey(sound))
        {
            AudioSource audioSource = playingAudioSources[sound];
            float startVolume = audioSource.volume;
            float startTime = Time.time;
            while (startTime + fadeInTime > Time.time)
            {
                float currentFadeTime = 0f;
                currentFadeTime = Time.time - startTime;
                audioSource.volume = Mathf.Lerp(0f, startVolume, currentFadeTime / fadeInTime);

                yield return null;
            }
        }
    }

    /// <summary>
    /// Fade out sound in specified seconds. Stops and destroys the audio source when faded out.
    /// </summary>
    /// <param name="time"></param>
    public IEnumerator FadeOutSound(SoundClips sound, float fadeOutTime)
    {
        if (playingAudioSources.ContainsKey(sound))
        {
            AudioSource audioSource = playingAudioSources[sound];
            float startVolume = audioSource.volume;
            float startTime = Time.time;
            while (startTime + fadeOutTime > Time.time)
            {
                float currentFadeTime = 0f;
                currentFadeTime = Time.time - startTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, currentFadeTime / fadeOutTime);

                yield return null;
            }
            audioSource.Stop();
            playingAudioSources.Remove(sound);
            Destroy(audioSource.gameObject);
        }
    }
}
