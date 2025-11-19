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
    public Scrollbar scrollbar;
    public float buttonDistance;
    public float visibleButtons;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => { SetHelpPage(index); });
        }
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
        UpdateButtonSizesAndScale();
    }

    void SetHelpPage(int page)
    {
        this.currentPage = page;
    }

    void SetButtonPositionsInBar()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
        }
    }
    void UpdateButtonSizesAndScale()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            //position
            Vector3 startPosition = Vector3.zero - new Vector3(0, i * buttonDistance, 0);
            Vector3 actualPosition = startPosition + new Vector3(0, Mathf.Max(0, buttonDistance * ((buttons.Length - 1 - visibleButtons) * scrollbar.value)), 0);

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
                currentButtonTransfrom.Translate(0, -(1 - buttonHeight) * currentButtonTransfrom.rect.height * 0.5f, 0, Space.Self);
            }
            else
            {
                buttonHeight = Mathf.Max(0, 1 - Mathf.Abs(currentButtonTransfrom.localPosition.y + visibleButtons * buttonDistance) / buttonDistance);
                currentButtonTransfrom.Translate(0, +(1 - buttonHeight) * currentButtonTransfrom.rect.height * 0.5f, 0, Space.Self);
            }
            currentButtonTransfrom.localScale = new Vector3(currentButtonTransfrom.localScale.x, buttonHeight, currentButtonTransfrom.localScale.z);
        }
    }
}
