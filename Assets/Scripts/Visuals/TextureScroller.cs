using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float speedX;
    public float speedY;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        
    }

    void Update()
    {
        float offsetX = Time.time * speedX;
        float offsetY = Time.time * speedY;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}

