using System;
using UnityEngine;
using UnityEngine.AI;

public class BaseGhostAI : MonoBehaviour
{
    public GameManager gm;
    public NavMeshAgent agent;

    protected bool isStunned;
    protected float carryingSouls;
    protected float speed;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            carryingSouls++;
            Destroy(other.gameObject);
        }
    }
        
    
}

