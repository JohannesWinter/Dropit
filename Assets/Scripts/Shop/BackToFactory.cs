using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToFactory : MonoBehaviour
{
    public GameObject backButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton.SetActive(false);
        backButton.GetComponent<Button>().onClick.AddListener(Back);
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.m.inShopDropper == true || Manager.m.inShopMachine == true)
        {
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(false);
        }
        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("ExitShop")))
        {
            Back();
        }
    }

    public void Back()
    {
        if (Manager.m.tutorial.IsButtonAllowed(backButton) == false)
        {
            return;
        }
        Manager.m.effectSpeaker.click();
        Manager.m.setKamera(Manager.m.lastDropperCamera);
        Manager.m.inShopDropper = false;
        Manager.m.inShopMachine = false;
    }
}
