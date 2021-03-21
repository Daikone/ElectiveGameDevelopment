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
    public enum STATE
    {
        Idle,
        Hunting,
        RoomChange,
        RoomRoam,
        SoulDeposit,
        Scanning
    }
    
    public enum ROOM{ MainHall, Hallway1, TopRightRoom}
    public enum INTERACTABLE{ None, Ghost, Pickup, Human}
    
    public class HaniAI : BaseGhostAI
    {
        //constantly in room change state
        
        public List<Room> rooms;
        
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

        // Start is called before the first frame update
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
           Debug.Log("HaniAI currentState = " + currentState);

           //stuck in room change
       }

        void ChangeState()
        {
            Debug.Log("change state called");
            int randomNumber = Random.Range(2, 3);

            switch (randomNumber)
            {
                case 1: StartCoroutine(RoamAround());
                    break;
                case 2: MoveToRandomPoint();
                    break;
                /*case 3: currentState = STATE.Idle;Debug.Log("idle");
                    break;*/
            }
        }

        private void CheckState()
        {
            /*if (carryingSouls >= 3)
               DepositSouls();*/
           
            
            if (CheckObjectsInfront() == INTERACTABLE.Human && currentState != STATE.SoulDeposit)
            {
                //CancelInvoke();
                ChaseHuman();
            }
            
            switch (currentState)
            {
                case STATE.Hunting:
                    if (currentHuman != null && Vector3.Distance(transform.position, currentHuman.transform.position) >= 10f)
                        currentHuman = null;
                    if (currentHuman == null)
                        currentState = STATE.Idle;
                    break;
                
                case STATE.RoomChange:
                    if (Vector3.Distance(transform.position, targetRoomPos) <= 1f)
                        currentState = STATE.Idle;
                    break;
                
                case STATE.Idle: MoveToRandomPoint(); break;
            }
        }
        
        private IEnumerator IdleLookAround()
        {
            transform.Rotate(0, rotationSpeed, 0);// this is still being called after

            yield return new WaitForSeconds(1f);
            transform.Rotate(0, -rotationSpeed, 0);

            yield return new WaitForSeconds(.1f);
            currentState = STATE.Hunting;// change to smething else
            //ChangeState(); 
        }

        IEnumerator RoamAround()
        {
            currentState = STATE.RoomRoam;
            
            Vector3 direction = new Vector3(0, Random.Range(-1f, 1f), 0);
            GetComponent<Rigidbody>().MovePosition(direction * speed * Time.deltaTime);
            
            // // maybe add speed
            //agent.SetDestination(rooms[0].GetPos());

            yield return new WaitForSeconds(3f);

            currentState = STATE.Idle;
            
            
        }

        private void MoveToRandomPoint()
        {
            
            if(currentState == STATE.Idle)
              randomNumber = Random.Range(0, rooms.Count);
            currentState = STATE.RoomChange;
            targetRoomPos = rooms[randomNumber].GetPos();
            agent.SetDestination(targetRoomPos);
            //Invoke("MoveToRandomPoint", 5f);
            //Debug.Log(randomNumber);
        }
        
        private void MovetoPoint(ROOM roomName)
        {
            currentState = STATE.RoomChange;
            
            string name = roomName.ToString();
            
            foreach (var room in rooms)
            {
                if (room.name == name)
                {
                    agent.SetDestination(room.GetPos());
                    targetRoomPos = room.GetPos();
                    break;
                }
            }
            //Maybe needs a way to debug if the room exists
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
                else if (nearestObject.CompareTag("Pickup"))
                {
                    return INTERACTABLE.Pickup;
                }
                else if (nearestObject.CompareTag("Ghost"))
                {
                    return INTERACTABLE.Ghost;
                }
            }

            return INTERACTABLE.None;
        }    

        protected void DepositSouls()
        {
            currentState = STATE.SoulDeposit;
            MovetoPoint(ROOM.MainHall);
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

//Vector3 direction = currentHuman.transform.position - transform.position;
//direction.Normalize();
//transform.Translate(direction * speed * Time.deltaTime);
//transform.forward = new Vector3(direction.x, 0, direction.z);
//transform.LookAt(currentHuman.transform.position, Vector3.up);
//rb.MovePosition(direction * speed * Time.deltaTime);