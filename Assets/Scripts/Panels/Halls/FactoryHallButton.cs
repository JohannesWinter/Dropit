using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryHallButton : MonoBehaviour
{
    public int hallNumber;

    public GameObject _lock;
    public GameObject numberTxt;
    public GameObject frame;
    public bool lastVisited;

    // Start is called before the first frame update
    void Start()
    {
        numberTxt.GetComponent<TextMeshProUGUI>().text = "" + hallNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.upgradeRessources[hallNumber-1] == true)
        {
            _lock.SetActive(false);
        }
        else if (Manager.m.level >= hallNumber)
        {
            _lock.SetActive(true);
            _lock.GetComponent<RawImage>().color = new Color(0, 220f / 255, 0, 1);
        }
        else
        {
            _lock.SetActive(true);
            _lock.GetComponent<RawImage>().color = new Color(0, 150f / 255, 1);
        }

        if (Manager.m.lastDropperCamera == Manager.m.factoryCameras[hallNumber-1])
        {
            lastVisited = true;
        }
        else
        {
            lastVisited = false;
        }
    }
}
