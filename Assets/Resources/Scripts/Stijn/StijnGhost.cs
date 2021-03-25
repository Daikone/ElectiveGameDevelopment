using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Stijn
{
    public class StijnGhost : BaseGhostAI
    {
        public NavMeshAgent navMeshAgent;

        public StijnStateMachine stateMachine;
        public WanderingState wandering;
        public ChasingState chasing;

        [HideInInspector] public List<GameObject> HumansVisible = new List<GameObject>();
        [HideInInspector] public GameObject nearbyHuman;
        public LayerMask HumanLayerCheck;

        [HideInInspector] public Vector3 latestPosHuman;


        private void Awake()
        {
            stateMachine = new StijnStateMachine(this);

            wandering = new WanderingState();
            chasing = new ChasingState();

            stateMachine.Initialize(wandering);
        }

        private void Update()
        {
            stateMachine.CurrentState.HandleInput(this);

            stateMachine.CurrentState.LogicUpdate(this);

            HumansVisible = CheckCloseObjectsInSight(gameObject, 15, HumanLayerCheck);
            if (HumansVisible.Count > 0)
            {
                nearbyHuman = ClosestObjectInList(gameObject, HumansVisible);
                latestPosHuman = nearbyHuman.transform.position;
            }
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate(this);
        }
    }
}