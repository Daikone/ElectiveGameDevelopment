using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridColliderList : MonoBehaviour
{
    public List<Collider[]> boxAreas;
    public List<List<Collider>> grid;

    private void Start()
    {
        boxAreas = new List<Collider[]>();
        grid = new List<List<Collider>>();
    }
}
