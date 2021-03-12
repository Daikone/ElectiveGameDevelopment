using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class GhostBehaviour : BaseGhostAI
{
    public bool hasPickup = false;
    public int speed = 1;
    
    
    //   !!!!!!! Matti Stuff !!!!!!!!!
    public string YourName;
    private GameObject Cauldron;

    public float getSouls()
    {
        return carryingSouls;
    }
    
    
    private void Awake()
    {
        // Matti Stuff
        Cauldron = GameObject.Find("Cauldron");
        carryingSouls = 0;
        isStunned = false;
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        //when ghost has pickup, activate it
        if (hasPickup)
        {
            hasPickup = false;
            GetComponent<PickupBehaviour>().usePickup();
        }

    }


    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Human"))
        {
            Destroy(obj);
            carryingSouls++;
        }
        
    }
}
