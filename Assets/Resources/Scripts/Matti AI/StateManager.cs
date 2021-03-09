using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    
    
    public class StateManager : BaseGhostAI
    {
        public State currentState;
        private void Update()
        {
            
            RunStateMachine();
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

