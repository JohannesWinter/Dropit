using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryConnections : MonoBehaviour
{

    public GameObject[] factoryConnections;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < factoryConnections.Length; i++)
        {
            factoryConnections[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < factoryConnections.Length; i++)
        {
            if (Manager.m.level >= i || Manager.m.inMainMenu)
            {
                factoryConnections[i].SetActive(true);
            }
            else
            {
                factoryConnections[i].SetActive(false);
            }
        }
    }
}
