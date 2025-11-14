using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuickTimeEventDisplay : MonoBehaviour
{
    public QuickTimeEvent displaying;
    public TextMeshProUGUI description;
    public RawImage descriptiveImage;
    public RawImage timeBar;
    public TextMeshProUGUI timeTxt;
    public float time;
    public float startTime;
    public GameObject buffSymbol;
    public GameObject debuffSymbol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (displaying != null)
        {
            this.startTime = displaying.getStartTime();
            this.time = displaying.getDuration();
            this.description.text = displaying.getDescription();
            timeBar.transform.localScale = new Vector3(time / startTime, 1, 1);
            timeBar.transform.localPosition = new Vector3((1 - (time / startTime)) * timeBar.GetComponent<RectTransform>().rect.width * -0.5f, 0, 0);
            if (time % 60 >= 10) timeTxt.text = Mathf.Floor(time / 60) + ":" + Mathf.Floor(time % 60);
            else timeTxt.text = Mathf.Floor(time / 60) + ":0" + Mathf.Floor(time % 60);

            if (displaying.isPositiveEvent())
            {
                buffSymbol.SetActive(true);
                debuffSymbol.SetActive(false);
            }
            else
            {
                buffSymbol.SetActive(false);
                debuffSymbol.SetActive(true);
            }
            descriptiveImage.texture = displaying.getDisplayImage();
        }
    }
}
