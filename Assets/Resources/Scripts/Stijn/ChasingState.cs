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
            Debug.Log("Stijn's Ghost is chasing");
            owner.navMeshAgent.ResetPath();
        }

        public override void LogicUpdate(StijnGhost owner)
        {
            
        }
    }
}
