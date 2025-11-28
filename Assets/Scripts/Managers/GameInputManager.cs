using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Http.Headers;

public static class GameInputManager
{
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[92]
    {
        "ClickLeft",
        "ClickRight",
        "ClickMiddle",
        "Click4",
        "Click5",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "N0",
        "N1",
        "N2",
        "N3",
        "N4",
        "N5",
        "N6",
        "N7",
        "N8",
        "N9",
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
        "Q",
        "R",
        "S",
        "T",
        "U",
        "V",
        "W",
        "X",
        "Y",
        "Z",
        "Space",
        "Backspace",
        "Enter",
        "KeypadEnter",
        "LeftShift",
        "RightShift",
        "LeftControl",
        "RightControl",
        "LeftAlt",
        "RightAlt",
        "Esc",
        "ArrowUp",
        "ArroDown",
        "ArrowRight",
        "ArrowLeft",
        "F1",
        "F2",
        "F3",
        "F4",
        "F5",
        "F6",
        "F7",
        "F8",
        "F9",
        "F10",
        //"Plus",
        "Minus",
        "Hash",
        "Comma",
        "Period",
        "Delete",
        "Insert",
        "End",
        "PageUp",
        "PageDown",
        "NPlus",
        "NMinus",
        "NDivide",
        "NMultiply",
        "Numpad",
        "Tab",
        "CapsLock",
    };
    static KeyCode[] defaults = new KeyCode[92]
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Mouse3,
        KeyCode.Mouse4,
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z,
        KeyCode.Space,
        KeyCode.Backspace,
        KeyCode.Return,
        KeyCode.KeypadEnter,
        KeyCode.LeftShift,
        KeyCode.RightShift,
        KeyCode.LeftControl,
        KeyCode.RightControl,
        KeyCode.LeftAlt,
        KeyCode.RightAlt,
        KeyCode.Escape,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        //KeyCode.Plus,
        KeyCode.Minus,
        KeyCode.Hash,
        KeyCode.Comma,
        KeyCode.Period,
        KeyCode.Delete,
        KeyCode.Insert,
        KeyCode.End,
        KeyCode.PageUp,
        KeyCode.PageDown,
        KeyCode.KeypadPlus, 
        KeyCode.KeypadMinus,
        KeyCode.KeypadDivide,
        KeyCode.KeypadMultiply,
        KeyCode.Numlock,
        KeyCode.Tab,
        KeyCode.CapsLock,
    };
    static string[] keyNames = new string[92]
    {
        "CL",
        "CR",
        "CM",
        "C4",
        "C5",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "N0",
        "N1",
        "N2",
        "N3",
        "N4",
        "N5",
        "N6",
        "N7",
        "N8",
        "N9",
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
        "Q",
        "R",
        "S",
        "T",
        "U",
        "V",
        "W",
        "X",
        "Y",
        "z",
        "SB",
        "BS",
        "ENT",
        "NENT",
        "LSHIF",
        "RSHIF",
        "LCTRL",
        "RCTRL",
        "LALT",
        "RALT",
        "ESC",
        "UP",
        "DOWN",
        "RIGHT",
        "LEFT",
        "F1",
        "F2",
        "F3",
        "F4",
        "F5",
        "F6",
        "F7",
        "F8",
        "F9",
        "F10",
        //"+",
        "-",
        "#",
        ",",
        ".",
        "Del",
        "Ins",
        "End",
        "PUp",
        "PDown",
        "N+",
        "N-",
        "NDiv",
        "NMul",
        "NP",
        "TAB",
        "CL",
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        string[] keys = keyMap.Split('+');

        if (string.IsNullOrEmpty(keyMap) || Manager.m.inHotkeySet || Manager.m.introScene.inIntroScene ||
            Manager.m.tutorial.inTutorial != 0 ||
            Manager.m.inFinalSequence || Manager.m.inMainMenu || Manager.m.loading)
        {
            return false;
        }

        if (keys.Length == 1)
        {
            for (int i = 0; i < keyMaps.Length; i++)
            {
                if (Input.GetKey(keyMapping[keyMaps[i]]) && keyMaps[i] != keys[0])
                {
                    return false;
                }
            }
            return Input.GetKeyDown(keyMapping[keys[0]]);
        }

        for (int i = 0; i < keys.Length - 1; i++)
        {
            if (!Input.GetKey(keyMapping[keys[i]]))
                return false;
        }

        return Input.GetKeyDown(keyMapping[keys[keys.Length - 1]]);
    }
    public static bool GetKeyUp(string keyMap)
    {
        if (keyMap != "" && keyMap != null)
        {
            return Input.GetKeyUp(keyMapping[keyMap]);
        }
        else
        {
            return false;
        }
    }
    public static bool GetKey(string keyMap)
    {
        if (keyMap != "" && keyMap != null)
        {
            return Input.GetKey(keyMapping[keyMap]);
        }
        else
        {
            return false;
        }
    }

    public static KeyCode[] GetKeyCodes()
    {
        return defaults;
    }
    
    public static string[] GetKeyMaps()
    {
        return keyMaps;
    }

    public static string[] GetKeyNames()
    {
        return keyNames;
    }
}
