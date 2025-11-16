using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Analytics;

public class Furnace : MonoBehaviour
{
    public float furnaceMultiplier;
    List<GameObject> missions;
    private void Start()
    {
        furnaceMultiplier = (float)gameObject.GetComponentInParent<RepairDropper>().furnaceMultiplier;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ore>() == true)
        {
            missions = Manager.m.missionManager.missions;
            if (this.GetComponentInParent<RepairDropper>().working == true)
            {
                for (int i = 0; i < missions.Count; i++)
                {
                    if (missions[i].GetComponent<Mission>().acceptedMission == true && missions[i].GetComponent<Mission>().finishedMission == false)
                    {
                        string identification = collision.gameObject.name;
                        identification = identification.Replace("Dropper", "");
                        identification = identification.Replace("Drop(Clone)", "");
                        int dropNumber = int.Parse(identification) - 1;
                        if (dropNumber == missions[i].GetComponent<Mission>().oreNumber && collision.gameObject.GetComponent<Ore>().upgradeLevel == missions[i].GetComponent<Mission>().upgradeLevel)
                        {
                            if (Manager.m.tutorial.inTutorial == false)
                            {
                                Manager.m.exp = Manager.m.exp + (collision.gameObject.GetComponent<Ore>().exp);
                            }
                            missions[i].GetComponent<Mission>().sold += 1;
                            if (Manager.m.qTEMissionBuff == dropNumber + 1 && missions[i].GetComponent<Mission>().sold < missions[i].GetComponent<Mission>().quantity)
                            {
                                missions[i].GetComponent<Mission>().sold += 1;
                            }
                            if (Manager.m.qTEMissionImpossible)
                            {
                                missions[i].GetComponent<Mission>().sold -= 0.5f;
                            }
                            if (missions[i].GetComponent<Mission>().sold > missions[i].GetComponent<Mission>().quantity)
                            {
                                missions[i].GetComponent<Mission>().sold = missions[i].GetComponent<Mission>().quantity;
                            }
                            Manager.m.factorySpeaker.missionAdd(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);
                            Destroy(collision.gameObject);
                            return;
                        }
                    }
                }
                float marketMultiplier = 1;

                for (int i = 0; i < Manager.m.marketManager.marketDrops.Length; i++)
                {
                    if (collision.gameObject.name == ("Dropper" + (i + 1) + "Drop(Clone)"))
                    {
                        int upgradeLevel = collision.gameObject.GetComponent<Ore>().upgradeLevel;
                        if (Manager.m.qTEMarketBoost == 0)
                        {
                            if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 1.5)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] -= 0.001f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 1)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] -= 0.0005f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 0.75)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] -= 0.0002f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 0.5)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] -= 0.0001f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed;
                            }
                        }
                        else
                        {
                            if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 1.5)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] += 0.0001f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed * Manager.m.qTEMarketBoost;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 1)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] += 0.0002f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed * Manager.m.qTEMarketBoost;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 0.75)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] += 0.0005f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed * Manager.m.qTEMarketBoost;
                            }
                            else if (Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] > 0.5)
                            {
                                Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] += 0.001f * Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i] * (1 + 0.01f * i) * Manager.m.droppers[i].GetComponent<RepairDropper>().dropSpeed * Manager.m.qTEMarketBoost;
                            }
                        }
                        marketMultiplier = Manager.m.marketManager.dropValueMultipliers[upgradeLevel][i];
                    }
                }
                int inversion = 1;
                if (Manager.m.qTEInvertedMarket) inversion = -1;
                if (collision.gameObject.GetComponent<Ore>().oreNumber == Manager.m.qTEOverheatingNumber - 1)
                {
                    Manager.m.money = Manager.m.money + (collision.gameObject.GetComponent<Ore>().value * (furnaceMultiplier + Manager.m.upgradeMultipliers[collision.gameObject.GetComponent<Ore>().upgradeLevel] + (marketMultiplier - 1)) * Manager.m.qTEOverheating * inversion);
                }
                else
                {
                    Manager.m.money = Manager.m.money + (collision.gameObject.GetComponent<Ore>().value * (furnaceMultiplier + Manager.m.upgradeMultipliers[collision.gameObject.GetComponent<Ore>().upgradeLevel] + (marketMultiplier - 1)) * inversion);
                }
                if (Manager.m.tutorial.inTutorial == false)
                {
                    Manager.m.exp = Manager.m.exp + (collision.gameObject.GetComponent<Ore>().exp);
                }
                Manager.m.incomeLastSecond += (collision.gameObject.GetComponent<Ore>().value * (furnaceMultiplier + Manager.m.upgradeMultipliers[collision.gameObject.GetComponent<Ore>().upgradeLevel]));
                Manager.m.factorySpeaker.sell(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);

                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
    private void Update()
    {
        
    }
}
