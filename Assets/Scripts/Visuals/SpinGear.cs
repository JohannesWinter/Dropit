using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinGear : MonoBehaviour
{
    private Transform thisTransform;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        this.thisTransform = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, Time.unscaledDeltaTime * speed);
    }
}   
