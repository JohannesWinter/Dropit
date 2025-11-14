using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEMarketBoost : QuickTimeEvent
{
    public float boost;
    public QTEMarketBoost(float duration, float boost) : base(duration, Manager.m.eventImages[13])
    {
        qteID = 13;
        this.boost = boost;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEMarketBoost = boost;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEMarketBoost = 0;
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
        return "High demand:\n+" + Mathf.Round((boost) * 100) + "% market increase on sell";
    }
    public override string getShortDescription()
    {
        return "High demand";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
