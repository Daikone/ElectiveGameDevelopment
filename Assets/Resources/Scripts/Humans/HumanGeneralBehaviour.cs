using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HumanGeneralBehaviour : BaseHumanBehaviour
{
    private Transform _transform;
    [HideInInspector]
    public List<GameObject> DoorsInSight = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> GhostsInSight = new List<GameObject>();
    
    public float GhostSpottingDistance;

    [HideInInspector]
    public GameObject closestDoor;
    [HideInInspector] 
    public GameObject secondClosestDoor;
    [HideInInspector]
    public GameObject closestGhost;
    
    public LayerMask DoorLayerCheck;
    public LayerMask GhostLayerCheck;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Detecting
        DoorsInSight = CheckCloseObjectsInSight(animator.gameObject,20f, DoorLayerCheck, false );
        GhostsInSight = CheckCloseObjectsInSight(animator.gameObject,GhostSpottingDistance, GhostLayerCheck, true);
        //Debug.Log(GhostsInSight.Count);
        
        if (DoorsInSight != null && DoorsInSight.Count > 0)
        {
            if (DoorsInSight.Count > 0)
            {
                closestDoor = ClosestObjectInList(animator.gameObject, DoorsInSight);
            }
            List<GameObject> secondClosestDoors = new List<GameObject>();
            foreach (var door in DoorsInSight)
            {
                if (door != closestDoor)
                {
                    secondClosestDoors.Add(door);
                }
            }

            if (secondClosestDoors.Count > 0)
            {
                secondClosestDoor = ClosestObjectInList(animator.gameObject, secondClosestDoors);
            }
            
            Debug.DrawLine(_transform.position, calculateDoorOffset(closestDoor), Color.blue);
        }

        if (GhostsInSight.Count > 0)
        {
            closestGhost = ClosestObjectInList(animator.gameObject, GhostsInSight);
            Debug.DrawLine(_transform.position, closestGhost.transform.position, Color.red);
        }
        
        //Behaviour Switches
        //Triggers behaviours
        if (GhostsInSight.Count > 0 && closestDoor != null)
        {
            animator.SetBool("SeesGhost", true);
        }
        else
        {
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
