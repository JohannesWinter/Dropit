using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenueEnviroment : MonoBehaviour
{
    public GameObject enviroment_wheel;
    public bool enviroment_squeakWheel;
    bool enviroment_squeakingWheel;
    public GameObject enviroment_electricLightOn;
    public GameObject enviroment_electricLightOff;
    public bool enviroment_switchLight;
    bool enviroment_switchingLight;
    public GameObject enviroment_barrel;
    public bool enviroment_rollBarrel;
    bool enviroment_rollingBarrel;
    public GameObject enviroment_railing;
    public bool enviroment_swingRailing;
    bool enviroment_swingingRailing;

    float hummingTimer;
    PlaySound currentHumming;
    PlaySound oldHumming;
    float currentNoiseVolume = 1;

    bool leavingMenue = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hummingTimer += Time.unscaledDeltaTime;
        if (currentHumming == null)
        {
            currentHumming = Manager.m.effectSpeaker.humming();
            currentHumming.audiosource.volume = 0;
            hummingTimer = 0;
        }
        else if (currentHumming != null && hummingTimer >= currentHumming.audiosource.clip.length * 0.9f)
        {
            oldHumming = currentHumming;
            currentHumming = null;
        }
        if (oldHumming != null)
        {
            oldHumming.audiosource.volume -= Time.unscaledDeltaTime;
        }
        if (currentHumming != null && currentHumming.audiosource.volume < 1 * Manager.m.effectsVolume.publicVolume)
        {
            currentHumming.audiosource.volume += Time.unscaledDeltaTime;
        }


        bool enteringGame = false;
        for (int i = 0; i < Manager.m.mainMenu.blackBoardUsed.Length; i++)
        {
            if (Manager.m.mainMenu.blackBoardUsed[i] == true)
            {
                enteringGame = true;
            }
        }
        if (Manager.m.inMainMenu && enteringGame == false)
        {
            currentNoiseVolume += Time.unscaledDeltaTime * 0.25f;
            leavingMenue = false;
        }
        else
        {
            currentNoiseVolume -= Time.unscaledDeltaTime * 0.25f;
            leavingMenue = true;
        }
        if (currentNoiseVolume > 1)
        {
            currentNoiseVolume = 1;
        }
        else if (currentNoiseVolume < 0)
        {
            currentNoiseVolume = 0;
        }
        if (Manager.m.inMainMenu || currentNoiseVolume > 0)
        {   
            if (currentHumming != null)
            {
                currentHumming.audiosource.mute = false;
                currentHumming.audiosource.volume = Manager.m.effectsVolume.publicVolume * currentNoiseVolume;
            }
            if (oldHumming != null)
            {
                oldHumming.audiosource.mute = false;
                oldHumming.audiosource.volume = Manager.m.effectsVolume.publicVolume * currentNoiseVolume;
            }
        }
        else
        {
            if (currentNoiseVolume > 1)
            {
                currentNoiseVolume = 1;
            }
            if (currentHumming != null)
            {
                currentHumming.audiosource.mute = true;
            }
            if (oldHumming != null)
            {
                oldHumming.audiosource.mute = true;
            }
        }


        switch (Random.Range(0, (int)(100 / Time.unscaledDeltaTime)))
        {
            case 0:
                {
                    enviroment_squeakWheel = true;
                    break;
                }
            case 1:
                {
                    enviroment_switchLight = true;
                    break;
                }
            case 2:
                {
                    enviroment_rollBarrel = true;
                    break;
                }
            case 3:
                {
                    enviroment_swingRailing = true;
                    break;
                }
        }
        if (enviroment_squeakWheel == true && (Manager.m.inMainMenu))
        {
            enviroment_squeakWheel = false;
            StartCoroutine(SqueakWheel());
        }
        if (enviroment_switchLight == true && (Manager.m.inMainMenu))
        {
            enviroment_switchLight = false;
            StartCoroutine(SwitchLight());
        }
        if (enviroment_rollBarrel == true && (Manager.m.inMainMenu))
        {
            enviroment_rollBarrel = false;
            StartCoroutine(RollBarrel());
        }
        if (enviroment_swingRailing == true && (Manager.m.inMainMenu))
        {
            enviroment_swingRailing = false;
            StartCoroutine(SwingRailing());
        }
    }

    public IEnumerator SqueakWheel()
    {
        if (enviroment_squeakingWheel == false)
        {
            enviroment_squeakingWheel = true;
            PlaySound sound = Manager.m.effectSpeaker.squeakWheel();
            float currentTime = Time.unscaledTime;
            while (currentTime > Time.unscaledTime - 2f)
            {
                float progress = Time.unscaledTime - currentTime;
                if (progress > 1)
                {
                    progress = 2 - progress;
                }
                enviroment_wheel.transform.Rotate(-50 * Time.unscaledDeltaTime * progress, 0, 0);
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            currentTime = Time.unscaledTime;
            while (currentTime > Time.unscaledTime - 1.4f)
            {
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            currentTime = Time.unscaledTime;
            while (currentTime > Time.unscaledTime - 1.5f)
            {
                float progress = Time.unscaledTime - currentTime;
                if (progress > 0.75f)
                {
                    progress = 1.5f - progress;
                }
                enviroment_wheel.transform.Rotate(30 * Time.unscaledDeltaTime * progress, 0, 0);
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            while (currentTime > Time.unscaledTime - 1f)
            {
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            enviroment_squeakingWheel = false;
        }
    }

    public IEnumerator SwitchLight()
    {
        if (enviroment_switchingLight == false)
        {
            enviroment_switchingLight = true;
            enviroment_electricLightOn.SetActive(true);
            enviroment_electricLightOff.SetActive(false);
            Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.clickLight, 2f);
            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 2f));
            if (leavingMenue == false)
            {
                Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.clickLight, 1.5f);
            }
            enviroment_electricLightOn.SetActive(false);
            enviroment_electricLightOff.SetActive(true);
            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 2f));
            enviroment_switchingLight = false;
            if (Random.Range(0, 4) != 0)
            {
                enviroment_switchLight = true;
            }
        }
    }

    public IEnumerator RollBarrel()
    {
        if (enviroment_rollingBarrel == false)
        {
            enviroment_rollingBarrel = true;
            PlaySound sound = null;
            if (Random.Range(0, 2) == 0)
            {
                sound = Manager.m.effectSpeaker.barrel1();
            }
            else
            {
                sound = Manager.m.effectSpeaker.barrel2();
            }
            while (enviroment_barrel.transform.localPosition.z < 0.1f)
            {
                float restDistance = 0.14f - enviroment_barrel.transform.localPosition.z;
                float restMul = restDistance * restDistance;
                enviroment_barrel.transform.localPosition += new Vector3(0, 0, Time.unscaledDeltaTime * 30f * restMul);
                enviroment_barrel.transform.localRotation = Quaternion.Euler(enviroment_barrel.transform.localRotation.eulerAngles + new Vector3(0, Time.unscaledDeltaTime * 6000 * restMul, 0));
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            while (enviroment_barrel.transform.localPosition.z > 0.0f)
            {
                float restDistance = 0.14f - enviroment_barrel.transform.localPosition.z;
                float restMul = restDistance * restDistance;
                enviroment_barrel.transform.localPosition -= new Vector3(0, 0, Time.unscaledDeltaTime * 30f * restMul);
                enviroment_barrel.transform.localRotation = Quaternion.Euler(enviroment_barrel.transform.localRotation.eulerAngles - new Vector3(0, Time.unscaledDeltaTime * 6000 * restMul, 0));
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            while (enviroment_barrel.transform.localPosition.z > -0.1f)
            {
                float restDistance = -0.14f - enviroment_barrel.transform.localPosition.z;
                float restMul = restDistance * restDistance;
                enviroment_barrel.transform.localPosition -= new Vector3(0, 0, Time.unscaledDeltaTime * 30f * restMul);
                enviroment_barrel.transform.localRotation = Quaternion.Euler(enviroment_barrel.transform.localRotation.eulerAngles - new Vector3(0, Time.unscaledDeltaTime * 6000 * restMul, 0));
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            while (enviroment_barrel.transform.localPosition.z < 0)
            {
                float restDistance = -0.14f - enviroment_barrel.transform.localPosition.z;
                float restMul = restDistance * restDistance;
                enviroment_barrel.transform.localPosition += new Vector3(0, 0, Time.unscaledDeltaTime * 30f * restMul);
                enviroment_barrel.transform.localRotation = Quaternion.Euler(enviroment_barrel.transform.localRotation.eulerAngles + new Vector3(0, Time.unscaledDeltaTime * 6000 * restMul, 0));
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }

            yield return new WaitForSecondsRealtime(1);
            enviroment_barrel.transform.localPosition = new Vector3(0, 0, 0);
            enviroment_barrel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            enviroment_rollingBarrel = false;
        }
    }

    public IEnumerator SwingRailing()
    {
        if (enviroment_swingingRailing == false)
        {
            enviroment_swingingRailing = true;
            PlaySound sound = Manager.m.effectSpeaker.railing();
            float startTime = Time.unscaledTime;
            while (startTime + 1 - Time.unscaledTime > 0)
            {
                float timeMult = Time.unscaledTime - startTime;
                enviroment_railing.transform.localRotation = Quaternion.Euler(enviroment_railing.transform.localRotation.eulerAngles - new Vector3(0, Time.unscaledDeltaTime * 70 * timeMult, 0));
                if (Manager.m.inMainMenu == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            startTime = Time.unscaledTime;
            if (Manager.m.inMainMenu)
            {
                while (startTime + 1 - Time.unscaledTime > 0)
                {
                    float timeMult = (startTime + 1) - Time.unscaledTime;
                    enviroment_railing.transform.localRotation = Quaternion.Euler(enviroment_railing.transform.localRotation.eulerAngles + new Vector3(0, Time.unscaledDeltaTime * 70 * timeMult, 0));
                    if (Manager.m.inMainMenu == false && sound != null)
                    {
                        Destroy(sound);
                    }
                    yield return null;
                }
            }
            enviroment_railing.transform.localRotation = Quaternion.Euler(0, 0, 0);
            enviroment_swingingRailing = false;
        }
        yield return null;
    }
}
