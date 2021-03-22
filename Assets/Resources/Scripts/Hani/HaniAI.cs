using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace HaniAISpace
{
    public enum STATE { Idle, Hunting, RoomChange, SoulDeposit } 
    public enum INTERACTABLE{ None, Ghost, Pickup, Human}
    
    public class HaniAI : BaseGhostAI
    {
        
        public List<Transform> rooms;
        
        private float rotationSpeed = 1f;
        private GameObject currentHuman;
        private float speed;

        private STATE currentState;
        private INTERACTABLE Interactable;
        private ABILITY currentAbility;
        private Vector3 targetRoomPos;
        private Rigidbody rb;
        private float distanceToCurrentHuman;

        private int randomNumber;

        // Chase fix + check distnace to wall and from wall to human
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            speed = GetSpeed();
            agent.speed = GetSpeed();
            //MovetoPoint(ROOM.Hallway1);
            currentState = STATE.Idle;
            currentAbility = ABILITY.none;
            
            //rooms = new List<Room>();
        }

       // Update is called once per frame
       void Update()
       {
           CheckState();
           //Debug.Log("HaniAI currentState = " + currentState);
       }

        private void CheckState()
        {
            /*if (carryingSouls >= 3)
               currentState = STATE.SoulDeposit;*/

            if (currentState != STATE.SoulDeposit || currentState != STATE.Hunting)
            {
                switch (CheckObjectsInfront())
                {
                    case INTERACTABLE.Human:
                        ChaseHuman();
                        break;
                
                    case INTERACTABLE.Pickup: ChaseHuman();
                        break;/*ChaseHuman();
                        break;*/
                    
                    case INTERACTABLE.Ghost: //ChaseHuman();
                        break;
                }
            }
            
            
            switch (currentState)
            {
                case STATE.SoulDeposit: //DepositSouls();
                    break;
                
                case STATE.Hunting:
                    if (currentHuman != null && Vector3.Distance(transform.position, currentHuman.transform.position) >= 10f)
                        currentHuman = null;
                    else if (currentHuman == null)
                        currentState = STATE.Idle;
                    break;
                
                case STATE.RoomChange:
                    if (Vector3.Distance(transform.position, targetRoomPos) <= 1f)
                        currentState = STATE.Idle;
                    break;
                
                case STATE.Idle: MoveToRandomPoint(); break;
            }
        }

        private void MoveToRandomPoint()
        {
            
            if(currentState == STATE.Idle)
              randomNumber = Random.Range(0, rooms.Count);
            
            currentState = STATE.RoomChange;
            targetRoomPos = rooms[randomNumber].position;
            agent.SetDestination(targetRoomPos);
        }
        
        private INTERACTABLE CheckObjectsInfront()
        {

            Collider[] objNearby = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Humans", "Walls", "Pickups", "Ghosts")); // variable instead of hardcode

            GameObject nearestObject = CheckClosestObject(objNearby);

            if (nearestObject != null)
            {
                if (nearestObject.CompareTag("Human"))
                {
                    currentHuman = nearestObject;
                    return INTERACTABLE.Human;
                }

                if (nearestObject.CompareTag("Pickup"))
                {
                    currentHuman = nearestObject;
                    return INTERACTABLE.Pickup;
                }

                if (nearestObject.CompareTag("Ghost"))
                {
                    currentHuman = nearestObject;
                    return INTERACTABLE.Ghost;
                }
                    
            }

            return INTERACTABLE.None;
        }    

        protected void DepositSouls()
        {
            currentState = STATE.SoulDeposit;
            targetRoomPos = rooms[0].position;
            agent.SetDestination(targetRoomPos);
            //reset to idle
        }

        protected void ChaseHuman()
        {
            currentState = STATE.Hunting;

            if (currentHuman != null)
            {
                agent.SetDestination(currentHuman.transform.position);
            }
        }

        private void Stealsouls()
        {
            currentAbility = ABILITY.SoulSteal;
        }

        private GameObject CheckClosestObject(Collider[] objList)
        {
            GameObject closeObj;

            if (objList.Length > 0)
            {
                closeObj = objList[0].gameObject;
                foreach (var obj in objList)
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) <
                        Vector3.Distance(transform.position, closeObj.transform.position))
                        closeObj = obj.gameObject;
                }
                
                return closeObj;
            }

            return null;
        }
    }
}