using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEEfficiency : QuickTimeEvent
{
    private int updateCounter;
    public float efficiency;
    public QTEEfficiency(float duration, float efficiency) : base(duration, Manager.m.eventImages[5])
    {
        qteID = 5;
        updateCounter = 0;
        this.efficiency = efficiency;
    }
    override public void start()
    {
        generateDisplay();
        Drop[] droppers = Manager.m.machineFolder.GetComponentsInChildren<Drop>();
        for (int i = 0; i < droppers.Length; i++)
        {
            droppers[i].quickTimeEventEfficiency = efficiency;
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
        Drop[] droppers = Manager.m.machineFolder.GetComponentsInChildren<Drop>();
        for (int i = 0; i < droppers.Length; i++)
        {
            droppers[i].quickTimeEventEfficiency = 1;
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
        return "efficient production:\n" + Mathf.Round(efficiency * 100) + "% Ressource Consumption";
    }
    public override string getShortDescription()
    {
        return "efficient production";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
