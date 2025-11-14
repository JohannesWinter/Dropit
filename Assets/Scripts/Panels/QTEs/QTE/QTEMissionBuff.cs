using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEMissionBuff : QuickTimeEvent
{
    public int dropperType;
    public QTEMissionBuff(float duration, int dropperType) : base(duration, Manager.m.eventImages[17])
    {
        qteID = 17;
        this.dropperType = dropperType;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEMissionBuff = dropperType;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEMissionBuff = 0;
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
        return "Easy Jobs:\nx2 " + Manager.m.oreIdentifications[dropperType - 1] + "-Mission Bonus ";
    }
    public override string getShortDescription()
    {
        return "Easy Jobs";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
