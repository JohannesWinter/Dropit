using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataOutput : MonoBehaviour
{
    public TextMeshProUGUI version;
    public TextMeshProUGUI fps;

    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        version.text = "version:" + Manager.m.officialVersion + "" + Manager.m.version;
        fps.text = "fps:" + 1 / Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (counter > 1 / Time.deltaTime)
        {
            version.text = "version:" + Manager.m.officialVersion + "" + Manager.m.version;
            fps.text = "fps:" + Mathf.Round(1 / Time.deltaTime);
            counter = 0;
        }
        counter++;
    }
}
