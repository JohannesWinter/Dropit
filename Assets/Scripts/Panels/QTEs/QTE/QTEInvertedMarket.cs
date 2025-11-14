using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEInvertedMarket : QuickTimeEvent
{
    public QTEInvertedMarket(float duration) : base(duration, Manager.m.eventImages[22])
    {
        qteID = 22;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEInvertedMarket = true;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEInvertedMarket = false;
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
        return "Reverse Capitalism:\n-200% value on market";
    }
    public override string getShortDescription()
    {
        return "Reverse Capitalism";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
