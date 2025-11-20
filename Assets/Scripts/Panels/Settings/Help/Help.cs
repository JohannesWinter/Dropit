using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public GameObject overlay;
    public int currentPage = 0;
    public Button[] buttons;
    public GameObject[] pages;
    public RectTransform buttonBar;
    public Scrollbar scrollbar;
    public float buttonDistance;
    public float visibleButtons;
    float currentHeightValue;
    public float correctionSpeed;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => { SetHelpPage(index); });
        }
    }
    /*
    Pages
    #01 Introduction
    #02 Miners
    #03 Conveyors
    #04 Furnaces
    #05 Upgrading
    #06 EditMode
    #07 Selling
    #08 Repairing
    #09 Market
    #10 Missions
    #11 Factory Halls
    #12 Events
    #13 Attacks
    #14 The End
     */

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.settings_help)
        {
            overlay.SetActive(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (currentPage != i)
                {
                    if (pages.Length > i)
                    {
                        pages[i].SetActive(false);
                    }
                    buttons[i].gameObject.GetComponent<RawImage>().color = Color.white;
                }
                else
                {
                    if (pages.Length > i)
                    {
                        pages[i].SetActive(true);
                    }
                    buttons[i].gameObject.GetComponent<RawImage>().color = new Color(0.8f, 0.8f, 0.8f);
                    buttons[i].gameObject.transform.SetSiblingIndex(buttons[i].gameObject.transform.parent.childCount);
                }
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                scrollbar.value += Input.mouseScrollDelta.y * (-1f) * (1f / buttons.Length);
                if (scrollbar.value < 0) { scrollbar.value = 0; }
                else if (scrollbar.value > 1f) { scrollbar.value = 1f; }
            }
            UpdateHightValue();
            UpdateButtonSizesAndScale();
        }
        else
        {
            overlay.SetActive(false);
            currentHeightValue = scrollbar.value;
        }
    }

    void SetHelpPage(int page)
    {
        Manager.m.effectSpeaker.click();
        this.currentPage = page;
    }

    void UpdateHightValue()
    {
        float difference = scrollbar.value - currentHeightValue;
        if (Mathf.Abs(difference) < 0.0001f)
        {
            currentHeightValue = scrollbar.value;
            return;
        }
        currentHeightValue += difference * Time.unscaledDeltaTime * correctionSpeed;
    }
    void UpdateButtonSizesAndScale()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //position
            Vector3 startPosition = Vector3.zero - new Vector3(0, i * buttonDistance, 0);
            Vector3 actualPosition = startPosition + new Vector3(0, Mathf.Max(0, buttonDistance * ((buttons.Length - 1 - visibleButtons) * currentHeightValue)), 0);

            Button currentButton = buttons[i];
            RectTransform currentButtonTransfrom = currentButton.GetComponent<RectTransform>();
            currentButtonTransfrom.localPosition = actualPosition;

            //scale
            float buttonHeight;
            if (currentButtonTransfrom.localPosition.y <= 0 && currentButtonTransfrom.localPosition.y >= -buttonDistance * visibleButtons)
            {
                buttonHeight = 1;
            }
            else if (currentButtonTransfrom.localPosition.y > 0)
            {
                buttonHeight = Mathf.Max(0, 1 - currentButtonTransfrom.localPosition.y / buttonDistance);
                currentButtonTransfrom.Translate(0, -(1 - buttonHeight) * currentButtonTransfrom.rect.height * 0.5f * currentButtonTransfrom.localScale.y, 0, Space.Self);
            }
            else
            {
                buttonHeight = Mathf.Max(0, 1 - Mathf.Abs(currentButtonTransfrom.localPosition.y + visibleButtons * buttonDistance) / buttonDistance);
                currentButtonTransfrom.Translate(0, +(1 - buttonHeight) * currentButtonTransfrom.rect.height * 0.5f * currentButtonTransfrom.localScale.y, 0, Space.Self);
            }
            currentButtonTransfrom.localScale = new Vector3(currentButtonTransfrom.localScale.x, buttonHeight, currentButtonTransfrom.localScale.z);
            if (buttonHeight == 0)
            {
                if (currentButton.gameObject.activeSelf)
                {
                    currentButton.gameObject.SetActive(false);
                }
            }
            else
            {
                if (currentButton.gameObject.activeSelf == false)
                {
                    currentButton.gameObject.SetActive(true);
                    Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.beep, 1 / ((currentHeightValue + 1.5f) * 0.5f));
                }
            }
        }
    }
}
