using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreUpdate : MonoBehaviour
{
    public GameManager gameManager;
    public GhostBehaviour[] ghostScores;

    public Text AlexScore;
    public Text AlexTotal;
    public Text StijnScore;
    public Text StijnTotal;
    public Text EsméScore;
    public Text EsméTotal;
    public Text MattiScore;
    public Text MattiTotal;
    public Text HaniScore;
    public Text HaniTotal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreUpdate()
    {
        AlexTotal.text = gameManager.ghostScores[3].ToString();
        StijnTotal.text = gameManager.ghostScores[2].ToString();
        EsméTotal.text = gameManager.ghostScores[0].ToString();
        MattiTotal.text = gameManager.ghostScores[4].ToString();
        HaniTotal.text = gameManager.ghostScores[1].ToString();

        AlexScore.text = ghostScores[0].getSouls().ToString();
        StijnScore.text = ghostScores[1].getSouls().ToString();
        EsméScore.text = ghostScores[2].getSouls().ToString();
        MattiScore.text = ghostScores[3].getSouls().ToString();
        HaniScore.text = ghostScores[4].getSouls().ToString();

    }
}
