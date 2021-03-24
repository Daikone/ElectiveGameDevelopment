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
        
        private Vector3 _destination;

        public override void Enter()
        {
            baseAI.agent.ResetPath();
            
            Debug.Log("SavingSouls");
        }

        public override void LogicUpdate()
        {

            if (baseAI.behaviour.carryingSouls == 0)
            {
                stateMachine.ChangeState(baseAI._idleState);
            }else if (baseAI.closestHuman != null)
            {
                if (Vector3.Distance(baseAI.transform.position, baseAI.closestHuman.transform.position) <= 7)
                {
                    _destination = baseAI.closestHuman.transform.position;
                }
            }
            else
            {
                _destination = baseAI.cauldron.transform.position;
            }

            baseAI.agent.SetDestination(_destination);
        }

        
    }
}

