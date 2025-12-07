using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public bool generalVolume;
    public float publicVolume;
    public int volume;

    public GameObject plus;
    public GameObject minus;

    public GameObject on;
    public GameObject off;

    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    public GameObject bar4;
    public GameObject bar5;
    public GameObject bar6;
    public GameObject bar7;
    public GameObject bar8;
    public GameObject bar9;
    public GameObject bar10;

    private int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (generalVolume)
        {
            Manager.m.generalVolume.volume = 10;
            Manager.m.factoryVolume.volume = 10;
            Manager.m.effectsVolume.volume = 10;
            Manager.m.voiceVolume.volume = 10;
            Manager.m.musicVolume.volume = 10;
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_GeneralVolume") != 0)
            {
                Manager.m.generalVolume.volume = PlayerPrefs.GetInt(Manager.m.version + "_" + "_GeneralVolume") - 1;
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_FactoryVolume") != 0)
            {
                Manager.m.factoryVolume.volume = PlayerPrefs.GetInt(Manager.m.version + "_" + "_FactoryVolume") - 1;
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_EffectsVolume") != 0)
            {
                Manager.m.effectsVolume.volume = PlayerPrefs.GetInt(Manager.m.version + "_" + "_EffectsVolume") - 1;
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_VoiceVolume") != 0)
            {
                Manager.m.voiceVolume.volume = PlayerPrefs.GetInt(Manager.m.version + "_" + "_VoiceVolume") - 1;
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_MusicVolume") != 0)
            {
                Manager.m.musicVolume.volume = PlayerPrefs.GetInt(Manager.m.version + "_" + "_MusicVolume") - 1;
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_GeneralVolume_Disabled") != 0)
            {
                Manager.m.generalVolume.off.SetActive(true);
                Manager.m.generalVolume.on.SetActive(false);
            }
            else
            {
                Manager.m.generalVolume.off.SetActive(false);
                Manager.m.generalVolume.on.SetActive(true);
            }

            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_FactoryVolume_Disabled") != 0)
            {
                Manager.m.factoryVolume.off.SetActive(true);
                Manager.m.factoryVolume.on.SetActive(false);
            }
            else
            {
                Manager.m.factoryVolume.off.SetActive(false);
                Manager.m.factoryVolume.on.SetActive(true);
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_EffectsVolume_Disabled") != 0)
            {
                Manager.m.effectsVolume.off.SetActive(true);
                Manager.m.effectsVolume.on.SetActive(false);
            }
            else
            {
                Manager.m.effectsVolume.off.SetActive(false);
                Manager.m.effectsVolume.on.SetActive(true);
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_VoiceVolume_Disabled") != 0)
            {
                Manager.m.voiceVolume.off.SetActive(true);
                Manager.m.voiceVolume.on.SetActive(false);
            }
            else
            {
                Manager.m.voiceVolume.off.SetActive(false);
                Manager.m.voiceVolume.on.SetActive(true);
            }
            if (PlayerPrefs.GetInt(Manager.m.version + "_" + "_MusicVolume_Disabled") != 0)
            {
                Manager.m.musicVolume.off.SetActive(true);
                Manager.m.musicVolume.on.SetActive(false);
            }
            else
            {
                Manager.m.musicVolume.off.SetActive(false);
                Manager.m.musicVolume.on.SetActive(true);
            }
        }

        plus.GetComponent<Button>().onClick.AddListener(Plus);
        minus.GetComponent<Button>().onClick.AddListener(Minus);

        on.GetComponent<Button>().onClick.AddListener(On);
        off.GetComponent<Button>().onClick.AddListener(Off);

        bar1.GetComponent<Button>().onClick.AddListener(Bar1);
        bar2.GetComponent<Button>().onClick.AddListener(Bar2);
        bar3.GetComponent<Button>().onClick.AddListener(Bar3);
        bar4.GetComponent<Button>().onClick.AddListener(Bar4);
        bar5.GetComponent<Button>().onClick.AddListener(Bar5);
        bar6.GetComponent<Button>().onClick.AddListener(Bar6);
        bar7.GetComponent<Button>().onClick.AddListener(Bar7);
        bar8.GetComponent<Button>().onClick.AddListener(Bar8);
        bar9.GetComponent<Button>().onClick.AddListener(Bar9);
        bar10.GetComponent<Button>().onClick.AddListener(Bar10);
    }

    // Update is called once per frame
    void Update()
    {
        if (generalVolume)
        {
            if (counter >= 300)
            {
                counter = 0;
                PlayerPrefs.SetInt(Manager.m.version + "_" + "_GeneralVolume", Manager.m.generalVolume.volume + 1);
                PlayerPrefs.SetInt(Manager.m.version + "_" + "_FactoryVolume", Manager.m.factoryVolume.volume + 1);
                PlayerPrefs.SetInt(Manager.m.version + "_" + "_EffectsVolume", Manager.m.effectsVolume.volume + 1);
                PlayerPrefs.SetInt(Manager.m.version + "_" + "_VoiceVolume", Manager.m.voiceVolume.volume + 1);
                PlayerPrefs.SetInt(Manager.m.version + "_" + "_MusicVolume", Manager.m.musicVolume.volume + 1);

                if (Manager.m.generalVolume.off.activeSelf == false)
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_GeneralVolume_Disabled", 0);
                }
                else
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_GeneralVolume_Disabled", 1);
                }
                if (Manager.m.factoryVolume.off.activeSelf == false)
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_FactoryVolume_Disabled", 0);
                }
                else
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_FactoryVolume_Disabled", 1);
                }
                if (Manager.m.effectsVolume.off.activeSelf == false)
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_EffectsVolume_Disabled", 0);
                }
                else
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_EffectsVolume_Disabled", 1);
                }
                if (Manager.m.voiceVolume.off.activeSelf == false)
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_VoiceVolume_Disabled", 0);
                }
                else
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_VoiceVolume_Disabled", 1);
                }
                if (Manager.m.musicVolume.off.activeSelf == false)
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_MusicVolume_Disabled", 0);
                }
                else
                {
                    PlayerPrefs.SetInt(Manager.m.version + "_" + "_MusicVolume_Disabled", 1);
                }
            }
            counter++;
        }


        if (off.activeSelf == true)
        {
            publicVolume = 0;
        }
        else
        {
            if (generalVolume)
            {
                publicVolume = volume * 0.1f;
            }
            else
            {
                publicVolume = volume * 0.1f * Manager.m.generalVolume.publicVolume;
            }
        }
        if (off.activeSelf == false && on.activeSelf == false)
        {
            on.SetActive(true);
        }


        if (volume >= 1)
        {
            bar1.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar1.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 2)
        {
            bar2.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar2.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 3)
        {
            bar3.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar3.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 4)
        {
            bar4.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar4.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 5)
        {
            bar5.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar5.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 6)
        {
            bar6.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar6.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 7)
        {
            bar7.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar7.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 8)
        {
            bar8.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar8.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 9)
        {
            bar9.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar9.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
        if (volume >= 10)
        {
            bar10.GetComponent<RawImage>().color = new Color(0, 1, 0);
        }
        else
        {
            bar10.GetComponent<RawImage>().color = new Color(0.2f, 0.5f, 0.2f, 1);
        }
    }
    void Plus()
    {
        if (volume < 10)
        {
            volume++;
        }
        Manager.m.effectSpeaker.click();
    }
    void Minus()
    {
        if (volume > 0)
        {
            volume--;
        }
        Manager.m.effectSpeaker.click();
    }
    public void On()
    {
        on.SetActive(false);
        off.SetActive(true);
        Manager.m.effectSpeaker.click();
    }
    public void Off()
    {
        off.SetActive(false);
        on.SetActive(true);
        Manager.m.effectSpeaker.click();
    }
    void Bar1()
    {
        volume = 1;
        Manager.m.effectSpeaker.click();
    }
    void Bar2()
    {
        volume = 2;
        Manager.m.effectSpeaker.click();
    }
    void Bar3()
    {
        volume = 3;
        Manager.m.effectSpeaker.click();
    }
    void Bar4()
    {
        volume = 4;
        Manager.m.effectSpeaker.click();
    }
    void Bar5()
    {
        volume = 5;
        Manager.m.effectSpeaker.click();
    }
    void Bar6()
    {
        volume = 6;
        Manager.m.effectSpeaker.click();
    }
    void Bar7()
    {
        volume = 7;
        Manager.m.effectSpeaker.click();
    }
    void Bar8()
    {
        volume = 8;
        Manager.m.effectSpeaker.click();
    }
    void Bar9()
    {
        volume = 9;
        Manager.m.effectSpeaker.click();
    }
    void Bar10()
    {
        volume = 10;
        Manager.m.effectSpeaker.click();
    }
}
