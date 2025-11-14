using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{

    public int dropNumber;
    public float[] valueMultipliers;
    public int currentUpgradeValue;
    public Color currentUpgradeColorEnabled;
    public Color currentUpgradeColorDisabled;
    public Color currentMarketCrashColor;
    public Color currentInvertedMarketColor;

    public GameObject MultiplierTxt;

    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    public GameObject bar4;
    public GameObject bar5;
    public GameObject bar6;
    public GameObject bar7;
    public GameObject bar8;
    public GameObject bar9;
    public GameObject bar10;
    public GameObject bar11;
    public GameObject bar12;
    public GameObject bar13;
    public GameObject bar14;
    public GameObject bar15;

    public GameObject oreUpgradeGlow;
    public Texture upgradeGlow1;
    public Texture upgradeGlow2;
    public Texture upgradeGlow3;
    public Texture locked;
    public Texture notLocked;
    public RawImage dropImage;

    public GameObject selectUpgrade0;
    public GameObject selectUpgrade1;
    public GameObject selectUpgrade2;
    public GameObject selectUpgrade3;

    public GameObject upgradeRepeator0;
    public GameObject upgradeRepeator1;
    public GameObject upgradeRepeator2;
    public GameObject upgradeRepeator3;

    public TextMeshProUGUI upgradeText0;
    public TextMeshProUGUI upgradeText1;
    public TextMeshProUGUI upgradeText2;
    public TextMeshProUGUI upgradeText3;

    public Texture marketDisabled;
    public Texture marketEnabled;

    public GameObject folder1;
    public GameObject folder2;
    public GameObject folder3;
    public GameObject folder4;

    public bool showAllBars = false;
    public GameObject bars;
    public GameObject showall;
    public GameObject showallBar0;
    public GameObject showallBar1;
    public GameObject showallBar2;
    public GameObject showallBar3;
    public GameObject showallButton;

    public GameObject exit;


    // Start is called before the first frame update
    void Start()
    {
        exit.GetComponent<Button>().onClick.AddListener(Exit);
        selectUpgrade0.GetComponent<Button>().onClick.AddListener(SelectUpgrade0);
        selectUpgrade1.GetComponent<Button>().onClick.AddListener(SelectUpgrade1);
        selectUpgrade2.GetComponent<Button>().onClick.AddListener(SelectUpgrade2);
        selectUpgrade3.GetComponent<Button>().onClick.AddListener(SelectUpgrade3);
        showallButton.GetComponent<Button>().onClick.AddListener(SelectShowAll);

        currentMarketCrashColor = new Color(0.4f, 0, 0, 1);
        currentInvertedMarketColor = new Color(1, 0, 0, 1);

        dropImage.texture = Manager.m.oreImages[dropNumber - 1].texture;
    }

    // Update is called once per frame
    void Update()
    {
        MultiplierTxt.GetComponent<TextMeshProUGUI>().text = "" + Mathf.Round(valueMultipliers[currentUpgradeValue] * 100) + "%";

        if (currentUpgradeValue == 0)
        {
            currentUpgradeColorEnabled = new Color(0, 1f, 0);
            currentUpgradeColorDisabled = new Color(0.2f, 0.65f, 0.2f, 1);
            oreUpgradeGlow.SetActive(false);
            MultiplierTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 1f, 0);
        }
        else if (currentUpgradeValue == 1)
        {
            currentUpgradeColorEnabled = new Color(0, 1f, 0);
            //currentUpgradeColorDisabled = new Color(0.2f, 0.65f, 0.2f, 1);
            oreUpgradeGlow.SetActive(true);
            oreUpgradeGlow.GetComponent<RawImage>().texture = upgradeGlow1;
            MultiplierTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 40/255f, 0);
        }
        else if (currentUpgradeValue == 2)
        {
            currentUpgradeColorEnabled = new Color(0, 1, 0);
            //currentUpgradeColorDisabled = new Color(0, 0, 0.5f, 1);
            oreUpgradeGlow.SetActive(true);
            oreUpgradeGlow.GetComponent<RawImage>().texture = upgradeGlow2;
            MultiplierTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0f, 160/255f);
        }
        else if (currentUpgradeValue == 3)
        {
            currentUpgradeColorEnabled = new Color(0, 1, 0);
            //currentUpgradeColorDisabled = new Color(0.65f, 0, 0.52f, 1);
            oreUpgradeGlow.SetActive(true);
            oreUpgradeGlow.GetComponent<RawImage>().texture = upgradeGlow3;
            MultiplierTxt.GetComponent<TextMeshProUGUI>().color = new Color(230/255f, 0f, 190/255f);
        }

        if (Manager.m.qTEInvertedMarket)
        {
            MultiplierTxt.GetComponent<TextMeshProUGUI>().color = currentInvertedMarketColor;
        }

        showallBar0.transform.localScale = new Vector3((valueMultipliers[0] - 0.5f) / 1.5f, showallBar0.transform.localScale.y, showallBar0.transform.localScale.z);
        showallBar0.transform.localPosition = new Vector3(-100 + ((valueMultipliers[0] - 0.5f) / 1.5f) * 200 * 0.5f, showallBar0.transform.localPosition.y, showallBar0.transform.localPosition.z);

        if (Manager.m.level >= 3)
        {
            upgradeRepeator1.GetComponent<RawImage>().texture = notLocked;
            upgradeText1.text = "1";
            showallBar1.transform.localScale = new Vector3((valueMultipliers[1] - 0.5f) / 1.5f, showallBar1.transform.localScale.y, showallBar1.transform.localScale.z);
            showallBar1.transform.localPosition = new Vector3(-100 + ((valueMultipliers[1] - 0.5f) / 1.5f) * 200 * 0.5f, showallBar1.transform.localPosition.y, showallBar1.transform.localPosition.z);
            showallBar1.SetActive(true);
        }
        else
        {
            upgradeRepeator1.GetComponent<RawImage>().texture = locked;
            upgradeText1.text = "";
            showallBar1.SetActive(false);
        }
        if (Manager.m.level >= 6)
        {
            upgradeRepeator2.GetComponent<RawImage>().texture = notLocked;
            upgradeText2.text = "2";
            showallBar2.transform.localScale = new Vector3((valueMultipliers[2] - 0.5f) / 1.5f, showallBar2.transform.localScale.y, showallBar2.transform.localScale.z);
            showallBar2.transform.localPosition = new Vector3(-100 + ((valueMultipliers[2] - 0.5f) / 1.5f) * 200 * 0.5f, showallBar2.transform.localPosition.y, showallBar2.transform.localPosition.z);
            showallBar2.SetActive(true);
        }
        else
        {
            upgradeRepeator2.GetComponent<RawImage>().texture = locked;
            upgradeText2.text = "";
            showallBar2.SetActive(false);
        }
        if (Manager.m.level >= 9)
        {
            upgradeRepeator3.GetComponent<RawImage>().texture = notLocked;
            upgradeText3.text = "3";
            showallBar3.transform.localScale = new Vector3((valueMultipliers[3] - 0.5f) / 1.5f, showallBar3.transform.localScale.y, showallBar3.transform.localScale.z);
            showallBar3.transform.localPosition = new Vector3(-100 + ((valueMultipliers[3] - 0.5f) / 1.5f) * 200 * 0.5f, showallBar3.transform.localPosition.y, showallBar3.transform.localPosition.z);
            showallBar3.SetActive(true);
        }
        else
        {
            upgradeRepeator3.GetComponent<RawImage>().texture = locked;
            upgradeText3.text = "";
            showallBar3.SetActive(false);
        }

        if (showAllBars == true)
        {
            showall.SetActive(true);
            bars.SetActive(false);
        }
        else
        {
            showall.SetActive(false);
            bars.SetActive(true);

            if (valueMultipliers[currentUpgradeValue] >= 0.59)
            {
                bar1.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                if (Manager.m.qTEInvertedMarket)
                {
                    bar1.GetComponent<RawImage>().color = currentInvertedMarketColor;
                }
            }
            else
            {
                bar1.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
            }
            if (valueMultipliers[currentUpgradeValue] >= 0.69)
            {
                bar2.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                if (Manager.m.qTEInvertedMarket)
                {
                    bar2.GetComponent<RawImage>().color = currentInvertedMarketColor;
                }
            }
            else
            {
                bar2.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
            }
            if (valueMultipliers[currentUpgradeValue] >= 0.79)
            {
                bar3.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                if (Manager.m.qTEInvertedMarket)
                {
                    bar3.GetComponent<RawImage>().color = currentInvertedMarketColor;
                }
            }
            else
            {
                bar3.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
            }
            if (valueMultipliers[currentUpgradeValue] >= 0.89)
            {
                bar4.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                if (Manager.m.qTEInvertedMarket)
                {
                    bar4.GetComponent<RawImage>().color = currentInvertedMarketColor;
                }
            }
            else
            {
                bar4.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
            }
            if (valueMultipliers[currentUpgradeValue] >= 0.99)
            {
                bar5.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                if (Manager.m.qTEInvertedMarket)
                {
                    bar5.GetComponent<RawImage>().color = currentInvertedMarketColor;
                }
            }
            else
            {
                bar5.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
            }
            if (Manager.m.qTEMarketCrash == 0)
            {
                if (valueMultipliers[currentUpgradeValue] >= 1.09)
                {
                    bar6.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar6.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar6.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.19)
                {
                    bar7.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar7.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar7.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.29)
                {
                    bar8.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar8.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar8.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.39)
                {
                    bar9.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar9.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar9.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.49)
                {
                    bar10.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar10.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar10.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.59)
                {
                    bar11.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar11.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar11.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.69)
                {
                    bar12.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar12.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar12.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.79)
                {
                    bar13.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar13.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar13.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.89)
                {
                    bar14.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar14.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar14.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
                if (valueMultipliers[currentUpgradeValue] >= 1.99)
                {
                    bar15.GetComponent<RawImage>().color = currentUpgradeColorEnabled;
                    if (Manager.m.qTEInvertedMarket)
                    {
                        bar15.GetComponent<RawImage>().color = currentInvertedMarketColor;
                    }
                }
                else
                {
                    bar15.GetComponent<RawImage>().color = currentUpgradeColorDisabled;
                }
            }
            else
            {
                bar6.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar7.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar8.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar9.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar10.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar11.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar12.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar13.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar14.GetComponent<RawImage>().color = currentMarketCrashColor;
                bar15.GetComponent<RawImage>().color = currentMarketCrashColor;
            }
        }
    }
    void SelectUpgrade0()
    {
        if (dropNumber == 1)
        {
            if (currentUpgradeValue != 0)
            {
                currentUpgradeValue = 0;
                selectUpgrade0.transform.SetParent(folder1.transform);
                selectUpgrade1.transform.SetParent(folder2.transform);
                selectUpgrade2.transform.SetParent(folder3.transform);
                selectUpgrade3.transform.SetParent(folder4.transform);

                Manager.m.effectSpeaker.accept();
            }
            else
            {
                Manager.m.effectSpeaker.click();
            }
        }
        else
        {
            if (currentUpgradeValue != 0)
            {
                currentUpgradeValue = 0;
                selectUpgrade0.transform.SetParent(folder1.transform);
                selectUpgrade1.transform.SetParent(folder2.transform);
                selectUpgrade2.transform.SetParent(folder3.transform);
                selectUpgrade3.transform.SetParent(folder4.transform);
            }
        }

    }
    void SelectUpgrade1()
    {
        if (dropNumber == 1)
        {
            if (Manager.m.level >= 3)
            {
                if (currentUpgradeValue != 1)
                {
                    currentUpgradeValue = 1;
                    selectUpgrade1.transform.SetParent(folder1.transform);
                    selectUpgrade0.transform.SetParent(folder2.transform);
                    selectUpgrade2.transform.SetParent(folder3.transform);
                    selectUpgrade3.transform.SetParent(folder4.transform);

                    Manager.m.effectSpeaker.accept();
                }
                else
                {
                    Manager.m.effectSpeaker.click();
                }
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
        else
        {
            if (Manager.m.level >= 3)
            {
                if (currentUpgradeValue != 1)
                {
                    currentUpgradeValue = 1;
                    selectUpgrade1.transform.SetParent(folder1.transform);
                    selectUpgrade0.transform.SetParent(folder2.transform);
                    selectUpgrade2.transform.SetParent(folder3.transform);
                    selectUpgrade3.transform.SetParent(folder4.transform);
                }
            }
        }
    }
    void SelectUpgrade2()
    {
        if (dropNumber == 1)
        {
            if (Manager.m.level >= 6)
            {
                if (currentUpgradeValue != 2)
                {
                    currentUpgradeValue = 2;
                    selectUpgrade2.transform.SetParent(folder1.transform);
                    selectUpgrade3.transform.SetParent(folder2.transform);
                    selectUpgrade1.transform.SetParent(folder3.transform);
                    selectUpgrade0.transform.SetParent(folder4.transform);

                    Manager.m.effectSpeaker.accept();
                }
                else
                {
                    Manager.m.effectSpeaker.click();
                }
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
        else
        {
            if (Manager.m.level >= 6)
            {
                if (currentUpgradeValue != 2)
                {
                    currentUpgradeValue = 2;
                    selectUpgrade2.transform.SetParent(folder1.transform);
                    selectUpgrade3.transform.SetParent(folder2.transform);
                    selectUpgrade1.transform.SetParent(folder3.transform);
                    selectUpgrade0.transform.SetParent(folder4.transform);
                }
            }
        }
    }
    void SelectUpgrade3()
    {
        if (dropNumber == 1)
        {
            if (Manager.m.level >= 9)
            {
                if (currentUpgradeValue != 3)
                {
                    currentUpgradeValue = 3;
                    selectUpgrade3.transform.SetParent(folder1.transform);
                    selectUpgrade2.transform.SetParent(folder2.transform);
                    selectUpgrade1.transform.SetParent(folder3.transform);
                    selectUpgrade0.transform.SetParent(folder4.transform);

                    Manager.m.effectSpeaker.accept();
                }
                else
                {
                    Manager.m.effectSpeaker.click();
                }
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
        else
        {
            if (Manager.m.level >= 9)
            {
                if (currentUpgradeValue != 3)
                {
                    currentUpgradeValue = 3;
                    selectUpgrade3.transform.SetParent(folder1.transform);
                    selectUpgrade2.transform.SetParent(folder2.transform);
                    selectUpgrade1.transform.SetParent(folder3.transform);
                    selectUpgrade0.transform.SetParent(folder4.transform);
                }
            }
        }
    }
    void SelectShowAll()
    {
        if (dropNumber == 1)
        {
            Manager.m.effectSpeaker.click();
        }
        if (showAllBars == false)
        {
            showAllBars = true;
            showallButton.GetComponent<RawImage>().texture = marketEnabled;
        }
        else
        {
            showAllBars = false;
            showallButton.GetComponent<RawImage>().texture = marketDisabled;
        }
    }


    void Exit()
    {
        if (dropNumber == 1)
        {
            Manager.m.effectSpeaker.click();
        }
        Manager.m.inMarket = false;
    }
}
