using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    double currenttime;

    public GameObject backround;
    public TextMeshProUGUI identificationTxt;
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI rewardTxt;
    public RawImage oreImage;
    RawImage instantiatedOreImage;
    RawImage instantiatedUpgradeImage;
    public RawImage progressBar;
    public TextMeshProUGUI progressTxt;
    public float time;
    public int oreNumber;
    public int upgradeLevel;
    public double reward;
    public float quantity;
    public float sold;

    public Button acceptMission;
    public Button declineMission;
    public Button cancelMission;
    public Button deliverMission;
    public bool acceptedMission;
    public bool declinedMission;
    public bool canceldMission;
    public bool finishedMission;
    public bool deliveredMission;

    public GameObject[] upgradeLevels;
    string[] oreNames;
    public RawImage[] oreImages;
    public RawImage[] upgradeImages;
    // Start is called before the first frame update
    void Start()
    {
        oreNames = Manager.m.oreIdentifications;
        oreImages = Manager.m.oreImages;
        upgradeImages = Manager.m.upgradeImages;
        identificationTxt.text = "" + oreNames[oreNumber];
        rewardTxt.text = "" + Money.NumberInUnit(reward, 1) + "$";

        instantiatedUpgradeImage = Instantiate(upgradeImages[upgradeLevel]);
        instantiatedUpgradeImage.transform.SetParent(oreImage.transform);
        instantiatedUpgradeImage.transform.localScale = new Vector3(instantiatedUpgradeImage.transform.localScale.x * 0.6f, instantiatedUpgradeImage.transform.localScale.y * 0.6f, 1);
        instantiatedUpgradeImage.transform.localPosition = new Vector3(0, 0, 0);

        instantiatedOreImage = Instantiate(oreImages[oreNumber]);
        instantiatedOreImage.transform.SetParent(oreImage.transform);
        instantiatedOreImage.transform.localScale = new Vector3(instantiatedOreImage.transform.localScale.x * 0.6f, instantiatedOreImage.transform.localScale.y * 0.6f, 1);
        instantiatedOreImage.transform.localPosition = new Vector3(0, 0, 0);

        currenttime = Time.time;

        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            upgradeLevels[i].SetActive(false);
        }
        upgradeLevels[upgradeLevel].SetActive(true);

        acceptMission.onClick.AddListener(AcceptMission);
        declineMission.onClick.AddListener(DeclineMission);
        cancelMission.onClick.AddListener(CancelMission);
        deliverMission.onClick.AddListener(DeliverMission);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.missionManager.missions.Contains(this.gameObject) == false)
        {
            Destroy(this.gameObject);
        }

        if (Manager.m.paused == false)
        {
            if (currenttime < Time.time)
            {
                currenttime = Time.time + 1;
                time -= 1;
            }
            double timeMinutes = time / 60;
            double timeSeconds = 60 * (timeMinutes - Mathf.Floor((float)timeMinutes));
            timeMinutes = Mathf.Floor((float)timeMinutes);
            timeSeconds = Mathf.Floor((float)timeSeconds);
            string _0;
            if (timeSeconds < 10)
            {
                _0 = "0";
            }
            else
            {
                _0 = "";
            }
            timeTxt.text = timeMinutes + ":" + _0 + "" + timeSeconds + "Min";
            progressTxt.text = sold + " / " + quantity;
            if (sold >= quantity)
            {
                acceptMission.gameObject.SetActive(false);
                declineMission.gameObject.SetActive(false);
                cancelMission.gameObject.SetActive(false);
                deliverMission.gameObject.SetActive(true);
                if (finishedMission == false)
                {
                    finishedMission = true;
                    Manager.m.notificationManager.AddNotification("!Info!\nA delivery is ready to be sent!", Manager.m.eventImages[25]);
                }
            }
            progressBar.transform.localScale = new Vector3(sold / quantity, 1, 1);
            progressBar.transform.localPosition = new Vector3((1 - (sold / quantity)) * progressBar.GetComponent<RectTransform>().rect.width * -0.5f, 0, 0);
        }
    }

    public void AcceptMission()
    {
        Manager.m.effectSpeaker.accept();
        acceptedMission = true;
        acceptMission.gameObject.SetActive(false);
        declineMission.gameObject.SetActive(false);
        cancelMission.gameObject.SetActive(true);
    }
    public void DeclineMission()
    {
        Manager.m.effectSpeaker.click();

        if (Manager.m.missionManager.missions.Count == 1)
        {
            Manager.m.declinedMission = 60;
        }
        declinedMission = true;
        acceptMission.gameObject.SetActive(false);
        declineMission.gameObject.SetActive(false);
        cancelMission.gameObject.SetActive(true);
    }
    public void CancelMission()
    {
        Manager.m.effectSpeaker.error();

        if (Manager.m.missionManager.missions.Count == 1)
        {
            Manager.m.declinedMission = (int)(60 * (1 - (sold / quantity)));
        }

        canceldMission = true;
    }
    public void DeliverMission()
    {
        Manager.m.effectSpeaker.sell();
        deliveredMission = true;
    }
}
