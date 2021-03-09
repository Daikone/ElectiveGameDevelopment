using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    public class HaniAI : BaseGhostAI
    {

        private float rotationSpeed = 100f;
        //private Transform[] rooms;

        private Vector3 targetPosition; //unused
        private GameObject currentHuman;

        private STATE currentState;
        //protected ROOM currentRoom;

        // Start is called before the first frame update
        void Start()
        {
            speed = 3f;
            agent.speed = speed;
            MovetoPoint("Hallway1");
            //currentRoom = ROOM.MainHall;
            //currentState = STATE.Idle;
       }

       // Update is called once per frame
       void Update()
       {
           if ( CheckHumanInfront()) // maybe only check when scouting or whatever so you can ignore them to deposit souls
               ChaseHuman();
           /*else if (currentState == STATE.Idle)
           {
               //InvokeRepeating("ChangeState", 0f, 1);
               StartCoroutine(IdleLookAround());
               //IdleLookAround();
           }*/
                // might not need invoke repeat
           Debug.Log(currentState);

           //CheckHumanInfront();
           
           /*if (Input.GetMouseButtonDown(0)) {
               RaycastHit hit;
                
               if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                   agent.destination = hit.point;
               }
           }*/
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
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    agent.destination = hit.point;
                }
            }
        }

        protected void RoamAround()
        {
            currentState = STATE.RoomRoam;
            //maybe self written as well
        }

        private IEnumerator IdleLookAround()
        {
            
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

            Debug.Log(Time.deltaTime);
            yield return new WaitForSeconds(2f);

            transform.Rotate(0, -Time.deltaTime * rotationSpeed, 0);

            yield return new WaitForSeconds(2f);
            yield return null;
            //Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -Time.deltaTime * rotationSpeed, 0), .5f);
        }
        
        public void MovetoPoint(string roomName)
        {
            foreach (var room in gm.rooms)
            {
                if (room.name == roomName)
                {
                    agent.SetDestination(room.GetPos());
                    //break;
                }
            }
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
            //ability us that has a coolDown
            //ability that disappears after one use
            
            //collide with ghost to steal thier soul (animation with sound)
                //if other ghost's current power up is GhostSteal
                    //nothing happens so go back to idling or whatever you were doing do something else
                //else
                    //gain half their soul and change current power up to none
            
            //i GUESS YOU CAN ONLY HAVE 1 power up at a time
        }

        /*protected void UpdateTargetPosition(string roomName) // optional way of implememnting rooms
        {
            switch (currentRoom)
            {
            case ROOM.MainHall: targetPosition = gm.rooms[0].GetPos();
                            break;
            case ROOM.Hallway1: targetPosition = gm.rooms[1].GetPos();
                            break;
            default: break;
            }
        }*/

        //CheckCloset()
    
        //protected PowerUp()*/
    
    }
}
