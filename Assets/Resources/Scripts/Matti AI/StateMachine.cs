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
        
        // Does it need this? You can just ChangeState in the owner
        // You do need to check for current state if(CurrentState) or CurrentState?.Exit();
        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            // This is a null propagation check
            CurrentState?.Exit();

            CurrentState = newState;
            // What happens if newState is null?
            // newState?.Enter(); <-- this would fix it
			newState.Enter();
		}
    }
    
}