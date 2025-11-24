using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;

public class QuickTimeEventManager : MonoBehaviour
{
    public List<QuickTimeEvent> currentEvents;
    public GameObject generalDisplay;
    public GameObject window;
    public Button exitButton;
    public Vector3[] positions;
    public GameObject displayFolder;
    public GameObject noEventsDisplay;
    public bool addRandomQuickTimeEvent;

    float currenttime;
    //public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        currentEvents = new List<QuickTimeEvent>();
        exitButton.GetComponent<Button>().onClick.AddListener(Exit);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor - 0.2f, Manager.m.graphicManager.gUIScaleFactor - 0.2f, Manager.m.graphicManager.gUIScaleFactor - 0.2f);
        if (Manager.m.inQuickTimeEvents == true)
        {
            window.SetActive(true);
        }
        else
        {
            window.SetActive(false);
        }
        if (Manager.m.paused == false)
        {
            for (int i = currentEvents.Count() - 1; i >= 0; i--)
            {
                var e = currentEvents[i];
                e.update();
                if (e.getDuration() <= 0)
                {
                    QuickTimeEvent nextEvent = e.end();
                    if (nextEvent != null)
                    {
                        currentEvents[i] = nextEvent;
                        nextEvent.generateDisplay();
                        nextEvent.start();
                    }
                    else
                    {
                        currentEvents.RemoveAt(i);
                    }
                }
            }
            if (currentEvents.Count == 0)
            {
                noEventsDisplay.SetActive(true);
            }
            else
            {
                noEventsDisplay.SetActive(false);
            }
            for (int i = 0; i < currentEvents.Count; i++)
            {
                if (i < positions.Length)
                {
                    currentEvents[i].getDisplay().transform.localPosition = positions[i];
                    currentEvents[i].getDisplay().SetActive(true);
                }
                else
                {
                    currentEvents[i].getDisplay().SetActive(false);
                }
                float screenSizeDiffrence = Manager.m.canvas.GetComponent<RectTransform>().rect.width / Manager.m.canvas.GetComponent<CanvasScaler>().referenceResolution.x;
                currentEvents[i].getDisplay().transform.localScale = new Vector3(screenSizeDiffrence * 0.6f, screenSizeDiffrence * 0.6f, 1);
            }
            if (addRandomQuickTimeEvent == true)
            {
                addRandomQuickTimeEvent = false;
                float intensity = Random.Range(0.0f, 0.55f + Manager.m.getHighestUnlockedType() * 0.05f);
                intensity = Mathf.Round(intensity * 10) / 10;
                var qte = generateQuickTimeEventInitiator(1, Manager.m.getHighestUnlockedQteType(), 120, 240, intensity);
                if (qte != null)
                {
                    currentEvents.Add(qte);
                    qte.start();
                }
            }
            if (currenttime < Time.time)
            {
                currenttime = Time.time + 1;
                if (Random.Range(0, 120 + (30 * currentEvents.Count) - 30 * (Manager.m.level / 11)) == 0 && Manager.m.acessQTEs == true && Manager.m.tutorial.inTutorial2 == false && Manager.m.tutorial.inTutorial3 == false)
                {
                    addRandomQuickTimeEvent = true;
                }
            }
        }
    }
    void Exit()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inQuickTimeEvents = false;
    }


    QuickTimeEvent generateQuickTimeEventInitiator(int minQteID, int maxQteID, float minDuration, float maxDuration, float intensity_0_1)
    {
        if (currentEvents.Count >= 9)
        {
            return null;
        }
        int qteID = Random.Range(minQteID, maxQteID + 1);
        int failedAttempts = 0;
        while (AllowQteStart(qteID) == false)
        {
            qteID = Random.Range(minQteID, maxQteID + 1);
            failedAttempts++;
            if (failedAttempts > 2)
            {
                return null;
            }
        }
        float duration = Random.Range(minDuration, maxDuration); //ca.3min
        int highestUnlocked = Manager.m.getHighestUnlockedType();
        int randomType = Random.Range(0, highestUnlocked + 1) + 1;
        int randomDropperType = Random.Range(Mathf.Max(highestUnlocked - 2, 0), highestUnlocked + 1) + 1;
        int randomMachineType = Random.Range(Mathf.Max(highestUnlocked - 3, 0), Mathf.Min(highestUnlocked + 1, 9)) + 1;
        QuickTimeEvent qTE;
        switch (qteID)
        {
            case 1:
                qTE = new QTEOverclock(duration, randomDropperType, 1.25f + intensity_0_1 * 0.25f);
                break;
            case 2:
                qTE = new QTEBrokenLights(Mathf.Max(duration * intensity_0_1, 30));
                break;
            case 3:
                qTE = new QTECheapMiners(duration / 3, randomDropperType, 0.9f - intensity_0_1 * 0.25f);
                break;
            case 4:
                qTE = new QTEExpensiveMiners(duration * 3, randomDropperType, 1.1f + intensity_0_1 * 0.4f);
                break;
            case 5:
                qTE = new QTEEfficiency(duration, 0.75f - intensity_0_1 * 0.5f);
                break;
            case 6:
                qTE = new QTEUnderclock(duration, randomDropperType, 0.9f - intensity_0_1 * 0.3f);
                break;
            case 7:
                qTE = new QTECheapMachines(duration / 3, randomMachineType, 0.9f - intensity_0_1 * 0.25f);
                break;
            case 8:
                qTE = new QTEExpensiveMachines(duration * 3, randomMachineType, 1.1f + intensity_0_1 * 0.4f);
                break;
            case 9:
                qTE = new QTECheapRepairs(duration * 0.25f, 0.9f - intensity_0_1 * 0.8f);
                break;
            case 10:
                qTE = new QTEExpensiveRepairs(duration * 2.5f, 1.1f + intensity_0_1 * 1.9f);
                break;
            case 11:
                qTE = new QTEOverheating(duration * 2, randomDropperType, 1.2f + intensity_0_1 * 0.8f);
                break;
            case 12:
                qTE = new QTEBrokenBelts(duration * 2, 0.7f - intensity_0_1 * 0.5f);
                break;
            case 13:
                qTE = new QTEMarketBoost(duration * 2, 0.5f + intensity_0_1 * 1.5f);
                break;
            case 14:
                qTE = new QTEMarketCrash(duration * 2, 0.2f + intensity_0_1 * 0.8f);
                break;
            case 15:
                qTE = new QTEQualityBelts(duration * 1.5f, (1 + intensity_0_1 * 4f) / 100);
                break;
            case 16:
                qTE = new QTEDestructiveBelts(duration * 1.5f, (-1 - intensity_0_1 * 0.4f) / 100);
                break;
            case 17:
                qTE = new QTEMissionBuff(duration * 0.5f * duration * intensity_0_1, randomDropperType);
                break;
            case 18:
                qTE = new QTEMissionImpossible(duration + duration * intensity_0_1 * 1.5f);
                break;
            case 19:
                qTE = new QTEMaintenanceBoost(duration, 0.1f + intensity_0_1 * 1.9f);
                break;
            case 20:
                qTE = new QTELockedFactory(duration * 2.5f, randomType);
                break;
            case 21:
                qTE = new QTEInterestCharges(duration / 10, (1 + intensity_0_1 * 3) / 100);
                break;
            case 22:
                qTE = new QTEInvertedMarket(duration * 0.5f + duration * intensity_0_1);
                break;
            case 23:
                qTE = new QTEUltimateProduction(duration * 0.5f + duration * intensity_0_1);
                break;
            case 24:
                qTE = new QTEUltimateWipeout(duration, 0.0025f + intensity_0_1 * 0.0075f);
                break;
            default:
                print("Unknown qteID:" + qteID);
                return null;
        }
        qTE.intensity = intensity_0_1;
        int initiationTimeType = Random.Range(0, 4);
        float initiationTime = 150;
        switch (initiationTimeType)
        {
            case 0:
                initiationTime = 30;
                break;
            case 1:
                initiationTime = 60;
                break;
            case 2:
                initiationTime = 90;
                break;
            case 3:
                initiationTime = 120;
                break;
            default:
                print("Invalid state: " + initiationTimeType);
                return null;
        }
        var qTEInitiate = new QTEInitiate(initiationTime, qTE);
        return qTEInitiate;
    }

    public bool AllowQteStart(int qteType)
    {
        List<int> qteGroup = new List<int> { qteType };
        switch (qteType)
        {
            case 1:
                qteGroup.Add(6);
                break;
            case 2:
                qteGroup.Add(20);
                qteGroup.Add(24);
                break;
            case 3:
                qteGroup.Add(4);
                break;
            case 4:
                qteGroup.Add(3);
                break;
            case 5:
                qteGroup.Add(23);
                break;
            case 6:
                qteGroup.Add(1);
                break;
            case 7:
                qteGroup.Add(8);
                break;
            case 8:
                qteGroup.Add(7);
                break;
            case 9:
                qteGroup.Add(10);
                break;
            case 10:
                qteGroup.Add(9);
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                qteGroup.Add(14);
                break;
            case 14:
                qteGroup.Add(13);
                break;
            case 15:
                qteGroup.Add(16);
                break;
            case 16:
                qteGroup.Add(15);
                break;
            case 17:
                qteGroup.Add(18);
                break;
            case 18:
                qteGroup.Add(17);
                break;
            case 19:
                qteGroup.Add(21);
                break;
            case 20:
                qteGroup.Add(2);
                qteGroup.Add(24);
                break;
            case 21:
                qteGroup.Add(19);
                break;
            case 22:
                break;
            case 23:
                qteGroup.Add(5);
                break;
            case 24:
                qteGroup.Add(2);
                qteGroup.Add(20);
                break;
        }
        for (int i = 0; i < currentEvents.Count; i++)
        {
            for (int x = 0; x < qteGroup.Count; x++)
            {
                if (currentEvents[i].qteID == qteGroup[x])
                {
                    return false;
                }
                var following = currentEvents[i].getFollowing();
                while (following != null)
                {
                    if (following.qteID == qteGroup[x])
                    {
                        return false;
                    }
                    following = following.getFollowing();
                }
            }
        }
        return true;
    }
}

public abstract class QuickTimeEvent
{
    public int qteID;
    public float intensity;
    public GameObject display;
    public Texture displayImage;
    public float duration { get; set; }
    public float startTime { get; set; }
    public QuickTimeEvent(float duration, Texture displayImage)
    {
        this.startTime = duration;
        this.duration = duration;
        this.displayImage = displayImage;
    }
    virtual public void update()
    {
        return;
    }
    virtual public void start()
    {
        generateDisplay();
    }
    virtual public QuickTimeEvent end()
    {
        return null;
    }
    virtual public void setFollowing(QuickTimeEvent following)
    {
        return;
    }
    virtual public QuickTimeEvent getFollowing()
    {
        return null;
    }
    virtual public float getDuration()
    {
        return float.PositiveInfinity;
    }
    virtual public float getStartTime()
    {
        return float.PositiveInfinity;
    }
    virtual public string getDescription()
    {
        return "Lorem Ipsum";
    }
    virtual public string getShortDescription()
    {
        return "Lorem Ipsum";
    }
    abstract public bool isPositiveEvent();
    public GameObject getDisplay()
    {
        return display;
    }
    public Texture getDisplayImage()
    {
        return displayImage;
    }
    public void generateDisplay()
    {
        if (display == null)
        {
            display = Object.Instantiate(Manager.m.quickTimeEventManager.generalDisplay);
            display.GetComponent<QuickTimeEventDisplay>().displaying = this;
            display.transform.SetParent(Manager.m.quickTimeEventManager.displayFolder.transform);
        }
    }
    virtual public void continueQTE()
    {
        return;
    }
}