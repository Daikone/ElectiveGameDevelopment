using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawn : MonoBehaviour
{
    public GameObject Human;
    // Start is called before the first frame update
    void Start()
    {
        
        GameManager.OnSpawnHumans += spawnMoreHumans;
    }

    private void spawnMoreHumans()
    {
        Debug.Log("Spawn Humans");
        for (var i = 0; i < 3; i++)
        {
            Instantiate(Human, gameObject.transform.position, Quaternion.identity);
        }
        
    }
}
