using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class SaveSoulsState : State
    {
        

        public override void LogicUpdate()
        {
            baseAI.agent.SetDestination(baseAI.cauldron.transform.position);
            if (baseAI.behaviour.carryingSouls == 0)
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
        }

        public SaveSoulsState(StateMachine sm) : base(sm)
        {
            
        }
    }
}

