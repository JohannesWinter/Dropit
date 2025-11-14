using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEDestructiveBelts : QuickTimeEvent
{
    public float percentage;
    public QTEDestructiveBelts(float duration, float percentage) : base(duration, Manager.m.eventImages[16])
    {
        qteID = 16;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEBelts = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEBelts = 1;
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
        return "shabby belts:\n" + Mathf.Round((-percentage) * 100) + "% value reduction / belt";
    }
    public override string getShortDescription()
    {
        return "shabby belts";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
