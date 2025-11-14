using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTECheapMachines : QuickTimeEvent
{
    public int machineType;
    public float percentage;
    public QTECheapMachines(float duration, int machineType, float percentage) : base(duration, Manager.m.eventImages[7])
    {
        qteID = 7;
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
        Manager.m.qTECheapMachinesNumber = machineType;
        Manager.m.qTECheapMachines = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTECheapMachines = 1;
        Manager.m.qTECheapMachinesNumber = 0;
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
        return "Machine Sale:\n-" + Mathf.Round((1 - percentage) * 100) + "% " + Manager.m.machineIdentifications[machineType - 1];
    }
    public override string getShortDescription()
    {
        return "Machine Sale";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
