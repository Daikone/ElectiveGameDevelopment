using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Stijn
{
    public class ChasingState : State
    {
        public override void Enter(StijnGhost owner)
        {
            owner.navMeshAgent.ResetPath();
        }

        public override void LogicUpdate(StijnGhost owner)
        {
            if (owner.HumansVisible.Count > 0)
            {
                owner.navMeshAgent.SetDestination(owner.nearbyHuman.transform.position);
            }

            else
            {
                owner.stateMachine.ChangeState(owner.wandering);
            }
        }
    }
}
