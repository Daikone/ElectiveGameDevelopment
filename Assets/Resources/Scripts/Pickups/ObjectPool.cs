﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Spawn amount of pickups over time, in a chosen radius
    IEnumerator SpawnPickup()
    {
        while(true)
        {
            Vector3 position = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)) + transform.position;
            Instantiate(Pickup[Random.Range(0, Pickup.Count)], position, Quaternion.identity, transform);

            yield return new WaitForSeconds(spawnTimer);
        }
    }
}