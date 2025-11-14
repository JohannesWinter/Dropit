using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEMarketCrash : QuickTimeEvent
{
    public float crash;
    public QTEMarketCrash(float duration, float crash) : base(duration, Manager.m.eventImages[14])
    {
        qteID = 14;
        this.crash = crash;

    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEMarketCrash = crash;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEMarketCrash = 0;
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
        return "It’s Just a Correction:\n+" + Mathf.Round((crash) * 100) + "% market decrease speed";
    }
    public override string getShortDescription()
    {
        return "Just a Correction";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
