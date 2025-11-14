using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEUltimateWipeout : QuickTimeEvent
{
    public float currenttime;
    public float percentage;
    public QTEUltimateWipeout(float duration, float percentage) : base(duration, Manager.m.eventImages[24])
    {
        qteID = 24;
        currenttime = Time.time;
    }
    override public void start()
    {
        currenttime = Time.time;
        Manager.m.musicSpeaker.ChangeMusic(4, 4, "scary2", 2);
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        if (Time.time > currenttime + 1)
        {
            currenttime = Time.time;
            double wealthDecrease = Manager.m.money * percentage;
            Manager.m.money -= wealthDecrease;
            Manager.m.incomeLastSecond -= wealthDecrease;
        }
        Manager.m.qTEUltimateWipeout = true;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.musicSpeaker.ChangeMusic(4, 4, "normal", 2);
        Manager.m.qTEUltimateWipeout = false;
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
        return "Critical Failure:\nTask is to survive...";
    }
    public override string getShortDescription()
    {
        return "Critical Failure";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
    public override void continueQTE()
    {
        Manager.m.musicSpeaker.ChangeMusic(1, 4, "scary2", 4);
    }
}
