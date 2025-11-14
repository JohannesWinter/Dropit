using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEQualityBelts : QuickTimeEvent
{
    public float percentage;
    public QTEQualityBelts(float duration, float percentage) : base(duration, Manager.m.eventImages[15])
    {
        qteID = 15;
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
        return "Conveyor Supreme:\n+" + Mathf.Round((percentage) * 100) + "% value bonus / belt";
    }
    public override string getShortDescription()
    {
        return "Conveyor Supreme";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
