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
    private Vector3 eyesPosition;
    private List<Collider> OthersColliders = new List<Collider>();

    GhostBehaviour ghostBehaviour;
    NavMeshAgent navMeshAgent;
    [SerializeField] bool ShowDebugLines;

    void Start ()
    {
        ghostBehaviour = gameObject.GetComponent<GhostBehaviour>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update ()
    {  
        detectingOthersColliders();
        selectGhostState();
    
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

        showDebugLines();
    }

    void detectingOthersColliders ()
    {
        eyesPosition = transform.position + new Vector3(0, 1, 0);
        Collider[] othersCollidersInRadius = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Humans", "Pickups"));
        OthersColliders = new List<Collider>();

        foreach (Collider otherCollider in othersCollidersInRadius)
        {
            if (!Physics.Linecast(eyesPosition, otherCollider.transform.position, LayerMask.GetMask("Walls"))) 
            {
                OthersColliders.Add(otherCollider);
            }
        }

        OthersColliders = OthersColliders.OrderBy(otherCollider => Vector3.Distance(otherCollider.transform.position, transform.position)).ToList();
    }

    void selectGhostState () {
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
                    else
                    {
                        _state = EState.patrolling;
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
        if (transform.position == patrolPosition || navMeshAgent.destination != patrolPosition || patrolPosition == new Vector3()) {
            if (NavMesh.SamplePosition(new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)) + transform.position, out NavMeshHit hit, 1.0f, 1))
            {
                patrolPosition = hit.position;
            }
        }

        navMeshAgent.destination = patrolPosition;
    }

    void getPickup ()
    {
        navMeshAgent.destination = OthersColliders[0].transform.position;
    }

    void attackHuman ()
    {
        navMeshAgent.destination = OthersColliders[0].transform.position;
    }

    void showDebugLines ()
    {
        if (ShowDebugLines)
        {
            switch (_state)
            {
                case EState.patrolling:
                {
                    Debug.DrawLine(eyesPosition, patrolPosition, Color.magenta);
                    break;
                }
                case EState.attackHuman:
                {
                    Debug.DrawLine(eyesPosition, OthersColliders[0].transform.position, Color.red);
                    break;
                }
                case EState.getPickup:
                {
                    Debug.DrawLine(eyesPosition, OthersColliders[0].transform.position, Color.green);
                    break;
                }
            }
        }
    }
}

