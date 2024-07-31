using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    public AudioClip fullTheme;
    public AudioClip loopableTheme;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySoundtrack());
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
