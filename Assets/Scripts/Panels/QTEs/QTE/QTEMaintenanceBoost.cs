using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEMaintenanceBoost : QuickTimeEvent
{
    public float percentage;
    public QTEMaintenanceBoost(float duration, float percentage) : base(duration, Manager.m.eventImages[19])
    {
        qteID = 19;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEMaintenanceBoost = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEMaintenanceBoost = 0;
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
        return "Maintenance Boost:\n" + Mathf.Round((percentage + 1) * 100) + "% maintenance payback";
    }
    public override string getShortDescription()
    {
        return "Maintenance Boost";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
