using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalSequence : MonoBehaviour
{
    public Camera finalCamera1;
    public Camera finalCamera2;
    public string[] texts;
    public int[] textChangePositions;
    public bool[] steps;
    public TextMeshProUGUI currentText;
    public RawImage blackScreen;
    bool blackingOut;
    bool changingText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < texts.Length; i++)
        {
            if(finalCamera2.gameObject.transform.localPosition.x >= textChangePositions[i] && changingText == false && currentText.text != texts[i] && finalCamera2.gameObject.transform.localPosition.x < textChangePositions[i+1])
            {
                StartCoroutine(changeText(currentText, texts[i]));
            }
        }
        if (Manager.m.hallUpgrader[Manager.m.hallUpgrader.Length - 1].inDisappearAnimation == true && Manager.m.inFinalSequence == false && Manager.m.finishedFinalSequence == false && Manager.m.changeSaveTimer == 0)
        {
            Manager.m.startFinalSequence = true;
        }
        if(Manager.m.startFinalSequence == true)
        {
            Manager.m.startFinalSequence = false;
            Manager.m.inFinalSequence = true;
            RenderSettings.fog = true;
            Manager.m.setKamera(finalCamera1);
            finalCamera1.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            finalCamera2.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            steps[0] = false;
            steps[1] = true;
        }
        if (steps[1] == true)
        {
            if (Manager.m.hallUpgrader[Manager.m.hallUpgrader.Length - 1].self.activeSelf == false)
            {
                steps[1] = false;
                blackScreen.color = new Color(0, 0, 0, 0);
                StartCoroutine(blackOut());
            }
        }
        if (steps[1] == true || blackingOut == true)
        {
            finalCamera1.gameObject.transform.Translate(0, 0, -1 * Time.unscaledDeltaTime);
        }
        if (steps[2] == true)
        {
            finalCamera2.gameObject.transform.localPosition = new Vector3(finalCamera2.gameObject.transform.localPosition.x + 15 * Time.unscaledDeltaTime, finalCamera2.gameObject.transform.localPosition.y, finalCamera2.gameObject.transform.localPosition.z);
            if (finalCamera2.gameObject.transform.localPosition.x > 1300)
            {
                RenderSettings.fogDensity += 0.0002f * Time.unscaledDeltaTime;
            }
            if (finalCamera2.gameObject.transform.localPosition.x > 1670 || (finalCamera2.gameObject.transform.localPosition.x > 100) && Input.GetButton("ClickLeft"))
            {
                steps[2] = false;
                StartCoroutine(blackOut2());
            }
        }
        if (steps[3] == true)
        {
            RenderSettings.fog = false;
            RenderSettings.fogDensity = 0.001f;
            Manager.m.finishedFinalSequence = true;
            Manager.m.inFinalSequence = false;
            Manager.m.hideFactoryUI = false;
            Manager.m.musicSpeaker.enableMusic = true;
            currentText.text = "";
        }
    }

    public IEnumerator blackOut()
    {
        blackingOut = true;
        Manager.m.musicSpeaker.ChangeMusic(3, 3, "credits", 0);
        for (int i = 0; i < 150; i++)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a + 0.01f);

            yield return new WaitForSecondsRealtime(0.05f);
            if(blackScreen.color.a > 1)
            {
                blackScreen.color = new Color(0, 0, 0, 1);
            }
        }
        steps[2] = true;
        Manager.m.setKamera(finalCamera2);
        blackingOut = false;

        for (int i = 0; i < 50; i++)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a - 0.02f);

            yield return new WaitForSecondsRealtime(0.05f);
            if (blackScreen.color.a < 0)
            {
                blackScreen.color = new Color(0, 0, 0, 0);
            }
        }
    }
    public IEnumerator blackOut2()
    {
        blackingOut = true;
        Manager.m.musicSpeaker.ChangeMusic(1, 5, "normal", 0);
        for (int i = 0; i < 150; i++)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a + 0.01f);
            finalCamera2.gameObject.transform.localPosition = new Vector3(finalCamera2.gameObject.transform.localPosition.x + 1.3f , finalCamera2.gameObject.transform.localPosition.y, finalCamera2.gameObject.transform.localPosition.z);
            yield return new WaitForSecondsRealtime(0.05f);
            if (blackScreen.color.a > 1)
            {
                blackScreen.color = new Color(0, 0, 0, 1);
            }
        }
        steps[3] = true;
        Manager.m.setKamera(Manager.m.lastDropperCamera);
        blackingOut = false;

        for (int i = 0; i < 50; i++)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a - 0.02f);

            yield return new WaitForSecondsRealtime(0.05f);
            if (blackScreen.color.a < 0)
            {
                blackScreen.color = new Color(0, 0, 0, 0);
            }
        }
    }
    public IEnumerator pauseMusicIn(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Manager.m.musicSpeaker.enableMusic = false;
    }

    public IEnumerator changeText(TextMeshProUGUI t, string newText)
    {
        changingText = true;
        for (int i = 0; i < 20; i++)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - 0.05f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        t.text = newText;
        for (int i = 0; i < 20; i++)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + 0.05f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        changingText = false;
    }
}
