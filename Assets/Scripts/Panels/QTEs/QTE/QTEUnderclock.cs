using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEUnderclock : QuickTimeEvent
{
    public int dropperType;
    private int updateCounter;
    public float clocking;
    public QTEUnderclock(float duration, int dropperType, float clocking) : base(duration, Manager.m.eventImages[6])
    {
        qteID = 6;
        this.dropperType = dropperType;
        updateCounter = 0;
        this.clocking = clocking;
    }
    override public void start()
    {
        generateDisplay();
        Drop[] droppers = Manager.m.machineFolder.GetComponentsInChildren<Drop>();
        for (int i = 0; i < droppers.Length; i++)
        {
            droppers[i].quickTimeEventClocking = clocking;
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
            droppers[i].quickTimeEventClocking = 1f;
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
        return "Underclock Production:\n" + Mathf.Round(clocking * 100) +"% " + Manager.m.oreIdentifications[dropperType - 1] + " production";
    }
    public override string getShortDescription()
    {
        return "Underclock";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
