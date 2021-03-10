
using System;

namespace Resources.Scripts.Matti_AI
{
    
    public abstract class State : StateMachine
    {
        
        public StateMachine getStateMachine()
        {
            return gameObject.transform.parent.gameObject.GetComponent<StateMachine>();
        }

        public abstract State RunCurrentState();

    }
}

