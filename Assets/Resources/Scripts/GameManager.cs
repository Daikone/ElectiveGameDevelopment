using System.Collections;
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
    private float currentTime;
    public GameObject[] currentHumans;

    public delegate void spawnHumans();
    public static event spawnHumans OnSpawnHumans;
    
    public float[] ghostScores = {0, 0, 0, 0};
    private const float MINHUMANS = 5;
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
        if (currentHumans.Length < MINHUMANS)
        {
            OnSpawnHumans?.Invoke();
        }

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
