using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSaveManager : MonoBehaviour
{
    public GameSave autoSave;
    public List<GameObject> saveGames;
    public List<GameObject> autoSaveGames;
    public GameObject saveTemplate;
    public Transform listTransform;
    public GameObject newSave;
    public Button newSaveButton;
    public GameObject overlay;

    public Button autoSaveButton;
    public TextMeshProUGUI autoSaveButtonText;
    float currenttime;
    public int countdownAutoSave = 300;
    int lastCountdownAnimation;
    int autoSaveState = 5;

    public Scrollbar scrollbar;
    public float changeSpeed;
    float toMove = 0;
    float localScollValue = 0;
    bool onScrollbarEnter = false;
    float beepCooldown = 0;

    //animation:
    public GameObject confirmationOverlay;
    public GameObject confirmationBackround;
    public TextMeshProUGUI confirmationTime;
    public GameObject confirmationCheckmark;

    //Mainmenu
    public GameObject menuLoadPlayersave;
    public GameObject menuLoadAutosave;
    public GameObject menuLoadNoPlayersave;

    public LoadingScreen loadingScreen;


    // Start is called before the first frame update

    void Start()
    {
        onScrollbarEnter = false;
        confirmationOverlay.SetActive(false);
        autoSaveButton.onClick.AddListener(ChangeAutoSave);
        newSaveButton.onClick.AddListener(AddSave);
        for (int i = 1; i <= PlayerPrefs.GetInt(Manager.m.version + "_" + "HighestSave"); i++)
        {
            LoadSave(i);
        }
        if (saveGames.Count > 1)
        {
            for (int i = 1; i < saveGames.Count; i++)
            {
                int position = PlayerPrefs.GetInt(Manager.m.version + "_" + "SaveNumber" + saveGames[i].GetComponent<GameSave>().saveNumber + "_Position");
                //print("saveGame " + saveGames[i].GetComponent<SaveGame>().saveNumber + ": Position" + position);
                GameObject h = saveGames[position];
                saveGames[position] = saveGames[i];
                saveGames[i] = h;
            }
        }
        if (PlayerPrefs.GetInt(Manager.m.version + "_" + "AutoSaveState") != 0)
        {
            autoSaveState = PlayerPrefs.GetInt(Manager.m.version + "_" + "AutoSaveState");
        }
    }

    // Update is called once per frame
    void Update()
    {
        overlay.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        int highestSaveNumber = 0;
        for (int i = 0; i < saveGames.Count; i++)
        {
            if (saveGames[i].GetComponent<GameSave>().saveNumber > highestSaveNumber)
            {
                highestSaveNumber = saveGames[i].GetComponent<GameSave>().saveNumber;            
            }
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "HighestSave", highestSaveNumber);
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Quicksave")))
        {
            int position = 0;

            bool emptySlot = false;

            for (int i = 0; i < autoSaveGames.Count; i++)
            {
                if (autoSaveGames[i].GetComponent<GameSave>().saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
                {
                    position = i;
                    emptySlot = true;
                    break;
                }
            }
            if (emptySlot == false)
            {
                float highestage = autoSaveGames[0].GetComponent<GameSave>().age;
                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    float age = autoSaveGames[i].GetComponent<GameSave>().age;
                    if (age <= highestage)
                    {
                        highestage = age;
                        position = i;
                    }
                }
            }
            AutoSave(position);
        }

        for (int i = 0; i  < saveGames.Count; i++)
        {
            PlayerPrefs.SetInt(Manager.m.version + "_" + "SaveNumber" + saveGames[i].GetComponent<GameSave>().saveNumber + "_Position", i);
        }

        if (Manager.m.settings_save || Manager.m.settings_load || Manager.m.settings_clear || Manager.m.settings_autosave || Manager.m.inMainMenuLoad)
        {
            if (onScrollbarEnter == true)
            {
                scrollbar.value = 0;
                localScollValue = 0;
                toMove = 0;
            }
            scrollbar.gameObject.SetActive(true);
            List<GameObject> currentMenuObjects = new List<GameObject>();
            Vector3 offset = new Vector3(0, 0, 0);

            if (Manager.m.settings_save  || Manager.m.settings_load || Manager.m.settings_clear)
            {
                scrollbar.transform.localPosition = new Vector3(250, 0, 0);
                if (Manager.m.settings_save == true)
                {
                    for (int i = 0; i < saveGames.Count; i++)
                    {
                        currentMenuObjects.Add(saveGames[i]);
                    }
                    currentMenuObjects.Add(newSave);
                    newSave.SetActive(true);
                    if (currentMenuObjects.Count == 0)
                    {
                        menuLoadNoPlayersave.SetActive(true);
                        currentMenuObjects.Add(menuLoadNoPlayersave);
                    }
                }
                else
                {
                    currentMenuObjects.Add(menuLoadPlayersave);
                    menuLoadPlayersave.SetActive(true);
                    for (int i = 0; i < saveGames.Count; i++)
                    {
                        currentMenuObjects.Add(saveGames[i]);
                    }
                    if (currentMenuObjects.Count <= 1)
                    {
                        menuLoadNoPlayersave.SetActive(true);
                        currentMenuObjects.Add(menuLoadNoPlayersave);
                    }
                    currentMenuObjects.Add(menuLoadAutosave);
                    menuLoadAutosave.SetActive(true);

                    List<GameObject> autoSaveList = new List<GameObject>();
                    for (int i = 0; i < autoSaveGames.Count; i++)
                    {
                        autoSaveList.Add(autoSaveGames[i]);
                        autoSaveGames[i].GetComponent<GameSave>().SetUiActive(true);
                    }
                    float[] ages = new float[autoSaveGames.Count];
                    for (int i = 0; i < autoSaveGames.Count; i++)
                    {
                        ages[i] = autoSaveGames[i].GetComponent<GameSave>().age;
                    }
                    for (int i = 0; i < ages.Length; i++)
                    {
                        for (int j = 0; j < ages.Length; j++)
                        {
                            if (ages[i] > ages[j])
                            {
                                float h = ages[i];
                                ages[i] = ages[j];
                                ages[j] = h;

                                GameObject gh = autoSaveList[i];
                                autoSaveList[i] = autoSaveList[j];
                                autoSaveList[j] = gh;
                            }
                        }
                    }
                    currentMenuObjects.AddRange(autoSaveList);
                }
            }
            else if (Manager.m.settings_autosave)
            {
                scrollbar.transform.localPosition = new Vector3(250, 0, 0);
                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    currentMenuObjects.Add(autoSaveGames[i]);
                    autoSaveGames[i].GetComponent<GameSave>().SetUiActive(true);
                }
                float[] ages = new float[autoSaveGames.Count];
                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    ages[i] = autoSaveGames[i].GetComponent<GameSave>().age;
                }
                for (int i = 0; i < ages.Length; i++)
                {
                    for (int j = 0; j < ages.Length; j++)
                    {
                        if (ages[i] > ages[j])
                        {
                            float h = ages[i];
                            ages[i] = ages[j];
                            ages[j] = h;

                            GameObject gh = currentMenuObjects[i];
                            currentMenuObjects[i] = currentMenuObjects[j];
                            currentMenuObjects[j] = gh;
                        }
                    }
                }
            }
            else if (Manager.m.inMainMenuLoad)
            {
                offset = new Vector3(80, 90, 0);
                scrollbar.transform.localPosition = new Vector3(300, 90, 0);
                menuLoadPlayersave.SetActive(true);
                menuLoadAutosave.SetActive(true);
                currentMenuObjects.Add(menuLoadPlayersave);
                if (saveGames.Count == 0)
                {
                    currentMenuObjects.Add(menuLoadNoPlayersave);
                    menuLoadNoPlayersave.SetActive(true);
                }
                for (int i = 0; i < saveGames.Count; i++)
                {
                    currentMenuObjects.Add(saveGames[i]);
                }

                currentMenuObjects.Add(menuLoadAutosave);

                List<GameObject> autoSaveObjects = new List<GameObject>();

                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    autoSaveObjects.Add(autoSaveGames[i]);
                    autoSaveGames[i].GetComponent<GameSave>().SetUiActive(true);
                }
                float[] ages = new float[autoSaveGames.Count];
                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    ages[i] = autoSaveGames[i].GetComponent<GameSave>().age;
                }
                for (int i = 0; i < ages.Length; i++)
                {
                    for (int j = 0; j < ages.Length; j++)
                    {
                        if (ages[i] > ages[j])
                        {
                            float h = ages[i];
                            ages[i] = ages[j];
                            ages[j] = h;

                            GameObject gh = autoSaveObjects[i];
                            autoSaveObjects[i] = autoSaveObjects[j];
                            autoSaveObjects[j] = gh;
                        }
                    }
                }
                currentMenuObjects.AddRange(autoSaveObjects);
            }

            for (int i = 0; i < currentMenuObjects.Count; i++)
            {
                currentMenuObjects[i].transform.localPosition = new Vector3(0 + offset.x, 50 - i * 70 + offset.y, 0 + offset.z);

                float value = localScollValue;
                if (currentMenuObjects.Count > 2)
                {
                    currentMenuObjects[i].transform.localPosition = new Vector3(0 + offset.x, currentMenuObjects[i].transform.localPosition.y + value * (currentMenuObjects.Count - 2.5f) * 70, 0 + offset.z);
                    if (currentMenuObjects[i].transform.localPosition.y - offset.y > 128 || currentMenuObjects[i].transform.localPosition.y - offset.y < -128)
                    {
                        currentMenuObjects[i].transform.localScale = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        if (Mathf.Abs(currentMenuObjects[i].transform.localPosition.y - offset.y) > 60)
                        {
                            float distance = Mathf.Abs(currentMenuObjects[i].transform.localPosition.y - offset.y);
                            float distanceSmaller = distance - 60;
                            if (currentMenuObjects[i].transform.localScale.y <= 0 && (1 - (distanceSmaller / 70f) > 0) && beepCooldown <= Time.unscaledTime && onScrollbarEnter == false)
                            {
                                Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.beep, 1 / ((localScollValue + 1.5f) * 0.5f));
                                beepCooldown = Time.unscaledTime + 0.05f;
                            }
                            currentMenuObjects[i].transform.localScale = new Vector3(1, 1 - (distanceSmaller / 70f), 1);
                        }
                        else
                        {
                            currentMenuObjects[i].transform.localScale = new Vector3(1, 1, 1);
                        }
                    }
                }
                else
                {
                    currentMenuObjects[i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
            if (currentMenuObjects.Count > 2)
            {
                if (localScollValue != scrollbar.value)
                {
                    float diffrence = scrollbar.value - localScollValue;

                    localScollValue += 0.15f * diffrence;
                    if (Mathf.Abs(diffrence) < 0.00001f)
                    {
                        localScollValue = scrollbar.value;
                    }
                }
                if (Input.mouseScrollDelta.y != 0)
                {
                    toMove += Input.mouseScrollDelta.y * (-1f) * (1f / currentMenuObjects.Count);
                }
                if (toMove != 0)
                {
                    scrollbar.value += toMove * 0.5f;
                    toMove = toMove * 0.50f;
                    if (Mathf.Abs(toMove) < 0.00001f)
                    {
                        toMove = 0;
                    }
                    if ((scrollbar.value == 1 && toMove > 0) || (scrollbar.value == 0 && toMove < 0))
                    {
                        toMove = 0;
                    }
                }
                if (scrollbar.value > 1)
                {
                    scrollbar.value = 1;
                }
                else if (scrollbar.value < 0)
                {
                    scrollbar.value = 0;
                }
            }
            else
            {
                scrollbar.value = 0;
            }
            onScrollbarEnter = false;
        }
        else
        {
            onScrollbarEnter = true;
            scrollbar.transform.localPosition = new Vector3(250, 0, 0);
            newSave.SetActive(false);
            for (int i = 0; i < autoSaveGames.Count; i++)
            {
                autoSaveGames[i].GetComponent<GameSave>().SetUiActive(false);
            }
            menuLoadPlayersave.SetActive(false);
            menuLoadAutosave.SetActive(false);
            menuLoadNoPlayersave.SetActive(false);
            scrollbar.gameObject.SetActive(false);
        }

        switch (autoSaveState)
        {
            case 1:
                {
                    autoSaveButtonText.text = "1<br><size=10>MIN";
                    break;
                }
            case 3:
                {
                    autoSaveButtonText.text = "3<br><size=10>MIN";
                    break;
                }
            case 5:
                {
                    autoSaveButtonText.text = "5<br><size=10>MIN";
                    break;
                }
            case 15:
                {
                    autoSaveButtonText.text = "15<br><size=10>MIN";
                    break;
                }
            case 30:
                {
                    autoSaveButtonText.text = "30<br><size=10>MIN";
                    break;
                }
            case 60:
                {
                    autoSaveButtonText.text = "60<br><size=10>MIN";
                    break;
                }
            default:
                {
                    autoSaveButtonText.text = "1<br><size=10>MIN";
                    break;
                }
        }

        if (countdownAutoSave == 10 && lastCountdownAnimation != 10)
        {
            lastCountdownAnimation = 10;
            StartCoroutine(AutosaveAnimation("<size=10>Autosave<br><size=27>10<br><size=10>SEC"));
        }
        if (countdownAutoSave == 30 && lastCountdownAnimation != 30)
        {
            lastCountdownAnimation = 30;
            StartCoroutine(AutosaveAnimation("<size=10>Autosave<br><size=27>30<br><size=10>SEC"));
        }
        if (countdownAutoSave == 61 && lastCountdownAnimation != 61)
        {
            lastCountdownAnimation = 61;
            StartCoroutine(AutosaveAnimation("<size=10>Autosave<br><size=27>1<br><size=10>MIN"));
        }
        if (countdownAutoSave == 181 && lastCountdownAnimation != 181)
        {
            lastCountdownAnimation = 181;
            StartCoroutine(AutosaveAnimation("<size=10>Autosave<br><size=27>3<br><size=10>MIN"));
        }
        if (countdownAutoSave == 601 && lastCountdownAnimation != 601)
        {
            lastCountdownAnimation = 601;
            StartCoroutine(AutosaveAnimation("<size=10>Autosave<br><size=27>10<br><size=10>MIN"));
        }

        if (currenttime < Time.unscaledTime && Manager.m.inMainMenu == false && Manager.m.paused == false)
        {
            currenttime = Time.unscaledTime + 1;
            countdownAutoSave -= 1;
            if (countdownAutoSave <= 0)
            {
                int position = 0;

                bool emptySlot = false;

                for (int i = 0; i < autoSaveGames.Count; i++)
                {
                    if (autoSaveGames[i].GetComponent<GameSave>().saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
                    {
                        position = i;
                        emptySlot = true;
                        break;
                    }
                }
                if (emptySlot == false)
                {
                    float highestage = autoSaveGames[0].GetComponent<GameSave>().age;
                    for (int i = 0; i < autoSaveGames.Count; i++)
                    {
                        float age = autoSaveGames[i].GetComponent<GameSave>().age;
                        if (age < highestage)
                        {
                            highestage = age;
                            position = i;
                        }
                    }
                }
                AutoSave(position);
                StartCoroutine(AutosaveAnimation());
                countdownAutoSave = autoSaveState * 60;
            }
        }

        for(int i = 0; i < saveGames.Count; i++)
        {
            if (saveGames[i].GetComponent<GameSave>().saveText.GetComponent<TextMeshProUGUI>().text == "Empty")
            {
                GameObject toDestroy = saveGames[i];
                saveGames.RemoveAt(i);
                Destroy(toDestroy);
            }
        }
    }

    void ChangeAutoSave()
    {
        if (autoSaveState == 1)
        {
            autoSaveState = 3;
        }
        else if (autoSaveState == 3)
        {
            autoSaveState = 5;
        }
        else if (autoSaveState == 5)
        {
            autoSaveState = 15;
        }
        else if (autoSaveState == 15)
        {
            autoSaveState = 30;
        }
        else if (autoSaveState == 30)
        {
            autoSaveState = 60;
        }
        else if (autoSaveState == 60)
        {
            autoSaveState = 1;
        }
        else
        {
            autoSaveState = 1;
            print("Error - wrong autoSaveState");
        }
        PlayerPrefs.SetInt(Manager.m.version + "_" + "AutoSaveState", autoSaveState);
        Manager.m.effectSpeaker.click();
        countdownAutoSave = autoSaveState * 60; //Minutes per Autosave
    }

    void AutoSave(int position)
    {
        autoSaveGames[position].GetComponent<GameSave>().save = true;
    }
    void AddSave()
    {
        GameObject newSave = Instantiate(saveTemplate);
        int highestSaveNumber = 1;
        for(int i = 0; i < saveGames.Count;i++)
        {
            if (saveGames[i].GetComponent<GameSave>().saveNumber > highestSaveNumber)
            {
                highestSaveNumber = saveGames[i].GetComponent<GameSave>().saveNumber;
            }
        }
        bool[] saveNumberTaken = new bool[highestSaveNumber + 1];
        for (int i = 0; i < saveGames.Count; i++)
        {
            saveNumberTaken[saveGames[i].GetComponent<GameSave>().saveNumber] = true;
        }
        int saveNumber = 0;
        for (int i = 1; i < saveNumberTaken.Length; i++)
        {
            if (saveNumberTaken[i] == false)
            {
                saveNumber = i;
                break;
            }
        }
        if (saveNumber == 0)
        {
            saveNumber = highestSaveNumber + 1;
        }
        newSave.name = "SaveGame (" + saveNumber + ")";
        newSave.transform.SetParent(listTransform);
        newSave.transform.localScale = new Vector3(1f, 1f, 1f);
        newSave.SetActive(true);
        newSave.GetComponent<GameSave>().saveNumber = saveNumber;
        newSave.GetComponent<GameSave>().save = true;
        newSave.GetComponent<GameSave>().saveText.GetComponent<TextMeshProUGUI>().text = "In Process";
        saveGames.Add(newSave);
        scrollbar.value = 0;
    }
    void LoadSave(int saveNumber)
    {
        GameObject newSave = Instantiate(saveTemplate);
        newSave.name = "SaveGame (" + saveNumber + ")";
        newSave.transform.SetParent(listTransform);
        newSave.transform.localScale = new Vector3(1f, 1f, 1f);
        newSave.SetActive(true);
        newSave.GetComponent<GameSave>().saveNumber = saveNumber;
        newSave.GetComponent<GameSave>().age = PlayerPrefs.GetFloat(Manager.m.version + "_" + "Save" + saveNumber + "_Age");
        newSave.GetComponent<GameSave>().saveText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString(Manager.m.version + "_" + "Save" + saveNumber + "_Text");
        saveGames.Add(newSave);
    }

    public IEnumerator AutosaveAnimation()
    {
        confirmationOverlay.SetActive(true);
        confirmationCheckmark.SetActive(true);
        confirmationTime.text = "";
        confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        confirmationCheckmark.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        confirmationOverlay.transform.localPosition = new Vector3(0, 100, 0);

        float rotation = 1440;
        float size = 0.0f;

        int steps = 80;
        for (int i = 0; i <= steps; i++)
        {
            confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationBackround.GetComponent<RawImage>().color.a + 1f / steps);
            confirmationCheckmark.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationCheckmark.GetComponent<RawImage>().color.a + 1f / steps);
            confirmationCheckmark.transform.rotation = Quaternion.Euler(0, 0, -rotation);
            confirmationCheckmark.transform.localScale = new Vector3(size, size, size);
            confirmationOverlay.transform.localPosition = new Vector3(0, confirmationOverlay.transform.localPosition.y * 0.95f, 0);
            rotation = rotation * 0.9f;
            if (i < steps / 2)
            {
                size += 1f / (steps / 2);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }

        size = 1;

        for (int i = 0; i <= steps / 2; i++)
        {
            confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationBackround.GetComponent<RawImage>().color.a - 1.5f / (steps / 2));
            confirmationCheckmark.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationCheckmark.GetComponent<RawImage>().color.a - 1.5f / (steps / 2));
            confirmationCheckmark.transform.localScale = new Vector3(size, size, size);
            confirmationOverlay.transform.localPosition = new Vector3(0, confirmationOverlay.transform.localPosition.y * 1.5f, 0);
            size = size * 0.95f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        confirmationOverlay.SetActive(false);
        yield return null;
    }
    public IEnumerator AutosaveAnimation(string restTime)
    {
        confirmationOverlay.SetActive(true);
        confirmationCheckmark.SetActive(false);
        confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        confirmationTime.color = new Color(0, 1, 0, 0);
        confirmationTime.text = restTime;
        confirmationOverlay.transform.localPosition = new Vector3(0, 100, 0);

        int steps = 80;
        for (int i = 0; i <= steps; i++)
        {
            confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationBackround.GetComponent<RawImage>().color.a + 1f/steps);
            confirmationTime.color = new Color(0, 1, 0, confirmationTime.color.a + 1f/steps);
            confirmationOverlay.transform.localPosition = new Vector3(0, confirmationOverlay.transform.localPosition.y * 0.95f, 0);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        for (int i = 0; i <= steps / 2; i++)
        {
            confirmationBackround.GetComponent<RawImage>().color = new Color(1, 1, 1, confirmationBackround.GetComponent<RawImage>().color.a - 1.5f / (steps / 2));
            confirmationTime.color = new Color(0, 1, 0, confirmationTime.color.a - 1.5f / (steps / 2));
            confirmationOverlay.transform.localPosition = new Vector3(0, confirmationOverlay.transform.localPosition.y * 1.5f, 0);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        confirmationOverlay.SetActive(false);

        yield return null;
    }
}
