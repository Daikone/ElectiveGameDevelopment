using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    
    public abstract class State : StateManager
    {
        public abstract State RunCurrentState();

    }
}

