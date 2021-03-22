using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    //public GameObject[] triggers;
    //public List<Collider[]> listofTriggers;
    
    private int humans;
    private int ghosts;
    private void Start()
    {
        /*listofTriggers = new List<Collider[]>();
        
        foreach (var trigger in triggers)
        {
            listofTriggers.Add(Physics.OverlapBox(trigger.transform.position, trigger.transform.position + new Vector3(4,0,4),Quaternion.identity, LayerMask.GetMask("Humans")));
        }*/

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
        Debug.Log(gameObject.name +"has Humans = " + humans + " : Ghosts = " + ghosts);
        
        //Check positions of everything and then see who all area approcximately close
        
        
        //physicsOverlap over random objects and then if many are next to it then teleport he camera to that location
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
            humans++;
        if (other.CompareTag("Ghost"))
            ghosts++;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided with" + other.collider.name);
        if (other.collider.CompareTag("Human"))
            humans++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))
            humans--;
        if (other.CompareTag("Ghost"))
            ghosts--;
    }
}
