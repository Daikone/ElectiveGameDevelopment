using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronScript : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject ghost = other.gameObject;
        if (ghost.CompareTag("Ghost"))
        {
            if (ghost.GetComponent<GhostBehaviour>().carryingSouls > 0)
            {
                GetComponent<AudioSource>().Play();
           
                // what are variables, am I right?
                gameManager.ghostScores[Array.IndexOf(gameManager.playerNames, ghost.GetComponent<GhostBehaviour>().YourName)] += ghost.GetComponent<GhostBehaviour>().carryingSouls;
                
                ghost.GetComponent<GhostBehaviour>().carryingSouls = 0;
            }
            
        }
    }
}
