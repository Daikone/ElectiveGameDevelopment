using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    public class HaniAI : BaseGhostAI
    {

        public List<Room> rooms;
        
        private float rotationSpeed = 1f;
        private GameObject currentHuman;

        private STATE currentState;
        private ABILITY currentAbility;
        private Vector3 targetRoomPos;
        private Rigidbody rb;
        private float distanceToCurrentHuman;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            agent.speed = GetSpeed();
            MovetoPoint(ROOM.Hallway1);
            currentAbility = ABILITY.none;
            

            //rooms = new List<Room>();
        }

       // Update is called once per frame
       void Update()
       {
           if(rb.velocity.magnitude < .7f) //change state after colliding with human or getting a point maybe
               Invoke("ChangeState", 2f);
               //Debug.Log("notmoving");
           
           Debug.Log(currentState);
           Debug.Log(rb.velocity.magnitude);
           if (currentHuman != null)
           {
               distanceToCurrentHuman = (currentHuman.transform.position - transform.position).magnitude;
               Debug.Log(distanceToCurrentHuman);
               if (distanceToCurrentHuman >= 3f)
                   currentState = STATE.Idle;
               /*else if (distanceToCurrentHuman <= 0.1f)
                   currentState = STATE.Idle;*/
           }

           

           if (transform.position == targetRoomPos)
           {
               currentState = STATE.Idle;
               targetRoomPos = new Vector3(0, 0, 0);
           }

           /*if (carryingSouls >= 3)
               DepositSouls();*/
           if ( CheckHumanInfront() && currentState != STATE.SoulDeposit) // maybe only check when scouting or whatever so you can ignore them to deposit souls
               ChaseHuman();
           else if (currentState == STATE.Idle)
           {
               ChangeState();
               //InvokeRepeating("ChangeState", 0f, 5f); // being called multiple times
               //StartCoroutine(IdleLookAround());
           }

           
       }

        void ChangeState()
        {
            Debug.Log("change state called");
            int randomNumber = Random.Range(1, 3);

            switch (randomNumber)
            {
                case 1: StartCoroutine(RoamAround());Debug.Log("Roam");
                    break;
                case 2: MoveToRandomPoint();Debug.Log("changeRoom");
                    break;
                /*case 3: currentState = STATE.Idle;Debug.Log("idle");
                    break;*/
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
            
            //GetComponent<Rigidbody>().MovePosition(transform.forward + Vector3.forward); // maybe add speed

            yield return new WaitForSeconds(3f);

            currentState = STATE.Idle;
        }

        private void MoveToRandomPoint()
        {
            int randomNumber = Random.Range(0, rooms.Count);
            currentState = STATE.RoomChange;
            agent.SetDestination(rooms[randomNumber].GetPos());
            targetRoomPos = rooms[randomNumber].GetPos();
            Debug.Log(randomNumber);
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
        
        protected bool CheckHumanInfront()
        {
            //currentState = STATE.Scanning;
        
            Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3); // variable instead of hardcode
        
            //if(humansNearby.Contains<GameObject>(gameObject))
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
            
            //check infron instead of around so it doesn't check through walls
            //check for ghosts as well?

            CheckCloseObjectsInSight(this.gameObject, 4f, 2);

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
                Debug.Log("human not nul");
                Vector3 direction = currentHuman.transform.position - transform.position;
                direction.Normalize();

                agent.SetDestination(currentHuman.transform.position);
                //transform.Translate(direction * speed * Time.deltaTime);
                transform.forward = new Vector3(direction.x, 0, direction.z);
                //transform.LookAt(currentHuman.transform.position, Vector3.up); 
            }
            
            //smooth rotate to target
            //stop chasing if the target goes too far
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

        /*void OnCollisionEnter(Collision collider)
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
        }*/

        //CheckCloset()
    
        //protected PowerUp()*/
    
    }
}
