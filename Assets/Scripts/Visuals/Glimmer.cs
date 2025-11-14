using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glimmer : MonoBehaviour
{

    public int darkness;
    int counter;
    public float speed;
    public RawImage target;
    bool goingDown = true;

    float currenttime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currenttime < Time.unscaledTime)
        {
            currenttime = Time.unscaledTime + 1 - speed;

            if (goingDown == true)
            {
                target.color = new Color(target.color.r - 2f/255, target.color.g - 2f/255, target.color.b - 2f/255);
                counter++;
                counter++;

                if (counter >= darkness)
                {
                    counter = 0;
                    goingDown = false;
                }
            }
            if (goingDown == false)
            {
                target.color = new Color(target.color.r + 2f / 255, target.color.g + 2f / 255, target.color.b + 2f / 255);

                if (target.color.r >= 1)
                {
                    goingDown = true;
                }
            }
        }
    }
}
