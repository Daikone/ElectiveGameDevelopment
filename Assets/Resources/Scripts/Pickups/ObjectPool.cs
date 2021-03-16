using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] List<GameObject> Pickup;
    [SerializeField] float spawnTimer = 1f;
    [SerializeField] float spawnRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPickup());
    }

    //Spawn amount of pickups over time, in a chosen radius, within the walkable NavMesh area
    IEnumerator SpawnPickup()
    {
        while (true)
        {
            Vector3 position = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)) + transform.position;
            NavMeshHit hit;
            Vector3 result;
            if (NavMesh.SamplePosition(position, out hit, 1.0f, 1))
            {
                result = hit.position;
                Instantiate(Pickup[Random.Range(0, Pickup.Count)], result, Quaternion.identity, transform);
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
