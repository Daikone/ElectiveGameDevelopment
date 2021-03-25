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
    private bool isblending = false;
    
    public TextMeshProUGUI[] winningNames;
   
    
    public TextMeshProUGUI[] winningScores;
    
    

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        
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

        if (isblending && _canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime*0.9f;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            
        }

        if (_canvasGroup.alpha >= 1)
        {
            stopGame();
        }
        
        
    }

    private void gameOver()
    {
        //extremely clunky and terribly written mechanic to sort a dictionary but I kinda ran out of time  
        
        Stack<KeyValuePair<string, float>> dictionaryStack = new Stack<KeyValuePair<string, float>>();
        
        int valueCount = 0;
        foreach (var pair in gameManager.ScoreBoard.OrderBy(key => key.Value))
        {
            if (valueCount >= gameManager.ScoreBoard.Count -winningNames.Length)
            {
                dictionaryStack.Push(pair);
            }

            valueCount++;
        }

        for (int i = 0; i < winningNames.Length; i++)
        {
            KeyValuePair<string, float> pair = new KeyValuePair<string, float>();
            pair = dictionaryStack.Pop();
            winningNames[i].text = pair.Key;
            winningScores[i].text = pair.Value.ToString();
        }

        isblending = true;


    }

    void stopGame()
    {
        Time.timeScale = 0;
    }
}
