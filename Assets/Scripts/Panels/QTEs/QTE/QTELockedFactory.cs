using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTELockedFactory : QuickTimeEvent
{
    public int factoryHall;
    public QTELockedFactory(float duration, int factoryHall) : base(duration, Manager.m.eventImages[20])
    {
        qteID = 20;
        this.factoryHall = factoryHall;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTELockedFactory = factoryHall;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTELockedFactory = 0;
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
        return "Danger Zone:\nHall " + factoryHall + " is not safe...";
    }
    public override string getShortDescription()
    {
        return "Danger Zone " + factoryHall;
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
