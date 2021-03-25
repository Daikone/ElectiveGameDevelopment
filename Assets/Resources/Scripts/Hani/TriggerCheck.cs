using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerCheck : MonoBehaviour
{
    private int randomNum;
    private Collider[] objectInArea;
    private GameObject followObject;

    private List<GameObject> ghosts;
    private List<GameObject> humans;
    private void Start()
    {
        InvokeRepeating("CheckEntitiesInMap", 0f, 5f);
    }

    private void Update()
    {
        FollowObject();
    }

    void CheckEntitiesInMap()
    {
        objectInArea = Physics.OverlapBox(new Vector3(-12f, 1f, -27.5f), new Vector3(16, 1, 16),
            Quaternion.identity, LayerMask.GetMask("Humans", "Ghosts"));

        randomNum = Random.Range(0, objectInArea.Length);
        

        humans = new List<GameObject>();
        ghosts = new List<GameObject>();
        
        foreach (var obj in objectInArea)
        {
            if (obj.CompareTag("Human"))
                humans.Add(obj.gameObject);
            if (obj.CompareTag("Ghost"))
                ghosts.Add(obj.gameObject);
        }
        
        //Debug.Log("There are " + ghosts.Count + " Ghosts and " + humans.Count + " Humans right  now!");

        CheckAroundCurrentObject();
    }

    void CheckAroundCurrentObject()
    {
        int randomGhost = Random.Range(0, ghosts.Count);
        int randomHuman = Random.Range(0, humans.Count);
        
        /*Collider[] objectsAroundEntity = Physics.OverlapSphere(objectInArea[randomNum].transform.position, 3f,
            LayerMask.GetMask("Humans", "Ghosts"));

        if (objectsAroundEntity.Length < 2)
        {
            randomNum = Random.Range(0, objectInArea.Length);
            CheckAroundCurrentObject(); //Maybe also check if its a ghost (Could store in 2 differrent list of colliders to specifically check)
        }
        else
        {
            followObject = objectInArea[randomNum].gameObject;
        }*/
        
        Collider[] objectsAroundGhosts = Physics.OverlapSphere(ghosts[randomGhost].transform.position, 3f,
            LayerMask.GetMask("Ghosts","Humans"));
        Collider[] objectsAroundHumans = Physics.OverlapSphere(humans[randomHuman].transform.position, 3f,
            LayerMask.GetMask("Ghosts","Humans"));

        if (objectsAroundGhosts.Length < 2)
        {
            CheckAroundCurrentObject();
        }
        else if (objectsAroundHumans.Length > 2)
        {
            followObject = humans[randomHuman].gameObject;
        }
        else
        {
            followObject = ghosts[randomGhost].gameObject;
        }
    }

    void FollowObject()
    {
        if (followObject != null) {
            
            float offsetDistance = 2.5f;
            Camera camerComp = GetComponent<Camera>();

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f,
                LayerMask.GetMask("Walls")))
            {
                transform.rotation = Quaternion.Euler(30, followObject.transform.eulerAngles.y, 0);
                offsetDistance = 1f;
                camerComp.fieldOfView = 90;
            }
            else
            {
                offsetDistance = 2.5f;
                transform.LookAt(followObject.transform);
                camerComp.fieldOfView = 60;
            }

            Vector3 monsterOffset = (followObject.transform.forward.normalized * -offsetDistance) + new Vector3(0f, offsetDistance, 0f);
            Vector3 newPo = Vector3.Lerp(transform.position, followObject.transform.position + monsterOffset, .125f);
            transform.position = newPo;


            //something to check if the camera has been in the same place for long

            //camera fixed position but looks at the target like cctv;
            
            //distance between the close objects
        }
    }
}

