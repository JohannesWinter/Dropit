using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public float[][] dropValueMultipliers;
    public GameObject[] marketDrops;
    public GameObject overlay;
    public GameObject exit;
    float currenttime;
    // Start is called before the first frame update
    void Start()
    {
        exit.GetComponent<Button>().onClick.AddListener(Exit);
        //dropValueMultiplier = new float[marketDrops.Length];
        dropValueMultipliers = new float[Manager.m.maxUpgradeNumber + 1][];
        for (int i = 0; i < dropValueMultipliers.Length; i++)
        {
            dropValueMultipliers[i] = new float[marketDrops.Length];

            for (int x = 0; x < marketDrops.Length; x++)
            {
                dropValueMultipliers[i][x] = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.inMarket)
        {
            overlay.SetActive(true);
        }
        else
        {
            overlay.SetActive(false);
        }
        if (currenttime < Time.time)
        {
            currenttime = Time.time + 1;

            for (int i = 0; i <= Manager.m.maxUpgradeNumber; i++) //updating marketplace to values in Manager
            {
                for (int x = 0; x < marketDrops.Length; x++)
                {
                    marketDrops[x].GetComponent<Market>().valueMultipliers[i] = dropValueMultipliers[i][x];
                }
            }
            for (int i = 0; i < dropValueMultipliers.Length; i++)
            {
                for (int x = 0; x < dropValueMultipliers[i].Length; x++)
                {
                    {
                        float averageMultiplier;

                        float allMultipliers = 0;
                        int amountMultipliers = 0;

                        for (int y = 0; y < dropValueMultipliers.Length; y++)
                        {
                            allMultipliers += dropValueMultipliers[y][x];
                            amountMultipliers++;
                        }
                        averageMultiplier = allMultipliers / amountMultipliers;

                        float diffrenceMultiplier = averageMultiplier - dropValueMultipliers[i][x];

                        dropValueMultipliers[i][x] += diffrenceMultiplier / 50;
                    }

                    if (UnityEngine.Random.Range(0, 1000 - Mathf.Round(dropValueMultipliers[i][x] * 100)) < 1) //Rare event on marketplace: Random value; highter Chance on higher current value
                    {
                        float random;
                        random = UnityEngine.Random.Range(60, 150);
                        dropValueMultipliers[i][x] = random / 100;
                    }
                    if (UnityEngine.Random.Range(0, 2400) < 1)
                    {
                        dropValueMultipliers[i][x] = (float)UnityEngine.Random.Range(50, 70) / 100;
                    }
                    if (UnityEngine.Random.Range(0, 2400) < 1)
                    {
                        dropValueMultipliers[i][x] = (float)UnityEngine.Random.Range(180, 200) / 100;
                    }
                    if (dropValueMultipliers[i][x] > 1.5f)
                    {
                        if (UnityEngine.Random.Range(0, 30) < 1)
                        {
                            if (UnityEngine.Random.Range(0, 10) > 1)
                            {
                                dropValueMultipliers[i][x] -= (float)UnityEngine.Random.Range(1, 5) / 100;
                            }
                            else
                            {
                                dropValueMultipliers[i][x] -= (float)UnityEngine.Random.Range(10, 30) / 100;
                            }
                        }
                    }
                    if (dropValueMultipliers[i][x] > 1.9f)
                    {
                        if (UnityEngine.Random.Range(0, 5) < 1)
                        {
                            if (UnityEngine.Random.Range(0, 10) > 1)
                            {
                                dropValueMultipliers[i][x] -= (float)UnityEngine.Random.Range(1, 3) / 100;
                            }
                            else
                            {
                                dropValueMultipliers[i][x] -= (float)UnityEngine.Random.Range(40, 80) / 100;
                            }
                        }
                    }
                    if (Manager.m.qTEMarketCrash == 0)
                    {
                        if (dropValueMultipliers[i][x] < 0.9f)
                        {
                            if (UnityEngine.Random.Range(0, 30) < 1)
                            {
                                if (UnityEngine.Random.Range(0, 10) > 1)
                                {
                                    dropValueMultipliers[i][x] += (float)UnityEngine.Random.Range(1, 5) / 100;
                                }
                                else
                                {
                                    dropValueMultipliers[i][x] += (float)UnityEngine.Random.Range(10, 30) / 100;
                                }
                            }
                        }
                        if (dropValueMultipliers[i][x] < 2) //Natural increase of drop value
                        {
                            dropValueMultipliers[i][x] += (0.001f * (UnityEngine.Random.Range(-600, 900) / 100)) / (1 + 0.05f * i);
                        }
                    }
                    else
                    {
                        if (dropValueMultipliers[i][x] > 1)
                        {
                            dropValueMultipliers[i][x] = 1;
                        }
                        if (dropValueMultipliers[i][x] < 2) //Natural decrease of drop value in crash
                        {
                            dropValueMultipliers[i][x] -= (0.001f * (UnityEngine.Random.Range(-600, 900) / 100)) / (1 + 0.05f * i) * Manager.m.qTEMarketCrash;
                        }
                    }

                    if (dropValueMultipliers[i][x] < 0.75f)
                    {
                        if (UnityEngine.Random.Range(0, 15) < 1)
                        {
                            if (UnityEngine.Random.Range(0, 20) > 1)
                            {
                                dropValueMultipliers[i][x] += (float)UnityEngine.Random.Range(1, 3) / 100;
                            }
                            else
                            {
                                dropValueMultipliers[i][x] += (float)UnityEngine.Random.Range(10, 30) / 100;
                            }
                        }
                    }


                    if (dropValueMultipliers[i][x] > 2)
                    {
                        dropValueMultipliers[i][x] = 2;
                    }
                    if (dropValueMultipliers[i][x] < 0.5f)
                    {
                        dropValueMultipliers[i][x] = 0.5f;
                    }
                }
            }
        }
    }
    void Exit()
    {
        if (Manager.m.tutorial.IsButtonAllowed(exit) == false)
        {
            return;
        }
        Manager.m.effectSpeaker.click();
        Manager.m.inMarket = false;
    }
}
