using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Resources.Scripts.Stijn
{
    public abstract class State
    {

        public virtual void Enter(StijnGhost owner)
        {

        }

        public virtual void HandleInput(StijnGhost owner)
        {

        }

        public virtual void LogicUpdate(StijnGhost owner)
        {

        }

        public virtual void PhysicsUpdate(StijnGhost owner)
        {

        }

        public virtual void Exit(StijnGhost owner)
        {

        }
    }
}