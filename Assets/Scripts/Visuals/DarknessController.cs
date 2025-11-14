using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarknessController : MonoBehaviour
{
    public Material darknessMat;
    public float radius = 0.05f;
    public float alphaRadius;
    public float changeSpeed;
    public GameObject darknessOverlay;
    public GameObject lightOverlay;
    public float changing; //0->total,1000->invisible

    void Update()
    {
        if ((Manager.m.qTEBrokenLights || Manager.m.qTEUltimateWipeout))
        {
            if (Manager.m.inMainMenu == false && Manager.m.inFinalSequence == false && Manager.m.inShopDropper == false && Manager.m.inShopMachine == false)
            {
                if (changing > 0)
                {
                    changing -= changeSpeed * Time.unscaledDeltaTime;
                    if (changing < 0)
                    {
                        changing = 0;
                    }
                }
                darknessOverlay.SetActive(true);
                lightOverlay.SetActive(true);

                lightOverlay.GetComponent<Image>().color = new Color(1, 0, 0, (200-changing) / 2000f);
                Vector2 mouseUV = new Vector2(
                    Input.mousePosition.x / Screen.width,
                    Input.mousePosition.y / Screen.height
                );

                darknessMat.SetVector("_MousePos", new Vector4(mouseUV.x, mouseUV.y, 0, 0));
                darknessMat.SetFloat("_AlphaRadius", alphaRadius + changing / 500);
                darknessMat.SetFloat("_Radius", radius + changing / 500);

                if (Manager.m.inUIMenu())
                {
                    darknessMat.SetVector("_MousePos", new Vector4(-100, -100, 0, 0));
                }
            }
            else
            {
                darknessOverlay.SetActive(false);
                lightOverlay.SetActive(false);
            }
        }
        else
        {
            if (changing >= 1000 || Manager.m.loading || Manager.m.inMainMenu)
            {
                darknessOverlay.SetActive(false);
                lightOverlay.SetActive(false);
                changing = 1000;
            }
            else
            {
                changing += changeSpeed * Time.unscaledDeltaTime;

                Vector2 mouseUV = new Vector2(
                    Input.mousePosition.x / Screen.width,
                    Input.mousePosition.y / Screen.height
                );
                darknessMat.SetVector("_MousePos", new Vector4(mouseUV.x, mouseUV.y, 0, 0));
                darknessMat.SetFloat("_AlphaRadius", alphaRadius + changing / 500);
                darknessMat.SetFloat("_Radius", radius + changing / 500);
                lightOverlay.GetComponent<Image>().color = new Color(1, 0, 0, (200 - changing) / 2000f);
            }
        }
    }
}
