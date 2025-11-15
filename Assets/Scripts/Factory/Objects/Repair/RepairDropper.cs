using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using System.Linq;
using System;

public class RepairDropper : MonoBehaviour
{
    public int id;
    public GameObject dropper;
    public string identification;
    public int dropperNumber;
    public int machineNumber;
    public double cost;
    public float costPerSecond;
    public Drop dropScript;

    public float oreValue;
    public float timeperpercent;
    public float durability;
    public float dropSpeed;

    public float conveyorBeltSpeed;
    public String conveyorBeltType;
    public float furnaceMultiplier;
    public int upgradeLevelMax;
    public bool isScrap;

    public bool working;
    public bool sold;
    public bool canBePlaced;

    double cuncurrenttime;
    double cuncurrenttime2;
    public GameObject wrench;
    public GameObject noMoneySymbol;
    public GameObject instantiatedWrench;

    public float[] inputOres;

    public float repairCost;
    public float sellValue;

    public Camera nextCam;
    public int factoryHall;

    public float quickTimeEventRepairs;

    private void Awake()
    {
        if (this.gameObject.tag == "FactoryObject") id = Manager.m.getID();
    }

    void Start()
    {
        quickTimeEventRepairs = 1;
        if (this.gameObject.tag == "FactoryObject")
        {
            dropScript = gameObject.transform.GetComponentInChildren<Drop>();
            instantiatedWrench = Instantiate(wrench);
            instantiatedWrench.GetComponent<WrenchUI>().parentScript = this;
            instantiatedWrench.SetActive(false);
            instantiatedWrench.transform.SetParent(Manager.m.wrenchFolder);
        }
        working = true;
        cuncurrenttime += Time.time + 1;
        canBePlaced = false;
        sold = false;
        repairCost = 0;
        if(gameObject.GetComponent<Drop>() == true)
        {
            gameObject.GetComponent<Drop>().oreValue = oreValue;
        }
        if (dropperNumber != 0)
        {
            identification = Manager.m.dropperIdentifications[dropperNumber - 1];
        }
        else if (machineNumber != 0)
        {
            identification = Manager.m.machineIdentifications[machineNumber - 1];
        }
        else
        {
            identification = "Error";
        }
        if (durability == 0)
        {
            durability = 100;
        }
        double distance = Vector3.Distance(this.gameObject.transform.position, Manager.m.factoryCameras[0].gameObject.transform.position) + 1;
        for(int i = 0; i < Manager.m.factoryCameras.Length; i++)
        {
            if (Vector3.Distance(this.gameObject.transform.position, Manager.m.factoryCameras[i].gameObject.transform.position) < distance)
            {
                distance = Vector3.Distance(this.gameObject.transform.position, Manager.m.factoryCameras[i].gameObject.transform.position);
                nextCam = Manager.m.factoryCameras[i];
                factoryHall = i + 1;
            }
        }
    }
    private void OnMouseEnter()
    {
        print("Mouseover");
        if (Manager.m.editMode_sell == true)
        {
            //dropper.transform.Translate(0, 3f, 0);

            MeshRenderer[] moveObjects = dropper.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < moveObjects.Length; i++)
            {
                if (moveObjects[i].GetComponent<MeshRenderer>().enabled)
                {
                    moveObjects[i].gameObject.transform.Translate(Vector3.up * 6, Space.World);
                }
            }
        }
    }
    private void OnMouseExit()
    {
        if (Manager.m.editMode_sell == true)
        {
            //dropper.transform.Translate(0, -3f, 0);
            MeshRenderer[] moveObjects = dropper.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < moveObjects.Length; i++)
            {
                if (moveObjects[i].GetComponent<MeshRenderer>().enabled)
                {
                    moveObjects[i].gameObject.transform.Translate(Vector3.down * 6, Space.World);
                }
            }
        }
        Manager.m.dropperInformationBox.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (Manager.m.editMode_sell == true)
        {
            if (isScrap == false)
            {
                if (sellValue > 0 || Manager.m.money + sellValue >= 0)
                {
                    Manager.m.editHistoryManager.AddEditEvent(this, EditEventType.Sold, sellValue);
                    Manager.m.effectSpeaker.sell();
                    Manager.m.money += sellValue;
                    gameObject.transform.Translate(0, -100, 0);
                    working = false;
                    if (gameObject.GetComponent<Drop>() == true)
                    {
                        //working = false;
                    }
                    sold = true;
                    gameObject.tag = "Destroyed";
                    Destroy(this.gameObject, Time.deltaTime);
                    Destroy(instantiatedWrench, Time.deltaTime);
                    instantiatedWrench.SetActive(false);
                    Manager.m.dropperInformationBox.SetActive(false);
                }
                else
                {
                    Manager.m.effectSpeaker.error();
                }
            }
            else
            {
                if (Manager.m.money - cost >= 0)
                {
                    Manager.m.editHistoryManager.AddEditEvent(this, EditEventType.Sold, -cost);
                    Manager.m.money -= cost;
                    Manager.m.effectSpeaker.removeScrap();
                    gameObject.transform.Translate(0, -100, 0);
                    working = false;
                    sold = true;
                    gameObject.tag = "Destroyed";
                    Destroy(this.gameObject, Time.deltaTime);
                    Destroy(instantiatedWrench, Time.deltaTime);
                    instantiatedWrench.SetActive(false);
                    Manager.m.dropperInformationBox.SetActive(false);
                }
                else
                {
                    Manager.m.effectSpeaker.error();
                }
            }
        }
        if (Manager.m.editMode_repair == true && Manager.m.editMode == false && Manager.m.editMode_placeDropper == false && Manager.m.editMode_placeMachine == false)
        {
            if (Manager.m.money >= repairCost && durability < 99.9f)
            {
                Manager.m.effectSpeaker.repair();
                Manager.m.money -= repairCost;
                durability = 100;
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnMouseOver()
    {
        if (Manager.m.editMode_repair == true && sold == false && this.gameObject.tag == "FactoryObject" && Manager.m.inUIMenu() == false)
        {
            Manager.m.dropperInformationBox.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor - 0.15f, Manager.m.graphicManager.gUIScaleFactor - 0.15f, Manager.m.graphicManager.gUIScaleFactor - 0.15f);
            if (dropperNumber != 0)
            {
                double durabilityMinutes = (timeperpercent * durability) / 60;
                double durabilitySeconds = 60 * (durabilityMinutes - Mathf.Floor((float)durabilityMinutes));
                durabilityMinutes = Mathf.Floor((float)durabilityMinutes);
                durabilitySeconds = Mathf.Floor((float)durabilitySeconds);
                string _0;
                if (durabilitySeconds < 10)
                {
                    _0 = "0";
                }
                else
                {
                    _0 = "";
                }
                if (durabilitySeconds < 0)
                {
                    durabilitySeconds = 0;
                }
                string repairCostChange = "";
                if (quickTimeEventRepairs < 0.99)
                {
                    repairCostChange = "(-" + (1 - quickTimeEventRepairs) * 100 + "%)";
                }
                else if (quickTimeEventRepairs > 1.01)
                {
                    repairCostChange = "(+" + (quickTimeEventRepairs - 1) * 100 + "%)";
                }
                Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text = 
                    "<size=25><b>Identification:</b><br> " + identification + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Maintenance:</b><br> " + Money.NumberToUnit((float)costPerSecond) + "/sec<br>" + 
                    ("<size=10> <size=25><br>") + ("<b>Durability:</b><br> " + Mathf.Round((float)durability * 10) / 10 + "%<br>") + 
                    ("<size=10> <size=25><br>") + ("<b>Working Time:</b><br> " + durabilityMinutes + ":" + _0 + durabilitySeconds + "Min<br>") + 
                    ("<size=10> <size=25><br>") + "<b>Value:</b><br> " + Money.NumberToUnit(sellValue) + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Repair:</b><br> " + repairCostChange + Money.NumberToUnit(repairCost) + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Storage:</b><br> ";
                bool noStorage = true;
                for (int i = 0; i < Manager.m.dropConsumeOres[Array.IndexOf(Manager.m.dropperIdentifications, identification)].Length; i++)
                {
                    if (Manager.m.dropConsumeOres[Array.IndexOf(Manager.m.dropperIdentifications, identification)][i] > 0)
                    {
                        Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text += Mathf.Round(inputOres[i] * 100) / 100 + "/" + Manager.m.dropInputCapacitys[Array.IndexOf(Manager.m.dropperIdentifications, identification)][i] + " " + Manager.m.dropperIdentifications[i] + "<br> ";
                        Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text = Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text.Replace("Electronic Parts", "Electornics");
                        noStorage = false;
                    }
                }
                if (noStorage == true)
                {
                    Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text += "--";
                }
            }
            else if (furnaceMultiplier > 0)
            {
                string repairCostChange = "";
                if (quickTimeEventRepairs < 0.99)
                {
                    repairCostChange = "(-" + (1 - quickTimeEventRepairs) * 100 + "%)";
                }
                else if (quickTimeEventRepairs > 1.01)
                {
                    repairCostChange = "(+" + (quickTimeEventRepairs - 1) * 100 + "%)";
                }
                Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text = 
                    "<size=25><b>Identification:</b><br> " + identification + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Maintenance:</b><br> " + Money.NumberToUnit((float)costPerSecond) + "/sec<br>" +
                    ("<size=10> <size=25><br>") + "<b>Durability:</b><br> " + Mathf.Round((float)durability * 10) / 10 + "%<br>" +
                    ("<size=10> <size=25><br>") + "<b>Value:</b><br> " + Money.NumberToUnit((float)cost * 0.5f) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Repair:</b><br> " + repairCostChange + Money.NumberToUnit(repairCost) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Modifier:</b><br> +" + furnaceMultiplier * 100 + "%";
            }
            else if (upgradeLevelMax > 0)
            {
                string repairCostChange = "";
                if (quickTimeEventRepairs < 0.99)
                {
                    repairCostChange = "(-" + (1 - quickTimeEventRepairs) * 100 + "%)";
                }
                else if (quickTimeEventRepairs > 1.01)
                {
                    repairCostChange = "(+" + (quickTimeEventRepairs - 1) * 100 + "%)";
                }
                Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text = 
                    "<size=25><b>Identification:</b><br> " + identification + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Maintenance:</b><br> " + Money.NumberToUnit((float)costPerSecond) + "/sec<br>" +
                    ("<size=10> <size=25><br>") + "<b>Durability:</b><br> " + Mathf.Round((float)durability * 10) / 10 + "%<br>" +
                    ("<size=10> <size=25><br>") + "<b>Value:</b><br> " + Money.NumberToUnit((float)cost * 0.5f) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Repair:</b><br> " + repairCostChange + Money.NumberToUnit(repairCost) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Level:</b><br> " + ("0-" + upgradeLevelMax + "(+" + Manager.m.upgradeMultipliers[upgradeLevelMax] * 100  + "%");
            }
            else if (conveyorBeltSpeed > 0)
            {
                string repairCostChange = "";
                if (quickTimeEventRepairs < 0.99)
                {
                    repairCostChange = "(-" + (1 - quickTimeEventRepairs) * 100 + "%)";
                }
                else if (quickTimeEventRepairs > 1.01)
                {
                    repairCostChange = "(+" + (quickTimeEventRepairs - 1) * 100 + "%)";
                }
                Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text =
                    "<size=25><b>Identification:</b><br> " + identification + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Maintenance:</b><br> " + Money.NumberToUnit((float)costPerSecond) + "/sec<br>" +
                    ("<size=10> <size=25><br>") + "<b>Durability:</b><br> " + Mathf.Round((float)durability * 10) / 10 + "%<br>" +
                    ("<size=10> <size=25><br>") + "<b>Value:</b><br> " + Money.NumberToUnit((float)cost * 0.5f) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Repair:</b><br> " + repairCostChange + Money.NumberToUnit(repairCost) + "<br>" +
                    ("<size=10> <size=25><br>") + "<b>Speed</b><br> " + conveyorBeltSpeed + "km/h";
;
            }
            else if (isScrap == true)
            {
                Manager.m.dropperInformationText.GetComponent<TextMeshProUGUI>().text = 
                    "<size=25><b>Identification:</b><br> " + identification + "<br>" + 
                    ("<size=10> <size=25><br>") + "<b>Removal:</b><br> -" + Money.NumberToUnit((float)cost) + "";
            }

            if (Input.mousePosition.x > Manager.m.canvas.GetComponent<RectTransform>().rect.width / 2)
            {
                Manager.m.dropperInformationBox.transform.localPosition = new Vector3(-380, -30, 0);
            }
            else
            {
                Manager.m.dropperInformationBox.transform.localPosition = new Vector3(380, -30, 0);
            }
            Manager.m.dropperInformationBox.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Blueprint")
        {
            Physics.SyncTransforms();
            BoxCollider[] colliders = GetComponents<BoxCollider>();
            bool foundHit = false;

            foreach (BoxCollider box in colliders)
            {
                Vector3 worldCenter = transform.TransformPoint(box.center);
                Vector3 halfExtents = box.size / 2f;

                Collider[] hits = Physics.OverlapBox(worldCenter, halfExtents, transform.rotation);

                foreach (Collider hit in hits)
                {
                    if (hit.gameObject != gameObject && (hit.CompareTag("FactoryObject") || hit.CompareTag("Wall")))
                    {
                        Destroy(gameObject);
                        foundHit = true;
                        break;
                    }
                }

                if (foundHit)
                    break;
            }
            if (foundHit == false)
            {
                MeshRenderer[] Meshs = GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < Meshs.Length; i++)
                {
                    Meshs[i].enabled = true;
                }
                canBePlaced = true;
            }
        }
        sellValue = (float)cost * 0.8f;
        if (durability < 90) { sellValue = (float)cost * 0.6f; }
        if (durability < 60) { sellValue = (float)cost * 0.3f; }
        if (durability < 20) { sellValue = (float)cost * 0f; }
        if (durability <= 0) { sellValue = (float)cost * -0.2f; }
        for (int i = 0; i < inputOres.Length; i++)
        {
            if (inputOres[i] < 0)
            {
                inputOres[i] = 0;
            }
        }
        if (Manager.m.creative == false)
        {
            if (Manager.m.paused == false)
            {
                if (Manager.m.money >= repairCost && durability <= 90f && sold == false && isScrap == false && this.gameObject.tag == "FactoryObject")
                {
                    if (dropperNumber > 0)
                    {
                        if (Manager.m.autoRepairDroppers[dropperNumber - 1] == true)
                        {
                            Manager.m.effectSpeaker.repair();
                            Manager.m.money -= repairCost;
                            durability = 100;
                        }
                    }
                    else if (machineNumber > 0)
                    {
                        if (Manager.m.autoRepairMachines[machineNumber - 1] == true)
                        {
                            Manager.m.effectSpeaker.repair();
                            Manager.m.money -= repairCost;
                            durability = 100;
                        }
                    }
                }
            }
            if (durability <= 0 && gameObject.tag != "Destroyed" && isScrap == false && sold == false)
            {
                GameObject scrap = null;
                if (this.dropperNumber != 0)
                {
                    scrap = (GameObject)Instantiate(Manager.m.ScrapDropper1, transform.position, transform.rotation);
                    scrap.transform.position = new Vector3(this.gameObject.transform.position.x, 102.1f, this.gameObject.transform.position.z);
                    scrap.transform.SetParent(Manager.m.scrapFolder.transform);
                }
                else if (this.machineNumber == 1 || this.machineNumber == 4 || this.machineNumber == 7 || this.machineNumber == 3)
                {
                    scrap = (GameObject)Instantiate(Manager.m.ScrapConveyorBelt1, transform.position, transform.rotation);
                    scrap.transform.position = new Vector3(this.gameObject.transform.position.x, 100f, this.gameObject.transform.position.z);
                    scrap.transform.SetParent(Manager.m.scrapFolder.transform);
                }
                else if (this.machineNumber == 2 || this.machineNumber == 5 || this.machineNumber == 8 || this.machineNumber == 6)
                {
                    scrap = (GameObject)Instantiate(Manager.m.ScrapFurnace1, transform.position, transform.rotation);
                    scrap.transform.position = new Vector3(this.gameObject.transform.position.x, 101.3f, this.gameObject.transform.position.z);
                    scrap.transform.SetParent(Manager.m.scrapFolder.transform);
                }
                else if (this.machineNumber == 9)
                {
                    scrap = (GameObject)Instantiate(Manager.m.ScrapUpgrader1, transform.position, transform.rotation);
                    scrap.transform.position = new Vector3(this.gameObject.transform.position.x, 100.1f, this.gameObject.transform.position.z);
                    scrap.transform.SetParent(Manager.m.scrapFolder.transform);
                }
                if (scrap != null)
                {
                    scrap.GetComponent<RepairDropper>().cost = this.cost * 0.2f;
                }
                Manager.m.factorySpeaker.destroy(nextCam);
                gameObject.transform.Translate(0, -100, 0);
                working = false;
                sold = true;
                gameObject.tag = "Destroyed";
                Destroy(this.gameObject, Time.deltaTime);
                Destroy(instantiatedWrench, Time.deltaTime);
                instantiatedWrench.SetActive(false);
                Manager.m.notificationManager.AddNotification("!Info!\nA machine broke down\nin hall " + factoryHall, Manager.m.eventImages[27]);
            }
            float basicRepairCost = (float)cost * 0.01f;
            float additionalCost = 0;

            float toRepair = 100 - (float)durability;

            if (toRepair > 0)
            {
                if (toRepair <= 10)
                {
                    additionalCost += (toRepair / 100) * (float)cost * 0.3f;
                    toRepair -= toRepair;
                }
                else
                {
                    additionalCost += (10f / 100) * (float)cost * 0.3f + (float)cost * 0.01f;
                    toRepair -= 10;
                }
            }
            if (toRepair > 0)
            {
                if (toRepair <= 30)
                {
                    additionalCost += (toRepair / 100) * (float)cost * 0.5f;
                    toRepair -= toRepair;
                }
                else
                {
                    additionalCost += (30f / 100) * (float)cost * 0.8f + (float)cost * 0.03f;
                    toRepair -= 30;
                }
            }
            if (toRepair > 0)
            {
                if (toRepair <= 40)
                {
                    additionalCost += (toRepair / 100) * (float)cost * 0.7f;
                    toRepair -= toRepair;
                }
                else
                {
                    additionalCost += (40f / 100) * (float)cost * 1f + (float)cost * 0.03f;
                    toRepair -= 40;
                }
            }
            if (toRepair > 0)
            {
                additionalCost += (toRepair / 100) * (float)cost * 0.9f;
                if (toRepair >= 20)
                {
                    additionalCost += (float)cost * 0.05f;
                }
                toRepair -= toRepair;
            }

            repairCost = (basicRepairCost + additionalCost) * quickTimeEventRepairs;

            if (sold == true)
            {
                durability = -999;
                Destroy(instantiatedWrench, Time.deltaTime);
            }
            if (Manager.m.editMode_repair == true && sold == false && this.gameObject.tag == "FactoryObject" && isScrap == false)
            {
                if (durability == 0)
                {
                    if (nextCam != null && nextCam == Manager.m.getCurrentCamera())
                    {
                        instantiatedWrench.SetActive(true);
                        if (instantiatedWrench.transform.position != nextCam.WorldToScreenPoint(this.transform.position))
                        {
                            instantiatedWrench.transform.position = nextCam.WorldToScreenPoint(this.transform.position);
                        }
                        instantiatedWrench.GetComponent<RawImage>().color = Color.red;
                    }
                    else
                    {
                        instantiatedWrench.SetActive(false);
                    }
                }
                else if (working == true && durability < 20)
                {
                    if (nextCam != null && nextCam == Manager.m.getCurrentCamera())
                    {
                        instantiatedWrench.SetActive(true);
                        if (instantiatedWrench.transform.position != nextCam.WorldToScreenPoint(this.transform.position))
                        {
                            instantiatedWrench.transform.position = nextCam.WorldToScreenPoint(this.transform.position);
                        }
                        instantiatedWrench.GetComponent<RawImage>().color = new Color(1, 120f / 255, 0);
                    }
                    else
                    {
                        instantiatedWrench.SetActive(false);
                    }
                }
                else if (working == true && durability < 60)
                {
                    if (nextCam != null && nextCam == Manager.m.getCurrentCamera())
                    {
                        instantiatedWrench.SetActive(true);
                        if (instantiatedWrench.transform.position != nextCam.WorldToScreenPoint(this.transform.position))
                        {
                            instantiatedWrench.transform.position = nextCam.WorldToScreenPoint(this.transform.position);
                        }
                        instantiatedWrench.GetComponent<RawImage>().color = new Color(1, 1, 0);
                    }
                    else
                    {
                        instantiatedWrench.SetActive(false);
                    }
                }
                else if (working == true && durability < 90)
                {
                    if (nextCam != null && nextCam == Manager.m.getCurrentCamera())
                    {
                        instantiatedWrench.SetActive(true);
                        if (instantiatedWrench.transform.position != nextCam.WorldToScreenPoint(this.transform.position))
                        {
                            instantiatedWrench.transform.position = nextCam.WorldToScreenPoint(this.transform.position);
                        }
                        instantiatedWrench.GetComponent<RawImage>().color = new Color(150f / 255, 240f / 255, 0);
                    }
                    else
                    {
                        instantiatedWrench.SetActive(false);
                    }
                }
                else if (working == true)
                {
                    if (nextCam != null && nextCam == Manager.m.getCurrentCamera())
                    {
                        instantiatedWrench.SetActive(true);
                        if (instantiatedWrench.transform.position != nextCam.WorldToScreenPoint(this.transform.position))
                        {
                            instantiatedWrench.transform.position = nextCam.WorldToScreenPoint(this.transform.position);
                        }
                        instantiatedWrench.GetComponent<RawImage>().color = new Color(0, 170f / 255, 0);
                    }
                    else
                    {
                        instantiatedWrench.SetActive(false);
                    }
                }
            }
            else if (working == true && this.gameObject.tag == "FactoryObject")
            {
                instantiatedWrench.SetActive(false);
            }

            if (Time.time > cuncurrenttime && timeperpercent != 0)
            {
                cuncurrenttime = Time.time + 1;



                if (durability > 0 && working == true && dropperNumber != 0)
                {
                    durability -= 1 / timeperpercent;
                    if (Manager.m.qTEUltimateWipeout)
                        durability -= 0.20f;

                    if (durability < 0)
                    {
                        durability = 0;
                    }
                }
                else
                {
                    working = false;
                }
            }
            if (Time.time > cuncurrenttime2 && sold == false && (costPerSecond > 0 || timeperpercent != 0)) // && working == true
            {
                if (working == true) //Manager.m.money >= costPerSecond
                {
                    cuncurrenttime2 = Time.time + 1;
                    if (Manager.m.qTEMaintenanceBoost == 0)
                    {
                        Manager.m.money -= costPerSecond;
                        Manager.m.incomeLastSecond -= costPerSecond;
                    }
                    else
                    {
                        Manager.m.money += costPerSecond * Manager.m.qTEMaintenanceBoost;
                        Manager.m.incomeLastSecond += costPerSecond * Manager.m.qTEMaintenanceBoost;
                    }
                    if (timeperpercent != 0)
                    {
                        float repairCostPerSecond = 0;
                        if (durability >= 90)
                        {
                            repairCostPerSecond = (float)cost * 0.3f * (1f / (100 * (float)timeperpercent));
                            repairCostPerSecond += (float)cost * 0.01f * (1f / (100 * (float)timeperpercent));
                        }
                        else if (durability >= 60)
                        {
                            repairCostPerSecond = (float)cost * 0.8f * (1f / (100 * (float)timeperpercent));
                            repairCostPerSecond += (float)cost * 0.01f * (1f / (100 * (float)timeperpercent));
                        }
                        else if (durability >= 20)
                        {
                            repairCostPerSecond = (float)cost * 1f * (1f / (100 * (float)timeperpercent));
                            repairCostPerSecond += (float)cost * 0.01f * (1f / (100 * (float)timeperpercent));
                        }
                        else // (durability >= 0)
                        {
                            repairCostPerSecond = (float)cost * 2f * (1f / (100 * (float)timeperpercent));
                            repairCostPerSecond += (float)cost * 0.01f * (1f / (100 * (float)timeperpercent));
                        }
                        Manager.m.incomeLastSecond -= repairCostPerSecond;
                    }
                }
                else
                {
                    //working = false;
                }
            }
            if (durability > 0 && Manager.m.money >= costPerSecond)
            {
                working = true;
            }
        }
        else
        {
            working = true;
            durability = 100;
        }
    }
}
