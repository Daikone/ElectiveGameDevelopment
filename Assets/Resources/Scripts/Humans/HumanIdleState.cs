using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanIdleState : StateMachineBehaviour
{
    private Transform _transform;

    private List<GameObject> DoorsInSight = new List<GameObject>();
    private List<GameObject> GhostsInSight = new List<GameObject>();
    private GameObject closestDoor;
    private GameObject closestGhost;
    
    public float randomMovementSpan;
    public float randomMovementChance;

    public LayerMask DoorLayerCheck;
    public LayerMask GhostLayerCheck;

    /*calculates the offset relative to the position of the door*/
    private Vector3 calculateDoorOffset( GameObject door)
    {
        Vector3 DoorOffset = (door.transform.position + new Vector3(0, 1, 0)) + door.transform.right.normalized;
        return DoorOffset;
    }
    List<GameObject> CheckCloseObjectsInSight(float radius, LayerMask layerMask, string tag)
    {

        List<GameObject> objectsInRange = new List<GameObject>();
        Collider[] Colliders = Physics.OverlapSphere(_transform.position, radius, layerMask );
        foreach (var Collider in Colliders)
        {
            //Checks if door is in sight
            Vector3 lineTarget = new Vector3();
            if (layerMask == DoorLayerCheck)
            {
                lineTarget = calculateDoorOffset(Collider.gameObject);
            }
            else
            {
                lineTarget = Collider.gameObject.transform.position;
            }
            
            
            if (Physics.Linecast(_transform.position, lineTarget, out var hit2))
            {
                GameObject hitObject = hit2.collider.gameObject;
                Debug.DrawLine(_transform.position, lineTarget);
                
                //adds to list 
                if (objectsInRange != null)
                {
                    if ( hitObject.CompareTag(tag) && !objectsInRange.Contains(Collider.gameObject))
                    {
                        objectsInRange.Add(Collider.gameObject);
                        
                    }

                    //removes door from list if out of sight
                    else if (objectsInRange.Contains(Collider.gameObject) && !hitObject.CompareTag(tag))
                    {
                        objectsInRange.Remove(Collider.gameObject);
                    }
                }
            }
        }

        return objectsInRange;
    }
    GameObject ClosestObjectInList(List<GameObject> list)
    {

        int objPos = new int();
        float closestDistance = Mathf.Infinity;
        
        foreach (var obj in list)
        {
            float distance = Vector3.Distance(_transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                objPos = list.FindIndex(a => a.gameObject == obj);
                closestDistance = distance;
            }
        }
        
        return list[objPos];

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
        
        
        
        
        
        DoorsInSight = CheckCloseObjectsInSight(20f, DoorLayerCheck, "Door");
        GhostsInSight = CheckCloseObjectsInSight(20f, GhostLayerCheck, "Ghost");
        
        if (DoorsInSight != null && DoorsInSight.Count > 0)
        {
            closestDoor = ClosestObjectInList(DoorsInSight);
            Debug.DrawLine(_transform.position, calculateDoorOffset(closestDoor), Color.blue);
            
        }
        
        if (GhostsInSight!= null && GhostsInSight.Count > 0)
        {
            closestGhost = ClosestObjectInList(GhostsInSight);
            Debug.DrawLine(_transform.position, closestGhost.transform.position, Color.green);
            
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
