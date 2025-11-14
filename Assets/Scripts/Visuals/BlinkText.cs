using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BlinkText : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    string text;
    TextMeshProUGUI tmp;
    bool blinking = false;
    bool blinkOn = false;
    float currenttime;
    public float blinkSpeed;
    public string blinkText;
    public void Start()
    {
        text = this.gameObject.GetComponent<TextMeshProUGUI>().text;
        tmp = this.gameObject.GetComponent<TextMeshProUGUI>();
        currenttime = Time.time;
    }
    public void Update()
    {
        if (blinking)
        {
            if (blinkOn == true)
            {
                string newText = text + blinkText;
                tmp.text = newText;
            }
            else
            {
                tmp.text = text;
            }
        }
        else
        {
            tmp.text = text;
        }

        if (currenttime < Time.time)
        {
            currenttime = Time.time + (1f / blinkSpeed);
            if (blinkOn == false)
            {
                blinkOn = true;
            }
            else
            {
                blinkOn = false;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        blinking = true;
        blinkOn = true;
        currenttime = Time.time + (1f / blinkSpeed);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        blinking = false;
    }
    public void OnSelect(BaseEventData eventData)
    {
        
    }

}
