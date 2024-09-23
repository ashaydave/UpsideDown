using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DimensionSwitchTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public float timeLeft = 5.0f;
    public bool timerOn = false;

    public TextMeshProUGUI timerText;

    public GameObject timerEndScreen;

    private bool isFlashing = false;
    private Color originalColor;


    void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                Debug.Log("Time has run out!");
                timerOn = false;
                timerEndScreen.SetActive(true);
                gameManager.EndGame();
            }
        }
    }

    public void updateTimer(float currentTime)
    {
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float ms = Mathf.FloorToInt((currentTime * 100) % 100);
        timerText.text = "Switch Dimension in: " + string.Format("{0:00}:{1:00}", seconds, ms);

        if (seconds <= 1 && !isFlashing)
        {
            StartCoroutine(FlashText());
        }
    }

    private IEnumerator FlashText()
    {
        isFlashing = true;
        Color flashColor = Color.red;
        // AudioManager.PlaySound2D(AudioManager.SoundClips.DimensionTimer, 1f, 1f, 0f);
        while (timeLeft > 0)
        {
            timerText.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            timerText.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        timerText.color = originalColor;
        isFlashing = false;
    }

    public void StartTimer()
    {
        timerOn = true;
        originalColor = timerText.color;
        updateTimer(timeLeft);

    }

    public void ResetTimer()
    {
        // Reset the timer for the next countdown
        timeLeft = 5.0f;
        timerOn = true;
        isFlashing = false;
        StopAllCoroutines();
        timerText.color = originalColor;
    }

    public void StopTimer()
    {
        timerOn = false;
    }
}
