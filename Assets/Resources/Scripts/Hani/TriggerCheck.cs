using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerCheck : MonoBehaviour
{
    private int randomNum;
    private Collider[] objectInArea;
    private bool followNewObject;
    private GameObject followObject;
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
        CheckAroundCurrentObject();
    }

    void CheckAroundCurrentObject()
    {
        Collider[] aroundStuff = Physics.OverlapSphere(objectInArea[randomNum].transform.position, 3f,
            LayerMask.GetMask("Humans", "Ghosts"));

        if (aroundStuff.Length < 2)
        {
            randomNum = Random.Range(0, objectInArea.Length);
            CheckAroundCurrentObject(); //Maybe also check if its a ghost (Could store in 2 differrent list of colliders to specifically check)
        }
        else
        {
            followNewObject = true;
            followObject = objectInArea[randomNum].gameObject;
        }
    }

    void FollowObject()
    {
        if (followObject != null && followNewObject == true) {
            
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
        }
    }
}

