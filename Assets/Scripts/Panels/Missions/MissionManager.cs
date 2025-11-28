using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameObject mission;
    public GameObject missionWindow;
    public GameObject exitButton;
    public List<GameObject> missions;
    public Vector3[] positions;
    public GameObject noMissions;
    public GameObject missionFolder;
    public bool containsFinishedMission;
    public int declinedMission;
    float currenttime;
    //public ObjectGlow objectGlow;

    void Start()
    {
        exitButton.GetComponent<Button>().onClick.AddListener(Exit);
        missions = new List<GameObject>();
        currenttime = Time.time;
        declinedMission = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor - 0.1f, Manager.m.graphicManager.gUIScaleFactor - 0.1f, Manager.m.graphicManager.gUIScaleFactor - 0.1f);
        if (Manager.m.inMissions == true)
        {
            missionWindow.SetActive(true);
        }
        else
        {
            missionWindow.SetActive(false);
        }
        containsFinishedMission = false;
        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].GetComponent<Mission>().finishedMission == true)
            {
                containsFinishedMission = true;
            }
            if (Manager.m.inMissions == true)
            {
                missions[i].transform.localPosition = positions[i];
            }
            else
            {
                missions[i].transform.localPosition = new Vector3(0, 1080, 0);
            }
            float screenSizeDiffrence = Manager.m.canvas.GetComponent<RectTransform>().rect.width / Manager.m.canvas.GetComponent<CanvasScaler>().referenceResolution.x;
            missions[i].transform.localScale = new Vector3(screenSizeDiffrence * 0.6f, screenSizeDiffrence * 0.6f, 1);
            if (Manager.m.paused == false)
            {
                if (missions[i].GetComponent<Mission>().declinedMission == true || missions[i].GetComponent<Mission>().canceldMission == true || missions[i].GetComponent<Mission>().time <= 0)
                {
                    GameObject mission = missions[i];
                    missions.Remove(missions[i]);
                    Destroy(mission);
                }
                else if (missions[i].GetComponent<Mission>().deliveredMission)
                {
                    Manager.m.money += missions[i].GetComponent<Mission>().reward;
                    Manager.m.incomeLastSecond += missions[i].GetComponent<Mission>().reward;
                    GameObject mission = missions[i];
                    missions.Remove(missions[i]);
                    Destroy(mission);
                }
            }
        }
        if (missions.Count == 0)
        {
            noMissions.SetActive(true);
        }
        else
        {
            noMissions.SetActive(false);
        }

        if (currenttime < Time.time)
        {
            currenttime = Time.time + 1;

            if (declinedMission > 0)
            {
                declinedMission -= 1;
            }
            else
            {
                declinedMission = 0;
            }

            //Creating random missions
            if (missions.Count == 0)
            {
                if (Manager.m.tutorial.inTutorial == 0)
                {
                    if (declinedMission != 0)
                    {
                        if (UnityEngine.Random.Range(0, 60) < 1)
                        {
                            createMission();
                        }
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(0, 10) < 1)
                        {
                            createMission();
                        }
                    }
                }
            }
            else
            if (missions.Count == 1)
            {
                if ((UnityEngine.Random.Range(0, 60) < 1 && Manager.m.tutorial.inTutorial == 0))
                {
                    createMission();
                }
            }
            if (missions.Count == 2)
            {
                if ((UnityEngine.Random.Range(0, 120) < 1 && Manager.m.tutorial.inTutorial == 0))
                {
                    createMission();
                }
            }
            if (missions.Count >= 3)
            {
                if ((UnityEngine.Random.Range(0, 180) < 1 && Manager.m.tutorial.inTutorial == 0))
                {
                    createMission();
                }
            }
        }
    }
    void Exit()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inMissions = false;
    }
    public void AddMission(int oreNumber, int upgradeLevel, int quantity, int seconds, double reward)
    {
        if (Manager.m.acessMissions)
        {
            if (missions.Count < positions.Length)
            {
                GameObject m = Instantiate(mission);
                m.GetComponent<Mission>().quantity = quantity;
                m.GetComponent<Mission>().upgradeLevel = upgradeLevel;
                m.GetComponent<Mission>().reward = reward;
                m.GetComponent<Mission>().oreNumber = oreNumber;
                m.GetComponent<Mission>().time = seconds;
                m.GetComponent<Mission>().sold = 0;
                m.GetComponent<Mission>().acceptedMission = false;
                m.GetComponent<Mission>().canceldMission = false;
                m.GetComponent<Mission>().finishedMission = false;
                m.GetComponent<Mission>().deliveredMission = false;
                m.GetComponent<Mission>().acceptMission.gameObject.SetActive(true);
                m.GetComponent<Mission>().declineMission.gameObject.SetActive(true);
                m.GetComponent<Mission>().cancelMission.gameObject.SetActive(false);
                m.GetComponent<Mission>().deliverMission.gameObject.SetActive(false);
                m.transform.SetParent(Manager.m.canvas.transform);
                m.transform.localPosition = new Vector3(0, 600, 0);
                m.transform.SetParent(missionFolder.transform);
                missions.Add(m);
                Manager.m.notificationManager.AddNotification("!Info!\nther is a new Order for\n" + quantity + " * " + Manager.m.oreIdentifications[oreNumber], Manager.m.eventImages[25]);
            }
        }
    }
    public void LoadMission(int oreNumber, int upgradeLevel, int quantity, int seconds, double reward, float sold, bool accepted)
    {
        if (missions.Count < positions.Length)
        {
            GameObject m = Instantiate(mission);
            m.GetComponent<Mission>().quantity = quantity;
            m.GetComponent<Mission>().upgradeLevel = upgradeLevel;
            m.GetComponent<Mission>().reward = reward;
            m.GetComponent<Mission>().oreNumber = oreNumber;
            m.GetComponent<Mission>().time = seconds;
            m.GetComponent<Mission>().sold = sold;
            if (accepted)
            {
                m.GetComponent<Mission>().acceptedMission = true;
                m.GetComponent<Mission>().acceptMission.gameObject.SetActive(false);
                m.GetComponent<Mission>().declineMission.gameObject.SetActive(false);
                m.GetComponent<Mission>().cancelMission.gameObject.SetActive(true);
                m.GetComponent<Mission>().deliverMission.gameObject.SetActive(false);
            }
            else
            {
                m.GetComponent<Mission>().acceptMission.gameObject.SetActive(true);
                m.GetComponent<Mission>().declineMission.gameObject.SetActive(true);
                m.GetComponent<Mission>().cancelMission.gameObject.SetActive(false);
                m.GetComponent<Mission>().deliverMission.gameObject.SetActive(false);
            }
            m.transform.SetParent(missionFolder.transform);
            m.transform.localPosition = new Vector3(0, 600, 0);
            missions.Add(m);
        }
    }

    public void createMission()
    {
        System.Random rand = new System.Random();
        int missionOreMax = Manager.m.getHighestUnlockedType();
        int missionOre = missionOreMax;
        float changeProbability = 55;
        for (int i = 0; i < missionOreMax; i++)
        {
            if (UnityEngine.Random.Range(0, 100) > changeProbability)
            {
                break;
            }
            else
            {
                changeProbability = changeProbability * 0.7f;
                missionOre--;
            }
        }
        if (missionOre < 0)
            missionOre = 0;
        else if (missionOre > 9)
            missionOre = 9;

        int unlockedUpgrades = (int)Mathf.Floor((missionOreMax + 1) / 3);
        int missionUpgrade = rand.Next(0, unlockedUpgrades + 1);
        if (missionUpgrade > 3)
        {
            missionUpgrade = 3;
        }
        else if (missionUpgrade < 0)
        {
            missionUpgrade = 0;
        }

        float timeDifficulty = UnityEngine.Random.Range(0.15f, 1.2f);
        float missionTimePerOre = timeDifficulty * Manager.m.droppers[missionOre].GetComponent<RepairDropper>().dropSpeed;

        int missionQuantity = (int)UnityEngine.Random.Range(0, 200 * (1 / Manager.m.droppers[missionOre].GetComponent<RepairDropper>().dropSpeed)) + (int)(100 / Manager.m.droppers[missionOre].GetComponent<RepairDropper>().dropSpeed * timeDifficulty);
        if (missionQuantity < 1000)
        {
            missionQuantity = (int)Mathf.Round((float)missionQuantity / 10) * 10;
        }
        else if (missionQuantity >= 1000)
        {
            missionQuantity = (int)Mathf.Round((float)missionQuantity / 100) * 100;
        }
        int missionTime = (int)Mathf.Round(missionQuantity * missionTimePerOre);
        if (missionTime > 600)
        {
            missionTime = 600;
        }
        string identification = Manager.m.droppers[missionOre].gameObject.name;
        identification = identification.Replace("Dropper", "");
        identification = identification.Replace("(Clone)", "");
        double MissionReward = (Manager.m.droppers[missionOre].GetComponent<RepairDropper>().oreValue * missionQuantity * (1 + Manager.m.upgradeMultipliers[missionUpgrade])) / timeDifficulty;

        AddMission(missionOre, missionUpgrade, missionQuantity, missionTime, MissionReward);
    }

    public void createMission(int missionOre, int missionUpgrade, int missionQuantity, int missionTime, double missionReward)
    {
        AddMission(missionOre, missionUpgrade, missionQuantity, missionTime, missionReward);
    }
}
