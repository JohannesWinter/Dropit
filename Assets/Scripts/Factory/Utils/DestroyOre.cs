using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOre : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ore>() == true)
        {
            Destroy(collision.gameObject);
        }
    }
}
