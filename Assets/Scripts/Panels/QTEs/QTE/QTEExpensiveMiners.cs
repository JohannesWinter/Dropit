using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEExpensiveMiners : QuickTimeEvent
{
    public int dropperType;
    public float percentage;
    public QTEExpensiveMiners(float duration, int dropperType, float percentage) : base(duration, Manager.m.eventImages[4])
    {
        qteID = 4;
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
        Manager.m.qTEExpensiveMinersNumber = dropperType;
        Manager.m.qTEExpensiveMiners = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEExpensiveMiners = 1;
        Manager.m.qTEExpensiveMinersNumber = 0;
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
        return "overpriced miners:\n+" + Mathf.Round((percentage - 1) * 100) + "% " + Manager.m.oreIdentifications[dropperType - 1];
    }
    public override string getShortDescription()
    {
        return "overpriced miners";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
