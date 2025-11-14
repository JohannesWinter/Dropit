using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEMissionImpossible : QuickTimeEvent
{
    public QTEMissionImpossible(float duration) : base(duration, Manager.m.eventImages[18])
    {
        qteID = 18;
    }
    override public void start()
    {
        MissionManager m = Manager.m.missionManager;
        for (int i = m.missions.Count - 1; i >= 0; i--)
        {
            if (m.missions[i].GetComponent<Mission>().acceptedMission)
            {
                m.missions[i].GetComponent<Mission>().CancelMission();
            }
        }
        generateDisplay();
    }
    override public void update()
    {
        this.duration -= Time.deltaTime;
        Manager.m.qTEMissionImpossible = true;
    }

    override public QuickTimeEvent end()
    {
        Manager.m.qTEMissionImpossible = false;
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
        return "Mission Impossible:\n-50% Mission delivery rate ";
    }
    public override string getShortDescription()
    {
        return "Mission Impossible";
    }

    override public bool isPositiveEvent()
    {
        return false;
    }
}
