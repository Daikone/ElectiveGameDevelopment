using UnityEngine;

[System.Serializable]
public class Room
{
    public string name;
    public Transform checkpoint;

    public Vector3 GetPos()
    {
        return checkpoint.position;
    }
}
