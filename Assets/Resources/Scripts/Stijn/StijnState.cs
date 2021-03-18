﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Resources.Scripts.Stijn
{
    public abstract class StijnState
    {
        protected StijnStateMachine stateMachine;
        protected StijnGhost character;

        protected StijnState(CharacterController character, StijnStateMachine stateMachine)
        {
            this.character = character;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void HandleInput()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }
    }
}