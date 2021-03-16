using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Matti_AI;
using UnityEngine;

namespace Resources.Scripts.Matti_AI
{
    public class MattiBaseAI : BaseGhostAI
    {
        private StateMachine _stateMachine;
        public IdleState _idleState;
        public ChaseState _chaseState;

        [HideInInspector]
        public List<GameObject> HumansInSight = new List<GameObject>();
        [HideInInspector] public GameObject closestHuman;
        
        [HideInInspector]
        public List<GameObject> DoorsInSight = new List<GameObject>();
        public LayerMask HumanLayerCheck;
    
    

        private void Awake()
        {
            _stateMachine = new StateMachine(gameObject);
            _idleState = new IdleState(_stateMachine);
            _chaseState = new ChaseState(_stateMachine);
            _stateMachine.Initialize(_idleState);
        }

        // Update is called once per frame
        void Update()
        {
            HumansInSight = CheckCloseObjectsInSight(gameObject, 10f, HumanLayerCheck);
            if (HumansInSight.Count > 0)
            {

                closestHuman = ClosestObjectInList(gameObject, HumansInSight);
            }



            _stateMachine.CurrentState.LogicUpdate();
        
        }
    }
}

