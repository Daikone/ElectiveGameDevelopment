using System.Collections;
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
    private float currentHumans;
    public float[] ghostScores = {0, 0, 0, 0};
    public const float MAXHUMANS = 3;
    private const float MAXTIME = 65;
    private const float MAXPOINTS = 10;

    public Room[] rooms;

    private void Start()
    {
        currentTime = MAXTIME;
        StartGame();
    }

    private void Update()
    {
        DisplayTime();
        DisplayScore();
        SpawnHumans();
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
    
    void SpawnHumans()
    {
        if (currentHumans < MAXHUMANS)
        {
            Vector3 position = new Vector3(Random.Range(-1.8f, 1.8f), 1, Random.Range(9.2f, 12.8f));
            Instantiate(human, position,Quaternion.identity);
            currentHumans++;
        }
    }
    
    void GameOver()
    {
        
    }
    void StartGame()
    {
        
    }
}
