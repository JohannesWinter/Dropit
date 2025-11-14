using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationBox : MonoBehaviour
{
    public GameObject informationBox;
    public GameObject informationBoxText;
    GameObject factoryObject;
    int dropperNumber;
    public GameObject[] upgrades;
    GameObject[][] upgradeNumbers;
    public GameObject[] u1;
    public GameObject[] u2;
    public GameObject[] u3;
    public GameObject[] u4;
    private void Start()
    {
        upgradeNumbers = new GameObject[4][];
        for (int i = 0; i < upgradeNumbers.Length; i++)
        {
            upgradeNumbers[i] = new GameObject[8];
        }
        for (int i = 0; i < upgradeNumbers[0].Length; i++)
        {
            upgradeNumbers[0][i] = u1[i];
        }
        for (int i = 0; i < upgradeNumbers[1].Length; i++)
        {
            upgradeNumbers[1][i] = u2[i];
        }
        for (int i = 0; i < upgradeNumbers[2].Length; i++)
        {
            upgradeNumbers[2][i] = u3[i];
        }
        for (int i = 0; i < upgradeNumbers[3].Length; i++)
        {
            upgradeNumbers[3][i] = u4[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.inShopDropper)
        {
            informationBox.SetActive(true);
            factoryObject = Manager.m.droppers[Manager.m.dropperNumber - 1];
            dropperNumber = Manager.m.dropperNumber - 1;
            StartCoroutine(WaitDropper());
        }
        else if (Manager.m.inShopMachine)
        {
            informationBox.SetActive(true);
            for (int i = 0; i < Manager.m.machines.Length; i++)
            {
                if (i == Manager.m.machineNumber - 1)
                {
                    factoryObject = Manager.m.machines[i];
                    if (i == 0 || i == 3 || i == 6)
                    {
                        StartCoroutine(WaitConveyorBelt());
                    }
                    else if (i == 1 || i == 4 || i == 7)
                    {
                        StartCoroutine(WaitFurnace());
                    }
                    else if (i == 2 || i == 5 || i == 8)
                    {
                        StartCoroutine(WaitUpgrader());
                    }
                    else
                    {
                        informationBoxText.GetComponent<TextMeshProUGUI>().text = "Error";
                    }
                }
            }
        }
        else
        {
            informationBox.SetActive(false);
            informationBoxText.GetComponent<TextMeshProUGUI>().text = "";
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);
            }
        }
    }

    private IEnumerator WaitDropper()
    {
        yield return new WaitForEndOfFrame();
        double durabilityMinutes = factoryObject.GetComponent<RepairDropper>().timeperpercent * 100 / 60;
        double durabilitySeconds = 60 * (durabilityMinutes - Mathf.Floor((float)durabilityMinutes));
        durabilityMinutes = Mathf.Floor((float)durabilityMinutes);
        durabilitySeconds = Mathf.Floor((float)durabilitySeconds);
        string _0;
        if (durabilitySeconds < 0)
        {
            durabilitySeconds = 0;
        }
        if (durabilitySeconds < 10)
        {
            _0 = "0";
        }
        else
        {
            _0 = "";
        }
        double cost = factoryObject.GetComponent<RepairDropper>().cost;
        string changedCost = "";
        if (Manager.m.qTECheapMinersNumber == factoryObject.GetComponent<RepairDropper>().dropperNumber)
        {
            cost = cost * Manager.m.qTECheapMiners;
            changedCost = "(-" + Mathf.Round((1 - Manager.m.qTECheapMiners)) * 100 + "%) ";
        }
        else if (Manager.m.qTEExpensiveMinersNumber == factoryObject.GetComponent<RepairDropper>().dropperNumber)
        {
            cost = cost * Manager.m.qTEExpensiveMiners;
            changedCost = "(+" + Mathf.Round((Manager.m.qTEExpensiveMiners - 1) * 100) + "%) ";
        }
        informationBoxText.GetComponent<TextMeshProUGUI>().text = ("<b>Identification:<br></b> " + Manager.m.dropperIdentifications[dropperNumber]) + 
            ("<size=20><br>") + ("<b>Cost:<br></b> " + "" + changedCost + Money.NumberInUnit(cost, 1) + "$") + 
            ("<size=20><br>") + ("<b>Maintenance:<br></b> " + Money.NumberInUnit(factoryObject.GetComponent<RepairDropper>().costPerSecond, 1) + "$/Sec") + 
            ("<size=20><br>") + ("<b>Ore-Value:<br></b> " + Money.NumberInUnit(factoryObject.GetComponent<RepairDropper>().oreValue, 1) + "$") + 
            ("<size=20><br>") + ("<b>Drop-rate:<br></b> " + Mathf.Round(60 / factoryObject.GetComponent<RepairDropper>().dropSpeed * 100) / 100 + "/Min") + 
            ("<size=20><br>") + ("<b>Durability:<br></b> " + durabilityMinutes + ":" + _0 + durabilitySeconds + "Min") + 
            ("<size=20><br>") + ("<b>Needs:</b><br>");

        List<int> upgradeHeight = new List<int>();

        for (int i = 0; i < Manager.m.dropInputCapacitys[dropperNumber].Length; i++)
        {
            if (Manager.m.dropInputCapacitys[dropperNumber][i] > 0)
            {
                informationBoxText.GetComponent<TextMeshProUGUI>().text += " " + Manager.m.dropConsumeOres[dropperNumber][i] + "<size=10> <size=20>*<size=10> <size=20>" + Manager.m.oreIdentifications[i] + "<br>  Lvl:<br>";
                upgradeHeight.Add(Manager.m.dropInputUpgrades[dropperNumber][i]);
            }
        }
        if (upgradeHeight.Count == 0)
        {
            informationBoxText.GetComponent<TextMeshProUGUI>().text += " --";
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);
            }
            for (int i = 0; i < upgradeHeight.Count; i++)
            {
                upgrades[i].SetActive(true);
            }
            for (int i = 0; i < upgradeNumbers.Length; i++)
            {
                for (int x = 0; x < upgradeNumbers[i].Length; x++)
                {
                    upgradeNumbers[i][x].SetActive(false);
                }
            }
            for (int i = 0; i < upgradeHeight.Count; i++)
            {
                for (int x = 0; x < upgradeNumbers[i].Length / 2; x++)
                {
                    upgradeNumbers[i][x].SetActive(true);
                }
            }
            for (int i = 0; i < upgradeHeight.Count; i++)
            {
                upgradeNumbers[i][upgradeHeight[i] + 4].SetActive(true);
            }
        }
    }
    private IEnumerator WaitConveyorBelt()
    {
        yield return new WaitForEndOfFrame();
        double cost = factoryObject.GetComponent<RepairDropper>().cost;
        string changedCost = "";
        if (Manager.m.qTECheapMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTECheapMachines;
            changedCost = "(-" + Mathf.Round((1 - Manager.m.qTECheapMachines) * 100) + "%) ";
        }
        else if (Manager.m.qTEExpensiveMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTEExpensiveMachines;
            changedCost = "(+" + Mathf.Round((Manager.m.qTEExpensiveMachines - 1) * 100) + "%) ";
        }
        informationBoxText.GetComponent<TextMeshProUGUI>().text = ("<size=20><b>Identification:<br></b> " + Manager.m.machineIdentifications[Manager.m.machineNumber - 1]) + 
            ("<size=20><br>") + ("<b>Cost:<br></b> " + changedCost + Money.NumberToUnit((float)cost)) + 
            ("<size=20><br>") + ("<b>Maintenance:<br></b> " + Money.NumberToUnit((float)factoryObject.GetComponent<RepairDropper>().costPerSecond) + "/Sec") + 
            ("<size=20><br>") + ("<b>Speed:<br></b> " + factoryObject.GetComponent<RepairDropper>().conveyorBeltSpeed + "km/h");
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].SetActive(false);
        }
    }
    private IEnumerator WaitFurnace()
    {
        yield return new WaitForEndOfFrame();
        double cost = factoryObject.GetComponent<RepairDropper>().cost;
        string changedCost = "";
        if (Manager.m.qTECheapMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTECheapMachines;
            changedCost = "(-" + Mathf.Round((1 - Manager.m.qTECheapMachines) * 100) + "%) ";
        }
        else if (Manager.m.qTEExpensiveMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTEExpensiveMachines;
            changedCost = "(+" + Mathf.Round((Manager.m.qTEExpensiveMachines - 1) * 100) + "%) ";
        }
        informationBoxText.GetComponent<TextMeshProUGUI>().text = ("<size=20><b>Identification:<br></b> " + Manager.m.machineIdentifications[Manager.m.machineNumber - 1]) + 
            ("<size=20><br>") + ("<b>Cost:<br></b> " + changedCost + Money.NumberToUnit((float)cost)) + 
            ("<size=20><br>") + ("<b>Maintenance:<br></b> " + Money.NumberToUnit((float)factoryObject.GetComponent<RepairDropper>().costPerSecond) + "/Sec") + 
            ("<size=20><br>") + ("<b>Multiplier:<br></b> " + factoryObject.GetComponent<RepairDropper>().furnaceMultiplier * 100 + "%");
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].SetActive(false);
        }
    }
    private IEnumerator WaitUpgrader()
    {
        yield return new WaitForEndOfFrame();
        double cost = factoryObject.GetComponent<RepairDropper>().cost;
        string changedCost = "";
        if (Manager.m.qTECheapMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTECheapMachines;
            changedCost = "(-" + Mathf.Round((1 - Manager.m.qTECheapMachines) * 100) + "%) ";
        }
        else if (Manager.m.qTEExpensiveMachinesNumber == factoryObject.GetComponent<RepairDropper>().machineNumber)
        {
            cost = cost * Manager.m.qTEExpensiveMachines;
            changedCost = "(+" + Mathf.Round((Manager.m.qTEExpensiveMachines - 1) * 100) + "%) ";
        }
        informationBoxText.GetComponent<TextMeshProUGUI>().text = ("<size=20><b>Identification:<br></b> " + Manager.m.machineIdentifications[Manager.m.machineNumber - 1]) + 
            ("<size=20><br>") + ("<b>Cost:<br></b> " + changedCost + Money.NumberToUnit((float)cost)) + 
            ("<size=20><br>") + ("<b>Maintenance:<br></b> " + Money.NumberToUnit((float)factoryObject.GetComponent<RepairDropper>().costPerSecond) + "/Sec") + 
            ("<size=20><br>") + ("<b>Level:<br></b> +1<0-" + factoryObject.GetComponent<RepairDropper>().upgradeLevelMax + "> <br> " +
            "<" + factoryObject.GetComponent<RepairDropper>().upgradeLevelMax + ">:+" 
            + Manager.m.upgradeMultipliers[factoryObject.GetComponent<RepairDropper>().upgradeLevelMax] * 100 + "%<br>");
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].SetActive(false);
        }
    }
}