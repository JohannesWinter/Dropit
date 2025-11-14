using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    bool playIntroScene = false;
    public bool inIntroScene = false;
    bool inBlackOut = false;
    bool inBlackIn = false;
    bool displayingText = false;
    bool writingText = false;
    int displayTextCounter = 0;
    public GameObject display;
    public GameObject backround;
    public GameObject overlay;
    public GameObject blackBoard;
    public GameObject textObject;
    public GameObject skip;
    public AudioSource wind;
    public AudioSource beep;
    public TextMeshProUGUI storyText;
    public string[] texts;

    double currenttime;
    // Start is called before the first frame update
    void Start()
    {
        beep.volume = 0;
        display.SetActive(false);
        backround.SetActive(false);
        overlay.SetActive(false);
        blackBoard.SetActive(false);
        textObject.SetActive(false);
        skip.SetActive(false);
        
        currenttime = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(playIntroScene == true && inIntroScene == false)
        {
            Manager.m.musicSpeaker.enableMusic = false;
            playIntroScene = false;
            inIntroScene = true;
            currenttime = Time.unscaledTime;
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            display.SetActive(true);
            backround.SetActive(true);
            overlay.SetActive(true);
            textObject.SetActive(true);
            if(PlayerPrefs.GetInt(Manager.m.version + "_" + "_IntroSkipable") == 1)
            {
                skip.SetActive(true);
            }
            //await Task.Delay((int)Mathf.Ceil(Time.unscaledDeltaTime * 1000));
            wind.Play();
            displayTextCounter = 0;
            displayingText = true;
        }
        if(displayingText == true && inIntroScene == true)
        {
            wind.volume = Manager.m.effectsVolume.publicVolume;
            beep.volume = Manager.m.effectsVolume.publicVolume * 0.8f;
            if (displayTextCounter < texts.Length)
            {
                if (writingText == false)
                {
                    if(displayTextCounter == 0)
                    {
                        StartCoroutine(displayText(texts[displayTextCounter], ""));
                    }
                    else
                    {
                        string textBefore = "";
                        for(int i = 0; i < texts[displayTextCounter - 1].Length - 1; i++)
                        {
                            textBefore += texts[displayTextCounter - 1][i];
                        }
                        StartCoroutine(displayText(texts[displayTextCounter], textBefore));
                    }
                }
            }
            else
            {
                displayingText = false;
                inBlackOut = true;
                StartCoroutine(blackOut());
            }
            if(skip.activeSelf == true)
            {
                if(Input.anyKeyDown)
                {
                    displayTextCounter = texts.Length; //ends loop for text display -> ends intro scene
                }
            }
        }


        if(inIntroScene == true && currenttime + 31 < Time.unscaledTime && inBlackOut == false && inBlackIn == false)
        {

        }
        if(inBlackIn == true)
        {
            display.SetActive(false);
            backround.SetActive(false);
            overlay.SetActive(false);
            skip.SetActive(false);
            textObject.SetActive(false);

            inIntroScene = false;
            Manager.m.musicSpeaker.enableMusic = true;
            PlayerPrefs.SetInt(Manager.m.version + "_" + "_IntroSkipable", 1);
        }
    }
    public bool PlayIntroScene()
    {
        if (inIntroScene == false)
        {
            playIntroScene = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator blackOut()
    {
        blackBoard.SetActive(true);
        for (int i = 0; i < 40; i++)
        {
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, blackBoard.GetComponent<RawImage>().color.a + 0.025f);
            wind.volume = (1 - (i + 5) * 0.025f) * Manager.m.effectsVolume.publicVolume;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        storyText.text = "";
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(blackIn());
        inBlackIn = true;
        inBlackOut = false;
        Manager.m.missionManager.missions.Clear();
    }

    public IEnumerator blackIn()
    {
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i < 20; i++)
        {
            blackBoard.GetComponent<RawImage>().color = new Color(0, 0, 0, blackBoard.GetComponent<RawImage>().color.a - 0.05f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        blackBoard.SetActive(false);
    }

    public IEnumerator displayText(string text, string textBefore)
    {
        writingText = true;
        string textNow = "";
        for (int i = 0; i < text.Length - 1; i++)
        {
            if(displayTextCounter >= texts.Length)
            {
                break;
            }
            textNow += text[i];
            if(text[i].Equals(' '))
            {
            }
            else
            { 
                storyText.text = textNow + "<br>" + textBefore;
                beep.Play();
                yield return new WaitForSecondsRealtime(0.15f);
            }
        }
        if (text[text.Length - 1].Equals('='))
        {
            yield return new WaitForSecondsRealtime(0.10f);
        }
        else if (text[text.Length - 1].Equals('-'))
        {
            yield return new WaitForSecondsRealtime(0.50f);
        }
        else if (text[text.Length - 1].Equals('+'))
        {
            yield return new WaitForSecondsRealtime(1.00f);
        }
        displayTextCounter += 1;
        writingText = false;
    }
}
