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
        public SaveSoulsState _saveSoulsState;

        public GhostBehaviour behaviour;

        [HideInInspector]
        public List<GameObject> HumansInSight = new List<GameObject>();
        [HideInInspector] public GameObject closestHuman;
        [HideInInspector] public Vector3 closestHumanLastPos;
        
        
        public List<GameObject> CloseDoors= new List<GameObject>();
        public LayerMask HumanLayerCheck;
        public LayerMask DoorLayerCheck;
        public LayerMask PickUpLayerCheck;
        
        public GameObject ClosestDoor;

        [HideInInspector] 
        public Vector3 ClosestDoorPos;


        public Queue<GameObject> PreviousDoors = new Queue<GameObject>();
        [SerializeField]
        

        [HideInInspector] public GameObject cauldron;

        [HideInInspector] public List<GameObject> PickUpsInSight = new List<GameObject>();
        [HideInInspector] public GameObject closestPickUp;
        
        
        


        private void Awake()
        {
            // Is this scalable? What if you get 10 more states? Maybe store a State Array or Dictionary with an Enum as a key (cast to int for Array index)
            _stateMachine = new StateMachine(gameObject);
            _idleState = new IdleState(_stateMachine);
            _chaseState = new ChaseState(_stateMachine);
            _saveSoulsState = new SaveSoulsState(_stateMachine);
            _stateMachine.Initialize(_idleState);

            behaviour = GetComponent<GhostBehaviour>();
            // Perhaps this could've been stored in a central location so that only one object has to find it
            cauldron = GameObject.Find("Cauldron");
        }

        // Update is called once per frame
        void Update()
        {
            carryingSouls = behaviour.carryingSouls;


			// 15? Arbitrary magic number
			//checking for humans 
			HumansInSight = CheckCloseObjectsInSight(gameObject, 15, HumanLayerCheck);
            //Getting the closest human
            if (HumansInSight.Count > 0)
            {
                closestHuman = ClosestObjectInList(gameObject, HumansInSight);
                closestHumanLastPos = closestHuman.transform.position;
            }
            else
            {
                closestHuman = null;
            }
            
            
            
            // checking for doors
            CloseDoors = CheckCloseObjects(gameObject, 20f, DoorLayerCheck);
            ClosestDoor = ClosestObjectInList(gameObject, CloseDoors);

            // adds the already used doors to a queue and removes them from the list of close doors, which makes the ghost patrol in circles around the map
            if (Vector3.Distance(transform.position, calculateDoorOffset(ClosestDoor)) <= 1f)
            {
                if (!PreviousDoors.Contains(ClosestDoor))
                {
                    PreviousDoors.Enqueue(ClosestDoor);
                }
                
                // 4? Arbitrary magic number
                if (PreviousDoors.Count >= 4)
                {
                     PreviousDoors.Dequeue();
                }
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



			// checking for pickUps 
			// 10? Arbitrary magic number
			PickUpsInSight = CheckCloseObjectsInSight(gameObject, 10, PickUpLayerCheck);
            if (PickUpsInSight.Count > 0)
            {
                closestPickUp = ClosestObjectInList(gameObject,PickUpsInSight );
            }
            
            
            
            _stateMachine.CurrentState.LogicUpdate();
            // Why clear them? Why not keep everything in a radius around the player and check if they're in the frustum?
            // Vector3.Dot(forward, TARGETDIRECTION)
            HumansInSight.Clear();

            
            
        }
    }
}

