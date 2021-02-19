using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanIdleState : StateMachineBehaviour
{
    private Transform _transform;



    public static List<GameObject> DoorsInSight = new List<GameObject>();
    private GameObject closestDoor;
    
    public float randomMovementSpan;
    public float randomMovementChance;

    public LayerMask DoorLayerCheck;

    /*calculates the offset relative to the position of the door*/
    private Vector3 calculateDoorOffset( GameObject DoorPosition)
    {
        Vector3 DoorOffset = (DoorPosition.transform.position + new Vector3(0, 1, 0)) + DoorPosition.transform.right.normalized;
        return DoorOffset;
    }
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //adds randomness to direction 
        if (Random.Range(0, 100) <= randomMovementChance)
        {
            _transform.forward += new Vector3(Random.Range(-randomMovementSpan, randomMovementSpan), 0, Random.Range(-randomMovementSpan, randomMovementSpan)) * Time.deltaTime;
        }
        
        
        //Checks for collisions
        if (Physics.Raycast(_transform.position, _transform.forward.normalized, out var hit, 1))
        {
            _transform.forward = Vector3.Reflect(_transform.forward, hit.normal);
        }
        
        //movement
        _transform.position += _transform.forward * (5f * Time.deltaTime);
        
        //anti fly away
        _transform.localEulerAngles = new Vector3(0f,_transform.localEulerAngles.y,0f);
        
        
        
        //Doors
        Collider[] doorColliders = Physics.OverlapSphere(_transform.position, 20, DoorLayerCheck );
        foreach (var doorCollider in doorColliders)
        {
            RaycastHit hit2;
           
            //Checks if door is in sight
            if (Physics.Linecast(_transform.position, calculateDoorOffset(doorCollider.gameObject), out hit2))
            {
                GameObject hitObject = hit2.collider.gameObject;
                Debug.DrawLine(_transform.position, calculateDoorOffset(doorCollider.gameObject));

                //adds door to list 
                if (DoorsInSight != null)
                {
                    if ( hitObject.CompareTag("Door") && !DoorsInSight.Contains(doorCollider.gameObject))
                    {
                        DoorsInSight.Add(doorCollider.gameObject);
                        
                    }

                    //removes door from list if out of sight
                    else if (DoorsInSight.Contains(doorCollider.gameObject) && !hitObject.CompareTag("Door"))
                    {
                        DoorsInSight.Remove(doorCollider.gameObject);
                    }
                }
            }
        }
        
        
        //checking fo rthe closest door in sight
        if (DoorsInSight == null) return;
        {
            float closestDistance = Mathf.Infinity;
            foreach (var Door in DoorsInSight)
            {
                float distance = Vector3.Distance(_transform.position, Door.transform.position);
                if (distance < closestDistance)
                {
                    closestDoor = Door;
                    closestDistance = distance;
                }
                
            }
            //Calculates offset relative to the pivot of the door 
            Vector3 doorLineTarget = (closestDoor.transform.position + new Vector3(0, 1, 0)) + closestDoor.transform.right.normalized;
                    
            Debug.DrawLine(_transform.position, doorLineTarget, Color.red);
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
