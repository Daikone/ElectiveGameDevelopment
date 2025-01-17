﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text winText;
    public Text[] scores;
    public Text timer;
    public GameObject human;
    //public bool isDone;
    public  float currentTime;
    public GameObject[] currentHumans;

    public delegate void spawnHumans();
    public static event spawnHumans OnSpawnHumans;

    public string[] playerNames = {"Esmé", "Hani", "Stijn", "Alex", "Matti"};
    public float[] ghostScores = {0, 0, 0, 0, 0};
    
    public SortedDictionary<string, float> ScoreBoard = new SortedDictionary<string, float>();
    private const float MINHUMANS = 7;
    private const float MAXTIME = 65;
    private const float MAXPOINTS = 10;

    public bool disableUI; 

    private void Start()
    {
        currentTime = MAXTIME;
        StartGame();
    }

    private void Update()
    {
        if (!disableUI)
        {
            DisplayTime();
            DisplayScore();
            
        }
        
        //expensive method but it was the fastest way
        currentHumans = GameObject.FindGameObjectsWithTag("Human");
        //spawns humans when below minhumans
        if (currentHumans.Length < MINHUMANS)
        {
            OnSpawnHumans?.Invoke();
        }
        
        //combines names with scores in a dictionary
        ScoreBoard.Clear();
        for (int i = 0; i < playerNames.Length; i++)
        {
            ScoreBoard.Add(playerNames[i], ghostScores[i]);
        }
        currentTime -= Time.deltaTime;

    }

    void DisplayTime()
    {
        if (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            int minute = (int) Mathf.Floor(currentTime / 60);
            int second = (int) Mathf.Floor(currentTime % 60);
            if(second < 10)
                timer.text = minute + " : 0" + second;
            else
                timer.text = minute + " : " + second;
        }
    }

    void DisplayScore()
    {
        for (int i = 0; i < ghostScores.Length; i++)
        {
            scores[i].text = ghostScores[i].ToString();
            if (ghostScores[i] >= MAXPOINTS)
            {
                GameOver();
                DisplayWin("Ghost" + (i + 1));
                break;
            }
        }
    }

    void DisplayWin(string ghost)
    {
        winText.text = ghost + " has won!";
        winText.gameObject.SetActive(true);
    }
    
    
    
    void GameOver()
    {
        
    }
    void StartGame()
    {
        
    }
}
