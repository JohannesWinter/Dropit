using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicManager : MonoBehaviour
{
    GraphicQuality generalQuality = GraphicQuality.Low;
    GraphicQuality textureQuality = GraphicQuality.Low;
    GraphicQuality resolutionQuality = GraphicQuality.Low;
    GraphicQuality antiAliasing = GraphicQuality.Off;
    GraphicQuality vSync = GraphicQuality.Off;
    GraphicQuality lightQuality = GraphicQuality.Low;
    GraphicQuality shadowQuality = GraphicQuality.Off;
    GraphicQuality reflectionQuality = GraphicQuality.Off;
    GraphicQuality enableHalos = GraphicQuality.Off;
    GraphicQuality gUIScale = GraphicQuality.Medium;



    public Button generalQualityIncreaseButton;
    public Button generalQualityDecreaseButton;
    public Button textureQualityIncreaseButton;
    public Button textureQualityDecreaseButton;
    public Button resolutionQualityIncreaseButton;
    public Button resolutionQualityDecreaseButton;
    public Button antiAliasingIncreaseButton;
    public Button antiAliasingDecreaseButton;
    public Button vSyncIncreaseButton;
    public Button vSyncDecreaseButton;
    public Button lightQualityIncreaseButton;
    public Button lightQualityDecreaseButton;
    public Button shadowQualityIncreaseButton;
    public Button shadowQualityDecreaseButton;
    public Button reflectionQualityIncreaseButton;
    public Button reflectionQualityDecreaseButton;
    public Button enableHalosIncreaseButton;
    public Button enableHalosDecreaseButton;
    public Button gUIScaleIncreaseButton;
    public Button gUIScaleDecreaseButton;

    public TMP_Text generalQualityText;
    public TMP_Text textureQualityText;
    public TMP_Text resolutionQualityText;
    public TMP_Text antiAliasingText;
    public TMP_Text vSyncText;
    public TMP_Text lightQualityText;
    public TMP_Text shadowQualityText;
    public TMP_Text reflectionQualityText;
    public TMP_Text enableHalosText;
    public TMP_Text gUIScaleText;

    RawImage generalQualityIncreaseImage;
    RawImage generalQualityDecreaseImage;
    RawImage textureQualityIncreaseImage;
    RawImage textureQualityDecreaseImage;
    RawImage resolutionQualityIncreaseImage;
    RawImage resolutionQualityDecreaseImage;
    RawImage antiAliasingIncreaseImage;
    RawImage antiAliasingDecreaseImage;
    RawImage vSyncIncreaseImage;
    RawImage vSyncDecreaseImage;
    RawImage lightQualityIncreaseImage;
    RawImage lightQualityDecreaseImage;
    RawImage shadowQualityIncreaseImage;
    RawImage shadowQualityDecreaseImage;
    RawImage reflectionQualityIncreaseImage;
    RawImage reflectionQualityDecreaseImage;
    RawImage enableHalosIncreaseImage;
    RawImage enableHalosDecreaseImage;
    RawImage gUIScaleIncreaseImage;
    RawImage gUIScaleDecreaseImage;

    Material[] allGameMaterials;

    public float gUIScaleFactor;
    public GraphicQuality enableHaloState;

    void Start()
    {
        string key = Manager.m.version + "_Graphics_";

        // Werte aus PlayerPrefs laden, sonst auf Minimalwert setzen
        generalQuality = LoadQualityFromPrefs(key + "GeneralQuality", GraphicQuality.Low);
        textureQuality = LoadQualityFromPrefs(key + "TextureQuality", GraphicQuality.Low);
        resolutionQuality = LoadQualityFromPrefs(key + "ResolutionQuality", GraphicQuality.Low);
        antiAliasing = LoadQualityFromPrefs(key + "AntiAliasing", GraphicQuality.Off);
        vSync = LoadQualityFromPrefs(key + "VSync", GraphicQuality.Off);
        lightQuality = LoadQualityFromPrefs(key + "LightQuality", GraphicQuality.Low);
        shadowQuality = LoadQualityFromPrefs(key + "ShadowQuality", GraphicQuality.Off);
        reflectionQuality = LoadQualityFromPrefs(key + "ReflectionQuality", GraphicQuality.Off);
        enableHalos = LoadQualityFromPrefs(key + "EnableHalos", GraphicQuality.Off);
        gUIScale = LoadQualityFromPrefs(key + "GUIScale", GraphicQuality.Medium);

        generalQualityIncreaseButton.onClick.AddListener(IncreaseGeneralQuality);
        generalQualityDecreaseButton.onClick.AddListener(DecreaseGeneralQuality);

        textureQualityIncreaseButton.onClick.AddListener(IncreaseTextureQuality);
        textureQualityDecreaseButton.onClick.AddListener(DecreaseTextureQuality);

        resolutionQualityIncreaseButton.onClick.AddListener(IncreaseResolutionQuality);
        resolutionQualityDecreaseButton.onClick.AddListener(DecreaseResolutionQuality);

        antiAliasingIncreaseButton.onClick.AddListener(IncreaseAntiAliasing);
        antiAliasingDecreaseButton.onClick.AddListener(DecreaseAntiAliasing);

        vSyncIncreaseButton.onClick.AddListener(IncreaseVSync);
        vSyncDecreaseButton.onClick.AddListener(DecreaseVSync);

        lightQualityIncreaseButton.onClick.AddListener(IncreaseLightQuality);
        lightQualityDecreaseButton.onClick.AddListener(DecreaseLightQuality);

        shadowQualityIncreaseButton.onClick.AddListener(IncreaseShadowQuality);
        shadowQualityDecreaseButton.onClick.AddListener(DecreaseShadowQuality);

        reflectionQualityIncreaseButton.onClick.AddListener(IncreaseReflectionQuality);
        reflectionQualityDecreaseButton.onClick.AddListener(DecreaseReflectionQuality);

        enableHalosIncreaseButton.onClick.AddListener(IncreaseEnableHalos);
        enableHalosDecreaseButton.onClick.AddListener(DecreaseEnableHalos);

        gUIScaleIncreaseButton.onClick.AddListener(IncreaseGUIScale);
        gUIScaleDecreaseButton.onClick.AddListener(DecreaseGUIScale);

        generalQualityIncreaseImage = generalQualityIncreaseButton.GetComponent<RawImage>();
        generalQualityDecreaseImage = generalQualityDecreaseButton.GetComponent<RawImage>();
        textureQualityIncreaseImage = textureQualityIncreaseButton.GetComponent<RawImage>();
        textureQualityDecreaseImage = textureQualityDecreaseButton.GetComponent<RawImage>();
        resolutionQualityIncreaseImage = resolutionQualityIncreaseButton.GetComponent<RawImage>();
        resolutionQualityDecreaseImage = resolutionQualityDecreaseButton.GetComponent<RawImage>();
        antiAliasingIncreaseImage = antiAliasingIncreaseButton.GetComponent<RawImage>();
        antiAliasingDecreaseImage = antiAliasingDecreaseButton.GetComponent<RawImage>();
        vSyncIncreaseImage = vSyncIncreaseButton.GetComponent<RawImage>();
        vSyncDecreaseImage = vSyncDecreaseButton.GetComponent<RawImage>();
        lightQualityIncreaseImage = lightQualityIncreaseButton.GetComponent<RawImage>();
        lightQualityDecreaseImage = lightQualityDecreaseButton.GetComponent<RawImage>();
        shadowQualityIncreaseImage = shadowQualityIncreaseButton.GetComponent<RawImage>();
        shadowQualityDecreaseImage = shadowQualityDecreaseButton.GetComponent<RawImage>();
        reflectionQualityIncreaseImage = reflectionQualityIncreaseButton.GetComponent<RawImage>();
        reflectionQualityDecreaseImage = reflectionQualityDecreaseButton.GetComponent<RawImage>();
        enableHalosIncreaseImage = enableHalosIncreaseButton.GetComponent<RawImage>();
        enableHalosDecreaseImage = enableHalosDecreaseButton.GetComponent<RawImage>();
        gUIScaleIncreaseImage = gUIScaleIncreaseButton.GetComponent<RawImage>();
        gUIScaleDecreaseImage = gUIScaleDecreaseButton.GetComponent<RawImage>();


        SetGUIScale(gUIScale);
        SetTextureQuality(textureQuality);
        SetShadowQuality(shadowQuality);
        SetLightQuality(lightQuality);
        SetReflectionQuality(reflectionQuality);
        SetResolutionQuality(resolutionQuality);
        SetVSync(vSync);
        SetAntiAliasing(antiAliasing);
        SetEnableHalos(enableHalos);
    }

    // Update is called once per frame
    private void Update()
    {
        enableHaloState = enableHalos;
        bool changed = false;
        if (!IsValidGeneralQuality(generalQuality))
        {
            generalQuality = GraphicQuality.Low;
            changed = true;
        }
        if (!IsValidTextureQuality(textureQuality))
        {
            textureQuality = GraphicQuality.Low;
            changed = true;
        }
        if (!IsValidResolutionQuality(resolutionQuality))
        {
            resolutionQuality = GraphicQuality.Low;
            changed = true;
        }
        if (!IsValidAntiAliasing(antiAliasing))
        {
            antiAliasing = GraphicQuality.Off;
            changed = true;
        }
        if (!IsValidVSync(vSync))
        {
            vSync = GraphicQuality.Off;
            changed = true;
        }
        if (!IsValidLightQuality(lightQuality))
        {
            lightQuality = GraphicQuality.Low;
            changed = true;
        }
        if (!IsValidShadowQuality(shadowQuality))
        {
            shadowQuality = GraphicQuality.Off;
            changed = true;
        }
        if (!IsValidReflectionQuality(reflectionQuality))
        {
            reflectionQuality = GraphicQuality.Off;
            changed = true;
        }
        if (!IsValidEnableHalos(enableHalos))
        {
            enableHalos = GraphicQuality.Off;
            changed = true;
        }
        if (!IsValidGUIScale(gUIScale))
        {
            gUIScale = GraphicQuality.Small;
            changed = true;
        }
        if (changed)
        {
            //OnQualityChanged();
        }

        UpdateAllButtonStates();
    }
    private bool IsValidGeneralQuality(GraphicQuality q) =>
        q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High || q == GraphicQuality.Ultra || q == GraphicQuality.Custom;

    private bool IsValidTextureQuality(GraphicQuality q) =>
        q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High || q == GraphicQuality.Ultra;

    private bool IsValidResolutionQuality(GraphicQuality q) =>
        q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High;

    private bool IsValidAntiAliasing(GraphicQuality q) =>
        q == GraphicQuality.Off || q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High;

    private bool IsValidVSync(GraphicQuality q) =>
        q == GraphicQuality.Off || q == GraphicQuality.Low || q == GraphicQuality.High;

    private bool IsValidLightQuality(GraphicQuality q) =>
        q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High || q == GraphicQuality.Ultra;

    private bool IsValidShadowQuality(GraphicQuality q) =>
        q == GraphicQuality.Off || q == GraphicQuality.Low || q == GraphicQuality.Medium || q == GraphicQuality.High;

    private bool IsValidReflectionQuality(GraphicQuality q) =>
        q == GraphicQuality.Off || q == GraphicQuality.On;

    private bool IsValidEnableHalos(GraphicQuality q) =>
        q == GraphicQuality.Off || q == GraphicQuality.Some || q == GraphicQuality.All;

    private bool IsValidGUIScale(GraphicQuality q) =>
        q == GraphicQuality.Small || q == GraphicQuality.Medium || q == GraphicQuality.Large;

    public void IncreaseGeneralQuality()
    {
        switch (generalQuality)
        {
            case GraphicQuality.Low: generalQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: generalQuality = GraphicQuality.High; break;
            case GraphicQuality.High: generalQuality = GraphicQuality.Ultra; break;
            case GraphicQuality.Custom: generalQuality = GraphicQuality.Ultra; break;
            case GraphicQuality.Ultra: break; // max
        }

        Manager.m.effectSpeaker.click();
        SetGeneralQuality(generalQuality);
        UpdateButtonState(generalQuality, generalQualityIncreaseButton, generalQualityIncreaseImage, GraphicQuality.Ultra, generalQualityDecreaseButton, generalQualityDecreaseImage, GraphicQuality.Low);
        ApplyGeneralQualityToSubValues();
        saveGraphicSettings();
    }

    public void DecreaseGeneralQuality()
    {
        switch (generalQuality)
        {
            case GraphicQuality.Ultra: generalQuality = GraphicQuality.High; break;
            case GraphicQuality.High: generalQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: generalQuality = GraphicQuality.Low; break;
            case GraphicQuality.Custom: generalQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: break; // min
        }

        Manager.m.effectSpeaker.click();
        SetGeneralQuality(generalQuality);
        UpdateButtonState(generalQuality, generalQualityIncreaseButton, generalQualityIncreaseImage, GraphicQuality.Ultra, generalQualityDecreaseButton, generalQualityDecreaseImage, GraphicQuality.Low);
        ApplyGeneralQualityToSubValues();
        saveGraphicSettings();
    }

    // --- TextureQuality ---
    public void IncreaseTextureQuality()
    {
        switch (textureQuality)
        {
            case GraphicQuality.Low: textureQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: textureQuality = GraphicQuality.High; break;
            case GraphicQuality.High: textureQuality = GraphicQuality.Ultra; break;
            case GraphicQuality.Ultra: break;
        }
        Manager.m.effectSpeaker.click();
        SetTextureQuality(textureQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(textureQuality, textureQualityIncreaseButton, textureQualityIncreaseImage, GraphicQuality.Ultra, textureQualityDecreaseButton, textureQualityDecreaseImage, GraphicQuality.Low);
    }

    public void DecreaseTextureQuality()
    {
        switch (textureQuality)
        {
            case GraphicQuality.Ultra: textureQuality = GraphicQuality.High; break;
            case GraphicQuality.High: textureQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: textureQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: break;
        }
        Manager.m.effectSpeaker.click();
        SetTextureQuality(textureQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(textureQuality, textureQualityIncreaseButton, textureQualityIncreaseImage, GraphicQuality.Ultra, textureQualityDecreaseButton, textureQualityDecreaseImage, GraphicQuality.Low);
    }

    // --- ResolutionQuality ---
    public void IncreaseResolutionQuality()
    {
        switch (resolutionQuality)
        {
            case GraphicQuality.Low: resolutionQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: resolutionQuality = GraphicQuality.High; break;
            case GraphicQuality.High: break;
        }
        Manager.m.effectSpeaker.click();
        SetResolutionQuality(resolutionQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(resolutionQuality, resolutionQualityIncreaseButton, resolutionQualityIncreaseImage, GraphicQuality.High, resolutionQualityDecreaseButton, resolutionQualityDecreaseImage, GraphicQuality.Low);
    }

    public void DecreaseResolutionQuality()
    {
        switch (resolutionQuality)
        {
            case GraphicQuality.High: resolutionQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: resolutionQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: break;
        }
        Manager.m.effectSpeaker.click();
        SetResolutionQuality(resolutionQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(resolutionQuality, resolutionQualityIncreaseButton, resolutionQualityIncreaseImage, GraphicQuality.High, resolutionQualityDecreaseButton, resolutionQualityDecreaseImage, GraphicQuality.Low);
    }

    // --- AntiAliasing ---
    public void IncreaseAntiAliasing()
    {
        switch (antiAliasing)
        {
            case GraphicQuality.Off: antiAliasing = GraphicQuality.Low; break;
            case GraphicQuality.Low: antiAliasing = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: antiAliasing = GraphicQuality.High; break;
            case GraphicQuality.High: break;
        }
        Manager.m.effectSpeaker.click();
        SetAntiAliasing(antiAliasing);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(antiAliasing, antiAliasingIncreaseButton, antiAliasingIncreaseImage, GraphicQuality.High, antiAliasingDecreaseButton, antiAliasingDecreaseImage, GraphicQuality.Off);
    }

    public void DecreaseAntiAliasing()
    {
        switch (antiAliasing)
        {
            case GraphicQuality.High: antiAliasing = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: antiAliasing = GraphicQuality.Low; break;
            case GraphicQuality.Low: antiAliasing = GraphicQuality.Off; break;
            case GraphicQuality.Off: break;
        }
        Manager.m.effectSpeaker.click();
        SetAntiAliasing(antiAliasing);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(antiAliasing, antiAliasingIncreaseButton, antiAliasingIncreaseImage, GraphicQuality.High, antiAliasingDecreaseButton, antiAliasingDecreaseImage, GraphicQuality.Off);
    }

    // --- vSync ---
    public void IncreaseVSync()
    {
        switch (vSync)
        {
            case GraphicQuality.Off: vSync = GraphicQuality.Low; break;
            case GraphicQuality.Low: vSync = GraphicQuality.High; break;
            case GraphicQuality.High: break;
        }
        Manager.m.effectSpeaker.click();
        SetVSync(vSync);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(vSync, vSyncIncreaseButton, vSyncIncreaseImage, GraphicQuality.High, vSyncDecreaseButton, vSyncDecreaseImage, GraphicQuality.Off);
    }

    public void DecreaseVSync()
    {
        switch (vSync)
        {
            case GraphicQuality.High: vSync = GraphicQuality.Low; break;
            case GraphicQuality.Low: vSync = GraphicQuality.Off; break;
            case GraphicQuality.Off: break;
        }
        Manager.m.effectSpeaker.click();
        SetVSync(vSync);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(vSync, vSyncIncreaseButton, vSyncIncreaseImage, GraphicQuality.High, vSyncDecreaseButton, vSyncDecreaseImage, GraphicQuality.Off);
    }

    // --- LightQuality ---
    public void IncreaseLightQuality()
    {
        switch (lightQuality)
        {
            case GraphicQuality.Low: lightQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: lightQuality = GraphicQuality.High; break;
            case GraphicQuality.High: lightQuality = GraphicQuality.Ultra; break;
            case GraphicQuality.Ultra: break;
        }
        Manager.m.effectSpeaker.click();
        SetLightQuality(lightQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(lightQuality, lightQualityIncreaseButton, lightQualityIncreaseImage, GraphicQuality.Ultra, lightQualityDecreaseButton, lightQualityDecreaseImage, GraphicQuality.Low);
    }

    public void DecreaseLightQuality()
    {
        switch (lightQuality)
        {
            case GraphicQuality.Ultra: lightQuality = GraphicQuality.High; break;
            case GraphicQuality.High: lightQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: lightQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: break;
        }
        Manager.m.effectSpeaker.click();
        SetLightQuality(lightQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(lightQuality, lightQualityIncreaseButton, lightQualityIncreaseImage, GraphicQuality.Ultra, lightQualityDecreaseButton, lightQualityDecreaseImage, GraphicQuality.Low);
    }

    // --- ShadowQuality ---
    public void IncreaseShadowQuality()
    {
        switch (shadowQuality)
        {
            case GraphicQuality.Off: shadowQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: shadowQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: shadowQuality = GraphicQuality.High; break;
            case GraphicQuality.High: break;
        }
        Manager.m.effectSpeaker.click();
        SetShadowQuality(shadowQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(shadowQuality, shadowQualityIncreaseButton, shadowQualityIncreaseImage, GraphicQuality.High, shadowQualityDecreaseButton, shadowQualityDecreaseImage, GraphicQuality.Off);
    }

    public void DecreaseShadowQuality()
    {
        switch (shadowQuality)
        {
            case GraphicQuality.High: shadowQuality = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: shadowQuality = GraphicQuality.Low; break;
            case GraphicQuality.Low: shadowQuality = GraphicQuality.Off; break;
            case GraphicQuality.Off: break;
        }
        Manager.m.effectSpeaker.click();
        SetShadowQuality(shadowQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(shadowQuality, shadowQualityIncreaseButton, shadowQualityIncreaseImage, GraphicQuality.High, shadowQualityDecreaseButton, shadowQualityDecreaseImage, GraphicQuality.Off);
    }

    // --- Reflections ---
    public void IncreaseReflectionQuality()
    {
        if (reflectionQuality == GraphicQuality.Off) reflectionQuality = GraphicQuality.On;
        Manager.m.effectSpeaker.click();
        SetReflectionQuality(reflectionQuality);
        UpdateGeneralQualityBasedOnSubValues();
        UpdateButtonState(reflectionQuality, reflectionQualityIncreaseButton, reflectionQualityIncreaseImage, GraphicQuality.On, reflectionQualityDecreaseButton, reflectionQualityDecreaseImage, GraphicQuality.Off);
    }

    public void DecreaseReflectionQuality()
    {
        if (reflectionQuality == GraphicQuality.On) reflectionQuality = GraphicQuality.Off;
        Manager.m.effectSpeaker.click();
        SetReflectionQuality(reflectionQuality);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(reflectionQuality, reflectionQualityIncreaseButton, reflectionQualityIncreaseImage, GraphicQuality.On, reflectionQualityDecreaseButton, reflectionQualityDecreaseImage, GraphicQuality.Off);
    }

    // --- EnableHalos ---
    public void IncreaseEnableHalos()
    {
        switch (enableHalos)
        {
            case GraphicQuality.Off: enableHalos = GraphicQuality.Some; break;
            case GraphicQuality.Some: enableHalos = GraphicQuality.All; break;
            case GraphicQuality.All: break;
        }
        Manager.m.effectSpeaker.click();
        SetEnableHalos(enableHalos);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(enableHalos, enableHalosIncreaseButton, enableHalosIncreaseImage, GraphicQuality.All, enableHalosDecreaseButton, enableHalosDecreaseImage, GraphicQuality.Off);
    }

    public void DecreaseEnableHalos()
    {
        switch (enableHalos)
        {
            case GraphicQuality.All: enableHalos = GraphicQuality.Some; break;
            case GraphicQuality.Some: enableHalos = GraphicQuality.Off; break;
            case GraphicQuality.Off: break;
        }
        Manager.m.effectSpeaker.click();
        SetEnableHalos(enableHalos);
        UpdateGeneralQualityBasedOnSubValues();
        saveGraphicSettings();
        UpdateButtonState(enableHalos, enableHalosIncreaseButton, enableHalosIncreaseImage, GraphicQuality.All, enableHalosDecreaseButton, enableHalosDecreaseImage, GraphicQuality.Off);
    }

    // --- GUI Scale ---
    public void IncreaseGUIScale()
    {
        switch (gUIScale)
        {
            case GraphicQuality.Small: gUIScale = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: gUIScale = GraphicQuality.Large; break;
            case GraphicQuality.Large: break;
        }
        Manager.m.effectSpeaker.click();
        SetGUIScale(gUIScale);
        saveGraphicSettings();
        UpdateButtonState(gUIScale, gUIScaleIncreaseButton, gUIScaleIncreaseImage, GraphicQuality.Large, gUIScaleDecreaseButton, gUIScaleDecreaseImage, GraphicQuality.Small);
    }

    public void DecreaseGUIScale()
    {
        switch (gUIScale)
        {
            case GraphicQuality.Large: gUIScale = GraphicQuality.Medium; break;
            case GraphicQuality.Medium: gUIScale = GraphicQuality.Small; break;
            case GraphicQuality.Small: break;
        }
        Manager.m.effectSpeaker.click();
        SetGUIScale(gUIScale);
        saveGraphicSettings();
        UpdateButtonState(gUIScale, gUIScaleIncreaseButton, gUIScaleIncreaseImage, GraphicQuality.Large, gUIScaleDecreaseButton, gUIScaleDecreaseImage, GraphicQuality.Small);
    }

    private void UpdateButtonState(GraphicQuality current, Button increaseBtn, RawImage increaseImg, GraphicQuality max, Button decreaseBtn, RawImage decreaseImg, GraphicQuality min)
    {
        if (current == max)
        {
            increaseBtn.interactable = false;
            increaseImg.color = new Color(69/255f, 154 / 255f, 69/255f, 1);
        }
        else
        {
            increaseBtn.interactable = true;
            increaseImg.color = Color.green;
        }

        if (current == min)
        {
            decreaseBtn.interactable = false;
            decreaseImg.color = new Color(69 / 255f, 154 / 255f, 69 / 255f, 1);
        }
        else
        {
            decreaseBtn.interactable = true;
            decreaseImg.color = Color.green;
        }
        UpdateQualityText();
    }

    private void UpdateQualityText()
    {
        generalQualityText.text = GraphicQualityToString(generalQuality);
        textureQualityText.text = GraphicQualityToString(textureQuality);
        resolutionQualityText.text = GraphicQualityToString(resolutionQuality);
        antiAliasingText.text = GraphicQualityToString(antiAliasing);
        vSyncText.text = GraphicQualityToString(vSync);
        lightQualityText.text = GraphicQualityToString(lightQuality);
        shadowQualityText.text = GraphicQualityToString(shadowQuality);
        reflectionQualityText.text = GraphicQualityToString(reflectionQuality);
        enableHalosText.text = GraphicQualityToString(enableHalos);
        gUIScaleText.text = GraphicQualityToString(gUIScale);
    }

    private void UpdateAllButtonStates()
    {
        UpdateButtonState(generalQuality, generalQualityIncreaseButton, generalQualityIncreaseImage, GraphicQuality.Ultra, generalQualityDecreaseButton, generalQualityDecreaseImage, GraphicQuality.Low);
        UpdateButtonState(textureQuality, textureQualityIncreaseButton, textureQualityIncreaseImage, GraphicQuality.Ultra, textureQualityDecreaseButton, textureQualityDecreaseImage, GraphicQuality.Low);
        UpdateButtonState(resolutionQuality, resolutionQualityIncreaseButton, resolutionQualityIncreaseImage, GraphicQuality.High, resolutionQualityDecreaseButton, resolutionQualityDecreaseImage, GraphicQuality.Low);
        UpdateButtonState(antiAliasing, antiAliasingIncreaseButton, antiAliasingIncreaseImage, GraphicQuality.High, antiAliasingDecreaseButton, antiAliasingDecreaseImage, GraphicQuality.Off);
        UpdateButtonState(vSync, vSyncIncreaseButton, vSyncIncreaseImage, GraphicQuality.High, vSyncDecreaseButton, vSyncDecreaseImage, GraphicQuality.Off);
        UpdateButtonState(lightQuality, lightQualityIncreaseButton, lightQualityIncreaseImage, GraphicQuality.Ultra, lightQualityDecreaseButton, lightQualityDecreaseImage, GraphicQuality.Low);
        UpdateButtonState(shadowQuality, shadowQualityIncreaseButton, shadowQualityIncreaseImage, GraphicQuality.High, shadowQualityDecreaseButton, shadowQualityDecreaseImage, GraphicQuality.Off);
        UpdateButtonState(reflectionQuality, reflectionQualityIncreaseButton, reflectionQualityIncreaseImage, GraphicQuality.On, reflectionQualityDecreaseButton, reflectionQualityDecreaseImage, GraphicQuality.Off);
        UpdateButtonState(enableHalos, enableHalosIncreaseButton, enableHalosIncreaseImage, GraphicQuality.All, enableHalosDecreaseButton, enableHalosDecreaseImage, GraphicQuality.Off);
        UpdateButtonState(gUIScale, gUIScaleIncreaseButton, gUIScaleIncreaseImage, GraphicQuality.Large, gUIScaleDecreaseButton, gUIScaleDecreaseImage, GraphicQuality.Small);
    }

    private void SetGeneralQuality(GraphicQuality q) { }
    private void SetTextureQuality(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Low: 
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                QualitySettings.particleRaycastBudget = 64;
                QualitySettings.streamingMipmapsActive = true;
                QualitySettings.streamingMipmapsAddAllCameras = true;
                QualitySettings.streamingMipmapsMemoryBudget = 256;
                QualitySettings.streamingMipmapsRenderersPerFrame = 256;
                QualitySettings.streamingMipmapsMaxLevelReduction = 2;
                QualitySettings.streamingMipmapsMaxFileIORequests = 512;
                QualitySettings.softParticles = false;
                break;
            case GraphicQuality.Medium: 
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                QualitySettings.particleRaycastBudget = 256;
                QualitySettings.streamingMipmapsActive = true;
                QualitySettings.streamingMipmapsAddAllCameras = true;
                QualitySettings.streamingMipmapsMemoryBudget = 512;
                QualitySettings.streamingMipmapsRenderersPerFrame = 512;
                QualitySettings.streamingMipmapsMaxLevelReduction = 1;
                QualitySettings.streamingMipmapsMaxFileIORequests = 1024;
                QualitySettings.softParticles = false;
                break;
            case GraphicQuality.High: 
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable; 
                QualitySettings.particleRaycastBudget = 512;
                QualitySettings.streamingMipmapsActive = false;
                QualitySettings.softParticles = false;
                break;
            case GraphicQuality.Ultra:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                QualitySettings.particleRaycastBudget = 1024;
                QualitySettings.streamingMipmapsActive = false;
                QualitySettings.softParticles = true;
                break;
        }
    }
    private void SetResolutionQuality(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Low: QualitySettings.resolutionScalingFixedDPIFactor = 0.5f; break;
            case GraphicQuality.Medium: QualitySettings.resolutionScalingFixedDPIFactor = 0.8f; break;
            case GraphicQuality.High: QualitySettings.resolutionScalingFixedDPIFactor = 1f; break;
        }
    }
    private void SetAntiAliasing(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Off: QualitySettings.antiAliasing = 0; break;
            case GraphicQuality.Low: QualitySettings.antiAliasing = 2; break;
            case GraphicQuality.Medium: QualitySettings.antiAliasing = 4; break;
            case GraphicQuality.High: QualitySettings.antiAliasing = 8; break;
        }
    }
    private void SetVSync(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Off: QualitySettings.vSyncCount = 0; break;
            case GraphicQuality.Low: QualitySettings.vSyncCount = 2; break;
            case GraphicQuality.High: QualitySettings.vSyncCount = 1; break;
        }
    }
    private void SetLightQuality(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Low: QualitySettings.pixelLightCount = 2; break;
            case GraphicQuality.Medium: QualitySettings.pixelLightCount = 4; break;
            case GraphicQuality.High: QualitySettings.pixelLightCount = 8; break;
            case GraphicQuality.Ultra: QualitySettings.pixelLightCount = 16; break;
        }
    }
    private void SetShadowQuality(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Off:
                QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.shadowResolution = ShadowResolution.Low;
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
                QualitySettings.shadowDistance = 20;
                QualitySettings.shadowNearPlaneOffset = 3;
                QualitySettings.shadowCascades = 0;
                break;

            case GraphicQuality.Low:
                QualitySettings.shadowmaskMode = ShadowmaskMode.Shadowmask;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadowProjection = ShadowProjection.StableFit;
                QualitySettings.shadowDistance = 50;
                QualitySettings.shadowNearPlaneOffset = 2.5f;
                QualitySettings.shadowCascades = 0;
                break;

            case GraphicQuality.Medium:
                QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                QualitySettings.shadowDistance = 120;
                QualitySettings.shadowNearPlaneOffset = 2.0f;
                QualitySettings.shadowCascades = 2;
                break;

            case GraphicQuality.High:
                QualitySettings.shadowmaskMode = ShadowmaskMode.DistanceShadowmask;
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                QualitySettings.shadowDistance = 240;
                QualitySettings.shadowNearPlaneOffset = 1.5f;
                QualitySettings.shadowCascades = 4;
                break;
        }
    }
    private void SetReflectionQuality(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Off: QualitySettings.realtimeReflectionProbes = false; break;
            case GraphicQuality.On: QualitySettings.realtimeReflectionProbes = true; break;
        }
    }
    private void SetEnableHalos(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Off: break;
            case GraphicQuality.Some: break;
            case GraphicQuality.All: break;
        }
    }
    private void SetGUIScale(GraphicQuality q) 
    {
        switch (q)
        {
            case GraphicQuality.Small: gUIScaleFactor = 0.7f; break;
            case GraphicQuality.Medium: gUIScaleFactor = 1f; break;
            case GraphicQuality.Large: gUIScaleFactor = 1.2f; break;
        }
    }

    public string GraphicQualityToString(GraphicQuality g)
    {
        switch (g)
        {
            case GraphicQuality.Low: return "Low";
            case GraphicQuality.Medium: return "Medium";
            case GraphicQuality.High: return "High";
            case GraphicQuality.Ultra: return "Ultra";
            case GraphicQuality.On: return "On";
            case GraphicQuality.Off: return "Off";
            case GraphicQuality.Some: return "Some";
            case GraphicQuality.All: return "All";
            case GraphicQuality.Small: return "Small";
            case GraphicQuality.Large: return "Large";
            case GraphicQuality.Custom: return "Custom";
            default: return "Error";
        }
    }

    public GraphicQuality GraphicQualityFromString(string s)
    {
        switch (s)
        {
            case "Low": return GraphicQuality.Low;
            case "Medium": return GraphicQuality.Medium;
            case "High": return GraphicQuality.High;
            case "Ultra": return GraphicQuality.Ultra;
            case "On": return GraphicQuality.On;
            case "Off": return GraphicQuality.Off;
            case "Some": return GraphicQuality.Some;
            case "All": return GraphicQuality.All;
            case "Small": return GraphicQuality.Small;
            case "Large": return GraphicQuality.Large;
            case "Custom": return GraphicQuality.Custom;
            default:
                Debug.LogWarning($"Unrecognized GraphicQuality string: {s}");
                return GraphicQuality.Custom;
        }
    }

    public void saveGraphicSettings()
    {
        string key = Manager.m.version + "_" + "Graphics" + "_";

        PlayerPrefs.SetString(key + "GeneralQuality", GraphicQualityToString(generalQuality));
        PlayerPrefs.SetString(key + "TextureQuality", GraphicQualityToString(textureQuality));
        PlayerPrefs.SetString(key + "ResolutionQuality", GraphicQualityToString(resolutionQuality));
        PlayerPrefs.SetString(key + "AntiAliasing", GraphicQualityToString(antiAliasing));
        PlayerPrefs.SetString(key + "VSync", GraphicQualityToString(vSync));
        PlayerPrefs.SetString(key + "LightQuality", GraphicQualityToString(lightQuality));
        PlayerPrefs.SetString(key + "ShadowQuality", GraphicQualityToString(shadowQuality));
        PlayerPrefs.SetString(key + "ReflectionQuality", GraphicQualityToString(reflectionQuality));
        PlayerPrefs.SetString(key + "EnableHalos", GraphicQualityToString(enableHalos));
        PlayerPrefs.SetString(key + "GUIScale", GraphicQualityToString(gUIScale));

        PlayerPrefs.Save();
    }

    private GraphicQuality LoadQualityFromPrefs(string key, GraphicQuality defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string s = PlayerPrefs.GetString(key);
            GraphicQuality q = GraphicQualityFromString(s);

            // Gültigkeit prüfen, sonst Default
            if (IsValidQuality(q))
                return q;
            else
                return defaultValue;
        }
        return defaultValue;
    }

    private bool IsValidQuality(GraphicQuality q)
    {
        return IsValidGeneralQuality(q) || IsValidTextureQuality(q) || IsValidResolutionQuality(q) ||
               IsValidAntiAliasing(q) || IsValidVSync(q) || IsValidLightQuality(q) ||
               IsValidShadowQuality(q) || IsValidReflectionQuality(q) || IsValidEnableHalos(q) ||
               IsValidGUIScale(q);
    }

    private void UpdateGeneralQualityBasedOnSubValues()
    {
        GraphicQuality tex = textureQuality;
        GraphicQuality res = resolutionQuality;
        GraphicQuality aa = antiAliasing;
        GraphicQuality vs = vSync;
        GraphicQuality light = lightQuality;
        GraphicQuality shadow = shadowQuality;
        GraphicQuality refl = reflectionQuality;
        GraphicQuality halos = enableHalos;

        bool isLow = tex == GraphicQuality.Low &&
                     res == GraphicQuality.Low &&
                     aa == GraphicQuality.Off &&
                     vs == GraphicQuality.Off &&
                     light == GraphicQuality.Low &&
                     shadow == GraphicQuality.Off &&
                     refl == GraphicQuality.Off &&
                     halos == GraphicQuality.Off;

        bool isMedium = tex == GraphicQuality.Medium &&
                        res == GraphicQuality.Medium &&
                        aa == GraphicQuality.Low &&
                        vs == GraphicQuality.Low &&
                        light == GraphicQuality.Medium &&
                        shadow == GraphicQuality.Low &&
                        refl == GraphicQuality.On &&
                        halos == GraphicQuality.Some;

        bool isHigh = tex == GraphicQuality.High &&
                      res == GraphicQuality.High &&
                      aa == GraphicQuality.Medium &&
                      vs == GraphicQuality.High &&
                      light == GraphicQuality.High &&
                      shadow == GraphicQuality.Medium &&
                      refl == GraphicQuality.On &&
                      halos == GraphicQuality.All;

        bool isUltra = tex == GraphicQuality.Ultra &&
                       res == GraphicQuality.High &&
                       aa == GraphicQuality.High &&
                       vs == GraphicQuality.High &&
                       light == GraphicQuality.Ultra &&
                       shadow == GraphicQuality.High &&
                       refl == GraphicQuality.On &&
                       halos == GraphicQuality.All;

        GraphicQuality oldGeneral = generalQuality;

        if (isLow) generalQuality = GraphicQuality.Low;
        else if (isMedium) generalQuality = GraphicQuality.Medium;
        else if (isHigh) generalQuality = GraphicQuality.High;
        else if (isUltra) generalQuality = GraphicQuality.Ultra;
        else generalQuality = GraphicQuality.Custom;

        if (oldGeneral != generalQuality)
        {
            UpdateAllButtonStates();
        }
    }

    private void ApplyGeneralQualityToSubValues()
    {
        switch (generalQuality)
        {
            case GraphicQuality.Low:
                textureQuality = GraphicQuality.Low;
                resolutionQuality = GraphicQuality.Low;
                antiAliasing = GraphicQuality.Off;
                vSync = GraphicQuality.Off;
                lightQuality = GraphicQuality.Low;
                shadowQuality = GraphicQuality.Off;
                reflectionQuality = GraphicQuality.Off;
                enableHalos = GraphicQuality.Off;
                break;

            case GraphicQuality.Medium:
                textureQuality = GraphicQuality.Medium;
                resolutionQuality = GraphicQuality.Medium;
                antiAliasing = GraphicQuality.Low;
                vSync = GraphicQuality.Low;
                lightQuality = GraphicQuality.Medium;
                shadowQuality = GraphicQuality.Low;
                reflectionQuality = GraphicQuality.On;
                enableHalos = GraphicQuality.Some;
                break;

            case GraphicQuality.High:
                textureQuality = GraphicQuality.High;
                resolutionQuality = GraphicQuality.High;
                antiAliasing = GraphicQuality.Medium;
                vSync = GraphicQuality.High;
                lightQuality = GraphicQuality.High;
                shadowQuality = GraphicQuality.Medium;
                reflectionQuality = GraphicQuality.On;
                enableHalos = GraphicQuality.All;
                break;

            case GraphicQuality.Ultra:
                textureQuality = GraphicQuality.Ultra;
                resolutionQuality = GraphicQuality.High;
                antiAliasing = GraphicQuality.High;
                vSync = GraphicQuality.High;
                lightQuality = GraphicQuality.Ultra;
                shadowQuality = GraphicQuality.High;
                reflectionQuality = GraphicQuality.On;
                enableHalos = GraphicQuality.All;
                break;

            case GraphicQuality.Custom:
                break;
        }

        UpdateAllButtonStates();
    }
}


public enum GraphicQuality
{
    Low,
    Medium,
    High,
    Ultra,
    On,
    Off,
    Some,
    All,
    Small,
    Large,
    Custom,
}
