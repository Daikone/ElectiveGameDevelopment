using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EsmeGhostAi : MonoBehaviour
{
    enum EState { patrolling, attackHuman, attackGhost, depositSouls, getPickup }
    [SerializeField] private EState _state = EState.patrolling;
    private Vector3 patrolPosition;

    List<Collider> OthersColliders = new List<Collider>();

    GhostBehaviour ghostBehaviour;
    NavMeshAgent navMeshAgent;

    void Start ()
    {
        ghostBehaviour = gameObject.GetComponent<GhostBehaviour>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {  
        detecting();
    
        switch (_state)
        {
            case EState.patrolling:
            {
                patrolling();
                break;
            }
            case EState.attackHuman:
            {
                attackHuman();
                break;
            }
            case EState.getPickup:
            {
                getPickup();
                break;
            }
        }
    }

    void detecting ()
    {
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Humans", "Pickups"));
        OthersColliders = new List<Collider>();

        foreach (Collider collider in collidersInRadius)
        {
            if (!Physics.Linecast(transform.position, collider.transform.position, LayerMask.GetMask("Walls"))) 
            {
                OthersColliders.Add(collider);
                // Debug.DrawLine(transform.position, collider.transform.position, Color.cyan);
            }
        }

        OthersColliders.OrderByDescending(collider => Vector3.Distance(collider.transform.position, transform.position));

        if (OthersColliders.Count > 0)
        {
            switch (OthersColliders[0].tag)
            {
                case "Pickup":
                {
                    if (!ghostBehaviour.hasPickup)
                    {
                        _state = EState.getPickup;
                    }
                    break;
                }
                case "Human":
                {
                    _state = EState.attackHuman;
                    break;
                }
                // case "Ghost":
                // {
                //     _state = EState.attackGhost;
                //     break;
                // }
            }
        }
        else
        {
            _state = EState.patrolling;
        }
    }

    void patrolling () {
        if (navMeshAgent.destination == patrolPosition || patrolPosition == new Vector3()) {
            if (NavMesh.SamplePosition(new Vector3(Random.Range(-16, 16), 0, Random.Range(-16, 16)), out NavMeshHit hit, 1.0f, 1))
            {
                patrolPosition = hit.position;
                navMeshAgent.destination = patrolPosition;
            }
        }
        Debug.DrawLine(transform.position, patrolPosition, Color.magenta);
    }

    void getPickup ()
    {
        patrolPosition = new Vector3();
        navMeshAgent.destination = OthersColliders[0].transform.position;
        Debug.DrawLine(transform.position, OthersColliders[0].transform.position, Color.green);
    }

    void attackHuman ()
    {
        patrolPosition = new Vector3();
        if (OthersColliders.Count > 0)
        {
            navMeshAgent.destination = OthersColliders[0].transform.position;
            Debug.DrawLine(transform.position, OthersColliders[0].transform.position, Color.red);
        }
    }
}
