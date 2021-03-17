using  System;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.AI;

public enum ABILITY{none, WallWalk, SoulSteal, SpeedBoost}
public class BaseGhostAI : MonoBehaviour
{
    public GameManager gm;
    public NavMeshAgent agent;

    protected bool isStunned;
    protected float carryingSouls;
    private static int NORMALSPEED = 4;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            carryingSouls++;
            Destroy(other.gameObject);
        }
    }

    public int GetSpeed()
    {
        return NORMALSPEED;
    }
    
    
    // !!!!!!!! Matti Stuff !!!!!!!!!!!!!
    
    
    /// Checks for objects in sight within a certain radius, returns a list with these GameObjects 
    /// <param name="self">GameObject from which the function should be executed </param>
    /// <param name="radius">Radius in which to check </param>
    /// <param name="layerMask">Layermask to only find objects on a certain layer, for example: doors </param>
    protected List<GameObject> CheckCloseObjectsInSight(GameObject self, float radius, LayerMask layerMask)
    {

        List<GameObject> objectsInRange = new List<GameObject>();
        LayerMask ignoreLayer;
        ignoreLayer =~(1 << 10);

        
        
        
        Collider[] colliders = Physics.OverlapSphere(self.transform.position, radius, layerMask );
        foreach (var collider in colliders)
        {
            //Checks if door is in sight
            Vector3 lineTarget;
            LayerMask doorlayer = new LayerMask();
            
            if (collider.gameObject.layer == 8)
            {
                lineTarget = calculateDoorOffset(collider.gameObject);
            }
            else
            {
                lineTarget = collider.gameObject.transform.position;
            }

            
                if (Physics.Linecast(self.transform.position, lineTarget, out var hit2, ignoreLayer))
                {
                    GameObject hitObject = hit2.collider.gameObject;
                    Debug.DrawLine(self.transform.position, lineTarget);
                
                    //adds to list 
                    if (objectsInRange != null)
                    {
                        if ( hitObject == collider.gameObject && !objectsInRange.Contains(collider.gameObject))
                        {
                            objectsInRange.Add(collider.gameObject);
                        
                        }

                        //removes door from list if out of sight
                        else if (objectsInRange.Contains(collider.gameObject) && hitObject != collider.gameObject)
                        {
                            objectsInRange.Remove(collider.gameObject);
                        }
                    }
                }
            
        }
        return objectsInRange;
    }
    
    
    /// <summary>
    /// Checks for the clostest object in a list
    /// </summary>
    /// <param name="self">GameObject form where the function is called </param>
    /// <param name="list"> List in which to look for the object </param>
    /// <returns>Closest Object </returns>
    protected GameObject ClosestObjectInList(GameObject self, List<GameObject> list)
    {
        if(list.Count > 0){}
        int objPos = new int();
        float closestDistance = Mathf.Infinity;
        
        foreach (var obj in list)
        {
            float distance = Vector3.Distance(self.transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                objPos = list.FindIndex(a => a.gameObject == obj);
                closestDistance = distance;
            }
        }
        
        return list[objPos];

    }

    /// <summary>
    /// Fixes the weird pivot points of doors in our assets and returns the center of the door 
    /// </summary>
    /// <param name="door">The door of which you want to calculate the correct position </param>
    /// <returns>The center position of the door instead of the pivot</returns>
    protected Vector3 calculateDoorOffset( GameObject door)
    {
        Vector3 doorOffset = (door.transform.position + new Vector3(0, 1, 0)) + door.transform.right.normalized;
        return doorOffset;
    }
        
    
}

