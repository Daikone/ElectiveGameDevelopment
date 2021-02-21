using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanEscapingState : BaseAIBehaviour
{
    private HumanGeneralBehaviour Behaviour;
    private Transform _transform;
    public float ghostAvoidanceDistance;
    public float speed;

    private GameObject escapeDoor;
    private bool isCloseToGhost;
    
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Behaviour = animator.GetBehaviour<HumanGeneralBehaviour>();
        _transform = animator.transform;
        escapeDoor = Behaviour.closestDoor;
        isCloseToGhost = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        
       Vector3 ghostAdjust = new Vector3();
       foreach (var ghost in Behaviour.GhostsInSight)
       {
           if (Vector3.Distance(_transform.position, ghost.transform.position) < ghostAvoidanceDistance)
           {
               ghostAdjust += _transform.position - ghost.transform.position;
               if (isCloseToGhost == false && Behaviour.DoorsInSight.Count > 0)
               {
                   int escapeDoorIndex = Behaviour.DoorsInSight.FindIndex(a => a.gameObject == escapeDoor);
                   if (escapeDoorIndex == Behaviour.DoorsInSight.Count -1)
                   {
                       escapeDoor = Behaviour.DoorsInSight.First();
                   }
                   else
                   {
                       escapeDoor = Behaviour.DoorsInSight[escapeDoorIndex + 1];
                   }
               }
               isCloseToGhost = true;
           }
           else
           {
               isCloseToGhost = false;
           }
            
       }

       _transform.forward = calculateDoorOffset(escapeDoor) - _transform.position;
       _transform.forward += ghostAdjust;
       
       _transform.position += _transform.forward.normalized * (speed * Time.deltaTime);
       
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
