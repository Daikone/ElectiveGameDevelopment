using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaniAITest : BaseGhostAI
{
    // Start is called before the first frame update
    void Start()
    {
        MovetoPoint(ROOM.Hallway1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
