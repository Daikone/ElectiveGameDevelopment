using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Resources.Scripts.Stijn
{
    public class StijnStateMachine
    {
        public StijnState CurrentState { get; private set; }

        public void Initialize(StijnState startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(StijnState newState)
        {
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
}

