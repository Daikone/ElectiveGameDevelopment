using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Resources.Scripts.Stijn
{
    public class StijnStateMachine
    {
        public State CurrentState { get; private set; }

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

