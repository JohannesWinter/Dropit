using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RemoveScript : MonoBehaviour
{
    public string componentTypeName;
    public Type toRemoveType;
    // Start is called before the first frame update
    void Start()
    {
        toRemoveType = Type.GetType(componentTypeName);
        if (toRemoveType != null)
        {
            Component[] foundComponents = this.gameObject.GetComponentsInChildren(toRemoveType);
            for (int i = 0; i < foundComponents.Length; i++)
            {
                Destroy(foundComponents[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
