using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject overlay;
    public GameObject buttonsOverlay;
    public bool inMainMenue;

    public GameObject save;
    public GameObject load;
    public GameObject sound;
    public GameObject graphics;
    public GameObject help;
    public GameObject hotkeys;
    //public GameObject exit;
    public GameObject mainMenu;

    public GameObject back;
    public GameObject close;
    public GameObject continueIngame;

    public GameObject volumesOverlay;
    public GameObject helpMenueOverlay;
    public GameObject graphicsOverlay;
    public GameObject ore_reset;
    public GameObject reset;
    public GameObject changeHotkeysOverlay;

    public Ask ask;
    bool askedReset;
    bool askedMainMenu;

    public TextMeshProUGUI headline;
    public GameObject emptyText;

    public RectTransform[] mainMenuButtonPositions;
    public RectTransform[] inGameButtonPositions;
    RectTransform[] allButtonTransforms;

    public List<GameObject> saveGames = new List<GameObject>();

    void Start()
    {
        save.GetComponent<Button>().onClick.AddListener(OptionsSave);
        load.GetComponent<Button>().onClick.AddListener(OptionsLoad);
        sound.GetComponent<Button>().onClick.AddListener(OptionsSound);
        graphics.GetComponent<Button>().onClick.AddListener(OptionsGraphics);
        help.GetComponent<Button>().onClick.AddListener(OptionHelp);
        hotkeys.GetComponent<Button>().onClick.AddListener(OptionHotkeys);
        mainMenu.GetComponent<Button>().onClick.AddListener(OptionsMainMenu);

        close.GetComponent<Button>().onClick.AddListener(OptionClose);
        continueIngame.GetComponent<Button>().onClick.AddListener(OptionClose);
        back.GetComponent<Button>().onClick.AddListener(OptionBack);

        ore_reset.GetComponent<Button>().onClick.AddListener(Ore_Reset);
        reset.GetComponent<Button>().onClick.AddListener(Reset);

        //static order:

        allButtonTransforms = new RectTransform[10];
        allButtonTransforms[0] = close.GetComponent<RectTransform>();
        allButtonTransforms[1] = save.GetComponent<RectTransform>();
        allButtonTransforms[2] = load.GetComponent<RectTransform>();
        allButtonTransforms[3] = hotkeys.GetComponent<RectTransform>();
        allButtonTransforms[4] = graphics.GetComponent<RectTransform>();
        allButtonTransforms[5] = sound.GetComponent<RectTransform>();
        allButtonTransforms[6] = help.GetComponent<RectTransform>();
        allButtonTransforms[7] = mainMenu.GetComponent<RectTransform>();
        allButtonTransforms[8] = continueIngame.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        saveGames = Manager.m.saveGameManager.saveGames;
        overlay.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);

        if (Manager.m.settings_save || Manager.m.settings_load || Manager.m.settings_clear || Manager.m.inMainMenuLoad)
        {
            for (int i = 0; i < saveGames.Count; i++)
            {
                saveGames[i].GetComponent<GameSave>().SetUiActive(true);
            }
        }
        else
        {
            for (int i = 0; i < saveGames.Count; i++)
            {
                saveGames[i].GetComponent<GameSave>().SetUiActive(false);
            }
        }
        if(save.activeSelf == true)
        {
            inMainMenue = true;
        }
        else
        {
            inMainMenue = false;
        }
        if (Manager.m.settings_load == false && Manager.m.settings_clear == false)
        {
            emptyText.SetActive(false);
        }
        else
        {
            if (Manager.m.saveGameManager.saveGames.Count == 0)
            {
                emptyText.SetActive(true);
            }
            else
            {
                emptyText.SetActive(false);
            }
        }

        if (Manager.m.inSettings == false)
        {
            overlay.SetActive(false);
            Manager.m.settings_save = false;
            Manager.m.settings_load = false;
            Manager.m.settings_clear = false;
            Manager.m.settings_autosave = false;
        }
        else
        {
            overlay.SetActive(true);
        }
        if (askedReset == true && ask.antwort == 2)
        {
            Manager.m.Reset(true);
            askedReset = false;
            askedMainMenu = false;
            ask.antwort = 0;

        }
        else if (askedReset == true && ask.antwort == 1)
        {
            askedReset = false;
            askedMainMenu = false;
            ask.antwort = 0;
        }
        if (askedMainMenu == true && ask.antwort == 2)
        {
            askedMainMenu = false;
            askedReset = false;
            ask.antwort = 0;

            Manager.m.inSettings = false;
            StartCoroutine(Manager.m.mainMenu.blackOutReturnMainMenu());
        }
        else if (askedMainMenu == true && ask.antwort == 1)
        {
            askedMainMenu = false;
            askedReset = false;
            ask.antwort = 0;
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("ResetOres")))
        {
            Ore_Reset();
        }
        UpdateButtonPositions();

    }
    void OptionsSave()
    {
        Manager.m.settings_save = true;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = true;
        Manager.m.settings_autosave = false;
        Manager.m.saveGameManager.saveGames.ForEach(x => x.GetComponent<GameSave>().counter = 30); //reloads saves

        for(int i = 0; i < saveGames.Count; i++) { saveGames[i].GetComponent<GameSave>().SetUiActive(true); }

        DisableEverything();
        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Saves";
    }
    void OptionsLoad()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = true;
        Manager.m.settings_clear = true;
        Manager.m.settings_autosave = false;
        Manager.m.saveGameManager.saveGames.ForEach(x => x.GetComponent<GameSave>().counter = 30); //reloads saves

        for (int i = 0; i < saveGames.Count; i++) { saveGames[i].GetComponent<GameSave>().SetUiActive(true); }

        DisableEverything();
        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Load";
    }
    void OptionsClear()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = true;
        Manager.m.settings_autosave = false;
        Manager.m.saveGameManager.saveGames.ForEach(x => x.GetComponent<GameSave>().counter = 30); //reloads saves

        for (int i = 0; i < saveGames.Count; i++) { saveGames[i].GetComponent<GameSave>().SetUiActive(true); }

        DisableEverything();
        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Delete";
    }
    void Reset()
    {
        ask.Asking("Give up and restart?");
        askedReset = true;
        Manager.m.effectSpeaker.click();
    }
    void Ore_Reset()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Ore");
        for (int i = 0; i < a.Length; i++)
        {
            a[i].SetActive(false);
            Destroy(a[i], 3);
        }
        Manager.m.effectSpeaker.click();
    }
    void OptionsSound()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = false;

        DisableEverything();

        volumesOverlay.SetActive(true);

        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Sound";
    }
    void OptionsGraphics()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = false;

        DisableEverything();

        back.SetActive(true);

        graphicsOverlay.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Graphics";


    }
    void OptionsMainMenu()
    {
        ask.Asking("Return to main menu?<br>Unsaved progress will be lost");
        askedMainMenu = true;
        Manager.m.effectSpeaker.click();
    }
    void OptionHelp()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = false;

        DisableEverything();

        Manager.m.settings_help = true;
        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Help";
    }
    void OptionHotkeys()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = false;
        Manager.m.settings_hotkeys = true;

        DisableEverything();

        changeHotkeysOverlay.SetActive(true);
        back.SetActive(true);
        Manager.m.effectSpeaker.click();
        headline.text = "Hotkeys";
    }
    public void OptionBack()
    {
        Manager.m.settings_save = false;
        Manager.m.settings_load = false;
        Manager.m.settings_clear = false;
        Manager.m.settings_hotkeys = false;
        Manager.m.settings_help = false;

        for (int i = 0; i < saveGames.Count; i++) { saveGames[i].GetComponent<GameSave>().SetUiActive(false); }
        DisableEverything();

        buttonsOverlay.SetActive(true);
        save.SetActive(true);
        load.SetActive(true);
        sound.SetActive(true);
        graphics.SetActive(true);
        hotkeys.SetActive(true);
        help.SetActive(true);
        mainMenu.SetActive(true);
        close.SetActive(true);
  
        back.SetActive(false);
        headline.text = "Settings";
        Manager.m.effectSpeaker.click();
    }
    void OptionClose()
    {
        Manager.m.inSettings = false;
        Manager.m.effectSpeaker.click();
    }
    //void OptionExit()
    //{
    //    ask.Asking("Exit Game?");
    //    askedMainMenu = true;
    //    Manager.m.effectSpeaker.click();
    //}

    void DisableEverything()
    {
        buttonsOverlay.SetActive(false);
        save.SetActive(false);
        load.SetActive(false);
        sound.SetActive(false);
        help.SetActive(false);
        hotkeys.SetActive(false);
        graphics.SetActive(false);
        //exit.SetActive(false);
        mainMenu.SetActive(false);
        back.SetActive(false);
        volumesOverlay.SetActive(false);
        helpMenueOverlay.SetActive(false);
        changeHotkeysOverlay.SetActive(false);
        graphicsOverlay.SetActive(false);
        close.SetActive(false);
    }

    void UpdateButtonPositions()
    {
        if (Manager.m.inMainMenu) 
        {
            for (int i = 0; i < mainMenuButtonPositions.Length; i++)
            {
                if (allButtonTransforms[i] != null)
                {
                    if (mainMenuButtonPositions[i].gameObject.activeSelf)
                    {
                        allButtonTransforms[i].gameObject.SetActive(true);
                        allButtonTransforms[i].position = mainMenuButtonPositions[i].position;
                    }
                    else
                    {
                        allButtonTransforms[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Error: Settings button not found");
                }
            }
        }
        else
        {
            for (int i = 0; i < inGameButtonPositions.Length; i++)
            {
                if (allButtonTransforms[i] != null)
                {
                    if (inGameButtonPositions[i].gameObject.activeSelf)
                    {
                        allButtonTransforms[i].gameObject.SetActive(true);
                        allButtonTransforms[i].position = inGameButtonPositions[i].position;
                    }
                    else
                    {
                        allButtonTransforms[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Error: Settings button not found");
                }
            }
        }
    }
}
