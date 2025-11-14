using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public GameObject overlayAround;
    public TextMeshProUGUI moneyOutput;
    public TextMeshProUGUI balanceChanges;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        overlayAround.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        if (Manager.m.money >= 0)
        {
            moneyOutput.color = Color.green;
        }
        else
        {
            moneyOutput.color = Color.red;
        }

        moneyOutput.text = NumberToUnit((float)Manager.m.money);

        if (counter > 1 / Time.deltaTime)
        {
            counter = 0;
            double avg = Manager.m.incomeLastMinute.Sum();
            avg = avg / Manager.m.incomeLastMinute.Count(); //or just Manage.m.incomeLastMinute.Average();
            avg = Mathf.Round((float)avg * 10) / 10;
            if (Manager.m.incomeLastMinute.Count() > 0)
            {
                if (avg >= 0)
                {
                    balanceChanges.text = "+" + NumberToUnit((float)avg) + "/<size=11>Sec";
                    balanceChanges.color = Color.green;
                }
                else
                {
                    balanceChanges.text = NumberToUnit((float)avg) + "/<size=11>Sec";
                    balanceChanges.color = Color.red;
                }
            }
            else
            {
                balanceChanges.text = "";
            }
        }
        if (Manager.m.hideFactoryUI == true)
        {
            moneyOutput.text = "";
            balanceChanges.text = "";
            counter = 99999;
        }
        else
        {
            counter++;
        }
    }

    public static string NumberInUnit(double number, int decimalPlace)
    {
        if (number >= 0 && decimalPlace >= 0)
        {
            if (number < 1000)
            {
                double x = Mathf.Floor(((float)number / 1) * Mathf.Pow(10, decimalPlace)) / Mathf.Pow(10, decimalPlace);
                if (x == 1000)
                {
                    x = 999f;
                }
                return x + "";
            }
            else if (number < 1000000)
            {
                double x  = Mathf.Floor(((float)number / 1000) * Mathf.Pow(10, decimalPlace)) / Mathf.Pow(10, decimalPlace);
                if (x == 1000)
                {
                    x = 999f;
                }
                return x + "K";
            }
            else if (number < 1000000000)
            {
                double x  = Mathf.Floor(((float)number / 1000000) * Mathf.Pow(10, decimalPlace)) / Mathf.Pow(10, decimalPlace);
                if (x == 1000)
                {
                    x = 999f;
                }
                return x + "M";
            }
            else if (number < 1000000000000)
            {
                double x = Mathf.Floor(((float)number / 1000000000) * Mathf.Pow(10, decimalPlace)) / Mathf.Pow(10, decimalPlace);
                if (x == 1000)
                {
                    x = 999f;
                }
                return x + "B";
            }
            else
            {
                double x = Mathf.Floor(((float)number / 1000000000000) * Mathf.Pow(10, decimalPlace)) / Mathf.Pow(10, decimalPlace);
                if (x == 1000)
                {
                    x = 999f;
                }
                return x + "T";
            }
        }
        else
        {
            return "";
        }
    }

    public static string NumberToUnit(float number)
    {
        bool isNegative = false;
        if(number < 0)
        {
            isNegative = true;
            number = number * -1;
        }

        string numberUnit = "";

        if (number < 10)
        {
            numberUnit = Mathf.Floor(number * 100f) / 100 + "$";
        }
        else if (number < 100)
        {
            numberUnit = Mathf.Floor(number * 10f) / 10 + "$";
        }
        else if (number < 1000)
        {
            numberUnit = Mathf.Floor(number * 1f) / 1 + "$";
        }
        else if (number < 10000)
        {
            numberUnit = Mathf.Floor((number / 1000) * 100f) / 100 + "K$";
        }
        else if (number < 100000)
        {
            numberUnit = Mathf.Floor((number / 1000) * 10f) / (10) + "K$";
        }
        else if (number < 1000000)
        {
            numberUnit = Mathf.Floor((number / 1000) * 1f) / (1) + "K$";
        }
        else if (number < 10000000)
        {
            numberUnit = Mathf.Floor((number / 1000000) * 100f) / (100) + "M$";
        }
        else if (number < 100000000)
        {
            numberUnit = Mathf.Floor((number / 1000000) * 10f) / (10) + "M$";
        }
        else if (number < 1000000000)
        {
            numberUnit = Mathf.Floor((number / 1000000) * 1f) / (1) + "M$";
        }
        else if (number < 10000000000)
        {
            numberUnit = Mathf.Floor((number / 1000000000) * 100f) / (100) + "B$";
        }
        else if (number < 100000000000)
        {
            numberUnit = Mathf.Floor((number / 1000000000) * 10f) / (10) + "B$";
        }
        else if (number < 1000000000000)
        {
            numberUnit = Mathf.Floor((number / 1000000000) * 1f) / (1) + "B$";
        }
        else
        {
            numberUnit = Mathf.Floor((number / 1000000000000) * 100f) / (100) + "T$";
        }

        if(isNegative == true)
        {
            numberUnit = "-" + numberUnit;
        }
        numberUnit = numberUnit.Replace(",", ".");

        return numberUnit;
    }

}
