using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public GameObject[] triggers;
    public List<Collider[]> listofTriggers;

    private void Start()
    {
        /*listofTriggers = new List<Collider[]>();
        
        foreach (var trigger in triggers)
        {
            listofTriggers.Add(Physics.OverlapBox(trigger.transform.position, trigger.transform.position + new Vector3(4,0,4),Quaternion.identity, LayerMask.GetMask("Humans")));
        }*/
        
        
        
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
    }
}
