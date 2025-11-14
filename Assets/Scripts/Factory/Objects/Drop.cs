using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Android;

public class Drop : MonoBehaviour
{
    GameObject i;
    public GameObject inputObject;
    Vector3 direction;
    public int dropperNumber;

    int Coroutines;
    int CoroutineNumber;
    int Coroutines2;
    int CoroutineNumber2;
    public int UpAndDown;
    public double inputDistance;

    public GameObject parent;

    public GameObject Ore;

    public GameObject Coal;
    public GameObject Iron;
    public GameObject Copper;
    public GameObject Silver;
    public GameObject Gold;
    public GameObject MagmaCapsual;
    public GameObject HardnedIron;
    public GameObject ElectronicParts;
    public GameObject UnstableCrystal;
    public GameObject Uranium;

    public GameObject Drill;
    public GameObject Thrower;

    public bool needOre;
    public bool canDrop;
    public double oreValue;
    public double exp;
    float speed;
    public float currenttime;
    float currenttime2;
    public bool Up;

    public int[] inputOreUpgrade;
    public float[] consumeOres;
    public int[] inputCapacity;
    public string consumeText;

    public Material acceptOre;
    public Material rejectOre;

    public RepairDropper repairDropperScript;
    public bool stopTimeReset;


    public float quickTimeEventClocking;
    public float quickTimeEventEfficiency;

    private void Awake()
    {
        quickTimeEventClocking = 1;
        quickTimeEventEfficiency = 1;
        Coal = Manager.m.ores[0];
        Iron = Manager.m.ores[1];
        Copper = Manager.m.ores[2];
        Silver = Manager.m.ores[3];
        Gold = Manager.m.ores[4];
        MagmaCapsual = Manager.m.ores[5];
        HardnedIron = Manager.m.ores[6];
        ElectronicParts = Manager.m.ores[7];
        UnstableCrystal = Manager.m.ores[8];
        Uranium = Manager.m.ores[9];

        repairDropperScript = gameObject.GetComponentInParent<RepairDropper>();
        repairDropperScript.inputOres = new float[10];
        for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
        {
            repairDropperScript.inputOres[i] = 0;
        }
        inputCapacity = new int[10];
        for (int i = 0; i < inputCapacity.Length; i++)
        {
            inputCapacity[i] = 0;
        }
        consumeOres = new float[10];
        for (int i = 0; i < inputCapacity.Length; i++)
        {
            consumeOres[i] = 0;
        }
        inputOreUpgrade = new int[10];
        for (int i = 0; i < inputOreUpgrade.Length; i++)
        {
            inputOreUpgrade[i] = 0;
        }
        dropperNumber = repairDropperScript.dropperNumber;
        switch (dropperNumber)
        {
            case 1:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[0];
                    consumeOres = Manager.m.dropConsumeOres[0];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[0];
                    Ore = Coal;
                    break;
                }
            case 2:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[1];
                    consumeOres = Manager.m.dropConsumeOres[1];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[1];
                    Ore = Iron;
                    break;
                }
            case 3:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[2];
                    consumeOres = Manager.m.dropConsumeOres[2];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[2];
                    Ore = Copper;
                    break;
                }
            case 4:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[3];
                    consumeOres = Manager.m.dropConsumeOres[3];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[3];
                    Ore = Silver;
                    break;
                }
            case 5:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[4];
                    consumeOres = Manager.m.dropConsumeOres[4];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[4];
                    Ore = Gold;
                    break;
                }
            case 6:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[5];
                    consumeOres = Manager.m.dropConsumeOres[5];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[5];
                    Ore = MagmaCapsual;
                    break;
                }
            case 7:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[6];
                    consumeOres = Manager.m.dropConsumeOres[6];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[6];
                    Ore = HardnedIron;
                    break;
                }
            case 8:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[7];
                    consumeOres = Manager.m.dropConsumeOres[7];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[7];
                    Ore = ElectronicParts;
                    break;
                }
            case 9:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[8];
                    consumeOres = Manager.m.dropConsumeOres[8];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[8];
                    Ore = UnstableCrystal;
                    break;
                }
            case 10:
                {
                    inputCapacity = Manager.m.dropInputCapacitys[9];
                    consumeOres = Manager.m.dropConsumeOres[9];
                    inputOreUpgrade = Manager.m.dropInputUpgrades[9];
                    Ore = Uranium;
                    break;
                }
        }
    }
    void Start()
    {
        if (stopTimeReset == false)
        {
            currenttime = 1;
        }
        UpAndDown = 0;
        Up = true;
        oreValue = parent.GetComponent<RepairDropper>().oreValue;
        exp = oreValue;
    }

    void Update()
    {
        if (Manager.m.paused == false)
        {
            currenttime -= Time.deltaTime;
            var vec = parent.transform.eulerAngles;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            parent.transform.eulerAngles = vec;

            if (parent.GetComponent<RepairDropper>().working == true && parent.GetComponent<RepairDropper>().sold == false)
            {
                switch (dropperNumber)
                {
                    case 1:
                        {
                            if (currenttime <= 0)
                            {

                                Manager.m.factorySpeaker.dropper1(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);
                                i = Instantiate(Ore, transform.position, transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                switch (parent.transform.rotation.eulerAngles.y)
                                {

                                    case 90:
                                        i.transform.Translate(3.8f, 6f, 0);
                                        direction = new Vector2(0, -1);
                                        break;

                                    case 180:
                                        i.transform.Translate(3.8f, 6f, 0);
                                        direction = new Vector2(-1, 0);
                                        break;

                                    case 270:
                                        i.transform.Translate(3.8f, 6f, 0);
                                        direction = new Vector2(0, 1);
                                        break;

                                    case 0:
                                        i.transform.Translate(3.8f, 6f, 0);
                                        direction = new Vector2(1, 0);
                                        break;
                                }
                                i.gameObject.GetComponent<Ore>().dropAnimationDirection = direction;
                                i.gameObject.GetComponent<Ore>().doDropAnimation = true;
                            }
                            break;
                        }
                    case 2:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper2(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.transform.Translate(8.8f, 11.2f, 0);
                                i.transform.Rotate(0, 90, 0);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;

                            }


                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 3:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper3(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                if (UnityEngine.Random.Range(0, 7) <= 4)
                                {
                                    int random = UnityEngine.Random.Range(0, 4);
                                    switch (random)
                                    {
                                        case 0:
                                            {
                                                i.transform.Translate(6.2f, 11.5f, 0);
                                                break;
                                            }
                                        case 1:
                                            {
                                                i.transform.Translate(6.2f, 11.5f, 1);
                                                break;
                                            }
                                        case 2:
                                            {
                                                i.transform.Translate(6.2f, 11.5f, 2);
                                                break;
                                            }
                                        case 3:
                                            {
                                                i.transform.Translate(6.2f, 11.5f, -1);
                                                break;
                                            }
                                        case 4:
                                            {
                                                i.transform.Translate(6.2f, 11.5f, -2);
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    int random = UnityEngine.Random.Range(0, 3);

                                    switch (random)
                                    {
                                        case 0:
                                            {
                                                i.transform.Translate(6.2f, 12.5f, 0.5f);
                                                break;
                                            }
                                        case 1:
                                            {
                                                i.transform.Translate(6.2f, 12.5f, -0.5f);
                                                break;
                                            }
                                        case 2:
                                            {
                                                i.transform.Translate(6.2f, 12.5f, 1.5f);
                                                break;
                                            }
                                        case 3:
                                            {
                                                i.transform.Translate(6.2f, 12.5f, -1.5f);
                                                break;
                                            }
                                    }
                                }

                                switch (transform.rotation.eulerAngles.y)
                                {
                                    case 180:
                                        {
                                            direction = new Vector2(-1, 0);
                                            break;
                                        }
                                    case 270:
                                        {
                                            direction = new Vector2(0, 1);
                                            break;
                                        }
                                    case 0:
                                        {
                                            direction = new Vector2(1, 0);
                                            break;
                                        }
                                    case 90:
                                        {
                                            direction = new Vector2(0, -1);
                                            break;
                                        }
                                }
                                i.gameObject.GetComponent<Ore>().dropAnimationDirection = direction;
                                i.gameObject.GetComponent<Ore>().doDropAnimation = true;
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 4:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper4(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.transform.Translate(0, -1.3f, 0);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                switch (transform.rotation.eulerAngles.y)
                                {

                                    case 90:
                                        i.transform.Translate(0, 19, 0);
                                        direction = new Vector2(0, -1);
                                        break;

                                    case 180:
                                        i.transform.Translate(0, 19, 0);
                                        direction = new Vector2(-1, 0);
                                        break;

                                    case 270:
                                        i.transform.Translate(0, 19, 0);
                                        direction = new Vector2(0, 1);
                                        break;

                                    default:
                                        i.transform.Translate(0, 19, 0);
                                        direction = new Vector2(1, 0);
                                        break;
                                }
                                i.gameObject.GetComponent<Ore>().dropAnimationDirection = direction;
                                i.gameObject.GetComponent<Ore>().doDropAnimation = true;
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 5:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper5(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                i.transform.Translate(10f, 20f, 0);
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 6:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper6(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                i.transform.Translate(2.7f, 12, 0f);
                            }

                            if (currenttime <= 02)
                            {
                                if (Up == false)
                                {
                                    currenttime2 = Time.time + speed;
                                    Drill.transform.Translate(0, -0.02f, 0);
                                    UpAndDown = UpAndDown - 1;
                                    Drill.transform.Rotate(0, -2f * 1.5f, 0);
                                    if (UpAndDown < 50)
                                    {
                                        speed = speed * 1.05f;
                                    }
                                    if (UpAndDown == 0)
                                    {
                                        Up = true;
                                    }
                                }
                                if (Up == true)
                                {
                                    currenttime2 = Time.time + 0.01f;
                                    Drill.transform.Translate(0, 0.02f, 0);
                                    UpAndDown = UpAndDown + 1;
                                    Drill.transform.Rotate(0, 0.45f, 0);
                                    if (UpAndDown == 300)
                                    {
                                        Up = false;
                                        speed = 0.01f;
                                        Drill.transform.rotation.eulerAngles.Set(0, 0, 0);
                                    }
                                }
                            }
                            //else
                            //{
                            //    currenttime = Time.time + 0.1f;
                            //    Drill.transform.Translate(0, 0.01f, 0);
                            //    UpAndDown = UpAndDown + 1;
                            //    Drill.transform.Rotate(0, 0.45f, 0);
                            //    start = true;
                            //}
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 7:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper7(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                i.transform.Translate(10, 19f, 0);
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 8:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper8(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                i.transform.Translate(9, 16.8f, 0);
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 9:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper9(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                                i = Instantiate(Ore, parent.transform.position, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;
                                switch (transform.rotation.eulerAngles.y)
                                {

                                    case 90:
                                        i.transform.Translate(3.8f, 8.5f, 0);
                                        direction = new Vector2(1, 0);
                                        break;

                                    case 180:
                                        i.transform.Translate(3.8f, 8.5f, 0);
                                        direction = new Vector2(0, -1);
                                        break;

                                    case 270:
                                        i.transform.Translate(3.8f, 8.5f, 0);
                                        direction = new Vector2(-1, 0);
                                        break;

                                    default:
                                        i.transform.Translate(3.8f, 8.5f, 0);
                                        direction = new Vector2(0, 1);
                                        break;
                                }
                                i.gameObject.GetComponent<Ore>().dropAnimationDirection = direction;
                                i.gameObject.GetComponent<Ore>().doDropAnimation = true;
                            }
                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                    case 10:
                        {
                            bool resoucesAvailable = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] < consumeOres[i])
                                {
                                    resoucesAvailable = false;
                                    if (Manager.m.creative == true)
                                    {
                                        resoucesAvailable = true;
                                    }
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {
                                for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                                {
                                    repairDropperScript.inputOres[i] -= consumeOres[i] / quickTimeEventEfficiency;
                                }
                            }
                            if (currenttime <= 0 && resoucesAvailable)
                            {

                                Manager.m.factorySpeaker.dropper10(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);
                                i = Instantiate(Ore, parent.transform.position + Vector3.up * 10, parent.transform.rotation);
                                i.gameObject.GetComponent<Ore>().value = oreValue;
                                i.gameObject.GetComponent<Ore>().baseValue = oreValue;
                                i.gameObject.GetComponent<Ore>().exp = exp;
                                i.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                i.gameObject.transform.parent = Manager.m.oreFolder.transform;


                                switch (transform.rotation.eulerAngles.y)
                                {
                                    case 90:
                                        {
                                            direction = new Vector2(1, 0);
                                            break;
                                        }
                                    case 180:
                                        {
                                            direction = new Vector2(0, -1);
                                            break;
                                        }
                                    case 270:
                                        {
                                            direction = new Vector2(-1, 0);
                                            break;
                                        }
                                    default:
                                        {
                                            direction = new Vector2(0, 1);
                                            break;
                                        }
                                }
                                i.gameObject.GetComponent<Ore>().dropAnimationDirection = direction;
                                i.gameObject.GetComponent<Ore>().doDropAnimation = true;
                            }

                            bool filled = true;
                            for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                            {
                                if (repairDropperScript.inputOres[i] <= inputCapacity[i] - 1)
                                {
                                    filled = false;
                                }
                            }
                            if (filled == true)
                            {
                                inputObject.GetComponent<Renderer>().material = rejectOre;
                            }
                            else
                            {
                                inputObject.GetComponent<Renderer>().material = acceptOre;
                            }
                            break;
                        }
                }
                if (i != null)
                {
                    if (Manager.m.qTEUltimateProduction)
                    {
                        i.gameObject.GetComponent<Ore>().upgradeLevel = 3;

                    }
                    if (Manager.m.creative == true)
                    {
                        i.gameObject.GetComponent<Ore>().isFacade = true;
                    }
                    else
                    {
                        i.gameObject.GetComponent<Ore>().isFacade = false;
                    }
                }
                if (Manager.m.qTEUltimateProduction)
                {
                    for (int i = 0; i < repairDropperScript.inputOres.Length; i++)
                    {
                        repairDropperScript.inputOres[i] = inputCapacity[i];
                    }
                }
            }
            if (currenttime <= 0)
            {
                currenttime += repairDropperScript.dropSpeed / quickTimeEventClocking;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (Manager.m.paused == false)
        {
            if (other.gameObject.tag == "Ore")
            {
                print("NTriggerd");
                int dropNumber = other.GetComponent<Ore>().oreNumber;
                if (dropNumber != -1)
                {
                    print("RightNumber");
                    if (repairDropperScript.inputOres[dropNumber] <= inputCapacity[dropNumber] - 1 && other.gameObject.GetComponent<Ore>().isDestroyed == false && other.gameObject.GetComponent<Ore>().upgradeLevel == inputOreUpgrade[dropNumber])
                    {
                        print("HIT");
                        other.gameObject.GetComponent<Ore>().isDestroyed = true;
                        other.GetComponent<Rigidbody>().isKinematic = true;
                        StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
                        repairDropperScript.inputOres[dropNumber] += 1;
                    }
                }

            }
        }
        //switch (parent.name)
        //{
        //    case "Dropper1(Clone)":
        //        {
        //            break;
        //        }
        //    case "Dropper2(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper1Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper3(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper1Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper4(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper1Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper5(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper1Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper6(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper1Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper7(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper6Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            if (repairDropperScript.inputOres[1] < inputCapacity[1] && other.gameObject.name == "Dropper2Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[1] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper8(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper6Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            if (repairDropperScript.inputOres[1] < inputCapacity[1] && other.gameObject.name == "Dropper3Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[1] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper9(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper6Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            if (repairDropperScript.inputOres[1] < inputCapacity[1] && other.gameObject.name == "Dropper4Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[1] += 1;
        //            }
        //            break;
        //        }
        //    case "Dropper10(Clone)":
        //        {
        //            if (repairDropperScript.inputOres[0] < inputCapacity[0] && other.gameObject.name == "Dropper6Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[0] += 1;
        //            }
        //            if (repairDropperScript.inputOres[1] < inputCapacity[1] && other.gameObject.name == "Dropper5Drop(Clone)" && other.gameObject.GetComponent<Ore>().isDestroyed == false)
        //            {
        //                other.gameObject.GetComponent<Ore>().isDestroyed = true;
        //                other.GetComponent<Rigidbody>().isKinematic = true;
        //                StartCoroutine(moveOre(inputObject.transform.position, other.gameObject, true, 1f));
        //                repairDropperScript.inputOres[1] += 1;
        //            }
        //            break;
        //        }
        //}
    }

    public IEnumerator bewegen(Vector3 richtung, float distanz, GameObject a)
    {
        Vector3 ausgangspunkt = a.transform.position;
        while (true)
        {
            if (a.gameObject)
            {
                if (Math.Abs(Vector3.Distance(a.transform.position, ausgangspunkt)) < distanz)
                {
                    a.transform.position += richtung * Time.deltaTime;
                    yield return null;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        Coroutines = Coroutines + 1;
    }
    public IEnumerator moveOre(Vector3 endpunkt, GameObject a, bool destroy, float speed)
    {
        if (a.GetComponent<Ore>())
        {
            a.GetComponent<Ore>().moveableByBelt = false;
            a.GetComponent<Collider>().enabled = false;

            Vector3 ausgangspunkt = a.transform.position;
            float anfangsDistanz = Math.Abs(Vector3.Distance(endpunkt, ausgangspunkt));

            if (a != null)
            {
                //while (Math.Abs(Vector3.Distance(a.transform.position, endpunkt)) > 1) //Math.Abs(Vector3.Distance(a.transform.position, ausgangspunkt)) < distanz
                //{
                //    if (a != null)
                //    {
                //        Vector3 richtung = endpunkt - a.transform.position;
                //        float richtungsdifferenz = 100 / (float)Math.Pow(Math.Abs(Vector3.Distance(endpunkt, a.transform.position)), 2f);
                //        richtung = new Vector3(richtung.x * richtungsdifferenz, richtung.y * richtungsdifferenz, richtung.z * richtungsdifferenz);

                //        if (Vector3.Distance(a.transform.position + richtung * speed * Time.deltaTime, endpunkt) < Vector3.Distance(a.transform.position, endpunkt))
                //        {
                //            a.transform.position += richtung * speed * Time.deltaTime;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //        yield return null;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                bool moveObjectDestroyed = false;
                while (moveObjectDestroyed == false) //Math.Abs(Vector3.Distance(a.transform.position, ausgangspunkt)) < distanz
                {
                    if (a != null)
                    {
                        if (Math.Abs(Vector3.Distance(a.transform.position, endpunkt)) > 1)
                        {
                            Vector3 richtung = endpunkt - a.transform.position;
                            float richtungsdifferenz = 100 / (float)Math.Pow(Math.Abs(Vector3.Distance(endpunkt, a.transform.position)), 2f);
                            richtung = new Vector3(richtung.x * richtungsdifferenz, richtung.y * richtungsdifferenz, richtung.z * richtungsdifferenz);

                            if (Vector3.Distance(a.transform.position + richtung * speed * Time.deltaTime, endpunkt) < Vector3.Distance(a.transform.position, endpunkt))
                            {
                                a.transform.position += richtung * speed * Time.deltaTime;
                            }
                            else
                            {
                                break;
                            }
                            yield return null;
                        }
                        else
                        {
                            moveObjectDestroyed = true;
                            break;
                        }
                    }
                    else
                    {
                        moveObjectDestroyed = true;
                        break;
                    }
                }
            }
            if (a != null)
            {
                Destroy(a);
            }
        }
    }
    public IEnumerator bewegen2(Vector3 richtung, float distanz, GameObject a)
    {
        if (a != null)
        {
            Vector3 ausgangspunkt = a.transform.position;

            bool moveObjectDestroyed = false;
            while (moveObjectDestroyed == false)
            {
                if (a != null)
                {
                    if (Math.Abs(Vector3.Distance(a.transform.position, ausgangspunkt)) < distanz)
                    {
                        a.transform.position += richtung * Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        moveObjectDestroyed = true;
                        break;
                    }
                }
                else
                {
                    moveObjectDestroyed = true;
                    break;
                }
            }
        }
        Coroutines2 = Coroutines2 + 1;
    }
    private IEnumerator ToggleKinematic(GameObject a, float time)
    {
        if (a.gameObject)
        {
            a.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            yield return new WaitForSeconds(time);
        }
        if (a.gameObject)
        {
            a.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private IEnumerator SetOre()
    {
        yield return new WaitForEndOfFrame();
    }
}
