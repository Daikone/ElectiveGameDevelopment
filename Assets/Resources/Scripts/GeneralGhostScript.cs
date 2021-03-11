using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CapsuleCollider))]

public class GeneralGhostScript : BaseGhostAI
{
    public string YourName;
    
    
    private SphereCollider _trigger;
    private CapsuleCollider _collider;
    private GameObject Cauldron;
    
    private LayerMask humanLayerCheck;
    private  List<GameObject> humansInSight = new List<GameObject>();
    private GameObject ClosestHuman;
    private void Awake()
    {
        
        Cauldron = GameObject.Find("Cauldron");
        
        humanLayerCheck = LayerMask.GetMask("Humans");
        
        
        carryingSouls = 0;
        isStunned = false;
        
    }

    private void Update()
    {
        humansInSight = CheckCloseObjectsInSight(gameObject, 5f, humanLayerCheck);
        if (humansInSight.Count > 0)
        {
            ClosestHuman = ClosestObjectInList(gameObject, humansInSight);
        }
        
        
        
        
        if (Vector3.Distance(gameObject.transform.position, Cauldron.transform.position) <= 5 && carryingSouls > 0)
        {
            Debug.Log("Kessel");
            carryingSouls = 0;
        }
        if (ClosestHuman != null && Vector3.Distance( gameObject.transform.position, ClosestHuman.transform.position) <= 1)
        {
            Destroy(ClosestHuman);
            carryingSouls++;
        }
        
    }

   
}
