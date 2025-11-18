using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotkeys : MonoBehaviour
{
    public GameObject[] entries;
    public GameObject list;
    public Scrollbar scrollbar;
    public float changeSpeed;
    public GameObject blocker;
    float toMove = 0;
    float localScollValue;
    public Button resetKeys;
    public Button alreadyResetKeys;
    public float beepCooldown;
    bool onHotkeyEnter;

    void Start()
    {
        entries = new GameObject[list.transform.childCount];
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i] = list.transform.GetChild(i).gameObject;
        }
        resetKeys.onClick.AddListener(ResetKeys);
        beepCooldown = 0;
    }


    void Update()
    {
        if (Manager.m.settings_hotkeys == true)
        {
            if (onHotkeyEnter == true)
            {
                toMove = 0;
                localScollValue = 0;
                scrollbar.value = 0;
            }
            if (OriginalKeys())
            {
                alreadyResetKeys.gameObject.SetActive(true);
                resetKeys.gameObject.SetActive(false);
            }
            else
            {
                alreadyResetKeys.gameObject.SetActive(false);
                resetKeys.gameObject.SetActive(true);
            }

            bool needBlock = false;
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i].transform.localPosition = new Vector3(0, 50 - i * 60, 0);

                float value = localScollValue;

                entries[i].transform.localPosition = new Vector3(0, entries[i].transform.localPosition.y + value * (entries.Length - 2.5f) * 60, 0);
                if (entries[i].transform.localPosition.y > 128 || entries[i].transform.localPosition.y < -128)
                {
                    entries[i].GetComponent<ChangeHotkey>().overlay.SetActive(false);
                }
                else
                {
                    if (entries[i].GetComponent<ChangeHotkey>().overlay.activeSelf == false && beepCooldown <= Time.unscaledTime && onHotkeyEnter == false)
                    {
                        Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.beep, 1 / ((localScollValue + 1.5f) * 0.5f));
                        beepCooldown = Time.unscaledTime + 0.05f;
                    }
                    entries[i].GetComponent<ChangeHotkey>().overlay.SetActive(true);
                    if (Mathf.Abs(entries[i].transform.localPosition.y) > 60)
                    {
                        float distance = Mathf.Abs(entries[i].transform.localPosition.y);
                        float distanceSmaller = distance - 60;
                        entries[i].GetComponent<ChangeHotkey>().overlay.transform.localScale = new Vector3(1, 1 - (distanceSmaller / 70f), 1);
                    }
                    else
                    {
                        entries[i].GetComponent<ChangeHotkey>().overlay.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                try
                {
                    if (entries[i].GetComponent<ChangeHotkey>().waitForKey)
                    {
                        needBlock = true;

                    }
                }
                catch { }
            }
            if (needBlock == true)
            {
                blocker.SetActive(true);
                Manager.m.inHotkeySet = true;
            }
            else
            {
                blocker.SetActive(false);
                StartCoroutine(SetManagerInHotkeySetFalse());
            }


            if (Input.mouseScrollDelta.y != 0 && needBlock == false)
            {
                toMove += Input.mouseScrollDelta.y * (-1f) * (1f / entries.Length);
            }


            if (toMove != 0)
            {
                scrollbar.value += toMove * 0.5f;
                toMove = toMove * 0.50f;
                if (Mathf.Abs(toMove) < 0.00001f)
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

            if (localScollValue != scrollbar.value)
            {
                float diffrence = scrollbar.value - localScollValue;

                localScollValue += 0.15f * diffrence;
                if (Mathf.Abs(diffrence) < 0.00001f)
                {
                    localScollValue = scrollbar.value;
                }
            }
            onHotkeyEnter = false;
        }
        else
        {
            onHotkeyEnter = true;
        }
    }

    public IEnumerator SetManagerInHotkeySetFalse()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Manager.m.inHotkeySet = false;
    }

    public void RemoveSameHotkey(string hotkeyDef, string hotkey)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].GetComponent<ChangeHotkey>().hotkey == hotkey && entries[i].GetComponent<ChangeHotkey>().hotkeyDef != hotkeyDef)
            {
                entries[i].GetComponent<ChangeHotkey>().hotkey = "";
            }
        }
    }
    public void RemoveSameHotkey(string hotkeyDef, string hotkey, string[] allowedSame)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].GetComponent<ChangeHotkey>().hotkey == hotkey && entries[i].GetComponent<ChangeHotkey>().hotkeyDef != hotkeyDef)
            {
                bool isAllowed = false;
                for (int x = 0; x < allowedSame.Length; x++)
                {
                    if (entries[i].GetComponent<ChangeHotkey>().hotkeyDef == allowedSame[x])
                    {
                        isAllowed = true;
                    }
                }
                if (isAllowed == false)
                {
                    entries[i].GetComponent<ChangeHotkey>().hotkey = "";
                }
            }
        }
    }

    public void SetHotkeyMiddle(string hotkeyDef)
    {
        int position = -1;

        for(int i = 0; i < entries.Length; i++)
        {
            if (entries[i].GetComponent<ChangeHotkey>().hotkeyDef == hotkeyDef)
            {
                position = i + 1;
            }
        }
        if (position == -1)
        {
            print("Error - Hotkey <" + hotkeyDef + "> not found");
        }
        else
        {
            float scrollbarValue = (-50 + ((float)position - 1) * 60) / (-90 + ((float)entries.Length - 1) * 60);
            if (scrollbarValue > 1)
            {
                scrollbarValue = 1;
            }
            else if (scrollbarValue < 0)
            {
                scrollbarValue = 0;
            }
            //toMove += scrollbarValue - scrollbar.value;
            scrollbar.value = scrollbarValue;
        }
    }

    void ResetKeys()
    {
        Manager.m.effectSpeaker.click();
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].GetComponent<ChangeHotkey>().hotkeyDef != "")
            {
                entries[i].GetComponent<ChangeHotkey>().hotkey = Manager.m.standardKeyActionTrigger[Array.IndexOf(Manager.m.keyActions, entries[i].GetComponent<ChangeHotkey>().hotkeyDef)];
            }
        }
    }

    bool OriginalKeys()
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].GetComponent<ChangeHotkey>().hotkeyDef != "")
            {
                if (entries[i].GetComponent<ChangeHotkey>().hotkey != Manager.m.standardKeyActionTrigger[Array.IndexOf(Manager.m.keyActions, entries[i].GetComponent<ChangeHotkey>().hotkeyDef)])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
