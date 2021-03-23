using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public GameManager gameManager;
    public bool manualGameOver;
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _winnerName;
    

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _winnerName = transform.Find("Winner").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameManager.currentTime <= 0)
        {
            gameOver();
        }

        if (manualGameOver)
        {
            gameOver();
            manualGameOver = false;
        }
    }

    private void gameOver()
    {
         KeyValuePair<string, float> winnerScore = new KeyValuePair<string, float>();
        float firstScore = 0;
        float secondScore = 0;
        float thirdScore = 0;
        foreach (var pair in gameManager.ScoreBoard)
        {
            if (pair.Value > firstScore)
            {
                firstScore = pair.Value;
                winnerScore = pair;
                
            }
        }
        
        
        _winnerName.text = winnerScore.Key;

        _canvasGroup.alpha = 1;
    }
}
