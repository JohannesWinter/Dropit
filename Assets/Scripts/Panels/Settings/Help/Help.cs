using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public GameObject overlay;
    public int currentPage = 0;
    public Button[] buttons;
    public GameObject[] pages;
    public RectTransform buttonBar;
    Vector3 barStart;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => { SetHelpPage(i); });
        }
        barStart = buttonBar.localPosition + new Vector3(buttonBar.localScale.x * 0.5f, 0, 0);
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
        SetButtonPositionsInBar();
    }

    void SetHelpPage(int page)
    {
        this.currentPage = page;
    }

    void SetButtonPositionsInBar()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.GetComponent<RectTransform>().localPosition = barStart - new Vector3(i * 150f, 0, 0);
        }
    }
}
