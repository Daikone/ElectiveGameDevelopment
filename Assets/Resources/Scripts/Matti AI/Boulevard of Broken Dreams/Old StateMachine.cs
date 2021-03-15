using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts.Matti_AI
{
    
    
    // public class  StateMachine : BaseGhostAI
    // {
    //     public  State _currentState;
    //     public IdleState idleState;
    //     public  ChaseState chaseState;
    //     
    //    public LayerMask humanLayerCheck;
    //    private  List<GameObject> humansInSight = new List<GameObject>();
    //    
    //    [HideInInspector]
    //     public GameObject ClosestHuman;
    //     
    //
    //
    //     private void Start()
    //     {
    //         _currentState = idleState;
    //     }
    //
    //     private void Update()
    //     {
    //         RunStateMachine();
    //         humansInSight = CheckCloseObjectsInSight(gameObject, 10f, humanLayerCheck);
    //         if (humansInSight.Count > 0)
    //         {
    //             ClosestHuman = ClosestObjectInList(gameObject, humansInSight);
    //         }
    //         else
    //         {
    //             ClosestHuman = null;
    //         }
    //     }
    //
    //     private void RunStateMachine()
    //     {
    //         State nextState = _currentState?.RunCurrentState();
    //
    //         if (nextState != null)
    //         {
    //             SwitchToNextState(nextState);
    //         }
    //     }
    //
    //     private void SwitchToNextState(State nextState)
    //     {
    //         _currentState = nextState;
    //     }
    //
    //     
    //}
    
}

