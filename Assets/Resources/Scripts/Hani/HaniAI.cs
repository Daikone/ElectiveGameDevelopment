using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace HaniAISpace
{
    public enum STATE { Idle, Hunting, RoomChange, SoulDeposit } 
    public enum INTERACTABLE{ None, Ghost, Pickup, Human}
    
    public class HaniAI : BaseGhostAI
    {
        
        public List<Transform> rooms; // Used to store the transforms of places that the ghost can go
        
        private STATE currentState;
        private INTERACTABLE Interactable;
        private Vector3 targetRoomPos;
        private GameObject currentChasableObject;
        private GhostBehaviour gb;
        private int randomNumber; // Used to get a random number to move rooms
        
        void Start()
        {
            gb = gameObject.GetComponent<GhostBehaviour>();
            currentState = STATE.Idle;
            //sheck if human in view
        }

       void Update()
       {
           CheckState(); //Check the state and update the ghost accordingly
       }

        private void CheckState()
        {
            if (gb.getSouls() >= 3) // Deposit the souls if you are carrying 3 or more
                DepositSouls();
            else
                CheckCloseInteractable(); //Check for humans or pickups nearby
            
            switch (currentState) // Check current state
            {
                case STATE.SoulDeposit: //change the state back to idle if the cauldron is reached
                    if(Vector3.Distance(transform.position, targetRoomPos) < .1f)
                        currentState = STATE.Idle;
                    break;
            
                case STATE.Hunting: // Change the state back to idle if the current human is destroyed or too far away
                    if (currentChasableObject == null)
                        currentState = STATE.Idle;
                    else if (currentChasableObject != null && Vector3.Distance(transform.position, currentChasableObject.transform.position) >= 10f)
                        currentChasableObject = null;
                    break;
            
                case STATE.RoomChange: // Change state to idle when target position almost reached
                    if (Vector3.Distance(transform.position, targetRoomPos) <= 1f)
                        currentState = STATE.Idle;
                    break;
            
                case STATE.Idle: MoveToRandomPoint(); break; // Move to a random location when idle
            }
        }

        private void MoveToRandomPoint() // Change state to room change and move to a random point on the map assigned in the list
        {
            
            if(currentState == STATE.Idle)
              randomNumber = Random.Range(0, rooms.Count);
            
            currentState = STATE.RoomChange;
            targetRoomPos = rooms[randomNumber].position;
            agent.SetDestination(targetRoomPos);
        }
        
        private INTERACTABLE CheckObjectsInfront() // Check objects around the current object or ghost
        {
            Collider[] objNearby = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Humans", "Pickups")); // store the humans and pickups in a list

            GameObject nearestObject = CheckClosestObject(objNearby); // get the closest object in the list of humans and pickups
            
            if (nearestObject != null)
            {
                if (gb.hasPickup == false && gb.isPickupActive == false && nearestObject.CompareTag("Pickup"))
                {
                    currentChasableObject = nearestObject;  // Set current object to chase to the pickup if my ghost does not have pickup and is nto using one
                    return INTERACTABLE.Pickup;
                }
            
                if (nearestObject.CompareTag("Human")) // Set current object to chase to human
                {
                    currentChasableObject = nearestObject;
                    return INTERACTABLE.Human; 
                }
            }
            return INTERACTABLE.None;
        }    

        void DepositSouls() // Go to the cauldron to deposit souls
        {
            currentState = STATE.SoulDeposit;
            targetRoomPos = rooms[0].position;
            agent.SetDestination(targetRoomPos);
        }

        void ChaseObject() // Chase the current object to Chase
        {
            currentState = STATE.Hunting;

            if (currentChasableObject != null)
                agent.SetDestination(currentChasableObject.transform.position);
        }

        private GameObject CheckClosestObject(Collider[] objList) // Return the closest object in a list of objects
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

        private void CheckCloseInteractable() // check what the current close object is (if it's a pickup or Human)
        {
            switch (CheckObjectsInfront())
            {
                case INTERACTABLE.Human:
                    RaycastHit hit;
                    if (!Physics.Linecast(transform.position, currentChasableObject.transform.position, out hit, //If it is a human then check if there is a wall blocking you and if it's not then chase that human
                        LayerMask.GetMask("Walls")))
                        ChaseObject();
                    else
                        currentState = STATE.Idle;
                    break;
                
                case INTERACTABLE.Pickup: //if it is a pickup got ot the pickup
                    ChaseObject();
                    break;
            }
        }
    }
}