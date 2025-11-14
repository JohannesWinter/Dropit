using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    public float speed;
    public float splitspeed;
    public GameObject splitter;
    public GameObject splitterHitbox;
    string toSide;
    // Start is called before the first frame update
    void Start()
    {
        speed = (float)GetComponentInParent<RepairDropper>().conveyorBeltSpeed;
        splitspeed = 10;

        toSide = "right";
    }
    // Update is called once per frame
    void Update()
    {
        if (toSide == "right")
        {
            splitter.transform.Translate(splitspeed * Time.deltaTime, 0, 0);
            splitterHitbox.transform.Translate(splitspeed * Time.deltaTime, 0, 0);
            if (splitterHitbox.transform.localPosition.x > 1)
            {
                toSide = "left";
            }
        }
        if (toSide == "left")
        {
            splitter.transform.Translate(-splitspeed * Time.deltaTime, 0, 0);
            splitterHitbox.transform.Translate(-splitspeed * Time.deltaTime, 0, 0);
            if (splitterHitbox.transform.localPosition.x < -1)
            {
                toSide ="right";
            }
        }
    }
}
