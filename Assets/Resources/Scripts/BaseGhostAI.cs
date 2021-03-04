using System;
using UnityEngine;
using UnityEngine.AI;

public enum STATE { Hunting, RoomChange, RoomRoam, SoulDeposit };
public enum ROOM {MainHall, Hallway1}
public class BaseGhostAI : MonoBehaviour
{
    private GameManager gm;
    private NavMeshAgent agent;
        
    public Transform[] rooms;

    protected bool humanInfront;
    protected bool isStunned;
    protected float carryingSouls;
    protected float speed;
    protected Vector3 targetPosition;

    protected GameObject currentHuman;

    protected STATE currentState;
    protected ROOM currentRoom;

    private void Start()
    {
        currentRoom = ROOM.MainHall;
        currentState = STATE.RoomChange;
        speed = 3f;
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        /*if (currentState == STATE.RoomChange)
            MovetoPoint(rooms[Random.Range(0,9)].position);
        else if (currentState == STATE.Hunting)
            CheckHumanInfront();
        else if (currentState == STATE.RoomRoam)
            RoamAround();
        else if (currentState == STATE.SoulDeposit)
            DepositSouls();*/
        if(currentState == STATE.Hunting)
            ChaseHuman(currentHuman);

        CheckHumanInfront();
        SetTargetPositions();
    }

    protected void MovetoPoint(ROOM room)
    {
        agent.SetDestination(targetPosition);
        currentState = STATE.RoomChange;
        //agent.speed = 8f;
        //some tweaking like setting/adjusting speed
    }

    protected void RoamAround()
    {
        currentState = STATE.RoomRoam;
        //maybe self written as well
    }

    protected void DepositSouls()
    {
        currentState = STATE.SoulDeposit;
        //maybe self written
    }

    protected GameObject CheckHumanInfront()
    {
        Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3); //Could optimiz further maybe
        GameObject humanObject = null;
        
        foreach (var human in humansNearby)
        {
            if (human.CompareTag("Human"))
            {
                currentHuman = human.gameObject;
                humanObject = currentHuman;
                currentState = STATE.Hunting;
            }
                
        }
        
        return humanObject;
    }

    protected void ChaseHuman(GameObject human)
    {
        currentState = STATE.Hunting;

        if (human != null)
        {
            Vector3 direction = human.transform.position - transform.position;
            direction.Normalize();

            transform.Translate(direction * speed * Time.deltaTime);
            transform.forward = new Vector3(direction.x, 0, direction.z);
            //transform.LookAt(currentHuman.transform.position, Vector3.up);
        }
        
        //Make it work with update when called from other script
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            carryingSouls++;
            Destroy(other.gameObject);
            //get soul and stop chasing to do soemthing else
        }
    }

    protected void SetTargetPositions()
    {
        switch (currentRoom)
        {
            case ROOM.MainHall: targetPosition = rooms[0].position;
                                break;
            case ROOM.Hallway1: targetPosition = rooms[1].position;
                                break;
            /*case ROOM.MainHall: targetPosition = rooms[0].position;
                break;
            case ROOM.MainHall: targetPosition = rooms[0].position;
                break;*/
            //maybe custom room object instead of enum would be better
            default: break;
        }
    }
}
    
    //CheckCloset()
    
    //protected PowerUp()

