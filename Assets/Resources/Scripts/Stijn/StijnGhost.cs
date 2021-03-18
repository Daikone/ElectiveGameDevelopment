using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Stijn
{
    public class StijnGhost : BaseGhostAI
    {
        public StijnStateMachine movementSM;
        public StandingState standing;
        public ChasingState chasing;

        private void Awake()
        {
            movementSM = new StijnStateMachine();

            standing = new StandingState(this, movementSM);
            chasing = new ChasingState(this, movementSM);

            movementSM.Initialize(standing);
        }

        private void Update()
        {
            movementSM.CurrentState.HandleInput();

            movementSM.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            movementSM.CurrentState.PhysicsUpdate();
        }
    }
}