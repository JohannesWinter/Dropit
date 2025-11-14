using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenue : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public TextMeshProUGUI continueButtonText;
    public Button loadGameButton;
    public GameObject loadGameHeadline;
    public Button settingsButton;
    public GameObject overlay;
    public GameObject backroundLG;
    public GameObject backroundSide;
    public GameObject saveGamesMenu;
    public GameObject mainMenuOverlay_NW;
    public GameObject mainMenuOverlay_SW;
    public GameObject mainMenuOverlay_SO;
    public GameObject mainMenuOverlay_NO;
    public GameObject mainMenuOverlay_N;
    public GameObject mainMenuOverlay_W;
    //public GameObject blackBoard;
    int lastSaveGameNumber;

    //tutorial
    public Button changeTutorial;
    public Image tutorialOn;
    public bool enableTutorial;

    //exit
    public Button exitButton;
    public Ask ask;
    bool askedExit;

    public GameObject[] blackBoards;
    public bool[] blackBoardUsed;


    public GameObject menuCam;
    // Start is called before the first frame update
    void Start()
    {
        blackBoardUsed = new bool[blackBoards.Length];
        for (int i = 0; i < blackBoards.Length; i++)
        {
            blackBoards[i].SetActive(false);
        }
        newGameButton.onClick.AddListener(NewGame);
        continueButton.onClick.AddListener(ContinueGame);
        changeTutorial.onClick.AddListener(ChangeTutorial);
        loadGameButton.onClick.AddListener(LoadGames);
        settingsButton.onClick.AddListener(Settings);
        exitButton.onClick.AddListener(ExitGame);
        lastSaveGameNumber = Manager.m.lastSaveNumber;

        enableTutorial = true;
        if (Manager.m.saveGameManager.saveGames.Count > 0)
        {
            enableTutorial = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float gUIScale = Math.Min(Manager.m.graphicManager.gUIScaleFactor, 1.1f);
        saveGamesMenu.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        mainMenuOverlay_NW.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        mainMenuOverlay_SW.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        mainMenuOverlay_SO.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        mainMenuOverlay_NO.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        
        if (Manager.m.graphicManager.gUIScaleFactor > 1)
        {
            mainMenuOverlay_N.SetActive(false);
            mainMenuOverlay_W.SetActive(false);
        }
        else
        {
            mainMenuOverlay_N.SetActive(true);
            mainMenuOverlay_W.SetActive(true);
        }

        mainMenuOverlay_N.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        mainMenuOverlay_W.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);


        lastSaveGameNumber = Manager.m.lastSaveNumber;
        if (Manager.m.inMainMenu == true)
        {
            backroundSide.transform.localScale = new Vector3(1, 1, 1);
            overlay.SetActive(true);
        }
        else
        {
            backroundSide.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            overlay.SetActive(false);
        }
        if (lastSaveGameNumber == 0)
        {
            continueButton.enabled = false;
            continueButton.targetGraphic.color = new Color(150f / 255, 150f / 255, 150f / 255, 1);
            continueButtonText.color = new Color(40f / 255, 120f / 255, 40f / 255, 1);
        }
        else
        {
            continueButton.enabled = true;
            continueButton.targetGraphic.color = new Color(1, 1, 1, 1);
            continueButtonText.color = new Color(0, 1, 0, 1);
        }
        if (enableTutorial == true)
        {
            tutorialOn.enabled = true;
        }
        else
        {
            tutorialOn.enabled = false;
        }
        if (Manager.m.inMainMenuLoad == true)
        {
            backroundLG.SetActive(true);
            loadGameHeadline.SetActive(true);
            loadGameButton.targetGraphic.color = new Color(200f / 255, 200f / 255, 200f / 255, 1);
            if (Manager.m.inSettings == true)
            {
                Manager.m.inMainMenuLoad = false;
            }
        }
        else
        {
            backroundLG.SetActive(false);
            loadGameHeadline.SetActive(false);
            loadGameButton.targetGraphic.color = new Color(1, 1, 1, 1);
        }
        if (askedExit == true && ask.antwort == 2)
        {
            askedExit = false;
            ask.antwort = 0;

            Manager.m.Exit();
        }
        else if (askedExit == true && ask.antwort == 1)
        {
            askedExit = false;
            ask.antwort = 0;
        }
    }

    void NewGame()
    {
        Manager.m.effectSpeaker.click();
        StartCoroutine(blackOutNewGame());
    }
    void ContinueGame()
    {
        if (lastSaveGameNumber != 0)
        {
            Manager.m.effectSpeaker.click();
            GameSave lastSaveGame;

            for (int i = 0; i < Manager.m.saveGameManager.saveGames.Count; i++)
            {
                if (Manager.m.saveGameManager.saveGames[i].GetComponent<GameSave>().saveNumber == lastSaveGameNumber)
                {
                    lastSaveGame = Manager.m.saveGameManager.saveGames[i].GetComponent<GameSave>();
                    lastSaveGame.load = true;
                    break;
                }
            }
        }
    }
    void ChangeTutorial()
    {
        if (enableTutorial == true)
        {
            enableTutorial = false;
            Manager.m.effectSpeaker.swipe();
        }
        else if (enableTutorial == false)
        {
            enableTutorial = true;
            Manager.m.effectSpeaker.swipe();
        }
    }
    void LoadGames()
    {
        if (Manager.m.inMainMenuLoad == false)
        {
            Manager.m.inMainMenuLoad = true;
        }
        else
        {
            Manager.m.inMainMenuLoad = false;
        }
        Manager.m.effectSpeaker.click();
    }
    void Settings()
    {
        Manager.m.inSettings = true;
        Manager.m.effectSpeaker.click();
    }

    void ExitGame()
    {
        ask.Asking("are you sure you want to<br>leave the game?", true);
        ask.antwort = 0;
        askedExit = true;
        Manager.m.effectSpeaker.click();
    }
    public int GetBlackboardPos()
    {
        for(int i = 0; i < blackBoards.Length; i++)
        {
            if (blackBoardUsed[i] == false)
            {
                blackBoardUsed[i] = true;
                return i;
            }
        }
        return 1;
    }

    public IEnumerator blackOutNewGame()
    {
        int blackBoardPos = GetBlackboardPos();

        GameObject blackBoard = blackBoards[blackBoardPos];

        blackBoard.SetActive(true);
        blackBoard.GetComponent<RawImage>().raycastTarget = true;
        blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        Manager.m.musicSpeaker.ChangeMusic(3, 3, "normal", 1);
        for (int i = 0; i < 30; i++)
        {
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, blackBoard.GetComponent<RawImage>().color.a + 1/30f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(blackIn(blackBoardPos));;
        Manager.m.inMainMenu = false;
        Manager.m.inMainMenuLoad = false;
        if (enableTutorial == true)
        {
            Manager.m.tutorial.StartTutorial();
        }
        else
        {
            Manager.m.tutorial.finishedTutorial2 = true;
            Manager.m.tutorial.finishedTutorial3 = true;
        }
        Manager.m.Reset(true);
    }
    public IEnumerator blackOutReturnMainMenu()
    {
        Manager.m.loading = true;
        int blackBoardPos = GetBlackboardPos();

        GameObject blackBoard = blackBoards[blackBoardPos];

        int factoryVolume = Manager.m.factoryVolume.volume;

        blackBoard.SetActive(true);
        blackBoard.GetComponent<RawImage>().raycastTarget = true;
        blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        Manager.m.musicSpeaker.ChangeMusic(3, 3, "normal", 1);
        for (int i = 0; i < 30; i++)
        {
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, blackBoard.GetComponent<RawImage>().color.a + 1 / 30f);
            if (i % 3 == 0)
            {
                Manager.m.factoryVolume.volume--;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        Manager.m.factoryVolume.volume = factoryVolume;
        StartCoroutine(blackIn(blackBoardPos));
        Manager.m.Reset(true);
        Manager.m.inMainMenu = true;
        Manager.m.loading = false;
    }

    public IEnumerator blackIn(int blackBoardPos)
    {
        GameObject blackBoard = blackBoards[blackBoardPos];

        blackBoard.GetComponent<RawImage>().raycastTarget = false;
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i < 20; i++)
        {
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, blackBoard.GetComponent<RawImage>().color.a - 0.05f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        blackBoard.SetActive(false);
        blackBoardUsed[blackBoardPos] = false;
    }
}
