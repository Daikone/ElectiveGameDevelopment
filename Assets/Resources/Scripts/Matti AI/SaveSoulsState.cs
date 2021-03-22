using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class SaveSoulsState : State
    {

        public SaveSoulsState(StateMachine sm) : base(sm)
        {
            stateMachine = sm;
            owner = sm.owner;
            baseAI = sm.owner.GetComponent<MattiBaseAI>();
        }

        public override void LogicUpdate()
        {
            baseAI.agent.SetDestination(baseAI.cauldron.transform.position);
            if (baseAI.behaviour.carryingSouls == 0)
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
        }
    }
}

