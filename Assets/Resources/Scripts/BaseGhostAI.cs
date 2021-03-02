using System;
using UnityEngine;

public enum STATE { Hunting, RoomChange, RoomRoam, SoulDeposit };
public class BaseGhostAI : MonoBehaviour
{
    public Transform[] rooms;

    protected bool humanInfront;
    protected bool isStunned;
    protected float carryingSouls;
    protected float speed;

    protected GameObject currentHuman;

    private STATE currentState;

    private void Start()
    {
        currentState = STATE.RoomChange;
        speed = 3f;
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
            ChaseHuman();

        CheckHumanInfront();
    }

    protected void MovetoPoint(Vector3 pos)
    {
        currentState = STATE.RoomChange;
    }

    protected void RoamAround()
    {
        currentState = STATE.RoomRoam;
    }

    protected void DepositSouls()
    {
        currentState = STATE.SoulDeposit;
    }

    protected void CheckHumanInfront()
    {
        Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3); //Could optimiz further maybe

        foreach (var human in humansNearby)
        {
            if (human.CompareTag("Human"))
            {
                Vector3 humanDirection = human.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, humanDirection) > .5f)
                {
                    currentHuman = human.gameObject;
                    currentState = STATE.Hunting;
                }
                //remove and check overlap sphere instead of dot matrix    
            }
                
        }
    }

    protected void ChaseHuman()
    {
        
        
        Vector3 direction = currentHuman.transform.position - transform.position;
        direction.Normalize();
        
        transform.Translate(direction * speed * Time.deltaTime);
        //transform.LookAt(currentHuman.transform.position);
        transform.forward = direction;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            //get soul and stop chasing to do soemthing else
        }
    }
}
    
    //CheckCloset()
    
    //protected PowerUp()

