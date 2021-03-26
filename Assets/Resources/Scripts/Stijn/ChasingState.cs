using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Stijn
{
    public class ChasingState : State
    {

        public float distance = 0;
        public override void Enter(StijnGhost owner)
        {
            owner.navMeshAgent.ResetPath();
            
        }

        public override void LogicUpdate(StijnGhost owner)
        {
            if (owner.nearbyHuman != null)
            {
                float distance = Vector3.Distance(owner.transform.position, owner.nearbyHuman.transform.position);
            }

            if (owner.HumansVisible.Count > 0)
            {
                if (owner.nearbyHuman != null)
                {
                    owner.navMeshAgent.SetDestination(owner.nearbyHuman.transform.position);    
                }

                float stopChaseDist = 9f;
                if (distance > stopChaseDist )
                {
                    Debug.Log("human is out of reach");
                    owner.stateMachine.ChangeState(owner.wandering);
                }
            }
            else 
            {
                owner.stateMachine.ChangeState(owner.wandering);
            }
        }
    }
}
