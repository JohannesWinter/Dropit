using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryHallsManager : MonoBehaviour
{
    public GameObject overlay;

    public GameObject hall1Button;
    public GameObject hall2Button;
    public GameObject hall3Button;
    public GameObject hall4Button;
    public GameObject hall5Button;
    public GameObject hall6Button;
    public GameObject hall7Button;
    public GameObject hall8Button;
    public GameObject hall9Button;
    public GameObject hall10Button;
    //public GameObject hall11Button;
    //public GameObject hall12Button;
    //public GameObject hall13Button;
    //public GameObject hall14Button;
    //public GameObject hall15Button;

    public Camera hall1Camera;
    public Camera hall2Camera;
    public Camera hall3Camera;
    public Camera hall4Camera;
    public Camera hall5Camera;
    public Camera hall6Camera;
    public Camera hall7Camera;
    public Camera hall8Camera;
    public Camera hall9Camera;
    public Camera hall10Camera;
    //public Camera hall11Camera;
    //public Camera hall12Camera;
    //public Camera hall13Camera;
    //public Camera hall14Camera;
    //public Camera hall15Camera;

    public GameObject close;

    float signal = 0;

    void Start()
    {
        close.GetComponent<Button>().onClick.AddListener(Close);
        hall1Button.GetComponent<Button>().onClick.AddListener(Hall1);
        hall2Button.GetComponent<Button>().onClick.AddListener(Hall2);
        hall3Button.GetComponent<Button>().onClick.AddListener(Hall3);
        hall4Button.GetComponent<Button>().onClick.AddListener(Hall4);
        hall5Button.GetComponent<Button>().onClick.AddListener(Hall5);
        hall6Button.GetComponent<Button>().onClick.AddListener(Hall6);
        hall7Button.GetComponent<Button>().onClick.AddListener(Hall7);
        hall8Button.GetComponent<Button>().onClick.AddListener(Hall8);
        hall9Button.GetComponent<Button>().onClick.AddListener(Hall9);
        hall10Button.GetComponent<Button>().onClick.AddListener(Hall10);
        //hall11Button.GetComponent<Button>().onClick.AddListener(Hall11);
        //hall12Button.GetComponent<Button>().onClick.AddListener(Hall12);
        //hall13Button.GetComponent<Button>().onClick.AddListener(Hall13);
        //hall14Button.GetComponent<Button>().onClick.AddListener(Hall14);
        //hall15Button.GetComponent<Button>().onClick.AddListener(Hall15);

        hall1Camera = Manager.m.factoryCameras[0];
        hall2Camera = Manager.m.factoryCameras[1];
        hall3Camera = Manager.m.factoryCameras[2];
        hall4Camera = Manager.m.factoryCameras[3];
        hall5Camera = Manager.m.factoryCameras[4];
        hall6Camera = Manager.m.factoryCameras[5];
        hall7Camera = Manager.m.factoryCameras[6];
        hall8Camera = Manager.m.factoryCameras[7];
        hall9Camera = Manager.m.factoryCameras[8];
        hall10Camera = Manager.m.factoryCameras[9];

        hall1Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall2Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall3Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall4Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall5Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall6Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall7Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall8Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall9Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall10Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall11Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall12Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall13Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall14Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall15Button.GetComponent<FactoryHallButton>().frame.SetActive(false);

        hall1Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall2Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall3Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall4Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall5Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall6Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall7Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall8Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall9Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall10Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall11Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall12Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall13Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall14Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall15Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
    }

    void Update()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(Manager.m.graphicManager.gUIScaleFactor - 0.1f, Manager.m.graphicManager.gUIScaleFactor - 0.1f, Manager.m.graphicManager.gUIScaleFactor - 0.1f);
        signal += Time.deltaTime;
        if (signal > 2)
        {
            signal -= 2;
        }
        hall1Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(1), signal, hall1Button.GetComponent<FactoryHallButton>().lastVisited);
        hall2Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(2), signal, hall2Button.GetComponent<FactoryHallButton>().lastVisited);
        hall3Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(3), signal, hall3Button.GetComponent<FactoryHallButton>().lastVisited);
        hall4Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(4), signal, hall4Button.GetComponent<FactoryHallButton>().lastVisited);
        hall5Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(5), signal, hall5Button.GetComponent<FactoryHallButton>().lastVisited);
        hall6Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(6), signal, hall6Button.GetComponent<FactoryHallButton>().lastVisited);
        hall7Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(7), signal, hall7Button.GetComponent<FactoryHallButton>().lastVisited);
        hall8Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(8), signal, hall8Button.GetComponent<FactoryHallButton>().lastVisited);
        hall9Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(9), signal, hall9Button.GetComponent<FactoryHallButton>().lastVisited);
        hall10Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = getTextColor(hallAttacked(10), signal, hall10Button.GetComponent<FactoryHallButton>().lastVisited);

        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall1")))
        {
            Hall1();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall2")))
        {
            Hall2();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall3")))
        {
            Hall3();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall4")))
        {
            Hall4();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall5")))
        {
            Hall5();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall6")))
        {
            Hall6();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall7")))
        {
            Hall7();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall8")))
        {
            Hall8();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall9")))
        {
            Hall9();
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("FactoryHall10")))
        {
            Hall10();
        }
        if (Manager.m.inFactoryHalls == true)
        {
            overlay.SetActive(true);
        }
        else
        {
            overlay.SetActive(false);
            //Manager.m.inFactoryHalls 
        }
    }

    void Close()
    {
        Manager.m.effectSpeaker.click();
        Manager.m.inFactoryHalls = false;
        hall1Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall2Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall3Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall4Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall5Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall6Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall7Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall8Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall9Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        hall10Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall11Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall12Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall13Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall14Button.GetComponent<FactoryHallButton>().frame.SetActive(false);
        //hall15Button.GetComponent<FactoryHallButton>().frame.SetActive(false);

        hall1Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall2Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall3Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall4Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall5Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall6Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall7Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall8Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall9Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        hall10Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall11Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall12Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall13Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall14Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
        //hall15Button.GetComponent<FactoryHallButton>().numberTxt.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.7f, 0);
    }

    bool hallAttacked(int hallNumber)
    {
        var gnomes = Manager.m.gnomeManager.gnomeList;
        for (int i = 0; i < gnomes.Count; i++)
        {
            if (gnomes[i].attackedHall == hallNumber)
            {
                return true;
            }
        }
        return false;
    }

    Color getTextColor(bool attacked, float signalState, bool lastVisited)
    {
        if (attacked)
        {
            if (lastVisited)
            {
                if (signalState <= 1)
                {
                    return new Color(signalState, 1f - signalState, 0);
                }
                else if (signalState <= 2)
                {
                    signalState -= 1;
                    return new Color(1f - signalState, signalState, 0);
                }
            }
            else
            {
                if (signalState <= 1)
                {
                    return new Color(signalState * 0.7f, (1f - signalState) * 0.7f, 0);
                }
                else if (signalState <= 2)
                {
                    signalState -= 1;
                    return new Color((1f - signalState) * 0.7f, signalState * 0.7f, 0);
                }
            }
        }
        else
        {
            if (lastVisited)
            {
                return Color.green;
            }
            else
            {
                return new Color(0, 0.7f, 0);
            }
        }
        return Color.green;
    }

    public void setHall(int hallNumber)
    {
        switch (hallNumber)
        {
            case 1:
                {
                    Hall1();
                    break;
                }
            case 2:
                {
                    Hall2();
                    break;
                }
            case 3:
                {
                    Hall3();
                    break;
                }
            case 4:
                {
                    Hall4();
                    break;
                }
            case 5:
                {
                    Hall5();
                    break;
                }
            case 6:
                {
                    Hall6();
                    break;
                }
            case 7:
                {
                    Hall7();
                    break;
                }
            case 8:
                {
                    Hall8();
                    break;
                }
            case 9:
                {
                    Hall9();
                    break;

                }
            case 10:
                {
                    Hall10();
                    break;
                }
            default:
                {
                    print("Error: Hallnumber " + hallNumber + " not found");
                    break;
                }
        }
    }
    void Hall1()
    {
        if (Manager.m.level >= 1)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall1Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall2()
    {
        if (Manager.m.level >= 2)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall2Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall3()
    {
        if (Manager.m.level >= 3)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall3Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall4()
    {
        if (Manager.m.level >= 4)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall4Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall5()
    {
        if (Manager.m.level >= 5)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall5Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall6()
    {
        if (Manager.m.level >= 6)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall6Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall7()
    {
        if (Manager.m.level >= 7)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall7Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall8()
    {
        if (Manager.m.level >= 8)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall8Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall9()
    {
        if (Manager.m.level >= 9)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall9Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    void Hall10()
    {
        if (Manager.m.level >= 10)
        {
            //Manager.m.effectSpeaker.click();
            Manager.m.setKamera(hall10Camera);
            Close();
        }
        else
        {
            Manager.m.effectSpeaker.error();
        }
    }
    //void Hall11()
    //{
    //    if (Manager.m.level >= 11)
    //    {
    //        //Manager.m.effectSpeaker.click();
    //        Manager.m.setKamera(hall11Camera);
    //        Close();
    //    }
    //    else
    //    {
    //        Manager.m.effectSpeaker.error();
    //    }
    //}
    //void Hall12()
    //{
    //    if (Manager.m.level >= 12)
    //    {
    //        //Manager.m.effectSpeaker.click();
    //        Manager.m.setKamera(hall12Camera);
    //        Close();
    //    }
    //    else
    //    {
    //        Manager.m.effectSpeaker.error();
    //    }
    //}
    //void Hall13()
    //{
    //    if (Manager.m.level >= 13)
    //    {
    //        //Manager.m.effectSpeaker.click();
    //        Manager.m.setKamera(hall13Camera);
    //        Close();
    //    }
    //    else
    //    {
    //        Manager.m.effectSpeaker.error();
    //    }
    //}
    //void Hall14()
    //{
    //    if (Manager.m.level >= 14)
    //    {
    //        //Manager.m.effectSpeaker.click();
    //        Manager.m.setKamera(hall14Camera);
    //        Close();
    //    }
    //    else
    //    {
    //        Manager.m.effectSpeaker.error();
    //    }
    //}
    //void Hall15()
    //{
    //    if (Manager.m.level >= 15)
    //    {
    //        //Manager.m.effectSpeaker.click();
    //        Manager.m.setKamera(hall15Camera);
    //        Close();
    //    }
    //    else
    //    {
    //        Manager.m.effectSpeaker.error();
    //    }
    //}
}
