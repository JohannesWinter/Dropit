using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using TMPro;

public class FactoryButtons : MonoBehaviour
{
    public GameObject overlay;

    public GameObject shop1Button;
    public GameObject doneButton;
    public GameObject shop2Button;
    public GameObject editOffButton;
    public GameObject editOnButton;

    public GameObject undoButtonEnabled;
    public GameObject redoButtonEnabled;
    public GameObject undoButtonDisabled;
    public GameObject redoButtonDisabled;
    public GameObject historyButtons;

    public GameObject repairOff;
    public GameObject repairOn;

    public GameObject directionButton;
    public GameObject machineTypeButton;
    public GameObject straightArrowButton;
    public GameObject rightArrowButton;
    public GameObject leftarrowButton;
    public GameObject fuseArrowButton;
    public GameObject splitArrowButton;

    public GameObject marketButton;
    public GameObject missionsButton;
    public GameObject quickTimeEventsButton;
    public GameObject factoryHallsButton;
    public GameObject optionsButton;

    public GameObject changeHallArrows;
    public GameObject changeHallRight;
    public GameObject changeHallDown;
    public GameObject changeHallLeft;
    public GameObject changeHallUp;

    public GameObject market;
    public GameObject missions;
    public GameObject quickTimeEvents;
    public GameObject factoryHalls;
    public GameObject options;

    public Texture lockIcon;
    public Texture factoryHallsIcon;
    public Texture qTEsIcon;
    public Texture repairOffIcon;
    public Texture repairOnIcon;

    public TextMeshProUGUI sellingTMP;
    public TextMeshProUGUI placingTMP;
    int editRotation;
    void Start()
    {
        Manager.m.objectType = "Straight";
        doneButton.SetActive(false);
        editRotation = 180;
        repairOff.SetActive(true);
        repairOn.SetActive(false);

        shop1Button.GetComponent<Button>().onClick.AddListener(Shop1);
        shop2Button.GetComponent<Button>().onClick.AddListener(Shop2);
        editOffButton.GetComponent<Button>().onClick.AddListener(EditOn);
        editOnButton.GetComponent<Button>().onClick.AddListener(EditOff);
        doneButton.GetComponent<Button>().onClick.AddListener(Done);
        undoButtonEnabled.GetComponent<Button>().onClick.AddListener(Undo);
        redoButtonEnabled.GetComponent<Button>().onClick.AddListener(Redo);
        undoButtonDisabled.GetComponent<Button>().onClick.AddListener(NotEnabled);
        redoButtonDisabled.GetComponent<Button>().onClick.AddListener(NotEnabled);


        repairOn.GetComponent<Button>().onClick.AddListener(Repair);
        repairOff.GetComponent<Button>().onClick.AddListener(Repair);
        directionButton.GetComponent<Button>().onClick.AddListener(Direction);
        machineTypeButton.GetComponent<Button>().onClick.AddListener(MachineType);
        marketButton.GetComponent<Button>().onClick.AddListener(Market);
        missionsButton.GetComponent<Button>().onClick.AddListener(Missions);
        quickTimeEventsButton.GetComponent<Button>().onClick.AddListener(QuickTimeEvents);
        factoryHallsButton.GetComponent<Button>().onClick.AddListener(FactoryHalls);
        optionsButton.GetComponent<Button>().onClick.AddListener(Options);

        changeHallRight.GetComponent<Button>().onClick.AddListener(ChangeHallRight);
        changeHallDown.GetComponent<Button>().onClick.AddListener(ChangeHallDown);
        changeHallLeft.GetComponent<Button>().onClick.AddListener(ChangeHallLeft);
        changeHallUp.GetComponent<Button>().onClick.AddListener(ChangeHallUp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gUIScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        float gUIScaleFactory = Manager.m.graphicManager.gUIScaleFactor;
        Vector3 gUIScaleSqr = new Vector3(gUIScaleFactory * gUIScaleFactory, gUIScaleFactory * gUIScaleFactory, gUIScaleFactory * gUIScaleFactory);
        this.gameObject.GetComponent<RectTransform>().localScale = gUIScale;
        changeHallRight.GetComponent<RectTransform>().localScale = gUIScaleSqr;
        changeHallDown.GetComponent<RectTransform>().localScale = gUIScaleSqr;
        changeHallLeft.GetComponent<RectTransform>().localScale = gUIScaleSqr;
        changeHallUp.GetComponent<RectTransform>().localScale = gUIScaleSqr;


        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Settings")))
        {
            if (Manager.m.tutorial.inTutorial == false && Manager.m.tutorial.inTutorial2 == false && Manager.m.tutorial.inTutorial3 == false && Manager.m.introScene.inIntroScene == false && Manager.m.inFinalSequence == false)
            {
                if (Manager.m.inMarket == true)
                {
                    Manager.m.inMarket = false;
                    Manager.m.effectSpeaker.click();
                }
                else if (Manager.m.inFactoryHalls == true)
                {
                    Manager.m.inFactoryHalls = false;
                    Manager.m.effectSpeaker.click();
                }
                else if (Manager.m.inMissions == true)
                {
                    Manager.m.inMissions = false;
                    Manager.m.effectSpeaker.click();
                }
                else if (Manager.m.inShopDropper == true || Manager.m.inShopMachine == true)
                {
                    Manager.m.effectSpeaker.click();
                    Manager.m.shopCamera.backScript.Back();
                }
                else if (Manager.m.inSettings == true)
                {
                    if (options.GetComponent<Settings>().inMainMenue == true)
                    {
                        Manager.m.inSettings = false;
                        Manager.m.effectSpeaker.click();
                        options.GetComponent<Settings>().ask.Cancel();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        options.GetComponent<Settings>().OptionBack();
                        options.GetComponent<Settings>().ask.Cancel();
                    }
                }
                else
                {
                    Options();
                }
            }
        }

        if (Manager.m.tutorial.inTutorial == false && Manager.m.tutorial.inTutorial2 == false && Manager.m.tutorial.inTutorial3 == false && Manager.m.introScene.inIntroScene == false && Manager.m.inFinalSequence == false)
        {
            if ((GameInputManager.GetKeyDown(Manager.m.ActionKey("Shop1")) || GameInputManager.GetKeyDown(Manager.m.ActionKey("Shop2"))) && (Manager.m.editMode_placeDropper || Manager.m.editMode_placeMachine))
            {
                if (Manager.m.inUIMenu() || Manager.m.editMode == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    Done();
                }
            }
            else if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Shop1")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inShopDropper == false || Manager.m.editMode == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inShopDropper == false)
                    {
                        Shop1();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.shopCamera.backScript.Back();
                    }
                }
            }
            else if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Shop2")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inShopMachine == false || Manager.m.editMode == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inShopMachine == false)
                    {
                        Shop2();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.shopCamera.backScript.Back();
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("EditMode")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.editMode == false)
                    {
                        EditOn();
                    }
                    else
                    {
                        EditOff();
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("RepairMode")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    Repair();
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Transform")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType();
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform1")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType(1);
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform2")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType(2);
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform3")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType(3);
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform4")))
            {
                if (Manager.m.inUIMenu())
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType(4);
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform5")))
            {
                if (Manager.m.inUIMenu())
                {
                     //Manager.m.effectSpeaker.error();
                }
                else
                {
                    MachineType(5);
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Market")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inMarket == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inMarket == false)
                    {
                        Market();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.inMarket = false;
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Mission")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inMissions == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inMissions == false)
                    {
                        Missions();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.inMissions = false;
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("QuickTimeEvents")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inQuickTimeEvents == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inQuickTimeEvents == false)
                    {
                        QuickTimeEvents();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.inQuickTimeEvents = false;
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHalls")))
            {
                if (Manager.m.inUIMenu() && Manager.m.inFactoryHalls == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    if (Manager.m.inFactoryHalls == false)
                    {
                        FactoryHalls();
                    }
                    else
                    {
                        Manager.m.effectSpeaker.click();
                        Manager.m.inFactoryHalls = false;
                    }
                }
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Undo")))
            {
                if (Manager.m.inUIMenu() || Manager.m.editMode == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    Undo();
                }
            }
            else if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Redo")))
            {
                if (Manager.m.inUIMenu() || Manager.m.editMode == false)
                {
                    //Manager.m.effectSpeaker.error();
                }
                else
                {
                    Redo();
                }
            }
        }

        machineTypeButton.transform.rotation = Quaternion.Euler(0, 0, (Manager.m.dropperRotation + editRotation) * -1);

        //locked Buttons
        if (Manager.m.acessFactoryHalls == true)
        {
            factoryHallsButton.GetComponent<RawImage>().texture = factoryHallsIcon;
        }
        else
        {
            factoryHallsButton.GetComponent<RawImage>().texture = lockIcon;
        }
        if (Manager.m.acessQTEs == true)
        {
            quickTimeEventsButton.GetComponent<RawImage>().texture = qTEsIcon;
        }
        else
        {
            quickTimeEventsButton.GetComponent<RawImage>().texture = lockIcon;
        }
        if (Manager.m.acessRepair == true)
        {
            repairOff.GetComponent<RawImage>().texture = repairOffIcon;
            repairOn.GetComponent<RawImage>().texture = repairOnIcon;
        }
        else
        {
            repairOff.GetComponent<RawImage>().texture = lockIcon;
            repairOn.GetComponent<RawImage>().texture = lockIcon;
        }



        if (Manager.m.inMarket == true)
        {
            market.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor - 0.2f, Manager.m.graphicManager.gUIScaleFactor - 0.2f, Manager.m.graphicManager.gUIScaleFactor - 0.2f);
            market.SetActive(true);
        }
        else
        {
            market.SetActive(false);
        }

        if (Manager.m.hideFactoryButtons == true)
        {
            overlay.SetActive(false);

            shop1Button.SetActive(false);
            shop2Button.SetActive(false);
            editOffButton.SetActive(false);
            editOnButton.SetActive(false);
            historyButtons.SetActive(false);
            repairOn.SetActive(false);
            repairOff.SetActive(false);
            directionButton.SetActive(false);
            machineTypeButton.SetActive(false);
            marketButton.SetActive(false);
            missionsButton.SetActive(false);
            quickTimeEventsButton.SetActive(false);
            factoryHallsButton.SetActive(false);
            optionsButton.SetActive(false);
            doneButton.SetActive(false);
            sellingTMP.gameObject.SetActive(false);
            placingTMP.gameObject.SetActive(false);
            changeHallArrows.SetActive(false);

        }
        else
        {
            overlay.SetActive(true);

            editOffButton.SetActive(true);
            repairOn.SetActive(true);
            repairOff.SetActive(true);
            marketButton.SetActive(true);
            quickTimeEventsButton.SetActive(true);
            factoryHallsButton.SetActive(true);
            missionsButton.SetActive (true);
            optionsButton.SetActive(true);
            changeHallArrows.SetActive(true);

            if (Manager.m.repairMode == false)
            {
                repairOn.SetActive(false);
                repairOff.SetActive(true);
            }
            else
            {
                repairOn.SetActive(true);
                repairOff.SetActive(false);
            }

            int currentHall = Manager.m.getCurrentFactoryHall() + 1;
            if (currentHall != 5 && currentHall != 10 && Manager.m.level >= currentHall + 1)
            {
                changeHallRight.SetActive(true);
            }
            else
            {
                changeHallRight.SetActive(false);
            }
            if (currentHall != 1 && currentHall != 6)
            {
                changeHallLeft.SetActive(true);
            }
            else
            {
                changeHallLeft.SetActive(false);
            }
            if (currentHall >= 1 && currentHall <= 5 && Manager.m.level >= currentHall + 5)
            {
                changeHallDown.SetActive(true);
            }
            else
            {
                changeHallDown.SetActive(false);
            }
            if (currentHall >= 6 && currentHall <= 10)
            {
                changeHallUp.SetActive(true);
            }
            else
            {
                changeHallUp.SetActive(false);
            }

            if (Manager.m.editMode)
            {
                editOnButton.SetActive(true);
                editOffButton.SetActive(false);
                historyButtons.SetActive(true);
                if (Manager.m.editHistoryManager.Undo(false))
                {
                    undoButtonEnabled.SetActive(true);
                    undoButtonDisabled.SetActive(false);
                }
                else
                {
                    undoButtonEnabled.SetActive(false);
                    undoButtonDisabled.SetActive(true);
                }
                if (Manager.m.editHistoryManager.Redo(false))
                {
                    redoButtonEnabled.SetActive(true);
                    redoButtonDisabled.SetActive(false);
                }
                else
                {
                    redoButtonEnabled.SetActive(false);
                    redoButtonDisabled.SetActive(true);
                }
                if (Manager.m.editMode_placeDropper == true || Manager.m.editMode_placeMachine)
                {
                    shop1Button.SetActive(false);
                    shop2Button.SetActive(false);
                    doneButton.SetActive(true);
                    sellingTMP.gameObject.SetActive(false);
                    if (Manager.m.editMode_placeDropper)
                    {
                        placingTMP.text = "Building: " + Manager.m.dropperIdentifications[Manager.m.dropperNumber - 1];
                    }
                    else
                    {
                        placingTMP.text = "Building: " + Manager.m.machineIdentifications[Manager.m.machineNumber - 1];
                    }
                    placingTMP.gameObject.SetActive(true);

                }
                else
                {
                    shop1Button.SetActive(true);
                    shop2Button.SetActive(true);
                    doneButton.SetActive(false);
                    sellingTMP.gameObject.SetActive(true);
                    placingTMP.gameObject.SetActive (false);
                }
                if (Manager.m.editMode_placeDropper == false && Manager.m.editMode_placeMachine == false)
                {
                    Manager.m.editMode_sell = true;
                }
                else
                {
                    Manager.m.editMode_sell = false;
                }
                if (Manager.m.editMode_placeDropper || Manager.m.editMode_placeMachine)
                {
                    if ((Manager.m.editMode_placeMachine && (Manager.m.machineNumber == 1 || Manager.m.machineNumber == 4 || Manager.m.machineNumber == 7)) == false)
                    {
                        MachineType(1);
                    }
                    directionButton.SetActive(true);
                    machineTypeButton.SetActive(true);
                }
                else
                {
                    directionButton.SetActive(false);
                    machineTypeButton.SetActive(false);
                }
            }
            else
            {
                editOnButton.SetActive(false);
                editOffButton.SetActive(true);
                shop1Button.SetActive(false);
                doneButton.SetActive(false);
                shop2Button.SetActive(false);
                directionButton.SetActive(false);
                machineTypeButton.SetActive(false);
                historyButtons.SetActive(false);
                sellingTMP.gameObject.SetActive(false);
                placingTMP.gameObject.SetActive(false);
                Manager.m.editMode_sell = false;
                Manager.m.editMode_placeDropper = false;
                Manager.m.editMode_placeMachine = false;
            }
        }
    }
    void Shop1()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.setKamera(0, 1);
        Manager.m.inShopDropper = true;
        Manager.m.editMode_placeDropper = false;
        Manager.m.editMode_placeMachine = false;
    }
    void Shop2()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.setKamera(0, 1);
        Manager.m.inShopMachine = true;
        Manager.m.editMode_placeDropper = false;
        Manager.m.editMode_placeMachine = false;
    }
    void EditOn()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.editMode = true;
        Manager.m.editHistoryManager.ResetEditHistory();
    }
    void EditOff()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.editMode = false;
        Manager.m.editHistoryManager.ResetEditHistory();
    }
    void Done()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.editMode_placeDropper = false;
        Manager.m.editMode_placeMachine = false;
    }
    void Repair()
    {
        if (Manager.m.editMode_placeDropper || Manager.m.editMode_placeMachine || Manager.m.acessRepair == false)
        {
            Manager.m.effectSpeaker.error();
        }
        else
        {
            Manager.m.effectSpeaker.click();
            if (Manager.m.repairMode == false)
            {
                Manager.m.repairMode = true;
                repairOn.SetActive(true);
                repairOff.SetActive(false);
            }
            else
            {
                Manager.m.repairMode = false;
                repairOn.SetActive(false);
                repairOff.SetActive(true);
            }
        }
    }
    void Direction()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.dropperRotation += 90;
    }
    void MachineType()
    {
        if (Manager.m.editMode_placeMachine && (Manager.m.machineNumber == 1 || Manager.m.machineNumber == 4 || Manager.m.machineNumber == 7))
        {
            Manager.m.effectSpeaker.click();
            if (straightArrowButton.activeSelf == true)
            {
                straightArrowButton.SetActive(false);
                rightArrowButton.SetActive(true);
                leftarrowButton.SetActive(false);
                fuseArrowButton.SetActive(false);
                splitArrowButton.SetActive(false);
                Manager.m.objectType = "Right";
                editRotation = 180 - 90;
            }
            else if (rightArrowButton.activeSelf == true)
            {
                straightArrowButton.SetActive(false);
                rightArrowButton.SetActive(false);
                leftarrowButton.SetActive(true);
                fuseArrowButton.SetActive(false);
                splitArrowButton.SetActive(false);
                Manager.m.objectType = "Left";
                editRotation = 180 + 90;
            }
            else if (leftarrowButton.activeSelf == true)
            {
                straightArrowButton.SetActive(false);
                rightArrowButton.SetActive(false);
                leftarrowButton.SetActive(false);
                fuseArrowButton.SetActive(true);
                splitArrowButton.SetActive(false);
                Manager.m.objectType = "Fuse";
                editRotation = 180;
            }
            else if (fuseArrowButton.activeSelf == true)
            {
                straightArrowButton.SetActive(false);
                rightArrowButton.SetActive(false);
                leftarrowButton.SetActive(false);
                fuseArrowButton.SetActive(false);
                splitArrowButton.SetActive(true);
                Manager.m.objectType = "Split";
                editRotation = 180;
            }
            else
            {
                straightArrowButton.SetActive(true);
                rightArrowButton.SetActive(false);
                leftarrowButton.SetActive(false);
                fuseArrowButton.SetActive(false);
                splitArrowButton.SetActive(false);
                Manager.m.objectType = "Straight";
                editRotation = 180;
            }
        }
        else
        {
            print("A");
            Manager.m.effectSpeaker.error();
        }
    }

    void MachineType(int i)
    {
        if ((Manager.m.editMode_placeMachine && (Manager.m.machineNumber == 1 || Manager.m.machineNumber == 4 || Manager.m.machineNumber == 7)) || i == 1)
        {
            switch (i)
            {
                case 1:
                    {
                        straightArrowButton.SetActive(true);
                        rightArrowButton.SetActive(false);
                        leftarrowButton.SetActive(false);
                        fuseArrowButton.SetActive(false);
                        splitArrowButton.SetActive(false);
                        Manager.m.objectType = "Straight";
                        editRotation = 180;
                        break;
                    }
                case 2:
                    {
                        straightArrowButton.SetActive(false);
                        rightArrowButton.SetActive(true);
                        leftarrowButton.SetActive(false);
                        fuseArrowButton.SetActive(false);
                        splitArrowButton.SetActive(false);
                        Manager.m.objectType = "Right";
                        editRotation = 180 - 90;
                        break;
                    }
                case 3:
                    {
                        straightArrowButton.SetActive(false);
                        rightArrowButton.SetActive(false);
                        leftarrowButton.SetActive(true);
                        fuseArrowButton.SetActive(false);
                        splitArrowButton.SetActive(false);
                        Manager.m.objectType = "Left";
                        editRotation = 180 + 90;
                        break;
                    }
                case 4:
                    {
                        straightArrowButton.SetActive(false);
                        rightArrowButton.SetActive(false);
                        leftarrowButton.SetActive(false);
                        fuseArrowButton.SetActive(true);
                        splitArrowButton.SetActive(false);
                        Manager.m.objectType = "Fuse";
                        editRotation = 180;
                        break;
                    }
                case 5:
                    {
                        straightArrowButton.SetActive(false);
                        rightArrowButton.SetActive(false);
                        leftarrowButton.SetActive(false);
                        fuseArrowButton.SetActive(false);
                        splitArrowButton.SetActive(true);
                        Manager.m.objectType = "Split";
                        editRotation = 180;
                        break;
                    }
                default:
                    {
                        print("Error - Tried to access machine type <" + i + ">");
                        break;
                    }
            }
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }

    void Undo()
    {
        if (Manager.m.editHistoryManager.Undo(false))
        {
            Manager.m.effectSpeaker.click();
            Manager.m.editHistoryManager.Undo(true);
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Redo()
    {
        if (Manager.m.editHistoryManager.Redo(false))
        {
            Manager.m.effectSpeaker.click();
            Manager.m.editHistoryManager.Redo(true);
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Market()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inMarket = true;
    }

    void Missions()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inMissions = true;
    }
    void QuickTimeEvents()
    {
        if (Manager.m.acessQTEs == true)
        {
            Manager.m.effectSpeaker.click();
            Manager.m.inQuickTimeEvents = true;
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void FactoryHalls()
    {
        if (Manager.m.acessFactoryHalls == true)
        {
            Manager.m.effectSpeaker.click();
            Manager.m.inFactoryHalls = true;
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Options()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inSettings = true;
        options.GetComponent<Settings>().OptionBack();
    }
    void NotEnabled()
    {
        Manager.m.effectSpeaker.error();
    }

    void ChangeHallRight()
    {
        int currentHall = Manager.m.getCurrentFactoryHall() + 1;
        if (currentHall != 5 && currentHall != 10)
        {
            if (changeHallRight.activeSelf == false)
            {
                return;
            }
            Manager.m.factoryHallsManager.setHall(currentHall + 1);
        }
        else
        {
            Debug.Log("Error: cant go right in factory hall " + currentHall);
        }
    }
    void ChangeHallDown()
    {
        int currentHall = Manager.m.getCurrentFactoryHall() + 1;
        if (currentHall >= 1 && currentHall <= 5)
        {
            if (changeHallDown.activeSelf == false)
            {
                return;
            }
            Manager.m.factoryHallsManager.setHall(currentHall + 5);
        }
        else
        {
            Debug.Log("Error: cant go down in factory hall " + currentHall);
        }
    }
    void ChangeHallLeft()
    {
        int currentHall = Manager.m.getCurrentFactoryHall() + 1;
        if (currentHall != 1 && currentHall != 6)
        {
            if (changeHallLeft.activeSelf == false)
            {
                return;
            }
            Manager.m.factoryHallsManager.setHall(currentHall - 1);
        }
        else
        {
            Debug.Log("Error: cant go left in factory hall " + currentHall);
        }
    }
    void ChangeHallUp()
    {
        int currentHall = Manager.m.getCurrentFactoryHall() + 1;
        if (currentHall >= 6 && currentHall <= 10)
        {
            if (changeHallUp.activeSelf == false)
            {
                return;
            }
            Manager.m.factoryHallsManager.setHall(currentHall - 5);
        }
        else
        {
            Debug.Log("Error: cant go up in factory hall " + currentHall);
        }
    }
}
