using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTECheapRepairs : QuickTimeEvent
{
    private int updateCounter;
    public float percentage;
    public QTECheapRepairs(float duration, float percentage) : base(duration, Manager.m.eventImages[9])
    {
        qteID = 9;
        this.percentage = percentage;
        updateCounter = 0;
    }
    override public void start()
    {
        generateDisplay();
        RepairDropper[] droppers = Manager.m.machineFolder.GetComponentsInChildren<RepairDropper>();
        for (int i = 0; i < droppers.Length; i++)
        {
            droppers[i].quickTimeEventRepairs = percentage;
        }
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        updateCounter++;
        if (updateCounter > 30)
        {
            updateCounter = 0;
            start();
        }
    }

    override public QuickTimeEvent end()
    {
        RepairDropper[] droppers = Manager.m.machineFolder.GetComponentsInChildren<RepairDropper>();
        for (int i = 0; i < droppers.Length; i++)
        {
            droppers[i].quickTimeEventRepairs = 1;
        }
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
        return "Easy Fix:\n-" + Mathf.Round((1 - percentage) * 100) + "% Repair Costs";
    }
    public override string getShortDescription()
    {
        return "Easy Fix";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
