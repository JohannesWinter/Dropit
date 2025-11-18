using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public GameObject overlay;
    public Button b1;
    public Button b2;
    public Button b3;
    public int currentPage = 0;
    public GameObject[] pages;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.settings_help)
        {
            overlay.SetActive(true);
            for (int i = 0; i < pages.Length; i++)
            {
                if (currentPage != i)
                {
                    pages[i].SetActive(false);
                }
                else
                {
                    pages[i].SetActive(true);
                }
            }
        }
        else
        {
            overlay.SetActive(false);
        }

    }


    void B1()
    {
        currentPage = 0;
    }
    void B2()
    {
        currentPage = 1;
    }
    void B3()
    {
        currentPage = 2;
    }
}
