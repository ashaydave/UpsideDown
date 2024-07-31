using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private float time;
    public bool timerOn = false;

    public TextMeshProUGUI timerText;



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
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float ms = Mathf.FloorToInt((currentTime * 100) % 100);
        timerText.text = "Time Alive: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ms);
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
