using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public Image[] screenPictures;
    public List<Image> loadingImages;
    public Image loadingBar;
    public TextMeshProUGUI loadingMessage;
    public TextMeshProUGUI loadingProgress;

    private void Start()
    {
        for (int i = 0; i < loadingImages.Count; i++)
        {
            loadingImages[i].enabled = false;
        }
        for (int i = 0; i < screenPictures.Length; i++)
        {
            screenPictures[i].enabled = false;
        }
        loadingMessage.enabled = false;
        loadingProgress.enabled = false;
        loadingBar.enabled = false;
    }

    public void setStatus(int loadingScreen, float transparency, string message, float progressInPercent)
    {
        for (int i = 0; i < screenPictures.Length; i++)
        {
            if (loadingScreen == i && transparency != 0)
            {
                screenPictures[loadingScreen].enabled = true;
                Color screenColor = screenPictures[loadingScreen].color;
                screenPictures[loadingScreen].color = new Color(screenColor.r, screenColor.g, screenColor.b, transparency);
            }
            else
            {
                screenPictures[i].enabled = false;
            }
        }
        foreach (Image i in loadingImages)
        {
            if (transparency != 0)
            {
                i.enabled = true;
                Color iColor = i.color;
                i.color = new Color(iColor.r, iColor.g, iColor.b, transparency);
            }
            else
            {
                i.enabled = false;
            }
        }
        loadingMessage.text = message;
        loadingMessage.color = new Color(0, 1, 0, transparency);
        loadingProgress.color = new Color(0, 0, 0, transparency);
        loadingProgress.text = Mathf.Floor(progressInPercent * 10) / 10 + "%";
        if (transparency == 0)
        {
            loadingMessage.enabled = false;
            loadingProgress.enabled = false;
        }
        else
        {
            loadingProgress.enabled = true;
            loadingMessage.enabled = true;
        }

        loadingBar.transform.localScale = new Vector3(progressInPercent / 100, 1, 1);
        loadingBar.transform.localPosition = new Vector3((1 - (progressInPercent / 100)) * loadingBar.GetComponent<RectTransform>().rect.width * -0.5f, 0, 0);
    }
}
