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
        //Detecting doors and ghosts 
        DoorsInSight = CheckCloseObjectsInSight(animator.gameObject,20f, DoorLayerCheck, false );
        GhostsInSight = CheckCloseObjectsInSight(animator.gameObject,GhostSpottingDistance, GhostLayerCheck, true);
        
        
        // Sets the closest door 
        if (DoorsInSight != null && DoorsInSight.Count > 0)
        {
            if (DoorsInSight.Count > 0)
            {
                closestDoor = ClosestObjectInList(animator.gameObject, DoorsInSight);
            }
            
            Debug.DrawLine(_transform.position, calculateDoorOffset(closestDoor), Color.blue);
        }
    
        //Sets the closest ghost
        if (GhostsInSight.Count > 0)
        {
            closestGhost = ClosestObjectInList(animator.gameObject, GhostsInSight);
            Debug.DrawLine(_transform.position, closestGhost.transform.position, Color.red);
        }
        else
        {
            closestGhost = null;
        }
        
        
        //Switches states if ghost is in sight
        if (GhostsInSight.Count > 0 && closestDoor != null)
        {
            animator.SetBool("SeesGhost", true);
        }
        else
        {
            animator.SetBool("SeesGhost", false);
        }
    }

}
