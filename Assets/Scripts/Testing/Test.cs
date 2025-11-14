using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    private void Start()
    {
        RepairDropper[] f = gameObject.GetComponentsInChildren<RepairDropper>();
        for (int i = 0; i < f.Length; i++)
        {
            Destroy(f[i]);
        }
    }
}
