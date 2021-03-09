using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts.Matti_AI
{
    
    
    public class  StateManager : BaseGhostAI
    {
        private static State currentState;

       
       public LayerMask HumanLayerCheck;
        private static List<GameObject> humansInSight = new List<GameObject>();

        [HideInInspector] 
        protected static GameObject ClosestHuman;
        

        private void Update()
        {
            RunStateMachine();
            humansInSight = CheckCloseObjectsInSight(gameObject, 10f, HumanLayerCheck);
            if (humansInSight.Count > 0)
            {
                ClosestHuman = ClosestObjectInList(gameObject, humansInSight);
            }
            else
            {
                ClosestHuman = null;
            }
            

        }

        private void RunStateMachine()
        {
            State nextState = currentState?.RunCurrentState();

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }

        private void SwitchToNextState(State nextState)
        {
            currentState = nextState;
        }
    }
    
}

