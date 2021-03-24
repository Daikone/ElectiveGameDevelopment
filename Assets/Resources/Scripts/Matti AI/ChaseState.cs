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

        private float _timeWhenOutOfSight;
        private bool _isGoingForPos;

        public override void Enter()
        {
            Debug.Log("Chasing");
            baseAI.agent.ResetPath();
            timeInState = 0;
            _isGoingForPos = false;

        }

        public override void LogicUpdate()
        {
            timeInState += Time.deltaTime;
            
            
            if (baseAI.HumansInSight.Count > 0)
            {
                baseAI.agent.SetDestination(baseAI.closestHuman.transform.position);
            }
            else if (baseAI.HumansInSight.Count == 0)
            {
                if (!_isGoingForPos)
                {
                    _timeWhenOutOfSight = timeInState;
                    _isGoingForPos = true;
                }

                if (timeInState - _timeWhenOutOfSight <= 3)
                {
                    baseAI.agent.SetDestination(baseAI.closestHumanLastPos);
                }
                else
                {
                    _isGoingForPos = false;
                    _timeWhenOutOfSight = 0;
                }
            }
            else
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
        }
    }
}

