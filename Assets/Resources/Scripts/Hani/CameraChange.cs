using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraChange : MonoBehaviour
{
    private Collider[] objectInArea;
    private GameObject followObject;

    private List<GameObject> ghosts;
    private List<GameObject> humans;
    private void Start()
    {
        InvokeRepeating("CheckEntitiesInMap", 0f, 2f); // Check all the living entities on the map 
    }

    private void Update()
    {
        FollowObject(); // Make camera follow
    }

    void CheckEntitiesInMap()
    {
        objectInArea = Physics.OverlapBox(new Vector3(-12f, 1f, -27.5f), new Vector3(16, 1, 16),    //Check all the living entities on the map and store it in a list 
            Quaternion.identity, LayerMask.GetMask("Humans", "Ghosts"));

        humans = new List<GameObject>();
        ghosts = new List<GameObject>();
        
        foreach (var obj in objectInArea) // Go through 
        {
            if (obj.CompareTag("Human"))
                humans.Add(obj.gameObject);
            if (obj.CompareTag("Ghost"))
                ghosts.Add(obj.gameObject);
        }

        CheckAroundCurrentObject(); // Checks around the the current ghost and human
    }

    void CheckAroundCurrentObject()
    {
        int randomGhost = Random.Range(0, ghosts.Count);
        int randomHuman = Random.Range(0, humans.Count);

        Collider[] objectsAroundGhosts = Physics.OverlapSphere(ghosts[randomGhost].transform.position, 3f,  //Store all the entities around this ghost in a list
            LayerMask.GetMask("Ghosts","Humans"));
        Collider[] objectsAroundHumans = Physics.OverlapSphere(humans[randomHuman].transform.position, 3f,  //Store all the entities around this human in a list
            LayerMask.GetMask("Ghosts","Humans"));

        if (objectsAroundGhosts.Length < 2) // Find new ghost to check 
            CheckAroundCurrentObject();
        
        else if (objectsAroundHumans.Length > 2) 
            followObject = humans[randomHuman].gameObject; // Make this human the current follow object if more than 2 entities next to it
        else 
            followObject = ghosts[randomGhost].gameObject; // Make this ghost the current follow object if more than 2 or more entities next to it
    }

    void FollowObject()
    {
        if (followObject != null) { // If there is an object to follow
            
            float offsetDistance = 2.5f;
            Camera camerComp = GetComponent<Camera>();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, //check if wall is blocking th camera
                LayerMask.GetMask("Walls")))
            {
                transform.rotation = Quaternion.Euler(30, followObject.transform.eulerAngles.y, 0);  // Move the camera infront of wall and increase fov
                offsetDistance -= 1f;
                camerComp.fieldOfView = 90;
            }
            else
            {
                offsetDistance = 2.5f;
                transform.LookAt(followObject.transform); //Set the field of view to normal and the distance from object ot 2.5 units and make the camer look at the entity
                camerComp.fieldOfView = 60;
            }

            Vector3 monsterOffset = (followObject.transform.forward.normalized * -offsetDistance) + new Vector3(0f, offsetDistance, 0f); // Move the camara near the enitity and follow it
            Vector3 newPo = Vector3.Lerp(transform.position, followObject.transform.position + monsterOffset, .125f);
            transform.position = newPo;
            
            //camera fixed position but looks at the target like cctv;
        }
    }
}

