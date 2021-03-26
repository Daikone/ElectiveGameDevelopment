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

            //changes state back if the souls are deposited 
            if (baseAI.behaviour.carryingSouls == 0)
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
            // still hunts a human if he is closer than fice
            else if (baseAI.closestHuman != null)
            {
                if (Vector3.Distance(baseAI.transform.position, baseAI.closestHuman.transform.position) <= 5)
                {
                    _destination = baseAI.closestHuman.transform.position;
                }
            }
            //goes to the cauldron
            else
            {
                _destination = baseAI.cauldron.transform.position;
            }

            baseAI.agent.SetDestination(_destination);
        }
        

        public override void Exit()
        {
            
        }
    }
}

