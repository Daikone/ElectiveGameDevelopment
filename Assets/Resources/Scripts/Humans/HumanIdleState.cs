using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class HumanIdleState : BaseHumanBehaviour
{
    private Transform _transform;
    private HumanGeneralBehaviour Behaviour;
    private NavMeshAgent _navAgent;

    public float randomMovementSpan;
    public float randomMovementChance;

    public float speed;
    
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        Behaviour = animator.GetBehaviour<HumanGeneralBehaviour>();
        _navAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _navAgent.ResetPath();
        
        //Movement
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
        
        //anti fly away
        _transform.localEulerAngles = new Vector3(0f,_transform.localEulerAngles.y,0f);
        _transform.position += _transform.forward * (speed * Time.deltaTime);
        
        
        
        




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
