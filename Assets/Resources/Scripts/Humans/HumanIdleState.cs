using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanIdleState : StateMachineBehaviour
{
    private Transform _transform;



    public static List<GameObject> DoorsInSight = new List<GameObject>();
    
    
    
    public float randomMovementSpan;
    public float randomMovementChance;

    public LayerMask DoorLayerCheck;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        //adds randomness to direction 
        if (Random.Range(0, 100) <= randomMovementChance)
        {
            _transform.forward += new Vector3(Random.Range(-randomMovementSpan, randomMovementSpan), 0, Random.Range(-randomMovementSpan, randomMovementSpan)) * Time.deltaTime;
        }
        
        
        //Checks for collisions
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(_transform.position, _transform.forward.normalized, out hit, 1))
        {
            
            _transform.forward = Vector3.Reflect(_transform.forward, hit.normal);
            
        }
        
        //movement
        _transform.position += _transform.forward * (5f * Time.deltaTime);
        
        //anti fly away
        _transform.localEulerAngles = new Vector3(0f,_transform.localEulerAngles.y,0f);
        */
        
        //Doors
        
        Collider[] DoortColliders = Physics.OverlapSphere(_transform.position, 20, DoorLayerCheck );
        foreach (var DoorCollider in DoortColliders)
        {
            RaycastHit hit2 = new RaycastHit();
           

            //Calculates offset relative to the pivot of the door 
            Vector3 DoorLineTarget = (DoorCollider.transform.position + new Vector3(0, 1, 0)) +
                                     DoorCollider.transform.right.normalized;

            //Checks if door is in sight
            Physics.Linecast(_transform.position, DoorLineTarget, out hit2);
            Debug.DrawLine(_transform.position, DoorLineTarget);
            
            GameObject HitObject = hit2.collider.gameObject;
            
            if ( HitObject.CompareTag("Door") && !DoorsInSight.Contains(DoorCollider.gameObject))
            {
                Debug.Log("added");
                DoorsInSight.Add(DoorCollider.gameObject);
            }

            //removes door from list if out of sight
             else if (DoorsInSight.Contains(DoorCollider.gameObject) && !HitObject.CompareTag("Door"))
            {
                Debug.Log("remove");
               DoorsInSight.Remove(DoorCollider.gameObject);
            }
            
            
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
