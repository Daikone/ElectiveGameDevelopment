using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Resources.Scripts.Stijn
{
    public class StijnStateMachine
    {
        public State CurrentState { get; private set; }

        private StijnGhost owner;

        public StijnStateMachine(StijnGhost owner)
        {
            this.owner = owner;
        }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            startingState.Enter(owner);
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit(owner);

            CurrentState = newState;
            newState.Enter(owner);
        }
    }
}

