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
        escapeDoor = Behaviour.closestDoor;
       
       

      // _transform.forward = calculateDoorOffset(escapeDoor) - _transform.position;
       //_transform.forward += ghostAdjust;
       
      // _transform.position += _transform.forward.normalized * (speed * Time.deltaTime);
      if (escapeDoor != null&& Behaviour.DoorsInSight.Count > 0 && Vector3.Distance(_transform.position, calculateDoorOffset(escapeDoor)) > 2)
      {
          _navAgent.SetDestination(calculateDoorOffset(escapeDoor));
      }
      else
      {
          _transform.forward = _transform.position - Behaviour.closestGhost.transform.position;
          _navAgent.SetDestination(_transform.position + _transform.forward.normalized);
      }
      
      
      if(Vector3.Distance(_transform.position, calculateDoorOffset(escapeDoor)) <= 1)
      {
          _navAgent.SetDestination(calculateDoorOffset(escapeDoor) + _transform.forward.normalized * 2) ;
          animator.SetBool("SeesGhost", false);
          
      }
      
    }

   
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
