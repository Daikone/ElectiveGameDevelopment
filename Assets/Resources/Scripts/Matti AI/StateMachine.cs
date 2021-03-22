using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Resources.Scripts.Matti_AI
{
    
    public class StateMachine
    {
        public State CurrentState;
        public readonly GameObject owner;

        public StateMachine(GameObject ow)
        {
            owner = ow;
        }
        
        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
            
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
    
}