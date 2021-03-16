using System.Collections;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public Transform[] monsters;
    public float offsetDistance = 1f;
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
            transform.rotation = Quaternion.Euler(45, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.M))
            followMonster = true;

        if (followMonster)
        {
            monsterOffset = -monsters[0].forward.normalized * offsetDistance;
            monsterOffset += new Vector3(0f, offsetDistance, 0f);
            transform.position = monsters[0].position + monsterOffset;
            //Debug.Log(monsters[0].rotation.eulerAngles);
            //transform.rotation = Quaternion.Euler(45, monsters[0].rotation.eulerAngles.y, 0);
            transform.LookAt(monsters[0]);
        }
            
    }

    void CameraChange(){
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
