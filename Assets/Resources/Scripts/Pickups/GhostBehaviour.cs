using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public bool hasPickup = false;
    public int speed = 1;

    // Update is called once per frame
    void Update()
    {
        //when ghost has pickup, activate it
        if (hasPickup) {
            hasPickup = false;
            GetComponent<PickupBehaviour>().usePickup();
        }
    }
}
