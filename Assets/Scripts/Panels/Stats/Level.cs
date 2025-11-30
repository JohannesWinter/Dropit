using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject overlayAround;
    public double[] expPerLevel;
    public TextMeshProUGUI level;
    public GameObject expBar;
    public GameObject backround;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        overlayAround.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);


        level.text = "" + Manager.m.level;
        for (int i = 1; i < expPerLevel.Length; i++)
        {
            if (Manager.m.level == i)
            {
                if (Manager.m.exp >= expPerLevel[i])
                {
                    Manager.m.level += 1;
                    Manager.m.exp -= expPerLevel[i];
                    StartCoroutine(Manager.m.tutorial.StartTutorial(i));
                }
                expBar.transform.localScale = new Vector3((float)(Manager.m.exp / expPerLevel[i]), expBar.transform.localScale.y, expBar.transform.localScale.z);
                expBar.transform.localPosition = new Vector3((1-(float)(Manager.m.exp / expPerLevel[i])) * expBar.GetComponent<RectTransform>().rect.width * -0.5f, 0, 0);
            }
        }

        if (Manager.m.hideFactoryUI == true)
        {
            expBar.SetActive(false);
            level.gameObject.SetActive(false);
            backround.SetActive(false);
        }
        else
        {
            expBar.SetActive(true);
            level.gameObject.SetActive(true);
            backround.SetActive(true);
        }
    }
}
