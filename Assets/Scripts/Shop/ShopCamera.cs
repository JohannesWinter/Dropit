using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCamera : MonoBehaviour
{
    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject autoRepair;
    public GameObject autoRepairEnabled;
    public Button switchAutoRepair;
    public GameObject lockObjects;
    public GameObject lockText;
    public GameObject podestDroppers;
    public GameObject podestMachines;

    public GameObject podestLightDroppers;
    public Color[] lightColorsDroppers;
    public float[] lightColorsDroppersIntensities;
    public GameObject podestLightMachines;
    public Color[] lightColorsMachines;
    public float[] lightColorsMachinesIntensities;
    public float[] dropperLowPosition;
    public float[] machineLowPosition;
    public float[] dropperHighPosition;
    public float[] machineHighPosition;
    public GameObject[] dropperDisplays;
    public GameObject[] machineDisplays;
    public GameObject informationBoard;
    public GameObject buyButtonEnabled;
    public GameObject buyButtonDisabled;
    public BackToFactory backScript;
    public BuyButton buyScript;
    public float changeSpeed;
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

    // Start is called before the first frame update
    void Start()
    {
        rightArrow.GetComponent<Button>().onClick.AddListener(Right);
        leftArrow.GetComponent<Button>().onClick.AddListener(Left);
        switchAutoRepair.onClick.AddListener(SwitchAutoRepair);

        enviroment_electricLightOff.SetActive(true);
        enviroment_electricLightOn.SetActive(false);

        podestDroppers.SetActive(true);
        podestMachines.SetActive(true);

        for (int i = 0; i < dropperDisplays.Length; i++)
        {
            dropperDisplays[i].transform.localPosition = new Vector3(0, dropperDisplays[i].transform.localPosition.y, 0);
            dropperLowPosition[i] = dropperDisplays[i].transform.localPosition.y;
            dropperDisplays[i].transform.Translate(0, 20, 0);
            dropperHighPosition[i] = dropperDisplays[i].transform.localPosition.y;
        }
        for (int i = 0; i < machineDisplays.Length; i++)
        {
            machineDisplays[i].transform.localPosition = new Vector3(0, machineDisplays[i].transform.localPosition.y, 0);
            machineLowPosition[i] = machineDisplays[i].transform.localPosition.y;
            machineDisplays[i].transform.Translate(0, 20, 0);
            machineHighPosition[i] = machineDisplays[i].transform.localPosition.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        informationBoard.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        leftArrow.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        rightArrow.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        lockObjects.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        buyButtonEnabled.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        buyButtonDisabled.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        autoRepair.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
        backScript.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);

        if (Manager.m.inShopDropper)
        {
            podestDroppers.transform.localPosition = new Vector3(0, 0, 0);
            podestMachines.transform.localPosition = new Vector3(0, -10, 0);

            if (dropperDisplays[Manager.m.dropperNumber - 1].transform.localPosition.y > dropperLowPosition[Manager.m.dropperNumber - 1])
            {
                GameObject currentlyDown = null;
                for (int i = 0; i < dropperDisplays.Length; i++)
                {
                    if (i != Manager.m.dropperNumber - 1)
                    {
                        if (dropperDisplays[i].transform.localPosition.y < dropperHighPosition[i] && currentlyDown == null)
                        {
                            currentlyDown = dropperDisplays[i];
                        }
                        else
                        {
                            dropperDisplays[i].transform.localPosition = new Vector3(0, dropperHighPosition[i], dropperDisplays[i].transform.localPosition.z);
                        }
                    }
                }
                if (currentlyDown != null)
                {
                    currentlyDown.transform.Translate(0, Time.unscaledDeltaTime * changeSpeed, 0);
                }
                else
                {
                    dropperDisplays[Manager.m.dropperNumber - 1].transform.Translate(0, -Time.unscaledDeltaTime * changeSpeed, 0);
                    if (dropperDisplays[Manager.m.dropperNumber - 1].transform.localPosition.y <= dropperLowPosition[Manager.m.dropperNumber - 1] && Manager.m.inShopDropper)
                    {
                        Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.slam, Random.Range(0.8f, 1.1f));
                    }
                }
                informationBoard.SetActive(false);
                buyButtonDisabled.SetActive(true);
                buyButtonEnabled.SetActive(false);
                lockObjects.SetActive(false);
                podestLightDroppers.GetComponent<Renderer>().material.color = Color.black;
                podestLightDroppers.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                podestLightDroppers.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.black);
            }
            else
            {
                dropperDisplays[Manager.m.dropperNumber - 1].transform.localPosition = new Vector3(0, dropperLowPosition[Manager.m.dropperNumber - 1], dropperDisplays[Manager.m.dropperNumber - 1].transform.localPosition.z);
                podestLightDroppers.GetComponent<Renderer>().material.color = lightColorsDroppers[Manager.m.dropperNumber - 1];
                podestLightDroppers.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                podestLightDroppers.GetComponent<Renderer>().material.SetColor("_EmissiveColor", lightColorsDroppers[Manager.m.dropperNumber - 1] * lightColorsDroppersIntensities[Manager.m.dropperNumber - 1]);
                if (Manager.m.getHighestUnlockedType() >= Manager.m.dropperNumber - 1)
                {
                    informationBoard.SetActive(true);
                    buyButtonEnabled.SetActive(true);
                    buyButtonDisabled.SetActive(false);
                    lockObjects.SetActive(false);
                }
                else
                {
                    lockObjects.SetActive(true);
                    informationBoard.SetActive(false);
                    buyButtonDisabled.SetActive(true);
                    buyButtonEnabled.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < dropperDisplays.Length; i++)
            {
                dropperDisplays[i].transform.localPosition = new Vector3(0, dropperHighPosition[i], dropperDisplays[i].transform.localPosition.z);
            }
        }

        if (Manager.m.inShopMachine)
        {
            podestDroppers.transform.localPosition = new Vector3(0, -10, 0);
            podestMachines.transform.localPosition = new Vector3(0, 0, 0);

            if (machineDisplays[Manager.m.machineNumber - 1].transform.localPosition.y > machineLowPosition[Manager.m.machineNumber - 1])
            {
                GameObject currentlyDown = null;
                for (int i = 0; i < machineDisplays.Length; i++)
                {
                    if (i != Manager.m.machineNumber - 1)
                    {
                        if (machineDisplays[i].transform.localPosition.y < machineHighPosition[i] && currentlyDown == null)
                        {
                            currentlyDown = machineDisplays[i];
                        }
                        else
                        {
                            machineDisplays[i].transform.localPosition = new Vector3(0, machineHighPosition[i], machineDisplays[i].transform.localPosition.z);
                        }
                    }
                }
                if (currentlyDown != null)
                {
                    currentlyDown.transform.Translate(0, Time.unscaledDeltaTime * changeSpeed, 0);
                }
                else
                {
                    machineDisplays[Manager.m.machineNumber - 1].transform.Translate(0, -Time.unscaledDeltaTime * changeSpeed, 0);
                    if (machineDisplays[Manager.m.machineNumber - 1].transform.localPosition.y <= machineLowPosition[Manager.m.machineNumber - 1] && Manager.m.inShopMachine)
                    {
                        Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.slam, Random.Range(0.8f, 1.1f));
                    }
                }
                informationBoard.SetActive(false);
                buyButtonDisabled.SetActive(true);
                buyButtonEnabled.SetActive(false);
                lockObjects.SetActive(false);
                podestLightMachines.GetComponent<Renderer>().material.color = Color.black;
                podestLightMachines.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                podestLightMachines.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.black);
            }
            else
            {
                machineDisplays[Manager.m.machineNumber - 1].transform.localPosition = new Vector3(0, machineLowPosition[Manager.m.machineNumber - 1], machineDisplays[Manager.m.machineNumber - 1].transform.localPosition.z);
                podestLightMachines.GetComponent<Renderer>().material.color = lightColorsMachines[Manager.m.machineNumber - 1];
                podestLightMachines.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                podestLightMachines.GetComponent<Renderer>().material.SetColor("_EmissiveColor", lightColorsMachines[Manager.m.machineNumber - 1] * lightColorsMachinesIntensities[Manager.m.machineNumber - 1]);
                if (Manager.m.getHighestUnlockedType() >= Manager.m.machineNumber - 1 || Manager.m.machineNumber == 2)
                {
                    informationBoard.SetActive(true);
                    buyButtonEnabled.SetActive(true);
                    buyButtonDisabled.SetActive(false);
                    lockObjects.SetActive(false);
                }
                else
                {
                    lockObjects.SetActive(true);
                    informationBoard.SetActive(false);
                    buyButtonDisabled.SetActive(true);
                    buyButtonEnabled.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < machineDisplays.Length; i++)
            {
                machineDisplays[i].transform.localPosition = new Vector3(0, machineHighPosition[i], machineDisplays[i].transform.localPosition.z);
            }
        }

        switch (Random.Range(0, (int) (100 / Time.unscaledDeltaTime)))
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
        if (enviroment_squeakWheel == true && (Manager.m.inShopDropper || Manager.m.inShopMachine))
        {
            enviroment_squeakWheel = false;
            StartCoroutine(SqueakWheel());
        }
        if (enviroment_switchLight == true && (Manager.m.inShopDropper || Manager.m.inShopMachine))
        {
            enviroment_switchLight = false;
            StartCoroutine(SwitchLight());
        }
        if (enviroment_rollBarrel == true && (Manager.m.inShopDropper || Manager.m.inShopMachine))
        {
            enviroment_rollBarrel = false;
            StartCoroutine(RollBarrel());
        }
        if (enviroment_swingRailing == true && (Manager.m.inShopDropper || Manager.m.inShopMachine))
        {
            enviroment_swingRailing = false;
            StartCoroutine(SwingRailing());
        }




        if (Manager.m.inShopDropper == true || Manager.m.inShopMachine == true)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SwipeRight")))
            {
                Right();
            }
            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SwipeLeft")))
            {
                Left();
            }
            if (Manager.m.inShopDropper == true)
            {
                autoRepair.SetActive(true);
                if (Manager.m.autoRepairDroppers[Manager.m.dropperNumber - 1] == true)
                {
                    autoRepairEnabled.SetActive(true);
                }
                else
                {
                    autoRepairEnabled.SetActive(false);
                }
            }
            if (Manager.m.inShopMachine == true)
            {
                autoRepair.SetActive(true);
                if (Manager.m.autoRepairMachines[Manager.m.machineNumber - 1] == true)
                {
                    autoRepairEnabled.SetActive(true);
                }
                else
                {
                    autoRepairEnabled.SetActive(false);
                }
            }
        }
        else
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            autoRepair.SetActive(false);
            buyButtonDisabled.SetActive(false);
            buyButtonEnabled.SetActive(false);
            lockObjects.SetActive(false);
        }

        if (Manager.m.inShopDropper == true)
        {
            if (Manager.m.level < Manager.m.dropperNumber)
            {
                lockText.GetComponent<TextMeshProUGUI>().text = "Requires<br>Level " + Manager.m.dropperNumber;
            }
            else if (Manager.m.upgradeRessources[Manager.m.dropperNumber - 1] == false)
            {
                lockText.GetComponent<TextMeshProUGUI>().text = "Need more<br>Ressources";
            }
        }
        else if (Manager.m.inShopMachine == true)
        {
            if (Manager.m.level < Manager.m.machineNumber)
            {
                lockText.GetComponent<TextMeshProUGUI>().text = "Requires<br>Level " + Manager.m.machineNumber;
            }
            else if (Manager.m.upgradeRessources[Manager.m.machineNumber - 1] == false)
            {
                lockText.GetComponent<TextMeshProUGUI>().text = "Missing<br>Resources";
            }
        }



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

        if (Manager.m.inShopDropper || Manager.m.inShopMachine)
        {
            if (currentHumming != null)
            {
                currentHumming.audiosource.mute = false;
                currentHumming.audiosource.volume = Manager.m.effectsVolume.publicVolume;
            }
            if (oldHumming != null)
            {
                oldHumming.audiosource.mute = false;
                oldHumming.audiosource.volume = Manager.m.effectsVolume.publicVolume;
            }
        }
        else
        {
            if (currentHumming != null)
            {
                currentHumming.audiosource.mute = true;
            }
            if (oldHumming != null)
            {
                oldHumming.audiosource.mute = true;
            }
        }
    }
    void Right()
    {
        Manager.m.effectSpeaker.swipe();
        if (Manager.m.inShopDropper == true)
        {
            if (Manager.m.dropperNumber < Manager.m.dropperMax)
            {
                Manager.m.dropperNumber += 1;
            }
            else if (Manager.m.dropperNumber == Manager.m.dropperMax)
            {
                Manager.m.dropperNumber = 1;
            }
        }
        if (Manager.m.inShopMachine == true)
        {
            if (Manager.m.machineNumber < Manager.m.machineMax)
            {
                Manager.m.machineNumber += 1;
            }
            else if (Manager.m.machineNumber == Manager.m.machineMax)
            {
                Manager.m.machineNumber = 1;
            }
        }
    }
    void Left()
    {
        Manager.m.effectSpeaker.swipe();
        if (Manager.m.inShopDropper == true)
        {
            if (Manager.m.dropperNumber > 1)
            {
                Manager.m.dropperNumber -= 1;
            }
            else if (Manager.m.dropperNumber == 1)
            {
                Manager.m.dropperNumber = Manager.m.dropperMax;
            }
        }
        if (Manager.m.inShopMachine == true)
        {
            if (Manager.m.machineNumber > 1)
            {
                Manager.m.machineNumber -= 1;
            }
            else if (Manager.m.machineNumber == 1)
            {
                Manager.m.machineNumber = Manager.m.machineMax;
            }
        }
    }
    public void ResetCameraPosition()
    {
        Manager.m.dropperNumber = 1;
        Manager.m.machineNumber = 1;
    }
    void SwitchAutoRepair()
    {
        if (Manager.m.inShopDropper == true)
        {
            if (Manager.m.autoRepairDroppers[Manager.m.dropperNumber - 1] == true)
            {
                Manager.m.autoRepairDroppers[Manager.m.dropperNumber - 1] = false;
                Manager.m.effectSpeaker.error();
            }
            else
            {
                Manager.m.autoRepairDroppers[Manager.m.dropperNumber - 1] = true;
                Manager.m.effectSpeaker.accept();
            }
        }
        if (Manager.m.inShopMachine == true)
        {
            if (Manager.m.autoRepairMachines[Manager.m.machineNumber - 1] == true)
            {
                Manager.m.autoRepairMachines[Manager.m.machineNumber - 1] = false;
                Manager.m.effectSpeaker.error();
            }
            else
            {
                Manager.m.autoRepairMachines[Manager.m.machineNumber - 1] = true;
                Manager.m.effectSpeaker.accept();
            }
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            currentTime = Time.unscaledTime;
            while (currentTime > Time.unscaledTime - 1.4f)
            {
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            while (currentTime > Time.unscaledTime - 1f)
            {
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,2f));
            if (Manager.m.inShopDropper == true || Manager.m.inShopMachine == true)
            {
                Manager.m.effectSpeaker.changePlaySoundParameters(Manager.m.effectSpeaker.clickLight, 1.5f);
            }
            enviroment_electricLightOn.SetActive(false);
            enviroment_electricLightOff.SetActive(true);
            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 2f));
            enviroment_switchingLight = false;
            if (Random.Range(0,4) != 0)
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
            while(enviroment_barrel.transform.localPosition.z < 0.1f)
            {
                float restDistance = 0.14f - enviroment_barrel.transform.localPosition.z;
                float restMul = restDistance * restDistance;
                enviroment_barrel.transform.localPosition += new Vector3 (0, 0, Time.unscaledDeltaTime * 30f * restMul);
                enviroment_barrel.transform.localRotation = Quaternion.Euler(enviroment_barrel.transform.localRotation.eulerAngles + new Vector3(0, Time.unscaledDeltaTime * 6000 * restMul, 0));
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
                if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
                {
                    Destroy(sound);
                }
                yield return null;
            }
            startTime = Time.unscaledTime;
            if (Manager.m.inShopDropper || Manager.m.inShopMachine)
            {
                while (startTime + 1 - Time.unscaledTime > 0)
                {
                    float timeMult = (startTime + 1) - Time.unscaledTime;
                    enviroment_railing.transform.localRotation = Quaternion.Euler(enviroment_railing.transform.localRotation.eulerAngles + new Vector3(0, Time.unscaledDeltaTime * 70 * timeMult, 0));
                    if (Manager.m.inShopDropper == false && Manager.m.inShopMachine == false && sound != null)
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
