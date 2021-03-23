﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class IdleState : State
    {
        
        public IdleState(StateMachine sm) : base(sm)
        {
            stateMachine = sm;
            owner = sm.owner;
            baseAI = sm.owner.GetComponent<MattiBaseAI>();
        }

        public override void Enter()
        {
            Debug.Log("Idle");
            baseAI.agent.ResetPath();
            baseAI.PreviousDoors.Clear();
        }

        
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void LogicUpdate()
        {
            if (baseAI.HumansInSight.Count > 0)
            {
                stateMachine.ChangeState(baseAI._chaseState);
            }
            else
            {
                baseAI.agent.SetDestination(baseAI.ClosestDoorPos);
            }

            if (baseAI.behaviour.carryingSouls >= 5)
            {
                stateMachine.ChangeState(baseAI._saveSoulsState);
            }

        }
    }

}
