
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class IdleState : State
    {
        public ChaseState _chaseState;
       
        
        public override State RunCurrentState()
        {
           
            if (ClosestHuman != null)
            {
                return _chaseState;
            }
            else
            {
                return this;
            }
        }
    }
}
