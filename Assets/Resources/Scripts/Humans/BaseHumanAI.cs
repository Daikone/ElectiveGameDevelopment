using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHumanBehaviour : StateMachineBehaviour
{
    
    protected List<GameObject> CheckCloseObjectsInSight(GameObject self, float radius, LayerMask layerMask, string tag, bool ignoringGhostSphere)
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

            if (ignoringGhostSphere)
            {
                if (Physics.Linecast(self.transform.position, lineTarget, out var hit2, ignoreLayer))
                {
                    GameObject hitObject = hit2.collider.gameObject;
                    Debug.DrawLine(self.transform.position, lineTarget);
                
                    //adds to list 
                    if (objectsInRange != null)
                    {
                        if ( hitObject.CompareTag(tag) && !objectsInRange.Contains(collider.gameObject))
                        {
                            objectsInRange.Add(collider.gameObject);
                        
                        }

                        //removes door from list if out of sight
                        else if (objectsInRange.Contains(collider.gameObject) && !hitObject.CompareTag(tag))
                        {
                            objectsInRange.Remove(collider.gameObject);
                        }
                    }
                }
            }
            else if(!ignoringGhostSphere)
            {
                if (Physics.Linecast(self.transform.position, lineTarget, out var hit2))
                {
                    GameObject hitObject = hit2.collider.gameObject;
                    Debug.DrawLine(self.transform.position, lineTarget);
                    
                
                    //adds to list 
                    if (objectsInRange != null)
                    {
                        if ( hitObject.CompareTag(tag) && !objectsInRange.Contains(collider.gameObject))
                        {
                            objectsInRange.Add(collider.gameObject);
                        
                        }

                        //removes door from list if out of sight
                        else if (objectsInRange.Contains(collider.gameObject) && !hitObject.CompareTag(tag))
                        {
                            objectsInRange.Remove(collider.gameObject);
                        }
                    }
                }
            }
            
        }
        return objectsInRange;
    }

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

    protected Vector3 calculateDoorOffset( GameObject door)
    {
        Vector3 doorOffset = (door.transform.position + new Vector3(0, 1, 0)) + door.transform.right.normalized;
        return doorOffset;
    }

   
}
