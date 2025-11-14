using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public GameObject buyButton;
    // Start is called before the first frame update
    //GameObject.Find("Shop Button").GetComponent<Renderer>().enabled = false;
    //GameObject.Find("Shop Button").GetComponent<SphereCollider>().enabled = false;
    //GameObject.Find("S").GetComponent<Renderer>().enabled = false;
    //GameObject.Find("Inpress").GetComponent<Renderer>().enabled = false;
    //GameObject.Find("Inpress").GetComponent<BoxCollider>().enabled = false;
    void Start()
    {
        buyButton.SetActive(false);
        buyButton.GetComponent<Button>().onClick.AddListener(Buy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.inShopDropper == true || Manager.m.inShopMachine)
        {
            buyButton.SetActive(true);

            if (GameInputManager.GetKeyDown(Manager.m.ActionKey("Buy")))
            {
                Buy();
            }
        }
        else
        {
            buyButton.SetActive(false);
        }
    }
    public void Buy()
    {
        if (Manager.m.inShopDropper)
        {
            if (Manager.m.upgradeRessources[Manager.m.dropperNumber - 1] == true)
            {
                Manager.m.effectSpeaker.click();
                Manager.m.editMode_placeDropper = true;
                Manager.m.setKamera(Manager.m.lastDropperCamera);
                Manager.m.inShopDropper = false;
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
        if (Manager.m.inShopMachine)
        {
            if (Manager.m.upgradeRessources[Manager.m.machineNumber - 1] ==  true || Manager.m.machineNumber <= 2)
            {
                Manager.m.effectSpeaker.click();
                Manager.m.editMode_placeMachine = true;
                Manager.m.setKamera(Manager.m.lastDropperCamera);
                Manager.m.inShopMachine = false;
            }
            else
            {
                Manager.m.effectSpeaker.error();
            }
        }
    }
}
