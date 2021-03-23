using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerCheck : MonoBehaviour
{
    //public GameObject[] triggers;
    //public List<Collider[]> listofTriggers;
    private int randomNum;
    private Collider[] objectInArea;
    
    private int humans;
    private int ghosts;

    private bool followNewObject;
    private GameObject followObject;
    //public Vector3 camraPos;
    private void Start()
    {
        /*listofTriggers = new List<Collider[]>();
        
        foreach (var trigger in triggers)
        {
            listofTriggers.Add(Physics.OverlapBox(trigger.transform.position, trigger.transform.position + new Vector3(4,0,4),Quaternion.identity, LayerMask.GetMask("Humans")));
        }*/
        
        InvokeRepeating("CheckStuff", 0f, 5f);

        humans = 0;
        ghosts = 0;
    }

    private void Update()
    {
        //Collider[] objectsInBox = Physics.OverlapBox(new Vector3(0f,1f,-15.25f), new Vector3(4,1,4),Quaternion.identity, LayerMask.GetMask("Humans"));
        //Debug.Log("HUMANS IN HERE: " + objectsInBox.Length);
        
        /*Vector3 size = itemTransform.TransformVector(itemCollider.size / 2);
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        size.z = Mathf.Abs(size.z);
        Collider[] results = Physics.OverlapBox(itemTransform.position, size);*/
        //Debug.Log(gameObject.name +"has Humans = " + humans + " : Ghosts = " + ghosts);
        
        
        //Check positions of everything and then see who all are approximately close
        
        //Debug.Log(gameObject.name +"has Humans = " + humans + " : Ghosts = " + ghosts);
        
        //physicsOverlap over random objects and then if many are next to it then teleport he camera to that location

        if (followObject != null && followNewObject == true)
        {
            float offsetDistance = 2.5f;
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, followObject.transform.position, out hit, 5f, LayerMask.GetMask("Walls")))
            {
                //Quaternion newRot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30, followObject.transform.eulerAngles.y, 0), .125f);
                //transform.Rotate(newRot);
                transform.rotation = Quaternion.Euler(30, followObject.transform.eulerAngles.y, 0);
                offsetDistance = 1.5f;
            }
            else
            {
                offsetDistance = 2.5f;
                transform.LookAt(followObject.transform);
            }
            
            //transform.position
            
            Vector3 monsterOffset = followObject.transform.forward.normalized * -offsetDistance;
            monsterOffset += new Vector3(0f, offsetDistance, 0f);
            //transform.position = followObject.transform.position + monsterOffset;
            Vector3 newPo = Vector3.Lerp(transform.position, followObject.transform.position + monsterOffset, .125f);
            transform.position = newPo;

            //HIT RAYCASTFROM CAMERA TO obj
            
            // Does the ray intersect any objects excluding the player layer
            
            //something to check if the camer has been in the same place for long
            
            //camera fixed position but looks at the target like cctv;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
            humans++;
        if (other.CompareTag("Ghost"))
            ghosts++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))
            humans--;
        if (other.CompareTag("Ghost"))
            ghosts--;
    }*/

    void CheckStuff()
    {
        objectInArea = Physics.OverlapBox(new Vector3(-12f, 1f, -27.5f), new Vector3(16, 1, 16),
            Quaternion.identity, LayerMask.GetMask("Humans", "Ghosts"));

        foreach (var obj in objectInArea)
        {
            if (obj.CompareTag("Human"))
                humans++;
            if (obj.CompareTag("Ghost"))
                ghosts++;
        }

        randomNum = Random.Range(0, objectInArea.Length);
        
        Invoke("ClearThis", 1f);
        CheckAround();
    }

    void ClearThis()
    {
        humans = 0;
        ghosts = 0;
    }

    void CheckAround()
    {
        Collider[] aroundStuff = Physics.OverlapSphere(objectInArea[randomNum].transform.position, 3f,
            LayerMask.GetMask("Humans", "Ghosts"));

        if (aroundStuff.Length < 2)
        {
            randomNum = Random.Range(0, objectInArea.Length);
            CheckAround(); //Maybe also check if its a ghost (Could store in 2 differrent list of colliders to specifically check)
        }
        else
        {
            followNewObject = true;
            followObject = objectInArea[randomNum].gameObject;
        }
        
        Debug.Log("Objects around this random object in a 3 float space: " + aroundStuff.Length);
    }
}
