using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronScript : BaseGhostAI
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject ghost = other.gameObject;
        if (ghost.CompareTag("Ghost"))
        {
            
        }
    }
}
