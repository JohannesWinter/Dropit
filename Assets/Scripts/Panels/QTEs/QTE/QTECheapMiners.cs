using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTECheapMiners : QuickTimeEvent
{
    public int dropperType;
    public float percentage;
    public QTECheapMiners(float duration, int dropperType, float percentage) : base(duration, Manager.m.eventImages[3])
    {
        qteID = 3;
        this.dropperType = dropperType;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTECheapMinersNumber = dropperType;
        Manager.m.qTECheapMiners = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTECheapMiners = 1;
        Manager.m.qTECheapMinersNumber = 0;
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
        return "Miner Sale:\n-" + Mathf.Round((1 - percentage) * 100) + "% " + Manager.m.oreIdentifications[dropperType - 1];
    }
    public override string getShortDescription()
    {
        return "Miner Sale";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
