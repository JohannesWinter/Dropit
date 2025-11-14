using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameSave : MonoBehaviour
{
    public bool isAutoSave;

    public GameObject saveButton1;
    public GameObject saveButton2;
    public GameObject loadButton1;
    public GameObject clearButton1;
    public Ask ask;

    public int saveNumber;
    public GameObject saveText;

    public GameObject ui;

    bool askedSave;
    bool askedLoad;
    bool askedClear;
    bool askedReload;

    public bool save;
    public bool load;

    public float age = 0;
    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        saveButton1.gameObject.GetComponent<Button>().onClick.AddListener(Save);
        saveButton2.gameObject.GetComponent<Button>().onClick.AddListener(Save);
        loadButton1.gameObject.GetComponent<Button>().onClick.AddListener(Load);
        clearButton1.gameObject.GetComponent<Button>().onClick.AddListener(Clear);
        if (isAutoSave)
        {
            age = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Age");
            saveText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString(Manager.m.version + "_" + "Save" + saveNumber + "_Text");
        }
    }

    // Update is called onc e per frame
    void Update()
    {
        if (this.saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
        {
            age = 0;
        }
        if (counter >= 30)
        {
            counter = 0;
            if (isAutoSave == false && Manager.m.introScene == false)
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_savePosition", Manager.m.saveGameManager.saveGames.IndexOf(this.gameObject));
            }
        }
        counter++;
        if (saveText.GetComponent<TextMeshProUGUI>().text == "")
        {
            saveText.GetComponent<TextMeshProUGUI>().text = "Empty";
        }

        if (Manager.m.inSettings == false && Manager.m.inMainMenuLoad == false)
        {
            SetUiActive(false);
        }

        if (saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
        {
            saveButton1.SetActive(false);
            saveButton2.SetActive(false);
            loadButton1.SetActive(false);
            clearButton1.SetActive(false);
        }
        else
        {
            if (Manager.m.inOptionSave == true)
            {
                saveButton1.SetActive(true);
                saveButton2.SetActive(true);

                loadButton1.SetActive(false);

                clearButton1.SetActive(true);
            }
            else if (Manager.m.inOptionLoad == true || Manager.m.inMainMenuLoad == true)
            {
                saveButton1.SetActive(false);
                saveButton2.SetActive(false);

                loadButton1.SetActive(true);

                clearButton1.SetActive(true);
            }
        }

        if ((askedSave == true && ask.antwort == 2) || save == true)
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;
            Manager.m.changeSaveTimer = 50;
            StartCoroutine(SaveGame());
        }
        else if ((askedSave == true && ask.antwort == 1))
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;
        }

        if ((askedLoad == true && saveText.GetComponent<TextMeshProUGUI>().text != "Empty" && ask.antwort == 2) || (load == true && saveText.GetComponent<TextMeshProUGUI>().text != "Empty") || (askedReload == true && ask.antwort == 2))
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;
            Manager.m.changeSaveTimer = 50;
            StartCoroutine(LoadGame());
        }
        else if ((askedLoad == true && ask.antwort == 1) || (askedReload == true && ask.antwort == 1) || load == true)
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;
        }

        if (ask.antwort == 2 && askedClear == true)
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;

            PlayerPrefs.DeleteKey(Manager.m.version + "_" + "Save" + saveNumber + "_Text");
            saveText.GetComponent<TextMeshProUGUI>().text = "";
            age = 0;
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "LastSave") == saveNumber)
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "LastSave", 0);
                Manager.m.lastSaveNumber = 0;
            }
        }
        else if (ask.antwort == 1 && askedClear == true)
        {
            ask.antwort = 0;
            askedSave = false;
            save = false;
            askedLoad = false;
            load = false;
            askedClear = false;
            askedReload = false;
        }
    }

    void Save()
    {
        Manager.m.effectSpeaker.click();
        if (saveText.GetComponent<TextMeshProUGUI>().text != "Empty")
        {
            askedSave = true;
            ask.Asking("Orverwrite Save?");
        }
        else
        {
            save = true;
        }
    }
    void Load()
    {
        if (Manager.m.inMainMenu == false)
        {
            Manager.m.effectSpeaker.click();
            if (saveText.GetComponent<TextMeshProUGUI>().text != "Empty")
            {
                GameObject[] g = GameObject.FindGameObjectsWithTag("FactoryObject");
                if (Manager.m.money == Manager.m.startMoney && g.Length == 0)
                {
                    load = true;
                }
                else
                {
                    askedLoad = true;
                    ask.Asking("Delete current scene?");
                }
            }
        }
        else
        {
            load = true;
        }
    }

    public void SaveQTEForKey(string key, int saveID, int depth, QuickTimeEvent qTE)
    {
        string baseKey = key + saveID + "_" + depth; 
        PlayerPrefs.SetFloat(baseKey + "_QteID", qTE.qteID);
        PlayerPrefs.SetFloat(baseKey + "_duration", qTE.duration);
        PlayerPrefs.SetFloat(baseKey + "_startTime", qTE.startTime);
        switch (qTE)
        {
            case QTEOverclock qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_clocking", qteType.clocking);
                    PlayerPrefs.SetInt(baseKey + "_dropperType", qteType.dropperType);
                    break;
                }
            case QTEBrokenLights qteType:
                {
                    break;
                }
            case QTECheapMiners qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    PlayerPrefs.SetInt(baseKey + "_dropperType", qteType.dropperType);
                    break;
                }
            case QTEExpensiveMiners qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    PlayerPrefs.SetInt(baseKey + "_dropperType", qteType.dropperType);
                    break;
                }
            case QTEEfficiency qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_efficiency", qteType.efficiency);
                    break;
                }
            case QTEUnderclock qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_clocking", qteType.clocking);
                    PlayerPrefs.SetInt(baseKey + "_dropperType", qteType.dropperType);
                    break;
                }
            case QTECheapMachines qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    PlayerPrefs.SetInt(baseKey + "_machineType", qteType.machineType);
                    break;
                }
            case QTEExpensiveMachines qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    PlayerPrefs.SetInt(baseKey + "_machineType", qteType.machineType);
                    break;
                }
            case QTECheapRepairs qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEExpensiveRepairs qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEOverheating qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    PlayerPrefs.SetInt(baseKey + "_ressourceType", qteType.ressourceType);
                    break;
                }
            case QTEBrokenBelts qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEMarketBoost qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_boost", qteType.boost);
                    break;
                }
            case QTEMarketCrash qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_crash", qteType.crash);
                    break;
                }
            case QTEQualityBelts qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEDestructiveBelts qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEMissionBuff qteType:
                {
                    PlayerPrefs.SetInt(baseKey + "_dropperType", qteType.dropperType);
                    break;
                }
            case QTEMissionImpossible qteType:
                {
                    break;
                }
            case QTEMaintenanceBoost qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTELockedFactory qteType:
                {
                    PlayerPrefs.SetInt(baseKey + "_factoryHall", qteType.factoryHall);
                    break;
                }
            case QTEInterestCharges qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEInvertedMarket qteType:
                {
                    break;
                }
            case QTEUltimateProduction qteType:
                {
                    break;
                }
            case QTEUltimateWipeout qteType:
                {
                    PlayerPrefs.SetFloat(baseKey + "_currenttime", qteType.currenttime);
                    PlayerPrefs.SetFloat(baseKey + "_percentage", qteType.percentage);
                    break;
                }
            case QTEInitiate qteType:
                {
                    break;
                }
        }
        if (qTE.getFollowing() != null)
        {
            PlayerPrefs.SetInt(baseKey + "_hasFollowing", 1);
            SaveQTEForKey(key, saveID, depth + 1, qTE.getFollowing());
        }
        else
        {
            PlayerPrefs.SetInt(baseKey + "_hadFollowing", 0);
        }
    }
    public void LoadQTEForKey(string key, int saveID, int depth, List<QuickTimeEvent> qTEList)
    {
        string baseKey = key + saveID + "_" + depth;
        float qteID = PlayerPrefs.GetFloat(baseKey + "_QteID");

        QuickTimeEvent qTE = null;

        switch (qteID)
        {
            case 0: // QTEInitiate
                qTE = new QTEInitiate(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    null
                );
                break;

            case 1: // QTEOverclock
                qTE = new QTEOverclock(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    clocking: PlayerPrefs.GetFloat(baseKey + "_clocking"),
                    dropperType: PlayerPrefs.GetInt(baseKey + "_dropperType")
                );
                break;

            case 2: // QTEBrokenLights
                qTE = new QTEBrokenLights(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration")
                );
                break;

            case 3: // QTECheapMiners
                qTE = new QTECheapMiners(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage"),
                    dropperType: PlayerPrefs.GetInt(baseKey + "_dropperType")
                );
                break;

            case 4: // QTEExpensiveMiners
                qTE = new QTEExpensiveMiners(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage"),
                    dropperType: PlayerPrefs.GetInt(baseKey + "_dropperType")
                );
                break;

            case 5: // QTEEfficiency
                qTE = new QTEEfficiency(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    efficiency: PlayerPrefs.GetFloat(baseKey + "_efficiency")
                );
                break;

            case 6: // QTEUnderclock
                qTE = new QTEUnderclock(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    clocking: PlayerPrefs.GetFloat(baseKey + "_clocking"),
                    dropperType: PlayerPrefs.GetInt(baseKey + "_dropperType")
                );
                break;

            case 7: // QTECheapMachines
                qTE = new QTECheapMachines(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage"),
                    machineType: PlayerPrefs.GetInt(baseKey + "_machineType")
                );
                break;

            case 8: // QTEExpensiveMachines
                qTE = new QTEExpensiveMachines(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage"),
                    machineType: PlayerPrefs.GetInt(baseKey + "_machineType")
                );
                break;

            case 9: // QTECheapRepairs
                qTE = new QTECheapRepairs(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 10: // QTEExpensiveRepairs
                qTE = new QTEExpensiveRepairs(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 11: // QTEOverheating
                qTE = new QTEOverheating(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage"),
                    ressourceType: PlayerPrefs.GetInt(baseKey + "_ressourceType")
                );
                break;

            case 12: // QTEBrokenBelts
                qTE = new QTEBrokenBelts(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 13: // QTEMarketBoost
                qTE = new QTEMarketBoost(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    boost: PlayerPrefs.GetFloat(baseKey + "_boost")
                );
                break;

            case 14: // QTEMarketCrash
                qTE = new QTEMarketCrash(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    crash: PlayerPrefs.GetFloat(baseKey + "_crash")
                );
                break;

            case 15: // QTEQualityBelts
                qTE = new QTEQualityBelts(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 16: // QTEDestructiveBelts
                qTE = new QTEDestructiveBelts(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 17: // QTEMissionBuff
                qTE = new QTEMissionBuff(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    dropperType: PlayerPrefs.GetInt(baseKey + "_dropperType")
                );
                break;

            case 18: // QTEMissionImpossible
                qTE = new QTEMissionImpossible(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration")
                );
                break;

            case 19: // QTEMaintenanceBoost
                qTE = new QTEMaintenanceBoost(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 20: // QTELockedFactory
                qTE = new QTELockedFactory(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    factoryHall: PlayerPrefs.GetInt(baseKey + "_factoryHall")
                );
                break;

            case 21: // QTEInterestCharges
                qTE = new QTEInterestCharges(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                break;

            case 22: // QTEInvertedMarket
                qTE = new QTEInvertedMarket(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration")
                );
                break;

            case 23: // QTEUltimateProduction
                qTE = new QTEUltimateProduction(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration")
                );
                break;

            case 24: // QTEUltimateWipeout
                qTE = new QTEUltimateWipeout(
                    duration: PlayerPrefs.GetFloat(baseKey + "_duration"),
                    percentage: PlayerPrefs.GetFloat(baseKey + "_percentage")
                );
                PlayerPrefs.GetFloat(baseKey + "_currenttime");
                break;

            default:
                Debug.LogWarning("Unknown QTE-Typ: " + PlayerPrefs.GetFloat(baseKey + "_QteID"));
                break;
        }
        if (qTE == null) return;

        qTE.startTime = PlayerPrefs.GetFloat(baseKey + "_startTime");

        if (depth == 0)
        {
            qTE.generateDisplay();
            qTE.continueQTE();
        }


        if (PlayerPrefs.GetInt(baseKey + "_hasFollowing", 0) == 1)
        {
            List<QuickTimeEvent> loadedFollowing = new List<QuickTimeEvent>();
            LoadQTEForKey(key, saveID, depth + 1, loadedFollowing);
            qTE.setFollowing(loadedFollowing[0]);
        }
        qTEList.Add(qTE);
    }

    void Clear()
    {
        Manager.m.effectSpeaker.click();
        if (saveText.GetComponent<TextMeshProUGUI>().text != "Empty")
        {
            askedClear = true;
            ask.Asking("Delete Save?");
        }
    }

    public bool SetUiActive()
    {
        return ui.activeSelf;
    }

    public void SetUiActive(bool active)
    {
        ui.SetActive(active);
    }

    public IEnumerator blackOutLoad()
    {
        if (saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
        {
            yield return null;
        }
        else
        {
            load = true;
        }
    }

    public IEnumerator SaveGame()
    {
        Manager.m.loading = true;
        LoadingScreen loadingScreen = Manager.m.saveGameManager.loadingScreen;
        loadingScreen.setStatus(0, 0, "Lorem", 0);
        Manager.m.musicSpeaker.StopMusic(9999);
        for (int i = 1; i <= 20; i++)
        {
            loadingScreen.setStatus(0, i * 0.05f, "Lorem", 0);
            yield return new WaitForSecondsRealtime(0.05f);

        }
        loadingScreen.setStatus(0, 1, "Lorem", 0);
        int loadingSteps = 100;
        float progress = 1;
        float percentPerStep = 100f / loadingSteps;

        //setup saveDataJSON
        SaveData saveData = new SaveData();
        saveData.managerVersion = Manager.m.version;
        saveData.saveNumber = saveNumber;

        //save auto repairing
        for (int i = 0; i < Manager.m.autoRepairDroppers.Length; i++)
        {
            if (Manager.m.autoRepairDroppers[i] == true)
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairDropper" + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairDropper" + i, 0);
            }
        }
        for (int i = 0; i < Manager.m.autoRepairMachines.Length; i++)
        {
            if (Manager.m.autoRepairMachines[i] == true)
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairMachines" + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairMachines" + i, 0);
            }
        }
        progress += 2;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save unlocked halls
        for (int i = 0; i < Manager.m.hallUpgrader.Length; i++)
        {
            for (int y = 1; y < Manager.m.hallUpgrader[i].inputRessources.Count; y++)
            {
                PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_hallUpgrader" + i + "_Drop" + y, Manager.m.hallUpgrader[i].inputRessources[y]);
            }
        }
        progress += 1.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        int x = -1;
        for (int i = 0; i < Manager.m.upgradeRessources.Length; i++)
        {
            if (Manager.m.upgradeRessources[i] == true)
            {
                x = i;
            }
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_upgradeRessources", x);
        progress += 1.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save market state
        for (int i = 0; i < Manager.m.dropValueMultipliers.Length; i++)
        {
            for (int y = 0; y < Manager.m.marketDrops.Length; y++)
            {
                PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Drop" + (i + 1) + "_UpgradeLevel" + i + "_ValueMult", Manager.m.dropValueMultipliers[i][y]);
            }
            loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 5 * ((float)i / Manager.m.marketDrops.Length));
            yield return new WaitForEndOfFrame();
        }
        progress += 5;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save missions
        for (int i = 0; i < Manager.m.missionManager.missions.Count; i++)
        {
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_OreNumber", Manager.m.missionManager.missions[i].GetComponent<Mission>().oreNumber);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_UpgradeLevel", Manager.m.missionManager.missions[i].GetComponent<Mission>().upgradeLevel);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Quantity", Manager.m.missionManager.missions[i].GetComponent<Mission>().quantity);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Time", Manager.m.missionManager.missions[i].GetComponent<Mission>().time);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Reward", (float)Manager.m.missionManager.missions[i].GetComponent<Mission>().reward);

            int accepted = 0;
            if (Manager.m.missionManager.missions[i].GetComponent<Mission>().acceptedMission == true) { accepted = 1; }
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Accepted", accepted);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Progress", Manager.m.missionManager.missions[i].GetComponent<Mission>().sold);

            loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 5 * ((float)i / Manager.m.missionManager.missions.Count));
            yield return new WaitForEndOfFrame();
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_mission_Count", Manager.m.missionManager.missions.Count);
        progress += 5;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save finished sequences
        if (Manager.m.tutorial.finishedTutorial2 == true)
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial2", 1);
        }
        else
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial2", 0);
        }
        if (Manager.m.tutorial.finishedTutorial3 == true)
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial3", 1);
        }
        else
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial3", 0);
        }
        if (Manager.m.finishedFinalSequence == true)
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FinishedFinalSequence", 1);
        }
        else
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FinishedFinalSequence", 0);
        }
        progress += 1;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save gnomes
        var gnomeList = Manager.m.gnomeManager.gnomeList;
        for (int i = 0; i < gnomeList.Count; i++)
        {
            var gnome = gnomeList[i];
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_PosX", gnome.transform.position.x);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_PosY", gnome.transform.position.y);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_PosZ", gnome.transform.position.z);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_RotX", gnome.transform.rotation.x);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_RotY", gnome.transform.rotation.y);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_RotZ", gnome.transform.rotation.z);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_ScaleX", gnome.transform.localScale.x);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_ScaleY", gnome.transform.localScale.y);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_ScaleZ", gnome.transform.localScale.z);

            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_AttackDamage", gnome.attackDamage);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_BaseMovementSpeed", gnome.baseMovementSpeed);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_BaseRotationSpeed", gnome.baseRotationSpeed);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_AimX", gnome.aimX);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_AimZ", gnome.aimZ);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_IdleWaitTimer", gnome.idleWaitTimer);
            PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_DiscoveryDuration", gnome.discoveryDuration);
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_AttackMode", gnome.attackMode);
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_AttackedHall", gnome.attackedHall);
            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome" + i + "_Fleeing", gnome.fleeing ? 1 : 0);

            gnome.transform.SetParent(Manager.m.gnomeManager.gnomeFolder);

            loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 9 * ((float)i / gnomeList.Count));
            yield return new WaitForEndOfFrame();
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome_Count", gnomeList.Count);
        progress += 9;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);

        //save quick time events
        var quickTimeEvents = Manager.m.quickTimeEventManager.currentEvents;
        for (int i = 0; i < quickTimeEvents.Count; i++)
        {
            var qTE = quickTimeEvents[i];
            SaveQTEForKey(Manager.m.version + "_" + "Save" + saveNumber + "_QTE", i, 0, qTE);
            loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 5 * ((float)i / quickTimeEvents.Count));
            yield return new WaitForEndOfFrame();
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_QTE_Count", quickTimeEvents.Count);
        progress += 5;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);

        //save enviroment stats
        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_DarknessControllerState", Manager.m.darknessController.changing);
        progress += 0.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //save general stats
        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Money", (float)Manager.m.money);
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Level", Manager.m.level);
        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Exp", (float)Manager.m.exp);
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Playtime", Manager.m.playTime);
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FactoryCamera", Array.IndexOf(Manager.m.factoryCameras, Manager.m.lastDropperCamera));
        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_DeclinedMission", Manager.m.declinedMission);
        progress += 1.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        ///save factory Objects
        //float startTime = Time.realtimeSinceStartup;
        //GameObject[] AllFactoryObjects = GameObject.FindGameObjectsWithTag("FactoryObject");
        //for (int i = 0; i < AllFactoryObjects.Length; i++)
        //{
        //    float startTimeObject = Time.realtimeSinceStartup;
        //    bool replaced = false;

        //    if (AllFactoryObjects[i].name.Contains("(Clone)"))
        //    {
        //        AllFactoryObjects[i].name = AllFactoryObjects[i].name.Replace("(Clone)", "");
        //        replaced = true;
        //    }

        //    PlayerPrefs.SetString(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i, AllFactoryObjects[i].name);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_x", AllFactoryObjects[i].transform.position.x);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_y", AllFactoryObjects[i].transform.position.y);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_z", AllFactoryObjects[i].transform.position.z);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_x", AllFactoryObjects[i].transform.rotation.eulerAngles.x);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_y", AllFactoryObjects[i].transform.rotation.eulerAngles.y);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_z", AllFactoryObjects[i].transform.rotation.eulerAngles.z);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_x", AllFactoryObjects[i].transform.localScale.x);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_y", AllFactoryObjects[i].transform.localScale.y);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_z", AllFactoryObjects[i].transform.localScale.z);
        //    PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_durability", (float)AllFactoryObjects[i].GetComponent<RepairDropper>().durability);
        //    PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_id", AllFactoryObjects[i].GetComponent<RepairDropper>().id);

        //    if (AllFactoryObjects[i].GetComponent<RepairDropper>().dropperNumber != 0)
        //    {
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_dropTime", AllFactoryObjects[i].GetComponentInChildren<Drop>().currenttime);
        //    }
        //    if (AllFactoryObjects[i].GetComponent<RepairDropper>().isScrap == true)
        //    {
        //        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_isScrap", 1);
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_isScrap", 0);
        //    }
        //    for (int y = 0; y < AllFactoryObjects[i].GetComponent<RepairDropper>().inputOres.Length; y++)
        //    {
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_inputOre" + y, AllFactoryObjects[i].GetComponent<RepairDropper>().inputOres[y]);
        //    }
        //    if (replaced == true)
        //    {
        //        AllFactoryObjects[i].name = AllFactoryObjects[i].name + "(Clone)";
        //    }
        //    loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 26 * ((float)i / AllFactoryObjects.Length));
        //    print("Save " + i + " on: " + (Time.realtimeSinceStartup - startTimeObject) + "sec");
        //    yield return new WaitForEndOfFrame();
        //    print("Save " + i + " in: " + (Time.realtimeSinceStartup - startTimeObject) + "sec");

        //}
        //PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_CountObjects", AllFactoryObjects.Length);
        //progress += 26;
        //loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        //print("FactoryObjectSaveTime: " + (Time.realtimeSinceStartup - startTime) + "sec");

        ////save dropped Ores
        //GameObject[] AllOres = GameObject.FindGameObjectsWithTag("Ore");
        //for (int i = 0; i < AllOres.Length; i++)
        //{
        //    try
        //    {
        //        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_Number", AllOres[i].GetComponent<Ore>().oreNumber);
        //        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_UpgradeLvl", AllOres[i].GetComponent<Ore>().upgradeLevel);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_value", (float)AllOres[i].GetComponent<Ore>().value);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_baseValue", (float)AllOres[i].GetComponent<Ore>().baseValue);

        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_x", AllOres[i].transform.position.x);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_y", AllOres[i].transform.position.y);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_z", AllOres[i].transform.position.z);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_x", AllOres[i].transform.rotation.x);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_y", AllOres[i].transform.rotation.y);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_z", AllOres[i].transform.rotation.z);
        //        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_w", AllOres[i].transform.rotation.w);

        //        var visitedBelts = AllOres[i].GetComponent<Ore>().visitedBelts;
        //        PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_visitedBeltsCount", visitedBelts.Count);
        //        for (int y = 0; y < visitedBelts.Count; y++)
        //        {
        //            PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_visitedBelts" + y, visitedBelts[y]);
        //        }
        //    }
        //    catch
        //    {
        //        print("Error with ore: " + AllOres[i].name + ", " + AllOres[i].transform.position);
        //    }
        //    loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 40 * ((float)i / AllFactoryObjects.Length));
        //    yield return new WaitForEndOfFrame();
        //}
        //PlayerPrefs.SetInt(Manager.m.version + "_" + "Save" + saveNumber + "_CountOres", AllOres.Length);
        //progress += 40;
        //loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);


        // save factory Objects
        float timeFrame = Time.realtimeSinceStartup;
        GameObject[] AllFactoryObjects = GameObject.FindGameObjectsWithTag("FactoryObject");
        saveData.fob_ids = new float[AllFactoryObjects.Length];
        saveData.fob_names = new string[AllFactoryObjects.Length];
        saveData.fob_durabilities = new float[AllFactoryObjects.Length];
        saveData.fob_dropTimers = new float[AllFactoryObjects.Length];
        saveData.fob_dropTimers = new float[AllFactoryObjects.Length];
        saveData.fob_areScraps = new bool[AllFactoryObjects.Length];
        saveData.fob_inputOres_serialized = new string[AllFactoryObjects.Length];
        saveData.fob_xPos = new float[AllFactoryObjects.Length];
        saveData.fob_yPos = new float[AllFactoryObjects.Length];
        saveData.fob_zPos = new float[AllFactoryObjects.Length];
        saveData.fob_xRot = new float[AllFactoryObjects.Length];
        saveData.fob_yRot = new float[AllFactoryObjects.Length];
        saveData.fob_zRot = new float[AllFactoryObjects.Length];
        saveData.fob_xScale = new float[AllFactoryObjects.Length];
        saveData.fob_yScale = new float[AllFactoryObjects.Length];
        saveData.fob_zScale = new float[AllFactoryObjects.Length];

        for (int i = 0; i < AllFactoryObjects.Length; i++)
        {
            bool replaced = false;

            if (AllFactoryObjects[i].name.Contains("(Clone)"))
            {
                AllFactoryObjects[i].name = AllFactoryObjects[i].name.Replace("(Clone)", "");
                replaced = true;
            }

            GameObject obj = AllFactoryObjects[i];
            RepairDropper dropper = obj.GetComponent<RepairDropper>();

            saveData.fob_ids[i] = dropper.id;
            saveData.fob_names[i] = obj.name;
            saveData.fob_durabilities[i] = (float)dropper.durability;
            saveData.fob_dropTimers[i] = (dropper.dropperNumber != 0 && obj.GetComponentInChildren<Drop>())
                                            ? obj.GetComponentInChildren<Drop>().currenttime
                                            : 0f;
            saveData.fob_areScraps[i] = dropper.isScrap;

            saveData.fob_xPos[i] = obj.transform.position.x;
            saveData.fob_yPos[i] = obj.transform.position.y;
            saveData.fob_zPos[i] = obj.transform.position.z;

            saveData.fob_xRot[i] = obj.transform.rotation.eulerAngles.x;
            saveData.fob_yRot[i] = obj.transform.rotation.eulerAngles.y;
            saveData.fob_zRot[i] = obj.transform.rotation.eulerAngles.z;

            saveData.fob_xScale[i] = obj.transform.localScale.x;
            saveData.fob_yScale[i] = obj.transform.localScale.y;
            saveData.fob_zScale[i] = obj.transform.localScale.z;

            string inputOresRow = string.Join("|", dropper.inputOres);
            saveData.fob_inputOres_serialized[i] = inputOresRow;

            if (replaced == true)
            {
                AllFactoryObjects[i].name = AllFactoryObjects[i].name + "(Clone)";
            }
            if (Time.realtimeSinceStartup - timeFrame > 0.01f)
            {
                timeFrame = Time.realtimeSinceStartup;
                loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 26 * ((float)i / AllFactoryObjects.Length));
                yield return new WaitForEndOfFrame();
            }

        }
        progress += 26;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);

        //save dropped Ores
        timeFrame = Time.realtimeSinceStartup;
        GameObject[] AllOres = GameObject.FindGameObjectsWithTag("Ore");
        saveData.ore_numbers = new int[AllOres.Length];
        saveData.ore_upgradeLevers = new int[AllOres.Length];
        saveData.ore_values = new float[AllOres.Length];
        saveData.ore_baseValues = new float[AllOres.Length];
        saveData.ore_xPos = new float[AllOres.Length];
        saveData.ore_yPos = new float[AllOres.Length];
        saveData.ore_zPos = new float[AllOres.Length];
        saveData.ore_xRot = new float[AllOres.Length];
        saveData.ore_yRot = new float[AllOres.Length];
        saveData.ore_zRot = new float[AllOres.Length];
        saveData.ore_visitedBeltsLists_serialized = new string[AllOres.Length];

        for (int i = 0; i < AllOres.Length; i++)
        {
            try
            {
                GameObject oreObj = AllOres[i];
                Ore oreComp = oreObj.GetComponent<Ore>();

                saveData.ore_numbers[i] = oreComp.oreNumber;
                saveData.ore_upgradeLevers[i] = oreComp.upgradeLevel;
                saveData.ore_values[i] = (float)oreComp.value;
                saveData.ore_baseValues[i] = (float)oreComp.baseValue;

                saveData.ore_xPos[i] = oreObj.transform.position.x;
                saveData.ore_yPos[i] = oreObj.transform.position.y;
                saveData.ore_zPos[i] = oreObj.transform.position.z;

                saveData.ore_xRot[i] = oreObj.transform.rotation.eulerAngles.x;
                saveData.ore_yRot[i] = oreObj.transform.rotation.eulerAngles.y;
                saveData.ore_zRot[i] = oreObj.transform.rotation.eulerAngles.z;

                //saveData.ore_veloc[i] = oreObj.GetComponent<Rigidbody>().velocity;
                //saveData.ore_trc[i] = oreObj.GetComponent<Rigidbody>().angularVelocity;

                var visitedBelts = oreComp.visitedBelts;
                saveData.ore_visitedBeltsLists_serialized[i] = string.Join("|", visitedBelts);
            }
            catch
            {
                print("Error with ore: " + AllOres[i].name + ", " + AllOres[i].transform.position);
            }
            if (Time.realtimeSinceStartup - timeFrame > 0.01f)
            {
                timeFrame = Time.realtimeSinceStartup;
                loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 40 * ((float)i / AllOres.Length));
                yield return new WaitForEndOfFrame();
            }
        }
        progress += 40;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);


        //save Savegame-Info
        float allTime = Manager.m.playTime;
        float playDays = allTime / (3600 * 24);
        playDays = Mathf.Floor(playDays);
        allTime -= playDays * (3600 * 24);
        float playHours = allTime / 3600;
        playHours = MathF.Floor(playHours);
        allTime -= playHours * 3600;
        float playMinuts = allTime / 60;
        playMinuts = MathF.Round(playMinuts);

        string playDaysText;
        if (playDays > 0)
        {
            playDaysText = playDays + "d:";
        }
        else
        {
            playDaysText = "";
        }
        string playHoursText;
        playHoursText = playHours + "h:";
        string playMinutsText;
        playMinutsText = playMinuts + "m";
        string playTimeText = playDaysText + "" + playHoursText + "" + playMinutsText;

        string dateTimeMonth = DateTime.Now.Month + "";
        if (dateTimeMonth.Length == 1)
        {
            dateTimeMonth = "0" + dateTimeMonth;
        }
        string dateTimeDay = DateTime.Now.Day + "";
        if (dateTimeDay.Length == 1)
        {
            dateTimeDay = "0" + dateTimeDay;
        }
        string dateTimeHour = DateTime.Now.Hour + "";
        if (dateTimeHour.Length == 1)
        {
            dateTimeHour = "0" + dateTimeHour;
        }
        string dateTimeMinute = DateTime.Now.Minute + "";
        if (dateTimeMinute.Length == 1)
        {
            dateTimeMinute = "0" + dateTimeMinute;
        }

        //save saveDataJSON

        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(Manager.m.version + "_" + "Save" + saveNumber + "_saveDataJSON", json);

        PlayerPrefs.SetString(Manager.m.version + "_" + "Save" + saveNumber + "_Text", "<size=17>" + playTimeText + "<br>M:" + GameObject.Find("MoneyOutput").GetComponent<TextMeshProUGUI>().text + " Lvl:" + Manager.m.level + "/10<br>" + DateTime.Now.Year + "." + dateTimeMonth + "." + dateTimeDay + " " + dateTimeHour + ":" + dateTimeMinute);
        this.saveText.GetComponent<TextMeshProUGUI>().text = ("<size=17>" + playTimeText + "<br>M:" + GameObject.Find("MoneyOutput").GetComponent<TextMeshProUGUI>().text + " Lvl:" + Manager.m.level + "/10<br>" + DateTime.Now.Year + "." + dateTimeMonth + "." + dateTimeDay + " " + dateTimeHour + ":" + dateTimeMinute);

        TimeSpan span = DateTime.Now.Subtract(new DateTime(2000, 1, 1, 0, 0, 0));

        PlayerPrefs.SetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Age", (float)span.TotalSeconds);
        this.age = (float)span.TotalSeconds;

        if (saveNumber > 0)
        {
            Manager.m.lastSaveNumber = saveNumber;
            PlayerPrefs.SetInt(Manager.m.version + "_" + "LastSave", saveNumber);
        }
        progress += 1;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);

        Manager.m.musicSpeaker.StopMusic(0);
        Manager.m.loading = false;
        Manager.m.inSettings = false;
        for (int i = 1; i <= 20; i++)
        {
            loadingScreen.setStatus(0, 1 - i * 0.05f, "Lorem", 100);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        loadingScreen.setStatus(0, 0, "", 0);
    }

    public IEnumerator LoadGame()
    {
        Manager.m.loading = true;
        LoadingScreen loadingScreen = Manager.m.saveGameManager.loadingScreen;
        loadingScreen.setStatus(0, 0, "Lorem", 0);
        Manager.m.musicSpeaker.StopMusic(9999);
        for (int i = 1; i <= 20; i++)
        {
            loadingScreen.setStatus(0, i * 0.05f, "Lorem", 0);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        Manager.m.Reset(false);
        if (Manager.m.inMainMenu == true)
        {
            Manager.m.inMainMenu = false;
            Manager.m.inMainMenuLoad = false;
        }
        int loadingSteps = 100;
        float progress = 1;
        float percentPerStep = 100f / loadingSteps;

        //load saveDataJSON

        string json = PlayerPrefs.GetString(Manager.m.version + "_" + "Save" + saveNumber + "_saveDataJSON");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        //load auto repair
        for (int i = 0; i < Manager.m.autoRepairDroppers.Length; i++)
        {
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairDropper" + i) == 1)
            {
                Manager.m.autoRepairDroppers[i] = true;
            }
            else
            {
                Manager.m.autoRepairDroppers[i] = false;
            }
        }
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 0.5f);
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < Manager.m.autoRepairMachines.Length; i++)
        {
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_autoRepairMachines" + i) == 1)
            {
                Manager.m.autoRepairDroppers[i] = true;
            }
            else
            {
                Manager.m.autoRepairDroppers[i] = false;
            }
        }
        progress += 2;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //load upgrade ressources
        for (int i = 0; i <= PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_upgradeRessources"); i++)
        {
            Manager.m.upgradeRessources[i] = true;
        }
        for (int i = 0; i < Manager.m.hallUpgrader.Length; i++)
        {
            for (int x = 1; x < Manager.m.hallUpgrader[i].inputRessources.Count; x++)
            {
                Manager.m.hallUpgrader[i].inputRessources[x] = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_hallUpgrader" + i + "_Drop" + x);
            }
        }
        progress += 1;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //load market state
        for (int i = 0; i < Manager.m.dropValueMultipliers.Length; i++)
        {
            for (int y = 0; y < Manager.m.marketDrops.Length; y++)
            {
                Manager.m.dropValueMultipliers[i][y] = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Drop" + (i + 1) + "_UpgradeLevel" + i + "_ValueMult");
            }
        }
        progress += 3;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + percentPerStep * 0.8f);
        yield return new WaitForEndOfFrame();

        //load missions
        Manager.m.missionManager.missions.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_mission_Count"); i++)
        {
            bool accepted = false;
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Accepted") == 1) { accepted = true; }
            Manager.m.missionManager.LoadMission(
                (int)PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_OreNumber"), 
                (int)PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_UpgradeLevel"), 
                (int)PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Quantity"), 
                (int)PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Time"), 
                PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Reward"), 
                PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_mission + " + i + "_Progress"), 
                accepted);
        }
        progress += 8;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        ////load Factory Objects
        //int factoryObjectCount = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_CountObjects");
        //for (int i = 0; i < factoryObjectCount; i++)
        //{
        //    GameObject a;
        //    a = (GameObject)Instantiate(Resources.Load(PlayerPrefs.GetString(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i)));
        //    a.transform.position = new Vector3(PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_x"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_y"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_pos_z"));
        //    a.transform.Rotate(PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_x"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_y"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_rot_z"));
        //    a.transform.localScale = new Vector3(PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_x"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_y"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_scal_z"));
        //    a.GetComponent<RepairDropper>().durability = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_durability");
        //    a.GetComponent<RepairDropper>().id = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_id");
        //    if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_isScrap") == 1)
        //    {
        //        a.GetComponent<RepairDropper>().isScrap = true;
        //        a.transform.parent = Manager.m.scrapFolder.transform;
        //    }
        //    else
        //    {
        //        a.GetComponent<RepairDropper>().isScrap = false;
        //        a.transform.parent = Manager.m.machineFolder.transform;
        //    }

        //    if (a.GetComponentInChildren<Drop>())
        //    {
        //        a.GetComponentInChildren<Drop>().stopTimeReset = true;
        //        a.GetComponentInChildren<Drop>().currenttime = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_dropTime");
        //    }

        //    for (int x = 0; x < a.GetComponent<RepairDropper>().inputOres.Length; x++)
        //    {
        //        a.GetComponent<RepairDropper>().inputOres[x] = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_FabObj" + i + "_inputOre" + x);
        //    }
        //    loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + (percentPerStep * 32) * i / factoryObjectCount);
        //    yield return new WaitForEndOfFrame();
        //}


        //load Factory Objects
        int factoryObjectCount = saveData.fob_names.Length;
        float timeFrame = Time.realtimeSinceStartup;

        for (int i = 0; i < factoryObjectCount; i++)
        {
            GameObject a = (GameObject)Instantiate(Resources.Load(saveData.fob_names[i]));

            a.transform.position = new Vector3(saveData.fob_xPos[i], saveData.fob_yPos[i], saveData.fob_zPos[i]);
            a.transform.rotation = Quaternion.Euler(saveData.fob_xRot[i], saveData.fob_yRot[i], saveData.fob_zRot[i]);
            a.transform.localScale = new Vector3(saveData.fob_xScale[i], saveData.fob_yScale[i], saveData.fob_zScale[i]);

            RepairDropper rd = a.GetComponent<RepairDropper>();
            rd.durability = saveData.fob_durabilities[i];
            rd.id = (int)saveData.fob_ids[i];
            rd.isScrap = saveData.fob_areScraps[i];

            if (rd.isScrap)
                a.transform.parent = Manager.m.scrapFolder.transform;
            else
                a.transform.parent = Manager.m.machineFolder.transform;

            Drop drop = a.GetComponentInChildren<Drop>();
            if (drop)
            {
                drop.stopTimeReset = true;
                drop.currenttime = saveData.fob_dropTimers[i];
            }

            if (saveData.fob_inputOres_serialized[i] != null)
            {
                string row = saveData.fob_inputOres_serialized[i];
                if (row != null && row != "")
                {
                    string[] rowToStringlist = row.Split('|');
                    for (int x = 0; x < rowToStringlist.Length; x++)
                    {
                        rd.inputOres[x] = float.Parse(rowToStringlist[x]);
                    }
                }
            }

            if (Time.realtimeSinceStartup - timeFrame > 0.01f)
            {
                timeFrame = Time.realtimeSinceStartup;
                loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + (percentPerStep * 32) * i / factoryObjectCount);
                yield return new WaitForEndOfFrame();
            }
        }
        progress += 32;

        //load finished sequences
        if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial2") == 0)
        {
            Manager.m.tutorial.finishedTutorial2 = false;
        }
        else
        {
            Manager.m.tutorial.finishedTutorial2 = true;
        }
        if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Tutorial3") == 0)
        {
            Manager.m.tutorial.finishedTutorial3 = false;
        }
        else
        {
            Manager.m.tutorial.finishedTutorial3 = true;
        }
        if (PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FinishedFinalSequence") == 1)
        {
            Manager.m.finishedFinalSequence = true;
        }
        else
        {
            Manager.m.finishedFinalSequence = false;
        }
        progress += 1;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //load gnomes
        int gnomeCount = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Gnome_Count");
        for (int i = 0; i < gnomeCount; i++)
        {
            string keyBase = Manager.m.version + "_Save" + saveNumber + "_Gnome" + i + "_";
            GameObject gnomeObj = Instantiate(Manager.m.gnomeManager.gnome);
            Gnome_Script gnome = gnomeObj.GetComponent<Gnome_Script>();

            // Transform
            Vector3 position = new Vector3(
                PlayerPrefs.GetFloat(keyBase + "PosX"),
                PlayerPrefs.GetFloat(keyBase + "PosY"),
                PlayerPrefs.GetFloat(keyBase + "PosZ")
            );
            Vector3 rotation = new Vector3(
                PlayerPrefs.GetFloat(keyBase + "RotX"),
                PlayerPrefs.GetFloat(keyBase + "RotY"),
                PlayerPrefs.GetFloat(keyBase + "RotZ")
            );
            Vector3 scale = new Vector3(
                PlayerPrefs.GetFloat(keyBase + "ScaleX"),
                PlayerPrefs.GetFloat(keyBase + "ScaleY"),
                PlayerPrefs.GetFloat(keyBase + "ScaleZ")
            );

            gnomeObj.transform.position = position;
            gnomeObj.transform.rotation = Quaternion.Euler(rotation);
            gnomeObj.transform.localScale = scale;
            gnomeObj.transform.parent = Manager.m.gnomeManager.gnomeFolder;

            // Values
            gnome.attackDamage = PlayerPrefs.GetFloat(keyBase + "AttackDamage");
            gnome.baseMovementSpeed = PlayerPrefs.GetFloat(keyBase + "BaseMovementSpeed");
            gnome.baseRotationSpeed = PlayerPrefs.GetFloat(keyBase + "BaseRotationSpeed");
            gnome.aimX = PlayerPrefs.GetFloat(keyBase + "AimX");
            gnome.aimZ = PlayerPrefs.GetFloat(keyBase + "AimZ");
            gnome.idleWaitTimer = PlayerPrefs.GetFloat(keyBase + "IdleWaitTimer");
            gnome.discoveryDuration = PlayerPrefs.GetFloat(keyBase + "DiscoveryDuration");
            gnome.attackMode = PlayerPrefs.GetInt(keyBase + "AttackMode");
            gnome.attackedHall = PlayerPrefs.GetInt(keyBase + "AttackedHall");
            gnome.fleeing = PlayerPrefs.GetInt(keyBase + "Fleeing") == 1;
            Manager.m.gnomeManager.gnomeList.Add(gnome);

            loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + (percentPerStep * 10) * i / gnomeCount);
            yield return new WaitForEndOfFrame();
        }
        progress += 15;

        //load quick time events
        Manager.m.quickTimeEventManager.currentEvents.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_QTE_Count"); i++)
        {
            LoadQTEForKey(Manager.m.version + "_" + "Save" + saveNumber + "_QTE", i, 0, Manager.m.quickTimeEventManager.currentEvents);
        }
        progress += 4;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //int oreCount = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_CountOres");
        //float timeStopTotal = Time.realtimeSinceStartup;
        //List<GameObject> ores = new List<GameObject>();
        //Physics.autoSimulation = false;
        //for (int i = 0; i < oreCount; i++)
        //{
        //    GameObject ore;
        //    ore = Instantiate(Manager.m.ores[PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_Number")]);
        //    ores.Add(ore);
        //    ore.GetComponent<Ore>().value = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_value");
        //    ore.GetComponent<Ore>().baseValue = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_baseValue");

        //    ore.transform.position = new Vector3(PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_x"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_y"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_pos_z"));
        //    ore.transform.rotation = new Quaternion(PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_x"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_y"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_z"), PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_rot_w"));
        //    ore.GetComponent<Ore>().oreNumber = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_Number");
        //    ore.GetComponent<Ore>().upgradeLevel = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_UpgradeLvl");
        //    ore.GetComponent<Ore>().moveableByBelt = true;
        //    ore.transform.parent = Manager.m.oreFolder.transform;
        //    for (int y = 0; y < PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_visitedBeltsCount"); y++)
        //    {
        //        ore.GetComponent<Ore>().visitedBelts.Add(PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Ore" + i + "_visitedBelts" + y));
        //    }
        //    loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + (percentPerStep * 32) * i / oreCount);
        //    if (i % 2 == 0)
        //        yield return new WaitForEndOfFrame();
        //}
        //Physics.autoSimulation = true;
        ////Debug.Log("TimeTotal: " + (Time.realtimeSinceStartup - timeStopTotal));
        ///

        //load ores

        int oreCount = saveData.ore_numbers.Length;
        List<GameObject> ores = new List<GameObject>();
        timeFrame = Time.realtimeSinceStartup;

        for (int i = 0; i < oreCount; i++)
        {
            GameObject ore = Instantiate(Manager.m.ores[saveData.ore_numbers[i]]);
            ores.Add(ore);

            Ore oreScript = ore.GetComponent<Ore>();

            oreScript.value = saveData.ore_values[i];
            oreScript.baseValue = saveData.ore_baseValues[i];
            oreScript.oreNumber = saveData.ore_numbers[i];
            oreScript.upgradeLevel = saveData.ore_upgradeLevers[i];
            oreScript.moveableByBelt = true;

            ore.transform.position = new Vector3(saveData.ore_xPos[i], saveData.ore_yPos[i], saveData.ore_zPos[i]);
            ore.transform.rotation = Quaternion.Euler(saveData.ore_xRot[i], saveData.ore_yRot[i], saveData.ore_zRot[i]);
            ore.transform.parent = Manager.m.oreFolder.transform;

            //ore.GetComponent<Rigidbody>().velocity = saveData.ore_veloc[i];
            //ore.GetComponent<Rigidbody>().angularVelocity = saveData.ore_trc[i];

            oreScript.visitedBelts.Clear();
            if (saveData.ore_visitedBeltsLists_serialized[i] != null && saveData.ore_visitedBeltsLists_serialized[i] != "")
            {
                string[] row = saveData.ore_visitedBeltsLists_serialized[i].Split("|");
                for (int j = 0; j < row.Length; j++)
                {
                    oreScript.visitedBelts.Add(int.Parse(row[j]));
                }
            }

            if (Time.realtimeSinceStartup - timeFrame > 0.01f)
            {
                timeFrame = Time.realtimeSinceStartup;
                loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress + (percentPerStep * 32) * i / oreCount);
                yield return new WaitForEndOfFrame();
            }
        }


        progress += 32;

        //load enviroment stats
        Manager.m.darknessController.changing = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_DarknessControllerState");
        progress += 0.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        //load general stats
        Manager.m.playTime = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Playtime");
        Manager.m.money = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Money");
        Manager.m.level = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_Level");
        Manager.m.exp = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Exp");
        Manager.m.setKamera(PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_FactoryCamera"), 0);
        Manager.m.declinedMission = PlayerPrefs.GetInt(Manager.m.version + "_" + "Save" + saveNumber + "_DeclinedMission");
        progress += 0.5f;
        loadingScreen.setStatus(0, 1, "Lorem", percentPerStep * progress);
        yield return new WaitForEndOfFrame();

        Manager.m.musicSpeaker.StopMusic(0);
        Manager.m.loading = false;
        Manager.m.inSettings = false;
        for (int i = 1; i <= 20; i++)
        {
            loadingScreen.setStatus(0, 1 - i * 0.05f, "Lorem", 100);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
        loadingScreen.setStatus(0, 0, "", 0);
    }
}
