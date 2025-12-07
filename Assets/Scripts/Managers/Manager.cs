using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class Manager : MonoBehaviour
{

    public static Manager m = null;
    private static readonly object padlock = new object();

    //Admin:
    public bool resetPlayerPrefs;
    public bool useMovieCam;
    public bool creative;

    public string version;
    public string officialVersion;

    public TMP_FontAsset gameFont;
    public List<TextMeshProUGUI> originalFont;

    //Basic
    public double money;
    public double startMoney;
    public int level;
    public double shownMoney;
    public double exp;
    public int playTime;
    public int frameCounter;
    private int id;
    public int qualityLevel;
    public GameObject canvas;
    public GameObject machineFolder;
    public GameObject oreFolder;
    public GameObject scrapFolder;
    public GameObject soundFolder;
    public GameObject factoryExitFolder;

    public PlayEffect effectSpeaker;
    public PlayMusic musicSpeaker;
    public PlayFactory factorySpeaker;

    public int dropperNumber;
    public int machineNumber;
    public int dropperMax;
    public int lastSaveNumber;
    public double currentCost;
    public int machineMax;
    public int dropperRotation;
    public int changeSaveTimer;
    public int maxUpgradeNumber;
    public double incomeLastSecond;
    public Texture2D cursor;
    public Texture2D buildCursor;
    public Texture2D repairCursor;
    public Texture2D sellCursor;
    public Texture2D noLightCursor;
    public Transform wrenchFolder;
    public MainMenue mainMenu;
    public IntroScene introScene;
    public Tutorial tutorial;
    public Level levelOutput;
    public FactoryButtons factoryButtons;
    public BuyButton buyButton;
    public ShopCamera shopCamera;
    public Hotkeys hotkeys;
    public GameSaveManager saveGameManager;
    public MarketManager marketManager;
    public MissionManager missionManager;
    public FactoryHallsManager factoryHallsManager;
    public QuickTimeEventManager quickTimeEventManager;
    public GnomeManager gnomeManager;
    public NotificationManager notificationManager;
    public CommunicationManager communicationManager;
    public EditHistoryManager editHistoryManager;
    public GraphicManager graphicManager;
    public DarknessController darknessController;

    public bool paused;
    public bool loading;
    public bool inMainMenu;
    public bool inShopDropper;
    public bool inShopMachine;
    public bool editMode;
    public bool editMode_sell;
    public bool editMode_placeDropper;
    public bool editMode_placeMachine;
    public bool editMode_repair;
    public bool settings_save;
    public bool settings_load;
    public bool settings_clear;
    public bool settings_autosave;
    public bool settings_hotkeys;
    public bool settings_help;
    public bool inMainMenuLoad;
    public bool inMarket;
    public bool inMissions;
    public bool inQuickTimeEvents;
    public bool inFactoryHalls;
    public bool inSettings;
    public bool inHotkeySet;
    public bool hideFactoryButtons;
    public bool hideFactoryUI;
    public bool acessMissions;
    public bool acessFactoryHalls;
    public bool acessQTEs;
    public bool acessRepair;
    public bool startFinalSequence;
    public bool inFinalSequence;
    public bool finishedFinalSequence;

    public String objectType;

    public Camera lastDropperCamera;
    public GameObject chain;
    public GameObject chainLock;
    public GameObject chainText;
    public GameObject dropperInformationBox;
    public GameObject dropperInformationText;
    public GameObject upgradeInformationBox;

    public Volume generalVolume;
    public Volume factoryVolume;
    public Volume effectsVolume;
    public Volume voiceVolume;
    public Volume musicVolume;

    public GameObject ConveyorBelt1;
    public GameObject ConveyorBelt1Blueprint;
    public GameObject ConveyorBelt1Right;
    public GameObject ConveyorBelt1RightBlueprint;
    public GameObject ConveyorBelt1Left;
    public GameObject ConveyorBelt1LeftBlueprint;
    public GameObject ConveyorBelt1Fuse;
    public GameObject ConveyorBelt1FuseBlueprint;
    public GameObject ConveyorBelt1Split;
    public GameObject ConveyorBelt1SplitBlueprint;
    public GameObject ConveyorBelt2;
    public GameObject ConveyorBelt2Blueprint;
    public GameObject ConveyorBelt2Right;
    public GameObject ConveyorBelt2RightBlueprint;
    public GameObject ConveyorBelt2Left;
    public GameObject ConveyorBelt2LeftBlueprint;
    public GameObject ConveyorBelt2Fuse;
    public GameObject ConveyorBelt2FuseBlueprint;
    public GameObject ConveyorBelt2Split;
    public GameObject ConveyorBelt2SplitBlueprint;
    public GameObject ConveyorBelt3;
    public GameObject ConveyorBelt3Blueprint;
    public GameObject ConveyorBelt3Right;
    public GameObject ConveyorBelt3RightBlueprint;
    public GameObject ConveyorBelt3Left;
    public GameObject ConveyorBelt3LeftBlueprint;
    public GameObject ConveyorBelt3Fuse;
    public GameObject ConveyorBelt3FuseBlueprint;
    public GameObject ConveyorBelt3Split;
    public GameObject ConveyorBelt3SplitBlueprint;

    public GameObject Upgrader1;
    public GameObject Upgrader1Blueprint;
    public GameObject Upgrader2;
    public GameObject Upgrader2Blueprint;
    public GameObject Upgrader3;
    public GameObject Upgrader3Blueprint;

    public GameObject Furnace1;
    public GameObject Furnace1Blueprint;
    public GameObject Furnace2;
    public GameObject Furnace2Blueprint;
    public GameObject Furnace3;
    public GameObject Furnace3Blueprint;

    public GameObject Dropper1;
    public GameObject Dropper1Blueprint;
    public GameObject Dropper2;
    public GameObject Dropper2Blueprint;
    public GameObject Dropper3;
    public GameObject Dropper3Blueprint;
    public GameObject Dropper4;
    public GameObject Dropper4Blueprint;
    public GameObject Dropper5;
    public GameObject Dropper5Blueprint;
    public GameObject Dropper6;
    public GameObject Dropper6Blueprint;
    public GameObject Dropper7;
    public GameObject Dropper7Blueprint;
    public GameObject Dropper8;
    public GameObject Dropper8Blueprint;
    public GameObject Dropper9;
    public GameObject Dropper9Blueprint;
    public GameObject Dropper10;
    public GameObject Dropper10Blueprint;

    public GameObject ScrapDropper1;
    public GameObject ScrapFurnace1;
    public GameObject ScrapUpgrader1;
    public GameObject ScrapConveyorBelt1;

    public string[] keyActions;
    public string[] keyActionTrigger;
    public string[] standardKeyActionTrigger;

    public GameObject[] droppers;
    public GameObject[] machines;
    public GameObject[] scraps;
    public GameObject[] factorys;
    public bool[] autoRepairDroppers;
    public bool[] autoRepairMachines;
    public GameObject[] ores;
    public string[] oreIdentifications;
    public string[] dropperIdentifications;
    public string[] machineIdentifications;
    public RawImage[] oreImages;
    public RawImage[] upgradeImages;
    public Texture[] eventImages;
    public float[] upgradeMultipliers;
    public HallUpgrade[] hallUpgrader;
    public bool[] upgradeRessources;
    public int[] startScraps;
    public float[] scrapRemovalCosts;
    public Vector3[] scrapPositions;
    public Vector3[] scrapRotations;
    public List<double> incomeLastMinute;
    public Camera[] factoryCameras;
    public Camera[] shopCameras;
    public Camera[] otherCameras;
    public List<Vector2> factoryExits;
    public List<Vector2> factoryCenters;

    public int[] inputCapacityDrop1;
    public float[] consumeOresDrop1;
    public int[] inputUpgradeDrop1;

    public int[] inputCapacityDrop2;
    public float[] consumeOresDrop2;
    public int[] inputUpgradeDrop2;

    public int[] inputCapacityDrop3;
    public float[] consumeOresDrop3;
    public int[] inputUpgradeDrop3;

    public int[] inputCapacityDrop4;
    public float[] consumeOresDrop4;
    public int[] inputUpgradeDrop4;

    public int[] inputCapacityDrop5;
    public float[] consumeOresDrop5;
    public int[] inputUpgradeDrop5;

    public int[] inputCapacityDrop6;
    public float[] consumeOresDrop6;
    public int[] inputUpgradeDrop6;

    public int[] inputCapacityDrop7;
    public float[] consumeOresDrop7;
    public int[] inputUpgradeDrop7;

    public int[] inputCapacityDrop8;
    public float[] consumeOresDrop8;
    public int[] inputUpgradeDrop8;

    public int[] inputCapacityDrop9;
    public float[] consumeOresDrop9;
    public int[] inputUpgradeDrop9;

    public int[] inputCapacityDrop10;
    public float[] consumeOresDrop10;
    public int[] inputUpgradeDrop10;

    public int[][] dropInputCapacitys;
    public float[][] dropConsumeOres;
    public int[][] dropInputUpgrades;

    double currenttime;

    public float qTECheapMiners;
    public int qTECheapMinersNumber;
    public float qTEExpensiveMiners;
    public int qTEExpensiveMinersNumber;

    public float qTECheapMachines;
    public int qTECheapMachinesNumber;
    public float qTEExpensiveMachines;
    public int qTEExpensiveMachinesNumber;

    public bool qTEBrokenLights;
    public float qTEOverheating;
    public int qTEOverheatingNumber;
    public float qTEBrokenBelts;
    public float qTEMarketBoost;
    public float qTEMarketCrash;
    public float qTEBelts;
    public int qTEMissionBuff;
    public bool qTEMissionImpossible;
    public float qTEMaintenanceBoost;
    public bool qTEInvertedMarket;
    public int qTELockedFactory;
    public bool qTEUltimateProduction;
    public bool qTEUltimateWipeout;

    void Awake()
    {
        lock (padlock)
        { 
            if (m == null)
            {
                m = this;

                //Start values here:

                setKamera(0, 0);

                TextMeshProUGUI[] tmps = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
                for (int i = 0; i < tmps.Length; i++)
                {
                    if (originalFont.Contains(tmps[i]) == false)
                    {
                        tmps[i].font = gameFont;
                        //print("Gameobject: " + tmps[i].gameObject.name + ", " + tmps[i].gameObject.transform.parent.name);
                    }
                }

                level = 1;
                editMode_placeDropper = false;
                editMode_placeMachine = false;
                dropperNumber = 1;
                machineNumber = 1;
                currenttime = Time.time + 1;

                dropInputCapacitys = new int[10][];
                dropInputCapacitys[0] = inputCapacityDrop1;
                dropInputCapacitys[1] = inputCapacityDrop2;
                dropInputCapacitys[2] = inputCapacityDrop3;
                dropInputCapacitys[3] = inputCapacityDrop4;
                dropInputCapacitys[4] = inputCapacityDrop5;
                dropInputCapacitys[5] = inputCapacityDrop6;
                dropInputCapacitys[6] = inputCapacityDrop7;
                dropInputCapacitys[7] = inputCapacityDrop8;
                dropInputCapacitys[8] = inputCapacityDrop9;
                dropInputCapacitys[9] = inputCapacityDrop10;

                dropConsumeOres = new float[10][];
                dropConsumeOres[0] = consumeOresDrop1;
                dropConsumeOres[1] = consumeOresDrop2;
                dropConsumeOres[2] = consumeOresDrop3;
                dropConsumeOres[3] = consumeOresDrop4;
                dropConsumeOres[4] = consumeOresDrop5;
                dropConsumeOres[5] = consumeOresDrop6;
                dropConsumeOres[6] = consumeOresDrop7;
                dropConsumeOres[7] = consumeOresDrop8;
                dropConsumeOres[8] = consumeOresDrop9;
                dropConsumeOres[9] = consumeOresDrop10;

                dropInputUpgrades = new int[10][];
                dropInputUpgrades[0] = inputUpgradeDrop1;
                dropInputUpgrades[1] = inputUpgradeDrop2;
                dropInputUpgrades[2] = inputUpgradeDrop3;
                dropInputUpgrades[3] = inputUpgradeDrop4;
                dropInputUpgrades[4] = inputUpgradeDrop5;
                dropInputUpgrades[5] = inputUpgradeDrop6;
                dropInputUpgrades[6] = inputUpgradeDrop7;
                dropInputUpgrades[7] = inputUpgradeDrop8;
                dropInputUpgrades[8] = inputUpgradeDrop9;
                dropInputUpgrades[9] = inputUpgradeDrop10;

                autoRepairDroppers = new bool[droppers.Length];
                autoRepairMachines = new bool[machines.Length];
                frameCounter = 0;

                for (int i = 0; i < startScraps.Length; i++)
                {
                    GameObject s = Instantiate(scraps[startScraps[i]]);
                    s.transform.position = scrapPositions[i];
                    s.transform.rotation = Quaternion.Euler(scrapRotations[i]);
                    s.transform.parent = scrapFolder.transform;
                    s.GetComponent<RepairDropper>().cost = scrapRemovalCosts[i];
                }
                incomeLastSecond = 0;
                incomeLastMinute = new List<double>();
                currentCost = 0;
                editMode_repair = false;
                money = startMoney;
                playTime = 0;
                dropperRotation = 180;
                chainLock.GetComponent<Button>().onClick.AddListener(Lock);

                factoryVolume.publicVolume = 1    ;
                effectsVolume.publicVolume = 1;
                musicVolume.publicVolume = 1;

                inMainMenu = true;

                resetPlayerPrefs = false;
                lastSaveNumber = PlayerPrefs.GetInt(version + "_" + "LastSave");

                const int keyActionCount = 43;
                keyActions = new string[keyActionCount]
                {
                    "Settings",        // 1
                    "Quicksave",       // 2
                    "AskYes",          // 3
                    "AskNo",           // 4
                    "EditMode",        // 5
                    "Shop1",           // 6
                    "Shop2",           // 7
                    "Buy",             // 8
                    "ExitShop",        // 9
                    "SwipeRight",      // 10
                    "SwipeLeft",       // 11
                    "CancelBuilding",  // 12 (Removed)
                    "RotateRight",     // 13
                    "RotateLeft",      // 14
                    "Transform",       // 15
                    "SetTransform1",   // 16
                    "SetTransform2",   // 17
                    "SetTransform3",   // 18
                    "SetTransform4",   // 19
                    "SetTransform5",   // 20
                    "Undo",            // 21
                    "Redo",            // 22
                    "RepairMode",      // 23
                    "Market",          // 24
                    "Mission",         // 25
                    "QuickTimeEvents", // 26
                    "FactoryHalls",    // 27
                    "FactoryHall1",    // 28
                    "FactoryHall2",    // 29
                    "FactoryHall3",    // 30
                    "FactoryHall4",    // 31
                    "FactoryHall5",    // 32
                    "FactoryHall6",    // 33
                    "FactoryHall7",    // 34
                    "FactoryHall8",    // 35
                    "FactoryHall9",    // 36
                    "FactoryHall10",   // 37
                    "SkipMusic",       // 38
                    "FactorySound",    // 39
                    "EffectSound",     // 40
                    "MusicSound",      // 41
                    "ResetOres",       // 42 (Removed)
                    "RestartGame",     // 43 (Removed)
                };

                standardKeyActionTrigger = new string[keyActionCount]
                {
                    "Esc",//1
                    "",//2
                    "Y",//3
                    "N",//4
                    "Tab",//5
                    "Q",//6
                    "W",//7
                    "Enter",//8
                    "Backspace",//9
                    "D",//10
                    "A",//11
                    "",//12
                    "ClickRight",//13
                    "",//14
                    "E",//15
                    "F1",//16
                    "F2",//17
                    "F3",//18
                    "F4",//19
                    "F5",//20
                    "A",//21
                    "D",//22
                    "R",//23
                    "M",//24
                    "I",//25
                    "Z",//26
                    "F",//27
                    "1",//28
                    "2",//29
                    "3",//30
                    "4",//31
                    "5",//32
                    "6",//33
                    "7",//34
                    "8",//35
                    "9",//36
                    "0",//37
                    "",//38
                    "",//39
                    "",//40
                    "",//41
                    "",//42
                    "",//43
                };
                keyActionTrigger = new string[keyActionCount];
                standardKeyActionTrigger.CopyTo(keyActionTrigger, 0);
                for (int i = 0; i < keyActionTrigger.Length; i++)
                {
                    if (PlayerPrefs.GetString(version + "_" + "Hotkey" + keyActions[i] + "_Trigger") != "")
                    {
                        if (PlayerPrefs.GetString(version + "_" + "Hotkey" + keyActions[i] + "_Trigger") != "--")
                        {
                            keyActionTrigger[i] = PlayerPrefs.GetString(version + "_" + "Hotkey" + keyActions[i] + "_Trigger");
                        }
                        else
                        {
                            keyActionTrigger[i] = "";
                        }
                    }
                }
                id = PlayerPrefs.GetInt(version + "_" + "max_id");

                Transform[] exits = factoryExitFolder.GetComponentsInChildren<Transform>();
                factoryExits = new List<Vector2>();
                for (int i = 1; i < exits.Length; i++)
                {
                    factoryExits.Add(new Vector2(exits[i].position.x, exits[i].position.z));
                }
                factoryCenters = new List<Vector2>();
                for (int i = 0; i < factorys.Length; i++)
                {
                    factoryCenters.Add(new Vector2(factorys[i].transform.position.x, factorys[i].transform.position.z));
                }

                qTECheapMiners = 1;
                qTECheapMinersNumber = 0;
                qTEExpensiveMiners = 1;
                qTEExpensiveMinersNumber = 0;

                qTECheapMachines = 1;
                qTECheapMachinesNumber = 0;
                qTEExpensiveMiners = 1;
                qTEExpensiveMachinesNumber = 0;

                qTEBrokenLights = false;
                qTEOverheating = 1;
                qTEOverheatingNumber = 1;
                qTEBrokenBelts = 1;
                qTEMarketBoost = 0;
                qTEMarketCrash = 0;
                qTEBelts = 0;
                qTEMissionBuff = 0;
                qTEMissionImpossible = false;
                qTEMaintenanceBoost = 0;
                qTEInvertedMarket = false;
                qTELockedFactory = 0;
                qTEUltimateProduction = false;
                qTEUltimateWipeout = false;

                //introScene.PlayIntroScene();
            }
            else if (m != this) Destroy(gameObject);
        }
    }

    private void Start()
    {

    }


    private void Update()
    {
        if (creative == true)
        {
            Manager.m.money = 1000000000000;
            Manager.m.level = 11;
            for (int i = 0; i < 11; i++)
            {
                upgradeRessources[i] = true;
            }
        }
        QualitySettings.SetQualityLevel(qualityLevel);
        frameCounter++;
        if (frameCounter > 100000000)
        {
            frameCounter = 0;
        }
        if (inMainMenu == true)
        {
            hideFactoryUI = true;
            
            setKamera(otherCameras[2]);
        }
        else if (otherCameras[2].enabled == true)
        {
            setKamera(factoryCameras[0]);
        }
        if (resetPlayerPrefs == true)
        {
            PlayerPrefs.DeleteAll();
            resetPlayerPrefs = false;
        }
        dropperRotation = (int)Mathf.Round(dropperRotation);

        if (inFinalSequence || introScene.inIntroScene)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
            if (inUIMenu() == false)
            {
                if (Manager.m.qTEBrokenLights || Manager.m.qTEUltimateWipeout)
                {
                    Cursor.SetCursor(noLightCursor, new Vector2(noLightCursor.width / 2f, noLightCursor.height / 2f), CursorMode.Auto);
                }
                else if (editMode_sell)
                {
                    Cursor.SetCursor(sellCursor, Vector2.zero, CursorMode.Auto);
                }
                else if (editMode_placeDropper || editMode_placeMachine)
                {
                    Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
                }
                else if (editMode_repair)
                {
                    Cursor.SetCursor(repairCursor, Vector2.zero, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            }
        }

        if (Manager.m.inShopDropper || Manager.m.inShopMachine == true || hideFactoryUI == true) //cases when onely buttons should be deactivated
        {
            hideFactoryButtons = true;
        }
        else
        {
            hideFactoryButtons = false;
        }
        if (otherCameras[0].enabled == true || otherCameras[1].enabled == true || otherCameras[2].enabled) //cases when ui should be deactivated
        {
            hideFactoryUI = true;
        }
        else
        {
            hideFactoryUI = false;
        }
        if (GameInputManager.GetKeyDown(ActionKey("RotateLeft")) && tutorial.inTutorial != 0)
        {
            dropperRotation -= 90;
        }

        if (GameInputManager.GetKeyDown(ActionKey("RotateRight")) && tutorial.inTutorial != 0)
        {
            dropperRotation += 90;
        }

        if (GameInputManager.GetKeyDown(ActionKey("FactorySound")))
        {
            if (factoryVolume.on.activeSelf == true)
            {
                factoryVolume.On();
            }
            else
            {
                factoryVolume.Off();
            }
        }
        if (GameInputManager.GetKeyDown(ActionKey("EffectSound")))
        {
            if (effectsVolume.on.activeSelf == true)
            {
                effectsVolume.On();
            }
            else
            {
                effectsVolume.Off();
            }
        }
        if (GameInputManager.GetKeyDown(ActionKey("MusicSound")))
        {
            if (musicVolume.on.activeSelf == true)
            {
                musicVolume.On();
            }
            else
            {
                musicVolume.Off();
            }
        }

        bool factoryCameraActive = false;
        for (int i = 0; i < factoryCameras.Length; i++)
        {
            if (factoryCameras[i].enabled == true)
            {
                factoryCameraActive = true;
                if (level > i && upgradeRessources[i] == true && qTELockedFactory - 1 != i)
                {
                    chain.SetActive(false);
                }
                else if (level <= i)
                {
                    chain.SetActive(true);
                    chainText.GetComponent<TextMeshProUGUI>().text = "Level " + (i + 1);
                }
                else if (upgradeRessources[i] == false)
                {
                    chain.SetActive(true);
                    chainText.GetComponent<TextMeshProUGUI>().text = "Need more<br>Ressources";
                }
                else
                {
                    chain.SetActive(true);
                    chainText.GetComponent<TextMeshProUGUI>().text = "Not safe<br>in here...";
                }
                if (i >= 10)
                {
                    chainText.GetComponent<TextMeshProUGUI>().text = "Not<br>Available";
                }

            }
        }
        if (factoryCameraActive == false)
        {
            chain.SetActive(false);
        }

        paused = UpdatePause();

        if (changeSaveTimer > 0)
        {
            changeSaveTimer--;
        }
        if (changeSaveTimer < 0)
        {
            changeSaveTimer = 0;
        }

        if (dropperRotation >= 360) dropperRotation = dropperRotation - 360;
        if (dropperRotation < 0) dropperRotation = dropperRotation + 360;

        if (paused == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if (Time.timeScale == 0f)
            {
                StartCoroutine(ContinueTime());
            }
            if (incomeLastMinute.Count > 60)
            {
                incomeLastMinute.RemoveAt(0);
            }
            if (currenttime < Time.time) //loop: 1 per second
            {
                for (int i = 0; i < keyActionTrigger.Length; i++)
                {
                    if (keyActionTrigger[i] != "")
                    {
                        PlayerPrefs.SetString(version + "_" + "Hotkey" + keyActions[i] + "_Trigger", keyActionTrigger[i]);
                    }
                    else
                    {
                        PlayerPrefs.SetString(version + "_" + "Hotkey" + keyActions[i] + "_Trigger", "--");
                    }
                }
                PlayerPrefs.SetInt(version + "_" + "max_id", id);

                currenttime = Time.time + 1f;
                playTime++;

                incomeLastMinute.Add(incomeLastSecond);
                incomeLastSecond = 0;
            }

            //button / functionalities acess
            if (level >= 2)
            {
                acessFactoryHalls = true;
            }
            else
            {
                acessFactoryHalls = false;
            }
            if (level >= 3)
            {
                acessMissions = true;
            }
            else
            {
                acessMissions = false;
            }
            if (level >= 4)
            {
                acessQTEs = true;
            }
            else
            {
                acessQTEs = false;
            }
            acessRepair = true;
        }
    }

    public void setKamera(int nummer, int list)
    {
        for (int i = 0; i < factoryCameras.Length; i++)
        {
            factoryCameras[i].enabled = false;
            factoryCameras[i].gameObject.GetComponent<AudioListener>().enabled = false;
        }
        for (int i = 0; i < shopCameras.Length; i++) shopCameras[i].enabled = false;
        for (int i = 0; i < otherCameras.Length; i++) otherCameras[i].enabled = false;
        if (list == 0)
        {
            factoryCameras[nummer].enabled = true;
            factoryCameras[nummer].gameObject.GetComponent<AudioListener>().enabled = true;
            lastDropperCamera = factoryCameras[nummer];
        }
        else if (list == 1)
        {
            shopCameras[nummer].enabled = true;
            lastDropperCamera.gameObject.GetComponent<AudioListener>().enabled = true;
        }
        else if (list == 3)
        {
            otherCameras[nummer].enabled = true;
            lastDropperCamera.gameObject.GetComponent<AudioListener>().enabled = true;
        }
    }

    public void setKamera(Camera cam)
    {
        for (int i = 0; i < factoryCameras.Length; i++)
        {
            factoryCameras[i].enabled = false;
            factoryCameras[i].gameObject.GetComponent<AudioListener>().enabled = false;
        }
        for (int i = 0; i < shopCameras.Length; i++) shopCameras[i].enabled = false;
        for (int i = 0; i < otherCameras.Length; i++) otherCameras[i].enabled = false;

        for (int i = 0; i < factoryCameras.Length; i++)
        {
            if (factoryCameras[i] == cam)
            {
                factoryCameras[i].enabled = true;
                factoryCameras[i].gameObject.GetComponent<AudioListener>().enabled = true;
                lastDropperCamera = factoryCameras[i];
            }
        }
        for (int i = 0; i < shopCameras.Length; i++)
        {
            if (shopCameras[i] == cam)
            {
                shopCameras[i].enabled = true;
                lastDropperCamera.gameObject.GetComponent<AudioListener>().enabled = true;
            }
        }
        for (int i = 0; i < otherCameras.Length; i++)
        {
            if (otherCameras[i] == cam)
            {
                otherCameras[i].enabled = true;
                lastDropperCamera.gameObject.GetComponent<AudioListener>().enabled = true;
            }
        }
    }
    void Lock()
    {
        effectSpeaker.error();
    }

    public IEnumerator wait (float t)
    {
        yield return new WaitForSeconds(t);
    }
    static public IEnumerator move(GameObject Objekt, Vector3 richtung, float distanz)
    {
        Vector3 startPoint = Objekt.transform.position;
        
        while (Math.Abs(Vector3.Distance(Objekt.transform.position, startPoint)) < distanz)
        {
            Objekt.transform.position += richtung * Time.deltaTime;
            print(Math.Abs(Vector3.Distance(Objekt.transform.position, startPoint)));
            yield return null;
        }
    }
    public void Reset(bool restartGame)
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("FactoryObject");
        for (int i = 0; i < a.Length; i++)
        {
            a[i].transform.Translate(0, -100, 0);
            a[i].GetComponent<RepairDropper>().working = false;
            a[i].GetComponent<RepairDropper>().sold = true;
            a[i].tag = "Destroyed";
            Destroy(a[i].gameObject, 5f);
        }
        if (restartGame == true)
        {
            for (int i = 0; i < startScraps.Length; i++)
            {
                GameObject s = Instantiate(scraps[startScraps[i]]);
                s.transform.position = scrapPositions[i];
                s.transform.rotation = Quaternion.Euler(scrapRotations[i]);
                s.transform.parent = scrapFolder.transform;
                s.GetComponent<RepairDropper>().cost = scrapRemovalCosts[i];
            }
        }
        for (int i = 0; i < autoRepairDroppers.Length; i++)
        {
            autoRepairDroppers[i] = false;
        }
        for (int i = 0; i < autoRepairMachines.Length; i++)
        {
            autoRepairMachines[i] = false;
        }
        GameObject[] o = GameObject.FindGameObjectsWithTag("Ore");
        editMode_repair = false;
        for (int i = 0; i < o.Length; i++)
        {
            o[i].SetActive(false);
            Destroy(o[i], 3f);
        }
        money = startMoney;
        exp = 0;
        level = 1;
        playTime = 0;
        incomeLastSecond = 0;
        incomeLastMinute = new List<double>();
        missionManager.missions.Clear();
        missionManager.declinedMission = 0;
        for (int i = quickTimeEventManager.currentEvents.Count - 1; i >= quickTimeEventManager.currentEvents.Count; i--)
        {
            quickTimeEventManager.currentEvents.RemoveAt(i);
        }
        var qTEdisplays = quickTimeEventManager.displayFolder.GetComponentsInChildren<QuickTimeEventDisplay>();
        for (int i = 0; i < qTEdisplays.Length; i++)
        {
            Destroy(qTEdisplays[i].gameObject);
        }
        quickTimeEventManager.currentEvents.Clear();
        for (int i = 0; i < hallUpgrader.Length; i++)
        {
            hallUpgrader[i].ResetUpgrader();
        }
        qTECheapMiners = 1;
        qTECheapMinersNumber = 0;
        qTEExpensiveMiners = 1;
        qTEExpensiveMinersNumber = 0;

        qTECheapMachines = 1;
        qTECheapMachinesNumber = 0;
        qTEExpensiveMiners = 1;
        qTEExpensiveMachinesNumber = 0;

        qTEBrokenLights = false;
        qTEOverheating = 1;
        qTEOverheatingNumber = 1;
        qTEBrokenBelts = 1;
        qTEMarketBoost = 0;
        qTEMarketCrash = 0;
        qTEBelts = 0;
        qTEMissionBuff = 0;
        qTEMissionImpossible = false;
        qTEMaintenanceBoost = 0;
        qTEInvertedMarket = false;
        qTELockedFactory = 0;
        qTEUltimateProduction = false;
        qTEUltimateWipeout = false;

        for (int i = gnomeManager.gnomeList.Count - 1; i >= 0; i--)
        {
            Destroy(gnomeManager.gnomeList[i].gameObject);
            gnomeManager.gnomeList.RemoveAt(i);
        }
        gnomeManager.gnomeList.Clear();

        shopCamera.ResetCameraPosition();
        try
        {
            for (int i = 0; i < marketManager.dropValueMultipliers.Length; i++)
            {
                for (int x = 0; x < marketManager.dropValueMultipliers[i].Length; x++)
                {
                    marketManager.dropValueMultipliers[i][x] = 1;
                }
            }
        }
        catch { }
    }
    public string ActionKey(string action)
    {
        int index = -1;

        for(int i = 0; i < keyActions.Length; i++)
        {
            if (keyActions[i] == action)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            return keyActionTrigger[index];
        }
        else
        {
            print("Error - Action <" + action + "> not found");
            return "";
        }
    }
    public static float standardRotation(float rotation)
    {
        float r = rotation;
        r = Mathf.Round(r * 100) / 100;
        float overflow = 0;
        while ((r >= 360 || r < 0) && overflow <= 10000)
        {
            if (r >= 360)
            {
                r -= 360;
            }
            if (r < 0)
            {
                r += 360;
            }
            overflow++;
        }
        if (overflow >= 10000)
        {
            print("Error - <" + rotation + "> Axis is too high rotation");
            return 0;
        }
        return r;
    }
    public bool inUIMenu()
    {
        return inMissions || inMarket || inShopDropper || inShopMachine || inSettings || inQuickTimeEvents || inFactoryHalls || inMainMenu;
    }
    public int getID()
    {
        id++;
        return id;
    }

    public bool UpdatePause()
    {
        if (inSettings || inMainMenu || loading || inFinalSequence || editMode || tutorial.pauseGame)
        {
            return true;
        }
        return false;
    }

    public Camera getCurrentCamera()
    {
        for (int i = 0; i < shopCameras.Length; i++)
        {
            if (shopCameras[i].enabled)
            {
                return shopCameras[i];
            }
        }
        for (int i = 0; i < otherCameras.Length; i++)
        {
            if (otherCameras[i].enabled)
            {
                return otherCameras[i];
            }
        }
        for (int i = 0; i < factoryCameras.Length; i++)
        {
            if (factoryCameras[i].enabled)
            {
                return factoryCameras[i];
            }
        }
        return null;
    }

    public Camera getNearestDropperCamera(Vector3 position)
    {
        Camera nearest = null;
        float distance = Mathf.Infinity;
        for (int i = 0; i < factoryCameras.Length; i++)
        {
            if ((factoryCameras[i].gameObject.transform.position - position).magnitude < distance)
            {
                nearest = factoryCameras[i];
                distance = (factoryCameras[i].gameObject.transform.position - position).magnitude;
            }
        }
        return nearest;
    }

    public int getHighestUnlockedType()
    {
        int highest = 0;
        for (int i = 0; i < upgradeRessources.Length; i++)
        {
            if (upgradeRessources[i] == true)
            {
                highest = i;
            }
        }
        return Mathf.Min(highest, 9);
    }

    public int getHighestUnlockedQteType()
    {
        switch (Manager.m.getHighestUnlockedType())
        {
            case 0:
                return 4;
            case 1:
                return 4;
            case 2:
                return 8;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                return 8 + (Manager.m.getHighestUnlockedType() - 1) * 2;
            case 8:
            case 9:
                return 24;
            default:
                print("highestUnlockedType is <" + Manager.m.getHighestUnlockedType() + "> and out of bounds");
                return -1;
        }
    }

    public int getCurrentFactoryHall()
    {
        for (int i = 0; i < Manager.m.factoryCameras.Length; i++)
        {
            if (lastDropperCamera == Manager.m.factoryCameras[i])
            {
                return i;
            }
        }
        print("Error: last factory camera not found");
        return -1;
    }

    IEnumerator ContinueTime()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1.0f;
    }


    public void Exit()
    {
        print("Exit");
        Application.Quit();
    }
}
