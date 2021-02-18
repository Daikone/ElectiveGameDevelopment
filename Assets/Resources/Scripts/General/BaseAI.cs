using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAI : MonoBehaviour
{
   public void MoveTo(Vector3 direction, float speed)
    {
        direction.Normalize();
        transform.position += direction * (speed * Time.deltaTime);
    }

   
}
