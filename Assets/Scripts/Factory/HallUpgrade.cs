using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HallUpgrade : MonoBehaviour
{
    public int factoryNumber;
    public GameObject leftSideBar;
    public GameObject rightSideBar;
    public GameObject leftSidePlate;
    public GameObject rightSidePlate;
    public GameObject self;
    public GameObject field;
    public GameObject hole;
    public Material white_neon;
    public Material white_transparent;
    public GameObject informationBoard;
    public GameObject informationText;
    public Transform imageFolder;
    public List<int> inputRessources;
    List<int> startInputRessources;
    public List<int> inputRessourcesLevel;
    public GameObject[] imageTransforms;
    public RawImage[] oreImages;
    public RawImage[] upgradeImages;
    public TextMeshProUGUI[] oreTexts;
    List<RawImage> instantiatedOreImages;
    List<RawImage> instantiatedUpgradeImages;
    int ressourcesLeft;
    bool startedFilling;
    bool finishedFilling;
    public bool inDisappearAnimation;
    Vector3 startPosition;
    Vector3 leftSideBarPosition;
    Vector3 rightSideBarPosition;
    Vector3 leftSidePlatePosition;
    Vector3 rightSidePlatePosition;
    Vector3 leftSidePlateScale;
    Vector3 rightSidePlateScale;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = self.transform.position;
        leftSideBarPosition = leftSideBar.transform.position;
        rightSideBarPosition = rightSideBar.transform.position;
        leftSidePlatePosition = leftSidePlate.transform.position;
        rightSidePlatePosition = rightSidePlate.transform.position;
        leftSidePlateScale = leftSidePlate.transform.localScale;
        rightSidePlateScale = rightSidePlate.transform.localScale;

        startInputRessources = new List<int>();
        startInputRessources.AddRange(inputRessources);
        if (field != null)
        {
            field.SetActive(false);
        }
        hole.SetActive(false);
        startedFilling = false;
        finishedFilling = false;
        instantiatedOreImages = new List<RawImage>();
        instantiatedUpgradeImages = new List<RawImage>();
        upgradeImages = Manager.m.upgradeImages;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.paused == false || Manager.m.inFinalSequence == true)
        {
            if (Manager.m.inMainMenu == false)
            {
                if (Manager.m.level >= factoryNumber + 1)
                {
                    if (startedFilling == false && finishedFilling == false)
                    {
                        StartCoroutine(open());
                    }

                    startedFilling = true;
                }
                ressourcesLeft = 0;
                if (finishedFilling == false)
                {
                    for (int i = 0; i < inputRessources.Count; i++)
                    {
                        ressourcesLeft += inputRessources[i];
                    }
                }
                if (Manager.m.upgradeRessources[factoryNumber] == true && inDisappearAnimation == false)
                {
                    for (int i = 0; i < instantiatedOreImages.Count; i++)
                    {
                        Destroy(instantiatedOreImages[i].gameObject);
                    }
                    instantiatedOreImages.Clear();
                    for (int i = 0; i < instantiatedUpgradeImages.Count; i++)
                    {
                        Destroy(instantiatedUpgradeImages[i].gameObject);
                    }
                    instantiatedUpgradeImages.Clear();

                    self.SetActive(false);
                    if (field != null)
                    {
                        field.SetActive(true);
                    }
                }
                if (finishedFilling == false && ressourcesLeft == 0)
                {
                    finishedFilling = true;
                    startedFilling = false;

                    for (int i = 0; i < instantiatedOreImages.Count; i++)
                    {
                        Destroy(instantiatedOreImages[i].gameObject);
                    }
                    instantiatedOreImages.Clear();
                    for (int i = 0; i < instantiatedUpgradeImages.Count; i++)
                    {
                        Destroy(instantiatedUpgradeImages[i].gameObject);
                    }
                    instantiatedUpgradeImages.Clear();

                    if (Manager.m.upgradeRessources[factoryNumber] == false)
                    {
                        StartCoroutine(disappear());
                    }
                }
            }
            else
            {
                leftSidePlate.SetActive(false);
                rightSidePlate.SetActive(false);
                leftSideBar.SetActive(false);
                rightSideBar.SetActive(false);
                self.SetActive(true);
                self.transform.position = startPosition;
            }
            if (Manager.m.upgradeRessources[factoryNumber] == true && inDisappearAnimation == false)
            {
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void OnMouseOver()
    {
        if (Manager.m.inMainMenu == false && Manager.m.inMarket == false && Manager.m.inFactoryHalls == false && Manager.m.inSettings == false && Manager.m.inMissions == false)
        {
            informationBoard.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor, Manager.m.graphicManager.gUIScaleFactor);
            for (int i = 0; i < instantiatedOreImages.Count; i++)
            {
                Destroy(instantiatedOreImages[i].gameObject);
            }
            instantiatedOreImages.Clear();
            for (int i = 0; i < instantiatedUpgradeImages.Count; i++)
            {
                Destroy(instantiatedUpgradeImages[i].gameObject);
            }
            instantiatedUpgradeImages.Clear();
            for (int i = 0; i < oreTexts.Length; i++)
            {
                oreTexts[i].text = "";
            }

            if (startedFilling == true && finishedFilling == false)
            {
                informationBoard.SetActive(true);

                informationText.GetComponent<TextMeshProUGUI>().text = "<align=center>Resources:<align=left>";
                for (int i = 0; i < inputRessources.Count; i++)
                {
                    if (inputRessources[i] != 0)
                    {
                        RawImage upgradeImage = Instantiate(upgradeImages[inputRessourcesLevel[i]]);
                        upgradeImage.transform.SetParent(imageTransforms[instantiatedUpgradeImages.Count].transform);
                        upgradeImage.transform.localPosition = new Vector3(0, 0, 0);
                        instantiatedUpgradeImages.Add(upgradeImage);
                        //informationText.GetComponent<TextMeshProUGUI>().text += "<br><br>     * " + inputRessources[i] + "";
                        RawImage oreImage = Instantiate(oreImages[i - 1]);
                        oreImage.transform.SetParent(imageTransforms[instantiatedOreImages.Count].transform);
                        oreImage.transform.localPosition = new Vector3(0, 0, 0);
                        instantiatedOreImages.Add(oreImage);

                        oreTexts[instantiatedOreImages.Count - 1].text = "* " + inputRessources[i] + "";
                    }
                }
            }
        }
    }
    private void OnMouseExit()
    {
        informationBoard.SetActive(false);
        for (int i = 0; i < instantiatedOreImages.Count; i++)
        {
            Destroy(instantiatedOreImages[i].gameObject);
        }
        instantiatedOreImages.Clear();
        for (int i = 0; i < instantiatedUpgradeImages.Count; i++)
        {
            Destroy(instantiatedUpgradeImages[i].gameObject);
        }
        instantiatedUpgradeImages.Clear();
        for (int i = 0; i < oreTexts.Length; i++)
        {
            oreTexts[i].text = "";
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(startedFilling == true && other.gameObject.tag == "Ore" && Manager.m.inMainMenu == false)
        {
            switch (other.gameObject.name)
            {
                case "Dropper1Drop(Clone)":
                    {
                        int i = 1;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper2Drop(Clone)":
                    {
                        int i = 2;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper3Drop(Clone)":
                    {
                        int i = 3;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper4Drop(Clone)":
                    {
                        int i = 4;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper5Drop(Clone)":
                    {
                        int i = 5;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper6Drop(Clone)":
                    {
                        int i = 6;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper7Drop(Clone)":
                    {
                        int i = 7;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper8Drop(Clone)":
                    {
                        int i = 8;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper9Drop(Clone)":
                    {
                        int i = 9;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
                case "Dropper10Drop(Clone)":
                    {
                        int i = 10;
                        if (inputRessources[i] > 0 && other.gameObject.GetComponent<Ore>().upgradeLevel >= inputRessourcesLevel[i])
                        {
                            inputRessources[i] -= 1;
                        }
                        break;
                    }
            }
            Destroy(other.gameObject);
        }
    }
    public void ResetUpgrader()
    {
        self.SetActive(true);
        if (field != null)
        {
            field.SetActive(false);
        }
        this.GetComponent<BoxCollider>().enabled = true;
        List<int> ressources = new List<int>();
        ressources.AddRange(startInputRessources);
        inputRessources = ressources;
        startedFilling = false;
        finishedFilling = false;
        self.transform.position = startPosition;
        leftSideBar.SetActive(true);
        rightSideBar.SetActive(true);
        leftSidePlate.SetActive(true);
        rightSidePlate.SetActive(true);
        hole.SetActive(false);

        leftSideBar.transform.position = leftSideBarPosition;
        rightSideBar.transform.position = rightSideBarPosition;
        leftSidePlate.transform.position = leftSidePlatePosition;
        rightSidePlate.transform.position = rightSidePlatePosition;
        leftSidePlate.transform.localScale = leftSidePlateScale;
        rightSidePlate.transform.localScale = rightSidePlateScale;
    }
    public IEnumerator open()
    {
        leftSideBar.GetComponent<BoxCollider>().enabled = false;
        rightSideBar.GetComponent<BoxCollider>().enabled = false;
        leftSidePlate.GetComponent<BoxCollider>().enabled = false;
        rightSidePlate.GetComponent<BoxCollider>().enabled = false;
        while (leftSidePlate.transform.localScale.x >= 0.1f)
        {
            leftSidePlate.transform.localScale = new Vector3(leftSidePlate.transform.localScale.x - 2 * Time.deltaTime, leftSidePlate.transform.localScale.y, leftSidePlate.transform.localScale.z);
            leftSidePlate.transform.Translate(1f * Time.deltaTime, 0, 0);
            leftSideBar.transform.Translate(2f * Time.deltaTime, 0, 0);

            rightSidePlate.transform.localScale = new Vector3(rightSidePlate.transform.localScale.x - 2 * Time.deltaTime, rightSidePlate.transform.localScale.y, rightSidePlate.transform.localScale.z);
            rightSidePlate.transform.Translate(-1f * Time.deltaTime, 0, 0);
            rightSideBar.transform.Translate(-2f * Time.deltaTime, 0, 0);

            yield return new WaitForEndOfFrame();
        }
        if (leftSidePlate.transform.localScale.x <= 0.1f)
        {
            leftSideBar.SetActive(false);
            rightSideBar.SetActive(false);
            leftSidePlate.SetActive(false);
            rightSidePlate.SetActive(false);
        }
    }

    public IEnumerator disappear()
    {
        inDisappearAnimation = true;
        hole.SetActive(true);
        for (int i = 0; i < 25; i++)
        {
            hole.transform.localScale = new Vector3(hole.transform.localScale.x + 0.08f - i * 0.0035f, hole.transform.localScale.y, hole.transform.localScale.z + 0.08f - i * 0.0035f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        this.GetComponent<BoxCollider>().enabled = false;
        for(int i = 0; i < 140; i++)
        {
            self.transform.Translate(0, -0.08f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        if (field != null)
        {
            field.transform.Translate(0, -0.3f, 0);
            field.SetActive(true);
            for (int i = 0; i < 15; i++)
            {
                field.transform.Translate(0, 0.02f, 0);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        Manager.m.upgradeRessources[factoryNumber] = true;
        for (int i = 0; i < 25; i++)
        {
            hole.transform.localScale = new Vector3(hole.transform.localScale.x - 0.08f + i * 0.0035f, hole.transform.localScale.y, hole.transform.localScale.z - 0.08f + i * 0.0035f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        hole.transform.localScale = new Vector3(0.1f, 0.02f, 0.1f);
        hole.SetActive(false);
        inDisappearAnimation = false;
    }
}
