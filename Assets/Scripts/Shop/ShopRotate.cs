 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRotate : MonoBehaviour
{
    float cuncurrenttime = 0;
    public double Speed;
    public bool Clockwise = true;
    public bool x;
    public bool y;
    public bool z;


    private void Start()
    {
        if((x && y && z) == false)
        {
            y = true;
        }
    }
    void Update()
    {
        if (cuncurrenttime < Time.unscaledTime)
        {
            cuncurrenttime = Time.unscaledTime + (float)Speed;
            if (Clockwise == true)
            {
                if (x == true)
                {
                    transform.Rotate(0.25f, 0, 0);
                }
                if (y == true)
                {
                    transform.Rotate(0, 0.25f, 0);
                }
                if (z == true)
                {
                    transform.Rotate(0, 0, 0.25f);
                }
            }
            else
            {
                if (x == true)
                {
                    transform.Rotate(-0.25f, 0, 0);
                }
                if (y == true)
                {
                    transform.Rotate(0, -0.25f, 0);
                }
                if (z == true)
                {
                    transform.Rotate(0, 0, -0.25f);
                }
            }
        }
    }
}
