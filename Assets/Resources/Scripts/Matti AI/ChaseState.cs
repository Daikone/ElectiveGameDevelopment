using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class ChaseState : State
    {
        public ChaseState(StateMachine sm) : base(sm)
        {
            stateMachine = sm;
            owner = sm.owner;
            baseAI = sm.owner.GetComponent<MattiBaseAI>();
            
        }

        public override void Enter()
        {
            Debug.Log("Chasing");
            baseAI.agent.ResetPath();
            
        }

        public override void LogicUpdate()
        {
            if (baseAI.HumansInSight.Count > 0)
            {
                baseAI.agent.SetDestination(baseAI.closestHuman.transform.position);
                
            }else if (baseAI.HumansInSight.Count == 0)
            {
                baseAI.agent.SetDestination(baseAI.closestHumanLastPos);
                
            }
            else
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
        }
    }
}

