using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAI : MonoBehaviour
{
    private List<Collider[]> gridCheckX;
    private List<List<Collider[]>> gridCheckY;
    private float colliderAmmount;
    private Collider[] currentMaxCollider;
    private Vector3 offset;

    private List<float> heatMap;

    private void Start()
    {
        heatMap = new List<float>();
        offset = new Vector3(-28, 0, -11);
        
        gridCheckX = new List<Collider[]>();
        gridCheckY = new List<List<Collider[]>>();

        for (int y = 4; y <= 28; y += 8)
        {
            for (int x = 4; x <= 28; x += 8)
            {
                //Debug.Log("BOX1");
                gridCheckX.Add(Physics.OverlapBox(new Vector3(x, 1, y) + offset, new Vector3(4, 1, 4), Quaternion.identity, LayerMask.GetMask("Humans", "Walls", "Ghosts")));
                //0,1,-15.25
                
                //-24,1,-39.25
            }
            //Debug.Log("BOXout");
            gridCheckY.Add(gridCheckX);
            gridCheckX.Clear();
        }
        
        
    }
    
    void Update()
    {
        foreach (var boxes in gridCheckY)
        {
            //Debug.Log("List in list");
            foreach (var coll in boxes)
            {
                //Debug.Log("Colliders in List");
                heatMap.Add(coll.Length);
            }
        }
        foreach (var value in heatMap)
        {
            int i = 1;
            Debug.Log(i + " has " + value);
            i++;
        }
        heatMap.Clear();
        Debug.Log("TopLeft1: " + heatMap.Count);
    }
}
    
