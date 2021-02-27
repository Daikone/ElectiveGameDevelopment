using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { Hunting, RoomChange, RoomRoam };
public class BaseGhostAI : MonoBehaviour
{
    public Transform[] rooms;

    private bool isStunned;
    private float carryingSouls;
    protected float speed;
    

    private STATE currentState;

    private void Start()
    {
        currentState = STATE.RoomChange;
    }

    protected void MovetoPoint(Vector3 pos)
    {
        if(currentState == STATE.RoomChange)
            Debug.Log("State Changed");
    }

    protected void DepositSouls()
    {
        
    }
    
    //CheckCloset()
    
    //protected PowerUp()
}
