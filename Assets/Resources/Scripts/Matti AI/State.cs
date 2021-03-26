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


        public abstract void Enter();
       


        public abstract void LogicUpdate();


        public abstract void Exit();


    }
}

