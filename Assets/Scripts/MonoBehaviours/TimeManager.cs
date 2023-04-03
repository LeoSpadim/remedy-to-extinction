using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RodDev.Timer;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Singleton
    {
        get
        {
            if(timeManager == null)
            {
                timeManager = FindObjectOfType<TimeManager>();
            }
            return timeManager;
        }
    }
    private static TimeManager timeManager;
    public Timer timer;
    [SerializeField] GameObject timerUI;
    [SerializeField] TMP_Text hologramText;

    private void CountTime()
    {
        if(!PauseMenu.PAUSED)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    private void MostraTempoRestante()
    {
        if(timerUI.activeSelf && !PauseMenu.PAUSED)
        {
            int minutes = Mathf.FloorToInt(timer.tempoRestante / 60);
            int seconds = Mathf.FloorToInt(timer.tempoRestante % 60);
            if(timer.tempoRestante > 0){seconds += 1;}
            hologramText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void Awake()
    {
        timer = new Timer(LevelManager.Singleton.tempoDoDia);
    }

    private void Update()
    {
        CountTime();
        MostraTempoRestante();
    }
}