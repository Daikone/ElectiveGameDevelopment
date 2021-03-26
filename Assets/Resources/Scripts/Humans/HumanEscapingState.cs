using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HumanEscapingState : BaseHumanBehaviour
{
    private HumanGeneralBehaviour Behaviour;
    private Transform _transform;
    public float ghostAvoidanceDistance;
    public float speed;
    private NavMeshAgent _navAgent;
    private Vector3 _destination;

    private GameObject escapeDoor;
    
    private static readonly int SeesGhost = Animator.StringToHash("SeesGhost");

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Behaviour = animator.GetBehaviour<HumanGeneralBehaviour>();
        _transform = animator.transform;
        _navAgent = animator.gameObject.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        

        if (Behaviour.closestGhost != null)
        {
            // if there is a ghost and a door in sight the destination is set to the closest door
            if (escapeDoor != null&& Behaviour.DoorsInSight.Count > 0 && Vector3.Distance(_transform.position, calculateDoorOffset(escapeDoor)) > 2)
            {
               _destination = calculateDoorOffset(escapeDoor);
            }
            // if there is no door insight and the distance to the ghost is less than three, the ghost runs in the opposite direction of the ghost 
            else if(Vector3.Distance(_transform.position, Behaviour.closestGhost.transform.position) <= 3)
            {
                _transform.forward = _transform.position - Behaviour.closestGhost.transform.position;
                _destination = _transform.position + _transform.forward.normalized*2;
                
                // prevents getting stuck in corners
                if (Physics.Raycast(_transform.position, _transform.forward.normalized, out var hit, 1.5f))
                {
                    _transform.forward = Vector3.Reflect(_transform.forward, hit.normal);
                        _destination = _transform.position + _transform.forward.normalized;
                }
            }

            if (escapeDoor != null)
            {
                // makes the human go through the door if close enough 
                if(Vector3.Distance(_transform.position, calculateDoorOffset(escapeDoor)) <= 1)
                {
                    _destination = calculateDoorOffset(escapeDoor) + _transform.forward.normalized * 2 ;
                    animator.SetBool("SeesGhost", false);
          
                }
            }


            _navAgent.SetDestination(_destination);

        }


    }

}
