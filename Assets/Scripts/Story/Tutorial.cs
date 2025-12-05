using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool enabledTutorials;
    public int inTutorial;
    public bool pauseGame;
    public Transform tutorial1;
    public GameObject[] tutorialObjects;
    List<Button> allowedButtons = new List<Button>();
    List<GameObject> blinkingUIs = new List<GameObject>();
    public float minWait;
    float currentStepTime;

    //Testing
    public bool testStartTutorial;
    public int testTutorialNumber;
    public bool testNextStep;



    public GameObject[][] allTutorialSteps;
    private void Start()
    {
        pauseGame = false;
        inTutorial = 0;
        allTutorialSteps = new GameObject[1][];

        allTutorialSteps[0] = GetChildren(tutorial1);
        for (int i = 0; i < allTutorialSteps.Length;i++)
        {
            for (int x = 0; x < allTutorialSteps[i].Length; x++)
            {
                allTutorialSteps[i][x].SetActive(false);
            }
        }
    }

    GameObject[] GetChildren(Transform parent)
    {
        GameObject[] children = new GameObject[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i).gameObject;
        }
        return children;
    }
    private void Update()
    {
        if (inTutorial != 0 && minWait <= currentStepTime)
        {
            CheckTutorial();
        }
        currentStepTime += Time.unscaledDeltaTime;

        if (testStartTutorial)
        {
            testStartTutorial = false;
            StartCoroutine(StartTutorial(testTutorialNumber));
        }
        if (testNextStep)
        {
            testNextStep = false;
            NextStep(allTutorialSteps[testTutorialNumber - 1]);
        }
    }
    public bool CheckActiveStep(GameObject step)
    {
        return step.activeSelf;
    }
    
    void CheckTutorial()
    {
        for (int i = 0; i < allTutorialSteps.Length; i++)
        {
            for (int x = 0; x < allTutorialSteps[i].Length; x++)
            {
                if (CheckActiveStep(allTutorialSteps[i][x]))
                {
                    if (CheckTutorialSwitch(i, x))
                    {
                        break;
                    }
                }
            }
        }
    }
    bool CheckTutorialSwitch(int tutorial, int step)
    {
        if (tutorial < 0)
        {
            Debug.Log("Error - tutorial <" + tutorial + "> does not exist");
        }
        bool switched = false;

        ActOnStep(tutorial, step, TutorialActType.onRepeat);
        if (ActOnStep(tutorial, step, TutorialActType.switchCond))
        {
            ActOnStep(tutorial, step, TutorialActType.onEnd);
            if (allTutorialSteps[tutorial].Length > step + 1)
            {
                ActOnStep(tutorial, step + 1, TutorialActType.onStart);
            }
            switched = NextStep(allTutorialSteps[tutorial]);
        }
        return switched;
    }

    bool ActOnStep(int tutorialNr, int stepNr, TutorialActType act)
    {
        GameObject step = allTutorialSteps[tutorialNr][stepNr];
        switch (tutorialNr)
        {
            case 0:
                {
                    switch (stepNr)
                    {
                        case 0:
                            {
                                if (act == TutorialActType.onStart)
                                {
                                    Manager.m.effectSpeaker.beep();
                                    StartBlinkUI(Manager.m.factoryButtons.marketButton);
                                    AddAllowedButton(Manager.m.factoryButtons.marketButton);
                                }
                                else if (act == TutorialActType.onRepeat)
                                {

                                }
                                else if (act == TutorialActType.onEnd)
                                {
                                    StopAllBlinkingUIs();
                                    RemoveAllowedButton(Manager.m.factoryButtons.marketButton);     
                                }
                                else if (act == TutorialActType.switchCond)
                                {
                                    return Manager.m.inMarket == true;
                                }
                                break;
                            }
                        case 1:
                            {
                                if (act == TutorialActType.onStart)
                                {
                                    Manager.m.effectSpeaker.beep();
                                }
                                else if (act == TutorialActType.onRepeat)
                                {

                                }
                                else if (act == TutorialActType.onEnd)
                                {
                                    Manager.m.effectSpeaker.accept();
                                }
                                else if (act == TutorialActType.switchCond)
                                {
                                    return Input.GetButtonDown("ClickLeft");
                                }
                                break;
                            }
                    }
                    break;
                }
        }
        return false;
    }

    bool NextStep(GameObject[] tutorialSteps)
    {
        currentStepTime = 0;
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if (tutorialSteps[i].activeSelf)
            {
                if (i < tutorialSteps.Length - 1)
                {
                    tutorialSteps[i + 1].SetActive(true);
                    tutorialSteps[i].SetActive(false);
                }
                else
                {
                    inTutorial = 0;
                    pauseGame = false;
                    tutorialSteps[i].SetActive(false);
                }
                break;
            }
        }
        return true;
    }

    public IEnumerator StartTutorial(int number)
    {
        if (number <= 0) 
        {
            Debug.Log("Error - Tutorial <" +  number + "> not existent");
            yield break;
        }
        while (inTutorial != 0)
        {
            if (inTutorial == number)
            {
                yield break;
            }
            yield return null;
        }
        currentStepTime = 0;
        inTutorial = number;
        allTutorialSteps[number-1][0].SetActive(true);
        ActOnStep(number - 1, 0, TutorialActType.onStart);
    }

    IEnumerator StartBlinkUIRoutine(GameObject blinking)
    {
        if (blinkingUIs.Contains(blinking))
        {
            yield break;
        }
        blinkingUIs.Add(blinking);
        Image toBlinkImage = blinking.GetComponent<Image>();
        if (toBlinkImage == null)
        {
            Debug.Log("Error - Gameobject <" + blinking.transform.parent.name + "/" + blinking.name + "> does not contain <Image> Component");
            yield break;
        }
        float a = toBlinkImage.color.a;

        while (blinkingUIs.Contains(blinking))
        {
            for (int i = 0; i < 10; i++)
            {
                toBlinkImage.color = toBlinkImage.color - new Color(0.06f, 0.06f, 0.06f, 0);
                if (blinkingUIs.Contains(blinking) == false)
                {
                    toBlinkImage.color = new Color(1, 1, 1, a);
                    yield break;
                }
                yield return new WaitForSecondsRealtime(0.025f);
            }
            for (int i = 0; i < 10; i++)
            {
                toBlinkImage.color = toBlinkImage.color + new Color(0.06f, 0.06f, 0.06f, 0);
                if (blinkingUIs.Contains(blinking) == false)
                {
                    toBlinkImage.color = new Color(1, 1, 1, a);
                    yield break;
                }
                yield return new WaitForSecondsRealtime(0.025f);
            }
        }
    }

    Coroutine StartBlinkUI(GameObject blinking)
    {
        if (blinkingUIs.Contains(blinking))
        {
            return null;
        }
        return StartCoroutine(StartBlinkUIRoutine(blinking));
    }

    bool StopBlinkUI(GameObject blinking)
    {
        if (blinkingUIs.Contains(blinking))
        {
            blinkingUIs.Remove(blinking);
            return true;
        }
        return false;
    }

    bool StopAllBlinkingUIs()
    {
        if (blinkingUIs.Count > 0)
        {
            for (int i = blinkingUIs.Count - 1; i >= 0; i--)
            {
                blinkingUIs.RemoveAt(i);
            }
            return true;
        }
        return false;
    }

    bool WaitForTime(float minTime)
    {
        if (currentStepTime > minTime)
        {
            return true;
        }
        return false;
    }

    public bool IsButtonAllowed(GameObject button)
    {
        if (allowedButtons.Contains(button.GetComponent<Button>()) || allowedButtons.Count == 0)
        {
            return true;
        }
        return false;
    }
    public bool IsButtonAllowed(Button button)
    {
        if (allowedButtons.Contains(button) || allowedButtons.Count == 0)
        {
            return true;
        }
        return false;
    }

    bool AddAllowedButton(GameObject button)
    {
        if (button == null || button.GetComponent<Button>() == null)
        {
            Debug.Log("Error - Gameobject <" + button.transform.parent.name + "/" + button.name + "> has no <Button> component or is null");
            return false;
        }
        if (allowedButtons.Contains(button.GetComponent<Button>()) == false)
        {
            allowedButtons.Add(button.GetComponent<Button>());
            return true;
        }
        return false;
    }

    bool AddAllowedButton(Button button)
    {
        if (button == null)
        {
            Debug.Log("Error - Button <" + button.transform.parent.name + "/" + button.name + "> is null");
            return false;
        }
        if (allowedButtons.Contains(button) == false)
        {
            allowedButtons.Add(button);
            return true;
        }
        return false;
    }

    bool RemoveAllowedButton(GameObject button) 
    {
        if (button == null || button.GetComponent<Button>() == null)
        {
            Debug.Log("Error - Gameobject <" + button.transform.parent.name + "/" + button.name + "> has no <Button> component or is null");
            return false;
        }
        if (allowedButtons.Contains(button.GetComponent<Button>()) == true)
        {
            allowedButtons.Remove(button.GetComponent<Button>());
            return true;
        }
        return false;
    }

    bool RemoveAllowedButton(Button button)
    {
        if (button == null)
        {
            Debug.Log("Error - Button <" + button.transform.parent.name + "/" + button.name + "> is null");
            return false;
        }
        if (allowedButtons.Contains(button) == true)
        {
            allowedButtons.Remove(button);
            return true;
        }
        return false;
    }

    bool ResetAllowedButtons()
    {
        if (allowedButtons.Count == 0)
        {
            return false;
        }
        allowedButtons.Clear();
        return true;
    }

































    //public bool inTutorial2;
    //public bool inTutorial3;
    //public bool finishedTutorial2;
    //public bool finishedTutorial3;
    //public GameObject drop1Example;
    //public GameObject drop2Example;
    //public RawImage[] lastSignalingObject;
    //public RawImage[] signalingObject;
    //float currenttime;
    //float waitingTime;
    //public float colorCounter;
    //bool startedRotation = false;
    //public RawImage final;
    //public GameObject activeStep;
    //public GameObject[] steps;
    //public GameObject[] stepObjects;
    //public GameObject activeStep2;
    //public GameObject[] steps2;
    //public GameObject[] stepObjects2;
    //public GameObject activeStep3;
    //public GameObject[] steps3;
    //GameObject tutorial2Trigger;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    currenttime = Time.time;
    //    signalingObject = new RawImage[10];
    //    lastSignalingObject = new RawImage[10];
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (inTutorial == true || inTutorial2 == true || inTutorial3 == true)
    //    {
    //        for (int i = 0; i < lastSignalingObject.Length; i++)
    //        {
    //            if (lastSignalingObject[i] != null)
    //            {
    //                lastSignalingObject[i].color = Color.white;
    //                lastSignalingObject[i] = null;
    //            }
    //        }
    //    }
    //    if (signalingObject != null)
    //    {
    //        if (currenttime < Time.time)
    //        {
    //            currenttime = Time.time + 0.05f;
    //            if (colorCounter <= 500)
    //            {
    //                for (int i = 0; i < signalingObject.Length; i++)
    //                {
    //                    if (signalingObject[i] != null)
    //                    {
    //                        signalingObject[i].color = new Color(colorCounter / 1000 + 0.5f, colorCounter / 1000 + 0.5f, colorCounter / 1000 + 0.5f, signalingObject[i].color.a);
    //                    }
    //                }
    //                colorCounter += 50;
    //            }
    //            else if (colorCounter <= 1000)
    //            {
    //                for (int i = 0; i < signalingObject.Length; i++)
    //                {
    //                    if (signalingObject[i] != null)
    //                    {
    //                        signalingObject[i].color = new Color((1000 - colorCounter) / 1000 + 0.5f, (1000 - colorCounter) / 1000 + 0.5f, (1000 - colorCounter) / 1000 + 0.5f, signalingObject[i].color.a);
    //                    }
    //                }
    //                colorCounter += 50;
    //            }
    //            else if (colorCounter > 1000)
    //            {   
    //                colorCounter = 0;
    //            }
    //        }
    //    }


    //    //Tutorial 1
    //    if (inTutorial == true)
    //    {
    //        for (int i = 0; i < steps.Length; i++)
    //        {
    //            if (steps[i].activeSelf == true)
    //            {
    //                activeStep = steps[i];
    //                break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        activeStep = null;
    //    }

    //    for(int i = 0; i < steps.Length;i++)
    //    {
    //        if (stepObjects[i] != null)
    //        {
    //            if (steps[i].activeSelf == true)
    //            {
    //                stepObjects[i].SetActive(true);
    //            }
    //            else
    //            {
    //                stepObjects[i].SetActive(false);
    //            }
    //        }
    //    }
    //    if (inTutorial == true)
    //    {
    //        if (steps[35].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[35].SetActive(false);
    //                steps[36].SetActive(true);
    //                waitingTime = Time.time + 0.05f;
    //            }
    //        }
    //        if (steps[36].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[36].SetActive(false);
    //                steps[37].SetActive(true);
    //                Signal(Manager.m.factoryButtons.editOffButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[37].activeSelf == true)
    //        {
    //            if (Manager.m.editMode == true)
    //            {
    //                steps[37].SetActive(false);
    //                steps[38].SetActive(true);
    //                StopSignal(1);
    //                Signal(GameObject.Find("tutorial_step(4)_marker(1)").GetComponent<RawImage>(), 1);
    //                Signal(GameObject.Find("tutorial_step(4)_marker(2)").GetComponent<RawImage>(), 2);
    //                Signal(GameObject.Find("tutorial_step(4)_marker(3)").GetComponent<RawImage>(), 3);
    //            }
    //        }
    //        if (steps[38].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 401)
    //            {
    //                steps[38].SetActive(false);
    //                steps[39].SetActive(true);
    //                StopSignal(1);
    //                StopSignal(2);
    //                StopSignal(3);
    //                Signal(Manager.m.factoryButtons.doneButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[39].activeSelf == true)
    //        {
    //            if (Manager.m.editMode == false)
    //            {
    //                steps[39].SetActive(false);
    //                steps[1].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.shop1Button.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[1].activeSelf == true)
    //        {
    //            if (Manager.m.inShopDropper == true)
    //            {
    //                steps[1].SetActive(false);
    //                steps[2].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.buyButton.GetComponentInChildren<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[2].activeSelf == true)
    //        {
    //            if (Manager.m.editMode_placeDropper == true)
    //            {
    //                steps[2].SetActive(false);
    //                steps[3].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[3].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 201)
    //            {
    //                steps[3].SetActive(false);
    //                steps[4].SetActive(true);
    //                Signal(Manager.m.factoryButtons.doneButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[4].activeSelf == true)
    //        {
    //            if (Manager.m.editMode_placeDropper == false)
    //            {
    //                steps[4].SetActive(false);
    //                steps[5].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.shop2Button.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[5].activeSelf == true)
    //        {
    //            if (Manager.m.inShopMachine == true)
    //            {
    //                steps[5].SetActive(false);
    //                steps[6].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.buyButton.GetComponentInChildren<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[6].activeSelf == true)
    //        {
    //            if (Manager.m.editMode_placeMachine == true)
    //            {
    //                steps[6].SetActive(false);
    //                steps[7].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[7].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 161)
    //            {
    //                steps[7].SetActive(false);
    //                steps[8].SetActive(true);
    //                Signal(Manager.m.factoryButtons.straightArrowButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[8].activeSelf == true)
    //        {
    //            if (Manager.m.objectType == "Right")
    //            {
    //                steps[8].SetActive(false);
    //                steps[9].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.directionButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[9].activeSelf == true)
    //        {
    //            if (Manager.m.dropperRotation == 270)
    //            {
    //                steps[9].SetActive(false);
    //                steps[10].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[10].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 151)
    //            {
    //                steps[10].SetActive(false);
    //                steps[11].SetActive(true);
    //                Signal(Manager.m.factoryButtons.rightArrowButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[11].activeSelf == true)
    //        {
    //            if (Manager.m.objectType == "Left")
    //            {
    //                steps[11].SetActive(false);
    //                steps[12].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.directionButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[12].activeSelf == true)
    //        {
    //            if (Manager.m.dropperRotation == 90)
    //            {
    //                steps[12].SetActive(false);
    //                steps[13].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[13].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 141)
    //            {
    //                steps[13].SetActive(false);
    //                steps[14].SetActive(true);
    //                Signal(Manager.m.factoryButtons.leftarrowButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[14].activeSelf == true)
    //        {
    //            if (Manager.m.objectType == "Fuse")
    //            {
    //                steps[14].SetActive(false);
    //                steps[15].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.directionButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[15].activeSelf == true)
    //        {
    //            if (Manager.m.dropperRotation == 180)
    //            {
    //                steps[15].SetActive(false);
    //                steps[16].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[16].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 131)
    //            {
    //                steps[16].SetActive(false);
    //                steps[17].SetActive(true);
    //                Signal(Manager.m.factoryButtons.doneButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[17].activeSelf == true)
    //        {
    //            if (Manager.m.editMode_placeMachine == false)
    //            {
    //                steps[17].SetActive(false);
    //                steps[18].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.shop2Button.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[18].activeSelf == true)
    //        {
    //            if (Manager.m.inShopMachine)
    //            {
    //                steps[18].SetActive(false);
    //                steps[19].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.shopCamera.rightArrow.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[19].activeSelf == true)
    //        {
    //            if (Manager.m.machineNumber == 2)
    //            {
    //                steps[19].SetActive(false);
    //                steps[20].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.buyButton.GetComponentInChildren<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[20].activeSelf == true)
    //        {
    //            if (Manager.m.editMode_placeMachine == true)
    //            {
    //                steps[20].SetActive(false);
    //                steps[21].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.directionButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[21].activeSelf == true)
    //        {
    //            if (Manager.m.dropperRotation == 0)
    //            {
    //                steps[21].SetActive(false);
    //                steps[22].SetActive(true);
    //                StopSignal(1);
    //            }
    //        }
    //        if (steps[22].activeSelf == true)
    //        {
    //            if (Manager.m.money <= 31)
    //            {
    //                steps[22].SetActive(false);
    //                steps[23].SetActive(true);
    //                Signal(Manager.m.factoryButtons.marketButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[23].activeSelf == true)
    //        {
    //            if (Manager.m.inMarket == true)
    //            {
    //                steps[23].SetActive(false);
    //                steps[24].SetActive(true);
    //                StopSignal(1);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[24].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[24].SetActive(false);
    //                steps[25].SetActive(true);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[25].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[25].SetActive(false);
    //                steps[26].SetActive(true);
    //                Signal(Manager.m.marketManager.exit.gameObject.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[26].activeSelf == true)
    //        {
    //            if (Manager.m.inMarket == false)
    //            {
    //                steps[26].SetActive(false);
    //                steps[27].SetActive(true);
    //                StopSignal(1);
    //                Signal(Manager.m.factoryButtons.missionsButton.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[27].activeSelf == true)
    //        {
    //            if (Manager.m.inMissions == true)
    //            {
    //                steps[27].SetActive(false);
    //                steps[28].SetActive(true);
    //                StopSignal(1);
    //                if(Manager.m.missionManager.missions.Count == 0)
    //                {
    //                    Manager.m.missionManager.createMission();
    //                }
    //                for (int i = 0; i < Manager.m.missionManager.missions.Count; i++)
    //                {
    //                    Manager.m.missionManager.missions[i].GetComponent<Mission>().time = 3600;
    //                }
    //                Signal(Manager.m.missionManager.missions[0].GetComponent<Mission>().acceptMission.gameObject.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[28].activeSelf == true)
    //        {
    //            if (Manager.m.missionManager.missions[0].GetComponent<Mission>().acceptedMission == true || Manager.m.missionManager.missions[0].GetComponent<Mission>().time < 2)
    //            {
    //                steps[28].SetActive(false);
    //                steps[29].SetActive(true);
    //                StopSignal(1);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[29].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[29].SetActive(false);
    //                steps[30].SetActive(true);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[30].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[30].SetActive(false);
    //                steps[31].SetActive(true);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[31].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[31].SetActive(false);
    //                steps[32].SetActive(true);
    //                waitingTime = Time.time + 0.5f;
    //            }
    //        }
    //        if (steps[32].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                steps[32].SetActive(false);
    //                steps[33].SetActive(true);
    //                Signal(Manager.m.missionManager.exitButton.gameObject.GetComponent<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[33].activeSelf == true)
    //        {
    //            if (Manager.m.inMissions == false)
    //            {
    //                StopSignal(1);
    //                steps[33].SetActive(false);
    //                steps[34].SetActive(true);
    //                waitingTime = Time.time + 0.5f;
    //                Signal(steps[34].GetComponentInChildren<RawImage>(), 1);
    //            }
    //        }
    //        if (steps[34].activeSelf == true)
    //        {
    //            if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //            {
    //                StopSignal(1);
    //                steps[34].SetActive(false);
    //                waitingTime = Time.time + 0.5f;
    //                inTutorial = false;
    //            }
    //        }
    //    }


    //    //Tutorial 2
    //    if (finishedTutorial2 == false && inTutorial2 == false && Manager.m.changeSaveTimer == 0 && inTutorial == false && inTutorial3 == false && waitingTime <= Time.time && Manager.m.inUIMenu() == false)
    //    {
    //        GameObject[] factoryObject = GameObject.FindGameObjectsWithTag("FactoryObject");
    //        for (int i = 0; i < factoryObject.Length; i++)
    //        {
    //            if (factoryObject[i].GetComponent<RepairDropper>().durability >= 0 && factoryObject[i].GetComponent<RepairDropper>().durability < 90 && factoryObject[i].GetComponent<RepairDropper>().conveyorBeltSpeed == 0 && factoryObject[i].GetComponent<RepairDropper>().furnaceMultiplier == 0 && factoryObject[i].GetComponent<RepairDropper>().sold == false && factoryObject[i].GetComponent<RepairDropper>().working == true)
    //            {
    //                inTutorial2 = true;
    //                Manager.m.editMode_placeDropper = false;
    //                Manager.m.editMode_placeMachine = false;
    //                Manager.m.editMode = false;
    //                Manager.m.editMode_repair = false;
    //                tutorial2Trigger = factoryObject[i];
    //                steps2[1].SetActive(true);
    //            }
    //        }
    //    }
    //    if (steps2[1].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft"))
    //        {
    //            steps2[1].SetActive(false);
    //            steps2[2].SetActive(true);
    //        }
    //    }
    //    if (steps2[2].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft"))
    //        {
    //            steps2[2].SetActive(false);
    //            steps2[3].SetActive(true);
    //            int nextFactoryCamera = 0;
    //            for(int i = 0; i < Manager.m.factoryCameras.Length; i++)
    //            {
    //                if (Vector3.Distance(Manager.m.factoryCameras[i].gameObject.transform.position, tutorial2Trigger.transform.position) < Vector3.Distance(Manager.m.factoryCameras[nextFactoryCamera].gameObject.transform.position, tutorial2Trigger.transform.position))
    //                {
    //                    nextFactoryCamera = i;
    //                }
    //            }
    //            Manager.m.setKamera(Manager.m.factoryCameras[nextFactoryCamera]);
    //            waitingTime = Time.deltaTime + 0.5f;
    //        }
    //    }
    //    if (steps2[3].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[3].SetActive(false);
    //            steps2[4].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps2[4].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[4].SetActive(false);
    //            steps2[5].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps2[5].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[5].SetActive(false);
    //            steps2[6].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps2[6].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[6].SetActive(false);
    //            steps2[7].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps2[7].activeSelf == true )
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[7].SetActive(false);
    //            steps2[8].SetActive(true);
    //            Signal(Manager.m.factoryButtons.repairOff.GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps2[8].activeSelf == true)
    //    {
    //        if (Manager.m.editMode_repair == true)
    //        {
    //            steps2[8].SetActive(false);
    //            steps2[10].SetActive(true);
    //            StopSignal(1);
    //            Signal(Manager.m.factoryButtons.repairOn.GetComponent<RawImage>(), 2);
    //        }
    //    }
    //    //skip step 9
    //    if (steps2[10].activeSelf == true)
    //    {
    //        if (Manager.m.dropperInformationBox.activeSelf == true || (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time))
    //        {
    //            steps2[10].SetActive(false);
    //            steps2[11].SetActive(true);
    //            StopSignal(2);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps2[11].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps2[11].SetActive(false);
    //            finishedTutorial2 = true;
    //            inTutorial2 = false;
    //        }
    //    }


    //    //Tutorial 3
    //    if (Manager.m.level >= 2 && Manager.m.inMainMenu == false && steps3[1].activeSelf == false && finishedTutorial3 == false && inTutorial == false && inTutorial2 == false && inTutorial3 == false && Manager.m.changeSaveTimer == 0 && Manager.m.inUIMenu() == false)
    //    {
    //        Manager.m.editMode_repair = false;
    //        steps3[1].SetActive(true);
    //        waitingTime = Time.time + 1f;
    //        inTutorial3 = true;
    //    }
    //    if (steps3[1].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[1].SetActive(false);
    //            steps3[10].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //        if (startedRotation == false)
    //        {
    //            startedRotation = true;
    //            StartCoroutine(moveOres());
    //        }
    //    }
    //    if (steps3[10].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[10].SetActive(false);
    //            steps3[11].SetActive(true);
    //            Signal(Manager.m.factoryButtons.quickTimeEventsButton.GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps3[11].activeSelf == true)
    //    {
    //        if (Manager.m.inQuickTimeEvents)
    //        {
    //            StopSignal(1);
    //            steps3[11].SetActive(false);
    //            steps3[12].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps3[12].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[12].SetActive(false);
    //            steps3[13].SetActive(true);
    //        }
    //    }
    //    if (steps3[13].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[13].SetActive(false);
    //            steps3[14].SetActive(true);
    //            Signal(Manager.m.quickTimeEventManager.exitButton.GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps3[14].activeSelf == true)
    //    {
    //        if (Manager.m.inQuickTimeEvents == false)
    //        {
    //            StopSignal(1);
    //            steps3[14].SetActive(false);
    //            steps3[2].SetActive(true);
    //            Signal(Manager.m.factoryButtons.factoryHallsButton.GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps3[2].activeSelf == true)
    //    {
    //        if(Manager.m.inFactoryHalls == true)
    //        {
    //            StopSignal(1);
    //            steps3[2].SetActive(false);
    //            steps3[3].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps3[3].activeSelf == true)
    //    {

    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[3].SetActive(false);
    //            steps3[4].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps3[4].activeSelf == true)
    //    {

    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[4].SetActive(false);
    //            steps3[5].SetActive(true);
    //            Signal(Manager.m.factoryButtons.factoryHalls.GetComponent<FactoryHallsManager>().hall1Button.GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps3[5].activeSelf == true)
    //    {
    //        if (Manager.m.inFactoryHalls == false)
    //        {
    //            StopSignal(1);
    //            steps3[5].SetActive(false);
    //            steps3[6].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //            Signal(GameObject.Find("tutorial3_step(6)_marker").GetComponent<RawImage>(), 1);
    //        }
    //    }
    //    if (steps3[6].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            StopSignal(1);
    //            steps3[6].SetActive(false);
    //            steps3[7].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //            Signal(GameObject.Find("tutorial3_step(7)_marker").GetComponent<RawImage>(), 2);
    //        }
    //    }
    //    if (steps3[7].activeSelf == true)
    //    {
    //        if (Manager.m.upgradeInformationBox.activeSelf && waitingTime <= Time.time)
    //        {
    //            StopSignal(2);
    //            steps3[7].SetActive(false);
    //            steps3[8].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps3[8].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[8].SetActive(false);
    //            steps3[9].SetActive(true);
    //            waitingTime = Time.time + 0.5f;
    //        }
    //    }
    //    if (steps3[9].activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("ClickLeft") && waitingTime <= Time.time)
    //        {
    //            steps3[9].SetActive(false);
    //            inTutorial3 = false;
    //            finishedTutorial3 = true;
    //        }
    //    }
    //}
    //public void StartTutorial()
    //{
    //    inTutorial = true;
    //    Manager.m.dropperRotation = 180;
    //    steps[35].SetActive(true);
    //    Manager.m.shopCamera.ResetCameraPosition();
    //    waitingTime = Time.time + 0.5f;
    //}
    //void Signal(RawImage image, int position)
    //{
    //    signalingObject[position] = image;
    //}
    //void StopSignal(int position)
    //{
    //    lastSignalingObject[position] = signalingObject[position];
    //    signalingObject[position] = null;
    //}

    //public IEnumerator moveOres()
    //{
    //    drop1Example.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
    //    drop1Example.GetComponent<RectTransform>().localPosition = new Vector3(-160, -120, 0);
    //    drop1Example.SetActive(true);
    //    drop2Example.SetActive(false);
    //    while (drop1Example.GetComponent<RectTransform>().localPosition.x < 0)
    //    {
    //        drop1Example.GetComponent<RectTransform>().localPosition = new Vector3(drop1Example.GetComponent<RectTransform>().localPosition.x + 3, drop1Example.GetComponent<RectTransform>().localPosition.y, drop1Example.GetComponent<RectTransform>().localPosition.z);
    //        drop1Example.GetComponent<RawImage>().color = new Color(1, 1, 1, drop1Example.GetComponent<RawImage>().color.a + 0.1f);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    while (drop1Example.GetComponent<RectTransform>().localPosition.y < -70)
    //    {
    //        drop1Example.GetComponent<RectTransform>().localPosition = new Vector3(drop1Example.GetComponent<RectTransform>().localPosition.x, drop1Example.GetComponent<RectTransform>().localPosition.y + 3, drop1Example.GetComponent<RectTransform>().localPosition.z);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    drop1Example.SetActive(false);
    //    drop2Example.GetComponent<RectTransform>().localScale = new Vector3(1.4f, 1.4f, 1f);
    //    drop2Example.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
    //    drop2Example.GetComponent<RectTransform>().localPosition = new Vector3(0, 30, 0);
    //    drop2Example.SetActive(true);

    //    while (drop2Example.GetComponent<RectTransform>().localScale.x > 1)
    //    {
    //        drop2Example.GetComponent<RectTransform>().localScale = new Vector3(drop2Example.GetComponent<RectTransform>().localScale.x - 0.05f, drop2Example.GetComponent<RectTransform>().localScale.y - 0.05f, 1);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    while (drop2Example.GetComponent<RectTransform>().localPosition.x < 140)
    //    {
    //        drop2Example.GetComponent<RectTransform>().localPosition = new Vector3(drop2Example.GetComponent<RectTransform>().localPosition.x + 3, drop2Example.GetComponent<RectTransform>().localPosition.y, drop2Example.GetComponent<RectTransform>().localPosition.z);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    while (drop2Example.GetComponent<RectTransform>().localPosition.x < 160)
    //    {
    //        drop2Example.GetComponent<RectTransform>().localPosition = new Vector3(drop2Example.GetComponent<RectTransform>().localPosition.x + 3, drop2Example.GetComponent<RectTransform>().localPosition.y, drop2Example.GetComponent<RectTransform>().localPosition.z);
    //        drop2Example.GetComponent<RawImage>().color = new Color(1, 1, 1, drop2Example.GetComponent<RawImage>().color.a - 0.1f);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    startedRotation = false;
    //}
}


public enum TutorialActType
{
    onStart,
    onRepeat,
    switchCond,
    onEnd,
}
