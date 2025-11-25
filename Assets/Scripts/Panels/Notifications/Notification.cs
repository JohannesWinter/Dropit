using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    public RawImage notificationImage;
    public GameObject overlay;
    public float lifetime;
    public float elapsed { get; private set; }
    public bool enable;
    public int state;

    public Vector3 slideTarget;
    public float slideSpeed;
    public bool shown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (shown)
            {
                elapsed += Time.unscaledDeltaTime;
            }

            if (slideTarget != gameObject.GetComponent<RectTransform>().localPosition)
            {
                Vector3 direction = slideTarget - GetComponent<RectTransform>().localPosition;
                gameObject.transform.localPosition += direction * slideSpeed * Time.unscaledDeltaTime + direction.normalized * 10f * Time.unscaledDeltaTime;


                if (direction.magnitude < (slideTarget - GetComponent<RectTransform>().localPosition).magnitude)
                {
                    gameObject.GetComponent<RectTransform>().localPosition = slideTarget;
                }
            }
            if (notificationImage.texture == null)
            {
                notificationImage.enabled = false;
            }
            else
            {
                notificationImage.enabled = true;
            }
        }
        overlay.SetActive(enable);
    }
    public void Slide(Vector3 to, float speed)
    {
        slideTarget = to;
        slideSpeed = speed;
    }
    public void Slide(Vector3 to, float speed, Vector3 from)
    {
        slideSpeed = speed;
        slideTarget = to;
        gameObject.GetComponent<RectTransform>().localPosition = from;
    }
}
