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

    private Collider[][] colliderlistList;

    private List<float> heatMap;

    private void Start()
    {
        heatMap = new List<float>();
        offset = new Vector3(0f, 1f, -15.25f);
        
        gridCheckX = new List<Collider[]>();
        gridCheckY = new List<List<Collider[]>>();
        
        for (int y = 0; y >= -24; y -= 8)
        {
            for (int x = 0; x >= -24; x -= 8)
            {
                //Debug.Log("BOX1");
                gridCheckX.Add(Physics.OverlapBox(new Vector3(x, 0, y) + offset, new Vector3(4, 1, 4), Quaternion.identity, LayerMask.GetMask("Humans", "Ghosts")));
                //0,1,-15.25
                
                //-24,1,-39.25
            }
        }

        for (int i = 0; i < 4; i++)
        {
            List<Collider[]> tempList = new List<Collider[]>();
            tempList = gridCheckX.GetRange(i, i + 3);
            gridCheckY.Add(tempList);
        }
    }
    
    void Update()
    {
        /*foreach (var boxes in gridCheckY)
        {
            //Debug.Log("List in list");
            /*foreach (var coll in boxes)
            {
                //Debug.Log("Colliders in List");
                heatMap.Add(coll.Length);
                Debug.Log(coll. + " has " + coll.Length);
            }#1#
        }*/
        
        /*for (int y = 0; y >= -24; y -= 8)
        {
            for (int x = 0; x >= -24; x -= 8)
            {
                gridCheckX.Add(Physics.OverlapBox(new Vector3(x, 0, y) + offset, new Vector3(4, 1, 4), Quaternion.identity, LayerMask.GetMask("Humans", "Ghosts")));
            }
        }*/
        
        //Debug.Log("Listboxes: " + gridCheckX.Count);
        //Debug.Log("List " + gridCheckY.Count);
        
       
        
        /*for (int j = 0; j < gridCheckX.Count; j++)
        {
            Debug.Log((j+1) + " has " + gridCheckX[j].Length);
        }*/
    }
}
    
