using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectGlow : MonoBehaviour
{
    public int length;
    public RawImage[] glowList;
    public float colorCounter;
    public float currenttime;
    // Start is called before the first frame update
    void Start()
    {
        glowList = new RawImage[length];
        colorCounter = 0;
        currenttime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        colorCounter += 50;
        if (currenttime < Time.time)
        {
            currenttime = Time.time + 0.05f;
            if (colorCounter <= 500)
            {
                for (int i = 0; i < glowList.Length; i++)
                {
                    if (glowList[i] != null)
                    {
                        glowList[i].color = new Color(colorCounter / 1000 + 0.5f, colorCounter / 1000 + 0.5f, colorCounter / 1000 + 0.5f, glowList[i].color.a);
                    }
                }
            }
            else if (colorCounter <= 1000)
            {
                for (int i = 0; i < glowList.Length; i++)
                {
                    if (glowList[i] != null)
                    {
                        glowList[i].color = new Color((1000 - colorCounter) / 1000 + 0.5f, (1000 - colorCounter) / 1000 + 0.5f, (1000 - colorCounter) / 1000 + 0.5f, glowList[i].color.a);
                    }
                }
            }
            else if (colorCounter > 1000)
            {
                colorCounter = -50;
            }
        }
    }

    public void add(GameObject g, int pos)
    {
        glowList[pos] = g.GetComponent<RawImage>();
    }
    public void remove(int pos)
    {
        if (glowList[pos] != null)
        {
            glowList[pos].color = new Color(1, 1, 1, glowList[pos].color.a);
            glowList[pos] = null;
        }
    }
}
