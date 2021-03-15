using System.Collections;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public Transform[] monsters;
    private Vector3 overviewPosition;
    private Vector3 monsterOffset;

    private bool posChanged;
    private int randomNumber;
    private bool followMonster;

    private void Start()
    {
        overviewPosition = new Vector3(-12.6f, 20f, -52f);
        monsterOffset = new Vector3(0f, 5f,-5f);
    }

    private void Update()
    {
        //InvokeRepeating("FocusMonster", 0f, 5f);
        //StartCoroutine(FocusMonster());

        //custom control with Keycodes
        if (Input.GetKeyDown(KeyCode.O))
        {
            followMonster = false;
            transform.position = overviewPosition;
        }

        if (Input.GetKeyDown(KeyCode.M))
            followMonster = true;

        if (followMonster)
        {
            transform.position = monsters[0].position + monsterOffset;
            //transform.rotation = Quaternion.Euler(45, monsters[0].rotation.y, 0);
        }
            
    }

    void CameraChange()
    {
        randomNumber = Random.Range(0, monsters.Length + 1);

        if(randomNumber == monsters.Length)
            transform.position = overviewPosition;
        else
            transform.position = monsters[randomNumber].position + monsterOffset;
        
    }

    IEnumerator FocusMonster()
    {
        randomNumber = Random.Range(0, monsters.Length);
        transform.position = monsters[randomNumber].position + monsterOffset;

        yield return new WaitForSeconds(5f);
        transform.position = overviewPosition;
        yield return new WaitForSeconds(5f);
    }
}
