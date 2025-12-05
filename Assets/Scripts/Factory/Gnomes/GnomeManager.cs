using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeManager : MonoBehaviour
{
    public GameObject gnome;
    public List<Gnome_Script> gnomeList;
    public Transform gnomeFolder;
    public float minSpawnTime;
    float gnomeSpawnTimer;
    float randomTimerAdd;
    // Start is called before the first frame update
    void Start()
    {
        gnomeList = new List<Gnome_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.paused == false)
        {
            gnomeList.RemoveAll(x => x == null);
            if (Manager.m.qTEBrokenLights == false && Manager.m.qTEUltimateWipeout == false)
            {
                for (int i = 0; i < gnomeList.Count; i++)
                {
                    gnomeList[i].attackMode = 0;
                    gnomeList[i].fleeing = true;
                }
                gnomeSpawnTimer = 0;
            }
            if (randomTimerAdd < 0)
            {
                randomTimerAdd = Random.Range(0, 15 + gnomeList.Count * 5);
            }
            if (Manager.m.qTEBrokenLights)
            {
                gnomeSpawnTimer += Time.deltaTime;
                if (gnomeSpawnTimer > minSpawnTime + randomTimerAdd * (2 - (Manager.m.getHighestUnlockedType() * 0.1f)))
                {
                    randomTimerAdd = -1;
                    gnomeSpawnTimer = 0;

                    int factoryHall = Manager.m.getCurrentFactoryHall() + 1;
                    int entrance = factoryHall + 10 * Random.Range(0, 6);
                    float speed = 1 + Random.Range(0.0f, 1f);
                    float damage = 0.5f + Random.Range(0.0f, 0.5f + 0.15f * (Manager.m.getHighestUnlockedType() + 1));
                    int attacks = 1 + (int)(Random.Range(0, Manager.m.getHighestUnlockedType() * 0.2f));
                    SpawnGnome(factoryHall, entrance, speed, damage, 1.5f, attacks);
                }
            }
            else if (Manager.m.qTEUltimateWipeout)
            {
                gnomeSpawnTimer += Time.deltaTime;
                if (gnomeSpawnTimer > minSpawnTime + randomTimerAdd)
                {
                    randomTimerAdd = -1;
                    gnomeSpawnTimer = 0;

                    int factoryHall = Manager.m.getCurrentFactoryHall() + 1;
                    int entrance = Manager.m.getCurrentFactoryHall() + 10 * Random.Range(0, 6);
                    float speed = 1 + Random.Range(0.0f, 1.5f);
                    float damage = 1.5f + Random.Range(0.0f, 0.5f + 0.15f * (Manager.m.getHighestUnlockedType() + 1));
                    int attacks = 1 + (int)(Random.Range(0, Manager.m.getHighestUnlockedType() * 0.3f));
                    SpawnGnome(factoryHall, entrance, speed, damage, 1.5f, attacks);
                }
            }
            else
            {
                foreach (Gnome_Script g in gnomeList)
                {
                    if (g != null)
                    {
                        g.attackMode = 0;
                    }
                }
            }
        }
    }

    void SpawnGnome(int factoryHall, int entrance, float speed, float damage, float discoveryDuration, int attacks)
    {
        GameObject g = Instantiate(gnome);
        g.transform.position = new Vector3(Manager.m.factoryExits[entrance].x, 100 + (g.GetComponent<CharacterController>().height + g.GetComponent<CharacterController>().radius) / 2, Manager.m.factoryExits[entrance].y);
        g.GetComponent<Gnome_Script>().attackedHall = factoryHall;
        g.GetComponent<Gnome_Script>().attackDamage = damage;
        g.GetComponent<Gnome_Script>().baseMovementSpeed = speed;
        g.GetComponent<Gnome_Script>().baseRotationSpeed = speed;
        g.GetComponent<Gnome_Script>().discoveryDuration = discoveryDuration;
        g.GetComponent<Gnome_Script>().attackMode = attacks;
        g.transform.parent = gnomeFolder;
        gnomeList.Add(g.GetComponent<Gnome_Script>());
        Manager.m.notificationManager.AddNotification("!WARNING!\nIntruder at Hall " + (factoryHall), Manager.m.eventImages[26]);
    }
}
