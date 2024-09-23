using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundtrackManager : MonoBehaviour
{
    public AudioClip fullTheme;
    public AudioClip loopableTheme;

    private AudioSource audioSource;
    private bool isPlaying = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main" && !isPlaying)
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlaySoundtrack());
            isPlaying = true;
        }
    }

    private void OnEnable()
    {
        // Register the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator PlaySoundtrack()
    {
        audioSource.clip = fullTheme;
        audioSource.PlayDelayed(2.5f);

        yield return new WaitForSeconds(fullTheme.length);

        audioSource.clip = loopableTheme;
        audioSource.loop = true;
        audioSource.Play();
    }
}
