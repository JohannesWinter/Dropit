using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEOverheating : QuickTimeEvent
{
    public float percentage;
    public int ressourceType;
    public QTEOverheating(float duration, int ressourceType,  float percentage) : base(duration, Manager.m.eventImages[11])
    {
        qteID = 11;
        this.percentage = percentage;
        this.ressourceType = ressourceType;
    }
    override public void start()
    {
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEOverheating = percentage;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEOverheating = 1;
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
        return "Overheating:\n+" + Mathf.Round((percentage - 1) * 100) + "% Market sell value";
    }
    public override string getShortDescription()
    {
        return "Overheating";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
