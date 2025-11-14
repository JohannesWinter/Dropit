using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEBrokenBelts : QuickTimeEvent
{
    public float percentage;
    public QTEBrokenBelts(float duration, float percentage) : base(duration, Manager.m.eventImages[12])
    {
        qteID = 12;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEBrokenBelts = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEBrokenBelts = 1;
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
        return "Broken Belt Bolts:\n-" + Mathf.Round((1 - percentage) * 100) + "% Conveyor Belt speed";
    }
    public override string getShortDescription()
    {
         return "Broken Belt Bolts";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
