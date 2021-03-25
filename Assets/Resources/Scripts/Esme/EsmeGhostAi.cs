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
    private Vector3 cauldronPosition;
    private Vector3 eyesPosition;
    private List<Collider> OthersColliders = new List<Collider>();
    private GhostBehaviour ghostBehaviour;
    private NavMeshAgent navMeshAgent;

    [SerializeField] public bool ShowDebugLines;

    [SerializeField] private float souls;

    void Start ()
    {
        ghostBehaviour = gameObject.GetComponent<GhostBehaviour>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        //get cauldron position on the NavMesh
        if (NavMesh.SamplePosition(GameObject.Find("Cauldron").transform.position, out NavMeshHit hit, 1.0f, 1))
        {
            cauldronPosition = hit.position;
        }
    }

    //check current state and performs that state's action
    void Update ()
    {
        souls = ghostBehaviour.getSouls();
        detectingOthersColliders();
        selectGhostState();
    
        switch (_state)
        {
            case EState.patrolling:
            {
                patrolling();
                break;
            }
            case EState.getPickup:
            {
                getPickup();
                break;
            }
            case EState.attackHuman:
            {
                attackHuman();
                break;
            }
            case EState.attackGhost:
            {
                attackGhost();
                break;
            }
            case EState.depositSouls:
            {
                depositSouls();
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

        //loop through all detected colliders, if the linecast does not detect a wall, add the collider to the otherColliders list
        foreach (Collider otherCollider in othersCollidersInRadius)
        {
            if (!Physics.Linecast(eyesPosition, otherCollider.transform.position, LayerMask.GetMask("Walls"))) 
            {
                OthersColliders.Add(otherCollider);
            }
        }

        //sort the list by distance
        OthersColliders = OthersColliders.OrderBy(otherCollider => Vector3.Distance(otherCollider.transform.position, transform.position)).ToList();
    }

    void selectGhostState () {
        //if you gather more than 5 souls, deposit to the cauldron
        if (ghostBehaviour.getSouls() >= 5) {
            _state = EState.depositSouls;
            return;
        }

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
                case "Ghost":
                {
                    if (ghostBehaviour.hasPickup && (int)GetComponent<PickupBehaviour>().Type == 1)
                    {
                        _state = EState.attackGhost;
                    }
                    else
                    {
                        _state = EState.patrolling;
                    }
                    break;
                }
            }
        }
        else
        {
            _state = EState.patrolling;
        }
    }

    //generate random position from AI location, then check whether it's on the NavMesh, then make it the new destination
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

    void attackGhost ()
    {
        navMeshAgent.destination = OthersColliders[0].transform.position;
    }

    void depositSouls ()
    {
        navMeshAgent.destination = cauldronPosition;
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

