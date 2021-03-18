using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexAISpace
{
    //All the AI States 
    public enum STATE
    {
        Idle,
        RoomChange,
        Hunting,
        HuntingGhost,
        RoomRoam,
        SoulDunking,
        Scanning,
        Walking,
    }

    //All the Rooms and hallways

    public enum ROOM
    {
        CommonRoom,
        InnerRoom,
        BedRoom,
        Kitchen,
        DinnerRoom
    }

    public class AlexAI : BaseGhostAI
    {
        //General AI var
        private float rotationSpeed = 1f;
        private GameObject currentHuman;
        private bool HasMoved;
        public AudioClip Kill;

        //Statemachine var
        public List<Room> rooms;

        protected ROOM currentRoom;
        protected ROOM NewRoom;  
        private STATE newState;
        private STATE currentState;

        //Ability var
        private ABILITY currentAbility;

        // Start is called before the first frame update
        void Start()
        {
            agent.speed = GetSpeed();
            currentAbility = ABILITY.none;
            currentRoom = ROOM.InnerRoom;
            newState = STATE.Idle;
            HasMoved = false;

            GetComponent<AudioSource>().playOnAwake = false;
            GetComponent<AudioSource>().clip = Kill;
        }

        // Update is called once per frame
        void Update()
        {
            //maybe an if(Souls = x) go dunk then an else if
            if (CheckHumanInfront()) // maybe only check when scouting or whatever so you can ignore them to deposit souls
                ChaseHuman();
            else if (currentState == STATE.Idle)
            {
                //Start look around
                StartCoroutine(IdleLookAround());
                
            }

            if (agent.remainingDistance < 0.1 && HasMoved == true)
            {
                currentState = STATE.Idle;
                HasMoved = false;
                Debug.Log("I Stopped");
            }
        }


        //The State change machine along a random number generator that chooses a random state if the character is idle
        void ChangeState()
        {
            Debug.Log("change state called");
            newState = ((STATE)Random.Range(1, 2));
            Debug.Log("The new state is " + newState);
            currentState = newState;
            if(currentState == STATE.RoomChange)
            {
                NewRoom = (ROOM)Random.Range(1, 5);
                Debug.Log("I am going to reap some souls in " + NewRoom);
                MovetoPoint(NewRoom);
                HasMoved = true;
            }
        }
        //if the character is idle look around for a bit
        private IEnumerator IdleLookAround()
        {
            currentState = STATE.Scanning;
            transform.Rotate(0, rotationSpeed, 0);// this is still being called after

            yield return new WaitForSeconds(1f);
            transform.Rotate(0, -rotationSpeed, 0);

            yield return new WaitForSeconds(.9f);
            ChangeState();
        }

/*        protected void RoamAround()
        {
            currentState = STATE.RoomRoam;
            
        }*/


        //For basic movement 
        public void MovetoPoint(ROOM roomName)
        {
            string name = roomName.ToString();

            foreach (var room in rooms)
            {
                if (room.name == name)
                {
                    agent.SetDestination(room.GetPos());
                    StartCoroutine(Wait());
                    break;
                }
            }
        }

        //Waiting function that I can use for more than 1 purpose if I want to
        protected IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);
            HasMoved = true;
        }

        //Human Hunter system
        protected bool CheckHumanInfront()
        {
            //currentState = STATE.Scanning;

            Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3);

            foreach (var human in humansNearby)
            {
                if (human.CompareTag("Human"))
                {
                    //Debug.Log("Human nearby");
                    currentHuman = human.gameObject;
                    Debug.Log("I got you!");
                    return true;
                }
            }
            return false;
        }
        protected void ChaseHuman()
        {
            currentState = STATE.Hunting;
            if (currentHuman != null)
            {
                //Debug.Log("human not nul");
                Vector3 direction = currentHuman.transform.position - transform.position;
                direction.Normalize();

                agent.SetDestination(currentHuman.transform.position);
                //transform.Translate(direction * speed * Time.deltaTime);
                transform.forward = new Vector3(direction.x, 0, direction.z);
                //transform.LookAt(currentHuman.transform.position, Vector3.up); 
            }
        }
        //Dunking souls
        protected void DepositSouls()
        {
            //WIP
        }


        //PVP mode (if needed)
        private void Stealsouls()
        {
            currentAbility = ABILITY.SoulSteal;
        }
    }
}
