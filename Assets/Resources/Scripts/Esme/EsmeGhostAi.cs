using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EsmeGhostAi : MonoBehaviour
{
    private enum _state {
        searching,
        attackHuman,
        attackGhost,
        depositSouls
    }

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start ()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        // search();
    }

    // Update is called once per frame
    void Update ()
    {
        search();
    }

    void search ()
    {
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Ghosts", "Humans"));
        List<Collider> collidersInVisibleRadius = new List<Collider>();


        foreach (Collider collider in collidersInRadius)
        {
            if (!Physics.Linecast(transform.position, collider.transform.position, LayerMask.GetMask("Walls"))) 
            {
                collidersInVisibleRadius.Add(collider);
                Debug.DrawLine(transform.position, collider.transform.position, Color.green);
            }
        }

        collidersInVisibleRadius.OrderByDescending(collider => Vector3.Distance(collider.transform.position, transform.position));
        if (collidersInVisibleRadius.Count > 0)
        {
            navMeshAgent.destination = collidersInVisibleRadius[0].transform.position;
        }
    }
}
