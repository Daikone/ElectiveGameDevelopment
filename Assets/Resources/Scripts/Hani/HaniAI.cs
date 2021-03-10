using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enumerable = System.Linq.Enumerable;

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
    
    public enum ABILITY{none, WallWalk, SoulSteal, Speedoost}
    public class HaniAI : BaseGhostAI
    {

        public List<Room> rooms;
        
        private float rotationSpeed = 1f;
        private GameObject currentHuman;

        private STATE currentState;
        private ABILITY currentAbility;
        //protected ROOM currentRoom;

        // Start is called before the first frame update
        void Start()
        {
            speed = 3f;
            agent.speed = speed;
            MovetoPoint(ROOM.MainHall);
            currentAbility = ABILITY.none;
            //currentState = STATE.Idle;

            //rooms = new List<Room>();
        }

       // Update is called once per frame
       void Update()
       {
           if ( CheckHumanInfront()) // maybe only check when scouting or whatever so you can ignore them to deposit souls
               ChaseHuman();
           else if (currentState == STATE.Idle)
           {
               //InvokeRepeating("ChangeState", 0f, 1);
               StartCoroutine(IdleLookAround());
               //IdleLookAround();
           }
                // might not need invoke repeat
           Debug.Log(currentState);
        }

        void ChangeState()
        {
            Debug.Log("change state called");
            int randomNumber = Random.Range(1, 4);

            /*switch (randomNumber)
            {
                case 1: currentState = STATE.RoomRoam;Debug.Log("Roam");
                    break;
                case 2: currentState = STATE.RoomChange;Debug.Log("changeRoom");
                    break;
                case 3: currentState = STATE.Idle;Debug.Log("idle");
                    break;
            }*/
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

        protected void RoamAround()
        {
            currentState = STATE.RoomRoam;
            //maybe self written as well
        }

        
        
        public void MovetoPoint(ROOM roomName)
        {
            string name = roomName.ToString();
            
            foreach (var room in rooms)
            {
                if (room.name == name)
                {
                    agent.SetDestination(room.GetPos());
                    break;
                }
            }
            //Maybe needs a way to debug if the room exists
        }
        
        protected bool CheckHumanInfront()
        {
            //currentState = STATE.Scanning;
        
            Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3);
        
            foreach (var human in humansNearby)
            {
                if (human.CompareTag("Human"))
                {
                    Debug.Log("Human nearby");
                    currentHuman = human.gameObject;
                    return true;
                }
            }
            return false;
        }    

        protected void DepositSouls()
        {
            //maybe self written
            //IN CAULDRON
        }

        protected void ChaseHuman()
        {
            currentState = STATE.Hunting;

            if (currentHuman != null)
            {
                Debug.Log("human not nul");
                Vector3 direction = currentHuman.transform.position - transform.position;
                direction.Normalize();

                agent.SetDestination(currentHuman.transform.position);
                //transform.Translate(direction * speed * Time.deltaTime);
                transform.forward = new Vector3(direction.x, 0, direction.z);
                //transform.LookAt(currentHuman.transform.position, Vector3.up); 
            }
        }

        private void Stealsouls()
        {
            currentAbility = ABILITY.SoulSteal;
            //ability that disappears after one use

            //collide with ghost to steal thier soul (animation with sound)
            //if other ghost's current power up is GhostSteal
            //nothing happens so go back to idling or whatever you were doing do something else
            //else
            //gain half their soul and change current power up to none

            //i GUESS YOU CAN ONLY HAVE 1 power up at a time
        }

        void OnCollisionEnter(Collision collider)
        {
            if (collider.collider.CompareTag("Ghost"))
            {
                if (currentAbility == ABILITY.SoulSteal)
                {
                    var otherAI = collider.collider.GetComponent<HaniAI>(); // current ability will be in the baseghostAI

                    if (otherAI.currentAbility != ABILITY.SoulSteal)
                    {
                        carryingSouls += otherAI.carryingSouls; // Add to yor souls and make a sound with mabe animation
                        ChangeState(); // Run away or do something else
                    }
                    else
                        ChangeState(); // Do something else if the other has the same ability
                }
            }
        }

        //CheckCloset()
    
        //protected PowerUp()*/
    
    }
}
