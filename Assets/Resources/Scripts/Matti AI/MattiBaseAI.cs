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
        public List<GameObject> CloseDoors= new List<GameObject>();
        public LayerMask HumanLayerCheck;
        public LayerMask DoorLayerCheck;

        [HideInInspector]
        public GameObject ClosestDoor;

        [HideInInspector] 
        public Vector3 ClosestDoorPos;


        public List<GameObject> PreviousDoors = new List<GameObject>();
        
        


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
            HumansInSight = CheckCloseObjectsInSight(gameObject, 30f, HumanLayerCheck);
            if (HumansInSight.Count > 0)
            {
                closestHuman = ClosestObjectInList(gameObject, HumansInSight);
            }
            
            
            
            CloseDoors = CheckCloseObjects(gameObject, 30f, DoorLayerCheck);
            ClosestDoor = ClosestObjectInList(gameObject, CloseDoors);

            if (Vector3.Distance(transform.position, calculateDoorOffset(ClosestDoor)) <= 1)
            {
                PreviousDoors.Insert(0,ClosestDoor);
            }
            foreach (var door in PreviousDoors)
            {
                CloseDoors.Remove(door);
                
            }

            if (CloseDoors.Count > 0)
            {
                ClosestDoor = ClosestObjectInList(gameObject, CloseDoors);
            }
            
            ClosestDoorPos = calculateDoorOffset(ClosestDoor);
            
            _stateMachine.CurrentState.LogicUpdate();
            

        }
    }
}

