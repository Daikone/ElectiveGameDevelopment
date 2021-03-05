using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaniAITest : BaseGhostAI
{
    private float rotationSpeed = 100f;

    private float initialYRotation;

    private bool yRotationReceived;
    // Start is called before the first frame update
    void Start()
    {
        //MovetoPoint("Hallway1");
    }

    // Update is called once per frame
    void Update()
    {
        /*if ( CheckHumanInfront() == true)
            ChaseHuman();
        else if (currentState == STATE.Idle)
        {
            InvokeRepeating("ChangeState", 0f, 1);
            StartCoroutine(IdleLookAround());
            //IdleLookAround();
        }
             // might not need invoke repeat
        Debug.Log(currentState);*/
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

    void RoamAround()
    {
        Debug.Log("fwafwaf");
    }

    private IEnumerator IdleLookAround()
    {
        //transform.rotation = Quaternion.Euler(0f,Time.deltaTime,0f)
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y + (Time.deltaTime * rotationSpeed), 0), .5f);
        
        //transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
        
        Debug.Log(Time.deltaTime);
        yield return new WaitForSeconds(2f);
        //rotation = 0;
        
        //transform.Rotate(0, -Time.deltaTime * rotationSpeed, 0);
        
        yield return new WaitForSeconds(2f);
        yield return null;
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -Time.deltaTime * rotationSpeed, 0), .5f);
    }

    /*private void IdleLookAround()
    {
        Debug.Log(initialYRotation);
        if (transform.rotation.y >= initialYRotation + 90)
            transform.Rotate(0, -Time.deltaTime * rotationSpeed, 0);
        else
            transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
            
    }*/
}

/*using System;
using UnityEngine;
using UnityEngine.AI;

//public enum STATE { Idle, Hunting, RoomChange, RoomRoam, SoulDeposit, Scanning };
public enum ROOM {MainHall, Hallway1}
public class BaseGhostAI : MonoBehaviour
{
    public GameManager gm;
    public NavMeshAgent agent;
    //private Transform[] rooms;

    protected bool isStunned;
    protected float carryingSouls;
    protected float speed;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            carryingSouls++;
            Destroy(other.gameObject);
            //currentState = STATE.Idle;
            //get soul and stop chasing to do soemthing else
            //could add animation and sound here
        }
    }
    
    //protected Vector3 targetPosition; //unused
    //protected GameObject currentHuman;

    //protected STATE currentState;
    //protected ROOM currentRoom;

    private void Start()
    {
        /*currentState = STATE.Idle;
        currentRoom = ROOM.MainHall;
        speed = 3f;
        agent.speed = speed;#1#
    }

    private void Update()
    {
        /*if(currentState == STATE.Hunting)
            ChaseHuman();#1#

        //CheckHumanInfront();
    }

    /*public void MovetoPoint(string roomName)
    {
        foreach (var room in gm.rooms)
        {
            if (room.name == roomName)
            {
                agent.SetDestination(room.GetPos());
                break;
            }
        }
    }#1#

    /*protected void RoamAround()
    {
        currentState = STATE.RoomRoam;
        //maybe self written as well
    }#1#

    protected void DepositSouls()
    {
        //maybe self written
        //IN CAULDRON
    }

    /*protected bool CheckHumanInfront()
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
    }#1#

    /*protected void ChaseHuman()
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
        
        //Make it work with update when called from other script
    }#1#

    
    

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
    }#1#
}
    
    //CheckCloset()
    
    //protected PowerUp()*/


