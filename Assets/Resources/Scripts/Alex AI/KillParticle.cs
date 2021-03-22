using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticle : MonoBehaviour
{
    /// <summary>
    /// 
    /// Alexanders code \\\
    /// 
    /// For simple destruction to make sure that the game is not gonna be filled with empty particle effects
    /// 
    /// </summary>
    /// 
    public int timer;
    void Start()
    {
       timer = 0;
    }
    private void Update()
    {
        timer++;
        if (timer >= 180)
        {
            Destroy(gameObject);
        }
    }
}
