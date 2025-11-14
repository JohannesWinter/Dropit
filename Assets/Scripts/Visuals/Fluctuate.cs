using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluctuate : MonoBehaviour
{
    public GameObject[] fluctuating;
    float currenttime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currenttime < Time.time)
        {
            currenttime = Time.time + 0.1f;
            for (int i = 0; i < fluctuating.Length; i++)
            {
                if (UnityEngine.Random.Range(0, 10) == 0)
                {
                    StartCoroutine(Blink(fluctuating[i]));
                }
            }
        }
    }

    IEnumerator Blink(GameObject blinking)
    {
        blinking.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(5, 50) / 100f);
        blinking.GetComponent<MeshRenderer>().enabled = true;
        yield return null;
    }
}
