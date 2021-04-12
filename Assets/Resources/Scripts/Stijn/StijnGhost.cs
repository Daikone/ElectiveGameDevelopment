using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Stijn
{
    public class StijnGhost : BaseGhostAI
    {
        // There is a navmesh agent in the base ghost, why did you need a seperate one?
        public NavMeshAgent navMeshAgent;

        public StijnStateMachine stateMachine;
        public WanderingState wandering;
        public ChasingState chasing;

        // Is there any particular reason why this is public? Look up encapsulation (you can achieve it by having a public get and a private set)
        // Getters and setters are also not visible in Unity Editor
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