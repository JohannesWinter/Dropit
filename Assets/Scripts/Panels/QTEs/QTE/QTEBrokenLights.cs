using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEBrokenLights : QuickTimeEvent
{
    private int updateCounter;
    public QTEBrokenLights(float duration) : base(duration, Manager.m.eventImages[2])
    {
        qteID = 2;
        updateCounter = 0;
    }
    override public void start()
    {
        Manager.m.musicSpeaker.ChangeMusic(4, 4, "scary1", 2);
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        updateCounter++;
        if (updateCounter > 30)
        {
            Manager.m.qTEBrokenLights = true;
        }
    }

    override public QuickTimeEvent end()
    {
        Manager.m.musicSpeaker.ChangeMusic(4, 4, "normal", 2);
        Manager.m.qTEBrokenLights = false;
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
        return "Broken Lights:\nA little hard to see...";
    }
    public override string getShortDescription()
    {
        return "Lights Failing";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }

    public override void continueQTE()
    {
        Manager.m.musicSpeaker.ChangeMusic(1, 4, "scary1", 4);
    }
}
