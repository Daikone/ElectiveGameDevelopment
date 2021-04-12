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

        // I would've sent the owner of the StateMachine to the enter, LogicUpdate and Exit
        // That would mean you can have some of the magic numbers as variables in the owner.
        // Thus improving scalability and adaptability
        public abstract void Enter();
       


        public abstract void LogicUpdate();


        public abstract void Exit();


    }
}

