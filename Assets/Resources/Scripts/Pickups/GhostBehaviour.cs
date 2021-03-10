using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class GhostBehaviour : BaseGhostAI
{
    public bool hasPickup = false;
    public int speed = 1;

    
    //   !!!!!!! Matti Stuff !!!!!!!!!
    public string YourName;
    private SphereCollider _trigger;
    private CapsuleCollider _collider;
    private GameObject Cauldron;
    private LayerMask humanLayerCheck;
    private  List<GameObject> humansInSight = new List<GameObject>();
    private GameObject ClosestHuman;
    
    
    private void Awake()
    {
        // Matti Stuff
        Cauldron = GameObject.Find("Cauldron");
        humanLayerCheck = LayerMask.GetMask("Humans");
        carryingSouls = 0;
        isStunned = false;
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        //when ghost has pickup, activate it
        if (hasPickup) {
            hasPickup = false;
            GetComponent<PickupBehaviour>().usePickup();
        }
        
        
        
        
        // Matti Stuff
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
