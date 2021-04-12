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
            // This is exactly why I think you should send the owner to your functions and cast that to your object (if you're not using generics)
            // GetComponent is not THAT light and you don't really need to in this case
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
            
            //goes for the closest human
            if (baseAI.HumansInSight.Count > 0)
            {
                baseAI.agent.SetDestination(baseAI.closestHuman.transform.position);
            }
            //going to the last position of seen human for one second
            else if (baseAI.HumansInSight.Count == 0)
            {
                if (!_isGoingForPos)
                {
                    _timeWhenOutOfSight = timeInState;
                    _isGoingForPos = true;
                }
                if (timeInState - _timeWhenOutOfSight <= 1) // arbitrary number
                {
                    baseAI.agent.SetDestination(baseAI.closestHumanLastPos);
                }
                else
                {
                    _isGoingForPos = false;
                    _timeWhenOutOfSight = 0;
                }
            }
            //Switching back to idle if human remains out of sight
            if(baseAI.HumansInSight.Count== 0 && !_isGoingForPos )
            {
                stateMachine.ChangeState(baseAI._idleState);
            }
            
        }

        // Empty function, do you need it?
        public override void Exit()
        {
            
        }
    }
}

