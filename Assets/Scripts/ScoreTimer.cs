using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Scoreboard scoreboard;
    private float time;
    public bool timerOn = false;
    
    public int finalScoreInMS;

    public TextMeshProUGUI timerText, finalScoreText;


    void Update()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
            updateTimer(time);
        }
    }

    public void updateTimer(float currentTime)
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int ms = Mathf.FloorToInt((currentTime * 100) % 100);
        finalScoreInMS = (minutes * 60 * 1000) + (seconds * 1000) + (ms * 10);

        timerText.text = "Time Alive: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ms);
        finalScoreText.text = "Final Score: " + finalScoreInMS;
    }

    public void StartTimer()
    {
        timerOn = true;
        updateTimer(time);
    }

    public void ResetTimer()
    {
        time = 0;
        timerOn = true;
    }
    public void StopTimer()
    {
        timerOn = false;
    }
}
