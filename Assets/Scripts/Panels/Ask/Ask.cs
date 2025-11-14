using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Ask : MonoBehaviour
{
    // Start is called before the first frame update
    public Button yes;
    public Button no;
    public GameObject backround;
    public GameObject cursorBlocker;
    public TextMeshProUGUI text;
    public int antwort;
    Color cursorBlockColor;
    void Start()
    {
        yes.onClick.AddListener(Yes);
        no.onClick.AddListener(No);

        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        backround.SetActive(false);
        cursorBlocker.SetActive(false);
        text.gameObject.SetActive(false);
        cursorBlockColor = new Color(cursorBlocker.GetComponent<RawImage>().color.r, cursorBlocker.GetComponent<RawImage>().color.g, cursorBlocker.GetComponent<RawImage>().color.b, cursorBlocker.GetComponent<RawImage>().color.a);
    }

    private void Update()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        if ((GameInputManager.GetKeyDown(Manager.m.ActionKey("AskNo")) || GameInputManager.GetKeyDown(Manager.m.ActionKey("Settings"))) && backround.activeSelf)
        {
            No();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("AskYes")) && backround.activeSelf)
        {
            Yes();
        }
    }

    void Yes()
    {
        antwort = 2;

        cursorBlocker.GetComponent<RawImage>().color = cursorBlockColor;
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        backround.SetActive(false);
        cursorBlocker.SetActive(false);
        text.gameObject.SetActive(false);
        Manager.m.effectSpeaker.accept();
    }
    void No()
    {
        antwort = 1;

        cursorBlocker.GetComponent<RawImage>().color = cursorBlockColor;
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        backround.SetActive(false);
        cursorBlocker.SetActive(false);
        text.gameObject.SetActive(false);
        Manager.m.effectSpeaker.error();
    }
    public void Cancel()
    {
        antwort = 1;

        cursorBlocker.GetComponent<RawImage>().color = cursorBlockColor;
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        backround.SetActive(false);
        cursorBlocker.SetActive(false);
        text.gameObject.SetActive(false);
    }
    public void Asking(String question)
    {
        cursorBlocker.GetComponent<RawImage>().color = cursorBlockColor;
        text.text = question;
        antwort = 0;
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        backround.SetActive(true);
        text.gameObject.SetActive(true);
    }
    public void Asking(String question, bool shownCursorBlocker)
    {
        cursorBlocker.GetComponent<RawImage>().color = cursorBlockColor;
        text.text = question;
        antwort = 0;
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        backround.SetActive(true);
        cursorBlocker.SetActive(true);
        text.gameObject.SetActive(true);
    }
    public void Asking(String question, bool shownCursorBlocker, Color blockerColor)
    {
        text.text = question;
        antwort = 0;
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        backround.SetActive(true);
        cursorBlocker.SetActive(true);
        cursorBlocker.GetComponent<RawImage>().color = blockerColor;
        text.gameObject.SetActive(true);
    }
}
