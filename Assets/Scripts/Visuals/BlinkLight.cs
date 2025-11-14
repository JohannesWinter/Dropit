using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    public GameObject[] blinkingLights;
    Color[] baseEmissionColor;
    float currenttime = 0;
    // Start is called before the first frame update
    void Start()
    {
        baseEmissionColor = new Color[blinkingLights.Length];
        for (int i = 0; i < blinkingLights.Length; i++)
        {
            baseEmissionColor[i] = blinkingLights[i].GetComponent<MeshRenderer>().material.GetColor("_EmissiveColor");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currenttime < Time.unscaledTime && Manager.m.inMainMenu == true)
        {
            currenttime = Time.unscaledTime + 0.1f;
            for (int i = 0; i < blinkingLights.Length; i++)
            {
                if (UnityEngine.Random.Range(0,(int)(2f / Time.unscaledDeltaTime)) == 0)
                {
                    StartCoroutine(Blink(blinkingLights[i], i));
                }
            }
        }
    }

    IEnumerator Blink(GameObject b, int position)
    {
        b.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", baseEmissionColor[position] * 10f);
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.3f, 5f));
        b.GetComponent<MeshRenderer>().material.SetColor("_EmissiveColor", baseEmissionColor[position]);
        yield return null;
    }
}
