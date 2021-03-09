
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class IdleState : State
    {
        public ChaseState _chaseState;
        public bool seesHuman = false;
        public override State RunCurrentState()
        {
            Debug.Log("I'm idle");
            if (seesHuman)
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
