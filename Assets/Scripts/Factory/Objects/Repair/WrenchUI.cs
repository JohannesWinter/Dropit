using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchUI : MonoBehaviour
{
    public RepairDropper parentScript;
    void Update()
    {
        if (parentScript == null)
        {
            Destroy(this.gameObject);
        }
    }
}
