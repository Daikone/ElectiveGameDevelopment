using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    //create list of pickup types
    private enum EType {Speed, Stun};
    [SerializeField] EType Type;

    private bool canStun = false;

    //when pickup touches ghost, and ghost does not have pickup yet, add pickup to ghost
    void OnTriggerEnter(Collider other)
    {
        if (tag == "Pickup" && other.tag == "Ghost")
        {
            PickupBehaviour ghostPickupBehavior = other.GetComponent<PickupBehaviour>();

            if (ghostPickupBehavior == null)
            {
                GhostBehaviour ghostBehaviour = other.GetComponent<GhostBehaviour>();
                ghostPickupBehavior = other.gameObject.AddComponent<PickupBehaviour>();
                ghostPickupBehavior.Type = Type;
                ghostBehaviour.hasPickup = true;
                Destroy(gameObject);
            }
        }
    }

    //if ghost can stun, on collision with ghost, activate stun
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (canStun && tag == "Ghost" && other.tag == "Ghost") {
            canStun = false;
            StartCoroutine(stunOtherGhost(other));
        }
    }

    //check pickup, and use that one
    public void usePickup()
    {
        switch (Type)
        {
            case EType.Speed:
            {
                StartCoroutine(increaseSpeed());
                break;
            }
            case EType.Stun:
            {
                canStun = true;
                break;
            }
        }
    }

    //store intitial speed, then increase speed for 5 seconds, then set back to intitial
    private IEnumerator increaseSpeed()
    {
        GhostBehaviour ghostBehaviour = GetComponent<GhostBehaviour>();
        int initialSpeed = ghostBehaviour.speed;

        ghostBehaviour.speed = 5;
        yield return new WaitForSeconds(5);
        ghostBehaviour.speed = initialSpeed;
        Destroy(this);
    }

    //get other ghost speed, set it to 0, after 5 seconds, set it back to intitial speed
    private IEnumerator stunOtherGhost(GameObject other)
    {
        GhostBehaviour otherGhostBehaviour = other.GetComponent<GhostBehaviour>();
        int initialSpeed = otherGhostBehaviour.speed;

        otherGhostBehaviour.speed = 0;
        yield return new WaitForSeconds(5);
        otherGhostBehaviour.speed = initialSpeed;
        Destroy(this);
    }
}
