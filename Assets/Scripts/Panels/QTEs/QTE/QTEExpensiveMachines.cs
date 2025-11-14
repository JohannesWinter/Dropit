using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEExpensiveMachines : QuickTimeEvent
{
    public int machineType;
    public float percentage;
    public QTEExpensiveMachines(float duration, int machineType, float percentage) : base(duration, Manager.m.eventImages[8])
    {
        qteID = 8;
        this.machineType = machineType;
        this.percentage = percentage;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEExpensiveMachinesNumber = machineType;
        Manager.m.qTEExpensiveMachines = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEExpensiveMachines = 1;
        Manager.m.qTEExpensiveMachinesNumber = 0;
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
        return "overpriced machines:\n+" + Mathf.Round((percentage - 1) * 100) + "% " + Manager.m.machineIdentifications[machineType - 1];
    }
    public override string getShortDescription()
    {
        return "overpriced machines";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
