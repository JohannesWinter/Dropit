using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEInitiate : QuickTimeEvent
{
    QuickTimeEvent toStartEvent;
    public QTEInitiate(float duration, QuickTimeEvent toStartEvent) : base(duration, Manager.m.eventImages[0])
    {
        qteID = 0;
        this.toStartEvent = toStartEvent;
    }
    override public void start()
    {
        generateDisplay();
        string intensityString = "";
        if (toStartEvent.intensity > 1.5f)
            intensityString = "Big ";
        else if (toStartEvent.intensity < 0.2f)
            intensityString = "Small ";

        if (duration % 60 >= 10)
        {
            Manager.m.notificationManager.AddNotification("!Info!\n" +
                intensityString + "Event: " + toStartEvent.getShortDescription() +
                " in " + Mathf.Floor(duration / 60) + ":" + Mathf.Floor(duration % 60) + " Min",
                Manager.m.eventImages[27]);
        }
        else
        {
            Manager.m.notificationManager.AddNotification("!Info!\n" +
                intensityString + "Event: " + toStartEvent.getShortDescription() +
                " in " + Mathf.Floor(duration / 60) + ":0" + Mathf.Floor(duration % 60) + " Min",
                Manager.m.eventImages[27]);
        }
    }

    override public float getDuration()
    {
        return duration;
    }
    public override float getStartTime()
    {
        return startTime;
    }
    override public void update()
    {
        duration -= Time.deltaTime;
    }
    override public QuickTimeEvent end()
    {
        Manager.Destroy(base.getDisplay());
        if (toStartEvent.isPositiveEvent())
            Manager.m.notificationManager.AddNotification("!Buff!\nStarting Event: " + toStartEvent.getShortDescription(), toStartEvent.getDisplayImage());
        else
            Manager.m.notificationManager.AddNotification("!Warning!\nStarting Event: " + toStartEvent.getShortDescription(), toStartEvent.getDisplayImage());
        return toStartEvent;
    }
    override public string getDescription()
    {
        string intensityString = "";
        if (toStartEvent.intensity > 1.5f)
            intensityString = "Big ";
        else if (toStartEvent.intensity < 0.2f)
            intensityString = "Small ";
        return "Initiate " + intensityString + "Event:\n" + toStartEvent.getShortDescription();
    }
    override public bool isPositiveEvent()
    {
        return toStartEvent.isPositiveEvent();
    }
    public override QuickTimeEvent getFollowing()
    {
        return toStartEvent;
    }
    public override void setFollowing(QuickTimeEvent following)
    {
        toStartEvent = following;   
    }
}
