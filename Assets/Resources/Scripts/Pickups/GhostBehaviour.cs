using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class GhostBehaviour : BaseGhostAI
{
    public bool hasPickup = false;
    
    
    //   !!!!!!! Matti Stuff !!!!!!!!!
    public string YourName;
    private GameObject Cauldron;
    public int speed;

    // !!!!!!!Alex Audio Solution!!!!!! \\
    public AudioClip KillSound;

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
        speed = GetSpeed();

        //Audio For killing humans (Alex)
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = KillSound;
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

        agent.speed = speed;

    }


    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Human"))
        {
            Destroy(obj);
            carryingSouls++;

            //audio
            GetComponent<AudioSource>().Play();
        }
        
    }
}
