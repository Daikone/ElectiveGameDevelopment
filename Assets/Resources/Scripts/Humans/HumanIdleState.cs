using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanIdleState : StateMachineBehaviour
{
    private Transform _transform;
    private Position _position;
    
    
    public float randomMovementSpan;
    public float randomMovementChance;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
        
        _transform.position += _transform.forward * (5f * Time.deltaTime);
        //anti fly away
        _transform.localEulerAngles = new Vector3(0f,_transform.localEulerAngles.y,0f);
        //animator.transform.position = new Vector3(animator.transform.position.x, 2f, animator.transform.position.z);

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
