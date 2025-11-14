using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChangeHotkey : MonoBehaviour
{
    public string hotkeyDef;
    public string hotkey;
    public Button hotkeyButton;
    public TextMeshProUGUI hotkeyText;
    public Glimmer glimmer;
    public RawImage hotkeyBox;
    public GameObject overlay;

    public bool waitForKey = false;

    //public string[] isAllowedWith;

    string[] keyMaps;
    KeyCode[] keyCodes;
    string[] keyNames;

    public bool isSection;

    public List<string> currentCombination;
    List<string> currentlyHold;

    float originalTextSize;

    // Start is called before the first frame update
    void Start()
    {
        if (isSection == false)
        {
            keyMaps = GameInputManager.GetKeyMaps();
            keyCodes = GameInputManager.GetKeyCodes();
            keyNames = GameInputManager.GetKeyNames();
            hotkeyButton.onClick.AddListener(buttonClick);
            currentlyHold = new List<string>();
            originalTextSize = hotkeyText.fontSize;

            if (hotkeyDef != "" && hotkey != null)
            {
                hotkey = Manager.m.keyActionTrigger[Array.IndexOf(Manager.m.keyActions, hotkeyDef)];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSection == false)
        {
            if (waitForKey == true)
            {
                Manager.m.keyActionTrigger[Array.IndexOf(Manager.m.keyActions, hotkeyDef)] = "";

                for (int i = 0; i < keyCodes.Length - 1; i++)
                {
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        currentlyHold.Add(keyMaps[i]);
                    }
                }

                for (int i = 1; i < keyCodes.Length; i++)
                {
                    if (Input.GetKeyUp(keyCodes[i]))
                    {
                        waitForKey = false;
                        string combination = "";
                        for (int x = 0; x < currentlyHold.Count; x++)
                        {
                            if (x == 0) combination = currentlyHold[x];
                            else combination += "+" + currentlyHold[x];
                        }
                        hotkey = combination;
                        currentCombination = new List<string>(currentlyHold);
                        print("set: " + combination);
                        if (hotkey == "ClickLeft")
                        {
                            Manager.m.effectSpeaker.error();
                        }
                        else
                        {
                            Manager.m.effectSpeaker.accept();
                        }
                        break;
                    }
                }
                glimmer.enabled = true;
            }
            else
            {
                currentlyHold.Clear();
                hotkeyBox.color = Color.white;
                if (hotkey == "ClickLeft")
                {
                    hotkey = "";
                }
                string[] keys = hotkey.Split("+");

                try
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (i == 0)
                            hotkeyText.text = keyNames[Array.IndexOf(keyMaps, keys[i])];
                        else
                            hotkeyText.text += "+<br>" + keyNames[Array.IndexOf(keyMaps, keys[i])];
                    }
                    hotkeyText.fontSize = originalTextSize / keys.Length;
                }
                catch
                {
                    hotkeyText.text = "--";
                }
                Manager.m.keyActionTrigger[Array.IndexOf(Manager.m.keyActions, hotkeyDef)] = hotkey;
                glimmer.enabled = false;

                if (Manager.m.inOptionHotkeys)
                {
                    currentCombination = hotkey.Split('+').ToList();
                    bool foundSame = false;
                    for (int i = 0; i < Manager.m.hotkeys.entries.Length; i++)
                    {
                        ChangeHotkey other = Manager.m.hotkeys.entries[i].GetComponent<ChangeHotkey>();
                        if (currentCombination != null)
                        {
                            for (int x = 0; x < currentCombination.Count; x++)
                            {
                                if (other != this && other.currentCombination.Count > 0 && other.currentCombination[0] == this.currentCombination[x] && this.currentCombination[0] != "")
                                {
                                    hotkeyText.color = new Color(1, 100/255f, 0);
                                    foundSame = true;
                                }
                            }
                        }
                    }
                    if (foundSame == false)
                    {
                        hotkeyText.color = Color.green;
                    }
                }

            }
        }
    }


    void buttonClick()
    {
        waitForKey = true;
        Manager.m.effectSpeaker.click();
        Manager.m.hotkeys.SetHotkeyMiddle(hotkeyDef);
    }
}
