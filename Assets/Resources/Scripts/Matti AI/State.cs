using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public abstract class State
    {
        protected StateMachine stateMachine;
        protected GameObject owner;
        protected MattiBaseAI baseAI;
        protected float timeInState;

        protected State(StateMachine sm)
        {
            stateMachine = sm;
            owner = sm.owner;
            baseAI = sm.owner.GetComponent<MattiBaseAI>();
            

        }

        
        
        
        public virtual void Enter()
        {
            
        }

        public virtual void HandleInput()
        {

        }

        public abstract void LogicUpdate();
        

        public virtual void Exit()
        {

        }

    }
}

