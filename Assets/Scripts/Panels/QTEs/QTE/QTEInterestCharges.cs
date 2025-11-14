using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEInterestCharges : QuickTimeEvent
{
    public float percentage;
    public QTEInterestCharges(float duration, float percentage) : base(duration, Manager.m.eventImages[21])
    {
        qteID = 21;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        float interestCharge = 0;
        GameObject[] factoryObjects = GameObject.FindGameObjectsWithTag("FactoryObject");
        for (int i = 0; i < factoryObjects.Length; i++)
        {
            if (factoryObjects[i].GetComponent<RepairDropper>().dropperNumber > 0)
            {
                interestCharge += factoryObjects[i].GetComponent<RepairDropper>().sellValue * percentage * Time.deltaTime;
            }
        }
        Manager.m.money += interestCharge;
        Manager.m.incomeLastSecond += interestCharge;
    }

    override public QuickTimeEvent end()
    {
        float interestCharge = 0;
        GameObject[] factoryObjects = GameObject.FindGameObjectsWithTag("FactoryObject");
        for (int i = 0; i < factoryObjects.Length; i++)
        {
            if (factoryObjects[i].GetComponent<RepairDropper>().dropperNumber > 0)
            {
                interestCharge += factoryObjects[i].GetComponent<RepairDropper>().sellValue * percentage * Time.deltaTime;
            }
        }
        Manager.m.money += interestCharge * (startTime / 5);
        Manager.m.incomeLastSecond += interestCharge * (startTime / 5);
        Manager.Destroy(base.getDisplay());
        return null;
    }
    override public float getDuration()
    {
        return duration;
    }
    public override float getStartTime()
    {
        return startTime;
    }

    override public string getDescription()
    {
        return "Interest Charges:\n" + Mathf.Round((percentage) * 100) + "% miner interes / sec";
    }
    public override string getShortDescription()
    {
        return "Interest Charges";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
