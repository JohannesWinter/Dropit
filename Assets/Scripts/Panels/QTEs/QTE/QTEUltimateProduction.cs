using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEUltimateProduction : QuickTimeEvent
{
    public QTEUltimateProduction(float duration) : base(duration, Manager.m.eventImages[23])
    {
        qteID = 23;
    }
    override public void start()
    {
        for (int i = 0; i < Manager.m.marketManager.dropValueMultipliers.Length; i++)
        {
            for (int x = 0; x < Manager.m.marketManager.dropValueMultipliers[i].Length; x++)
            {
                Manager.m.marketManager.dropValueMultipliers[i][x] = 2;
            }
        }
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEUltimateProduction = true;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEUltimateProduction = false;
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
        return "Power Surge:\nJust like priting money...";
    }
    public override string getShortDescription()
    {
        return "Power Surge";
    }

    override public bool isPositiveEvent()
    {
        return true;
    }
}
