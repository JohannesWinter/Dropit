// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using Derive;

namespace DeriveUtils
{
    public enum PREVIEWMESH
    {
        plane,
        uVSphere,
        torus,
        roundedCube,
        cloth1,
        cloth2,
        goldBar,
        pipe,
        bridge,
        d_Cube,
        d_Cylinder,
        customMesh
    }

    public enum PREVIEWCONTENT
    {
        threeDimensional,
        maps
    }

    public enum LIGHTSETUP
    {
        DirectionalOnly,
        DirectionalAndAmbient
    }

    public enum RENDERINGPATH
    {
        forward,
        deferred
    }

    public enum PREVIEWSHADER
    {
        UnityStandard,
        DerivePOM,
        DeriveTessellated,
        DeriveMobile
    }

    public enum SPECULARCOLORSOURCE
    {
        uniformColor,
        SpecularMapRGB
    }

    public enum SPECULARMAPSOURCE
    {
        SpecularMapAlpha,
        AlbedoAlpha
    }

    /// <summary>
    /// This class handles the preview in the preview view
    /// 3D-preview as well as the preview of the individual maps is covered by this class
    /// </summary>
    [Serializable]
    public class DD_Preview
    {
        #region public variables
        public PREVIEWCONTENT m_previewContent = PREVIEWCONTENT.threeDimensional;
        public LIGHTSETUP m_lightSetup = LIGHTSETUP.DirectionalAndAmbient;
        public PREVIEWMESH m_previewMesh = PREVIEWMESH.plane;
        public RENDERINGPATH m_renderingPath = RENDERINGPATH.deferred;
        public PREVIEWSHADER m_previewShader = PREVIEWSHADER.DerivePOM;
        public SPECULARCOLORSOURCE m_SpecularColorSource = SPECULARCOLORSOURCE.uniformColor;
        public SPECULARMAPSOURCE m_specularMapSource = SPECULARMAPSOURCE.SpecularMapAlpha;



        public Mesh m_mesh = null;
        public float m_tiling = 1;
        public float m_normalStrength = 0;
        public Color m_specColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        public float m_specStrength = 0;
        public float m_gloss = 0;
        public float m_displacement = 0;
        public float m_ambientOcclusion = 0;
        public float m_tessellation = 1;

        public bool[] m_showMap = { true, false, false, false, false };
        public string[] m_mapNames = { "Albedo", "Normal Map", "Displacement Map", "Ambient Occlusion", "Specular Map" };

        public Vector3 m_renderObjectRotation = Vector3.zero;
        public Vector3 m_lightObjectRotation = Vector3.zero;
        public Vector3 m_cameraPosition = new Vector3(0, 0, -3);
        public float m_fOV = 60;

        public DD_NodeBase m_masterNode;

        public Material m_surfMat;

        public float m_zoomFactor = 1;
        #endregion

        #region private variables
        GameObject m_deriveObj;

        GameObject m_cameraObj;
        public Camera m_camera;

        GameObject m_gizmoCameraObj;
        Camera m_gizmoCamera;

        GameObject m_lightObj;
        Light m_light;

        GameObject m_refProbeObj;
        ReflectionProbe m_refProbe;

        GameObject m_gizmoLightObj;
        Light m_gizmoLight;

        GameObject m_renderObj;
        GameObject m_gizmoFlashLight;

        Rect m_previewAreaRect = Rect.zero;
        Rect m_previewTextureRect = Rect.zero;

        Rect m_previewSettingsRect = Rect.zero;
        Rect m_previewSettingsFrameRect = Rect.zero;
        bool m_showPreviewSettings = false;

        RenderTexture m_target;
        RenderTexture m_targetGizmos;
        RenderTexture m_targetFinal;
        Material m_PostProcessingMat;

        bool m_rotateObject = false;
        bool m_panCamera = false;
        bool m_rotateLight = false;
        bool m_updateRederingPath = false;

        int m_maxLayers = 31;

        Vector2 m_scrollPos = Vector2.zero;

        Vector2 m_previewTextureScrollPos = Vector3.zero;

        bool m_startUp = true;

        Texture2D m_nullTexture;

        string[] m_shaderPropertiesUnityStandard = { "_MainTex", "_BumpMap", "_ParallaxMap", "_OcclusionMap", "_SpecGlossMap" };
        string[] m_shaderPropertiesDerive = { "_MainTex", "_NormalMap", "_DisplacementMap", "_AmbientOcclusionMap", "_SpecularMap" };

        bool m_resetTransforms = false;
        float m_time = 0;
        float m_resetDuration = 0.2f;

        bool m_zoomIn = false;
        bool m_zoomOut = false;
        float m_zoomDuration = 0.1f;
        #endregion

        #region main methods
        /// <summary>
        /// Update is called by the project file, so there is no preview if there is no project open
        /// </summary>
        public void Update()
        {
            //Set up the layers for the preview
            DD_PreviewUtils.RunLayerControl(m_maxLayers, m_camera, m_gizmoCamera, m_light, m_gizmoLight, m_refProbe);

            //Check the object setup and redo setup if necessary
            CheckObjectSetup();

            //Null texture is black - it shows when a master node input is selected for preview but is not connected
            if (m_nullTexture == null) m_nullTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            //Set up preview Material and assign it to the render object
            if (m_surfMat == null) m_surfMat = new Material(Shader.Find("Derive/POM"));
            if (m_renderObj != null) m_renderObj.GetComponent<MeshRenderer>().sharedMaterial = m_surfMat;

            //Check if the material uses the desired shader
            switch (m_previewShader)
            {
                case PREVIEWSHADER.UnityStandard:
                    m_surfMat.shader = Shader.Find("Standard (Specular setup)");
                    m_surfMat.EnableKeyword("_NORMALMAP");
                    //m_surfMat.EnableKeyword("_SPECGLOSSMAP");
                    m_surfMat.EnableKeyword("_PARALLAXMAP");
                    //m_surfMat.DisableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                    break;

                case PREVIEWSHADER.DerivePOM:
                    m_surfMat.shader = Shader.Find("Derive/POM");
                    break;

                case PREVIEWSHADER.DeriveTessellated:
                    m_surfMat.shader = Shader.Find("Derive/Tessellation");
                    break;

                case PREVIEWSHADER.DeriveMobile:
                    m_surfMat.shader = Shader.Find("Derive/POM (Mobile)");
                    break;

                default:
                    break;
            }

            //Update rotations and positions of lights, cameras and objects
            //UpdateTransforms();

            //Smooth reset rotations and positions of lights, cameras and objects by lerping via time
            if (m_resetTransforms)
            {
                m_time += Time.deltaTime;
                m_renderObjectRotation = Vector3.Lerp(m_renderObjectRotation, new Vector3(-20, 10, 0), m_time / m_resetDuration);
                m_lightObjectRotation = Vector3.Lerp(m_lightObjectRotation, new Vector3(50, 10, 0), m_time / m_resetDuration);
                m_cameraPosition = Vector3.Lerp(m_cameraPosition, new Vector3(0, 0, -3), m_time / m_resetDuration);
                m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, 60, m_time / m_resetDuration);

                if (m_time >= m_resetDuration)
                {
                    m_resetTransforms = false;
                    m_time = 0;
                }
            }

            //Smooth zooming the camera in and out by lerping field of view via time
            if (m_zoomIn || m_zoomOut)
            {
                m_time += Time.deltaTime;

                m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_fOV, m_time / m_zoomDuration);

                if (m_time >= m_zoomDuration)
                {
                    m_zoomIn = false;
                    m_zoomOut = false;
                    m_time = 0;
                }
            }
            else
            {
                m_camera.fieldOfView = m_fOV;
            }

            //Update the rendering path on the camera capturing the preview image
            if (m_updateRederingPath)
            {
                m_updateRederingPath = false;

                if (m_renderingPath == RENDERINGPATH.forward) m_camera.renderingPath = RenderingPath.Forward;
                else m_camera.renderingPath = RenderingPath.DeferredShading;
            }

            //Update mesh on startup
            if (m_startUp) UpdateMesh();

            //Master node must not be null
            if (m_masterNode == null) m_masterNode = DD_PreviewUtils.GetMasterNode();

            //See method descriptions for more informations
            AdjustRenderObjectSize();

            CheckRenderTextureSetup();

            CaptureAndProcessImage();
        }

        public void OnPreviewGUI()
        {
            //Camera must not be null
            if (m_cameraObj == null) return;

            //Update rotations and positions of lights, cameras and objects
            if(DD_EditorUtils.currentEvent.type != EventType.Repaint && DD_EditorUtils.currentEvent.type != EventType.Layout)
                UpdateTransforms();

            //Calculate the rect for the preview area and draw a background frame
            m_previewAreaRect = new Rect(DD_EditorUtils.viewRect_previewView.position + new Vector2(16, 40), DD_EditorUtils.viewRect_previewView.size - new Vector2(32, 56));
            GUI.Box(m_previewAreaRect, "", DD_EditorUtils.editorSkin.GetStyle("PropertyFrame"));

            //Additional settings can be shown on the right side in an expandable box
            //This area handles the rects for the preview and the preview settings
            if (m_showPreviewSettings)
            {
                m_previewTextureRect = new Rect(m_previewAreaRect.position + new Vector2(10, 12), m_previewAreaRect.size - new Vector2(Mathf.Min(m_previewAreaRect.width / 2, 348), 24));
                m_previewSettingsRect = new Rect(m_previewTextureRect.position + new Vector2(m_previewTextureRect.width + 12, 0), new Vector2(Mathf.Min(m_previewAreaRect.width - m_previewTextureRect.width - 44, 348), m_previewTextureRect.height));
                m_previewSettingsFrameRect = new Rect(m_previewSettingsRect.position + new Vector2(24, 0), m_previewSettingsRect.size - new Vector2(16, 4));

                GUI.Box(m_previewSettingsRect, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG_SlideIn"));
                GUI.Box(m_previewSettingsFrameRect, "", DD_EditorUtils.editorSkin.GetStyle("PropertyFrame"));

                if (GUI.Button(new Rect(m_previewTextureRect.position + new Vector2(m_previewTextureRect.width + 12, m_previewAreaRect.height / 2 - 24), new Vector2(24, 48)), "", DD_EditorUtils.editorSkin.GetStyle("ArrowRightFlat")))
                    m_showPreviewSettings = false;
            }
            else
            {
                m_previewTextureRect = new Rect(m_previewAreaRect.position + new Vector2(10, 12), m_previewAreaRect.size - new Vector2(48, 22));
                m_previewSettingsRect = new Rect(m_previewTextureRect.position + new Vector2(m_previewTextureRect.width + 12, 0), new Vector2(Mathf.Min(m_previewAreaRect.width - m_previewTextureRect.width - 44, 348), m_previewTextureRect.height));

                GUI.Box(m_previewSettingsRect, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG_SlideIn"));

                if (GUI.Button(new Rect(m_previewTextureRect.position + new Vector2(m_previewTextureRect.width + 12, m_previewAreaRect.height / 2 - 24), new Vector2(24, 48)), "", DD_EditorUtils.editorSkin.GetStyle("ArrowLeftFlat")))
                    m_showPreviewSettings = true;
            }

            //Set up Layout area and scroll view
            GUILayout.BeginArea(new Rect(m_previewSettingsFrameRect.position + new Vector2(10, 0), m_previewSettingsFrameRect.size - new Vector2(20, 0)));
            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);

            //Show preview settings
            if (m_showPreviewSettings)
            {
                DD_PreviewGUILayout.TitleLabel("Preview Settings", m_previewSettingsFrameRect.width - 20);

                //Preview content can be a 3D view or a preview of the maps inputted into the master node
                DD_PreviewGUILayout.Label("Preview Content", m_previewSettingsFrameRect.width - 20);
                m_previewContent = (PREVIEWCONTENT)EditorGUILayout.EnumPopup(m_previewContent, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                EditorGUILayout.Space(10);

                switch (m_previewContent)
                {
                    case PREVIEWCONTENT.threeDimensional:
                        DD_PreviewGUILayout.Label("Mesh", m_previewSettingsFrameRect.width - 20);

                        EditorGUI.BeginChangeCheck();
                        m_previewMesh = (PREVIEWMESH)EditorGUILayout.EnumPopup(m_previewMesh, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                        if (EditorGUI.EndChangeCheck()) UpdateMesh();

                        EditorGUILayout.Space(10);

                        if (m_previewMesh == PREVIEWMESH.customMesh)
                        {
                            EditorGUI.BeginChangeCheck();
                            m_mesh = DD_PreviewGUILayout.MeshField("Mesh", m_mesh, m_previewSettingsFrameRect.width - 20);
                            if (EditorGUI.EndChangeCheck()) UpdateMesh();
                        }

                        EditorGUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.GetControlRect(GUILayout.Width(m_previewSettingsFrameRect.width / 4));
                        if (GUILayout.Button("Reset Transforms", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(25)))
                        {
                            m_resetTransforms = true;
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space(20);

                        DD_PreviewGUILayout.Label("Rendering Path", m_previewSettingsFrameRect.width - 20);
                        EditorGUI.BeginChangeCheck();
                        m_renderingPath = (RENDERINGPATH)EditorGUILayout.EnumPopup(m_renderingPath, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                        if (EditorGUI.EndChangeCheck()) m_updateRederingPath = true;
                        EditorGUILayout.Space(10);

                        DD_PreviewGUILayout.Label("Lighting", m_previewSettingsFrameRect.width - 20);
                        EditorGUI.BeginChangeCheck();
                        m_lightSetup = (LIGHTSETUP)EditorGUILayout.EnumPopup(m_lightSetup, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (m_lightSetup == LIGHTSETUP.DirectionalAndAmbient)
                            {
                                m_refProbe.nearClipPlane = 0.1f;
                                m_refProbe.farClipPlane = 10;
                                //m_refProbe.RenderProbe();
                                m_refProbeObj.SetActive(true);
                            }
                            else
                            {
                                m_refProbe.nearClipPlane = 9;
                                m_refProbe.farClipPlane = 10;
                                m_refProbe.RenderProbe();
                                m_refProbeObj.SetActive(false);
                            }

                            //The diffuse ambient light is controlled by the camera controller
                        }
                        EditorGUILayout.Space(10);

                        DD_PreviewGUILayout.Label("Preview Shader", m_previewSettingsFrameRect.width - 20);
                        m_previewShader = (PREVIEWSHADER)EditorGUILayout.EnumPopup(m_previewShader, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                        EditorGUILayout.Space(10);

                        switch (m_previewShader)
                        {
                            case PREVIEWSHADER.UnityStandard:
                                m_tiling = Mathf.Max(DD_PreviewGUILayout.FloatField("Tiling", m_tiling), 0.1f);
                                m_normalStrength = DD_PreviewGUILayout.Slider("Normal Strength", "", m_normalStrength, 0, 1, m_previewSettingsFrameRect.width - 20);
                                DD_PreviewGUILayout.Label("Specular Map Source", m_previewSettingsFrameRect.width - 20);
                                m_specularMapSource = (SPECULARMAPSOURCE)EditorGUILayout.EnumPopup(m_specularMapSource, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                                EditorGUILayout.Space(20);
                                m_specColor = DD_PreviewGUILayout.ColorField("Specular Color", m_specColor);
                                m_gloss = DD_PreviewGUILayout.Slider("Gloss", "Specular Map is read from the alpha channel", m_gloss, 0, 1, m_previewSettingsFrameRect.width - 20);
                                m_displacement = DD_PreviewGUILayout.Slider("Displacement", "", m_displacement, 0, 0.1f, m_previewSettingsFrameRect.width - 20);
                                m_ambientOcclusion = DD_PreviewGUILayout.Slider("Ambient Occlusion", "", m_ambientOcclusion, 0, 1, m_previewSettingsFrameRect.width - 20);
                                break;

                            case PREVIEWSHADER.DerivePOM:
                                m_tiling = Mathf.Max(DD_PreviewGUILayout.FloatField("Tiling", m_tiling), 0.1f);
                                m_normalStrength = DD_PreviewGUILayout.Slider("Normal Strength", "", m_normalStrength, 0, 1, m_previewSettingsFrameRect.width - 20);
                                DD_PreviewGUILayout.Label("Specular Color Source", m_previewSettingsFrameRect.width - 20);
                                m_SpecularColorSource = (SPECULARCOLORSOURCE)EditorGUILayout.EnumPopup(m_SpecularColorSource, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                                EditorGUILayout.Space(20);
                                if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                                    m_specColor = DD_PreviewGUILayout.ColorField("Specular Color", m_specColor);
                                m_gloss = DD_PreviewGUILayout.Slider("Gloss", "Specular Map is read from the alpha channel", m_gloss, 0, 1, m_previewSettingsFrameRect.width - 20);
                                m_displacement = DD_PreviewGUILayout.Slider("Displacement", "", m_displacement, 0, 0.1f, m_previewSettingsFrameRect.width - 20);
                                m_ambientOcclusion = DD_PreviewGUILayout.Slider("Ambient Occlusion", "", m_ambientOcclusion, 0, 1, m_previewSettingsFrameRect.width - 20);
                                break;

                            case PREVIEWSHADER.DeriveTessellated:
                                m_tiling = Mathf.Max(DD_PreviewGUILayout.FloatField("Tiling", m_tiling), 0.1f);
                                m_normalStrength = DD_PreviewGUILayout.Slider("Normal Strength", "", m_normalStrength, 0, 1, m_previewSettingsFrameRect.width - 20);
                                DD_PreviewGUILayout.Label("Specular Color Source", m_previewSettingsFrameRect.width - 20);
                                m_SpecularColorSource = (SPECULARCOLORSOURCE)EditorGUILayout.EnumPopup(m_SpecularColorSource, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Height(25));
                                EditorGUILayout.Space(20);
                                if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                                    m_specColor = DD_PreviewGUILayout.ColorField("Specular Color", m_specColor);
                                m_gloss = DD_PreviewGUILayout.Slider("Gloss", "Specular Map is read from the alpha channel", m_gloss, 0, 1, m_previewSettingsFrameRect.width - 20);
                                m_displacement = DD_PreviewGUILayout.Slider("Displacement", "", m_displacement, 0, 0.1f, m_previewSettingsFrameRect.width - 20);
                                m_ambientOcclusion = DD_PreviewGUILayout.Slider("Ambient Occlusion", "", m_ambientOcclusion, 0, 1, m_previewSettingsFrameRect.width - 20);
                                m_tessellation = DD_PreviewGUILayout.Slider("Tessellation", "", m_tessellation, 1, 80, m_previewSettingsFrameRect.width - 20);
                                break;

                            case PREVIEWSHADER.DeriveMobile:
                                m_tiling = Mathf.Max(DD_PreviewGUILayout.FloatField("Tiling", m_tiling), 0.1f);
                                m_normalStrength = DD_PreviewGUILayout.Slider("Normal Strength", "", m_normalStrength, 0, 1, m_previewSettingsFrameRect.width - 20);
                                EditorGUILayout.Space(10);
                                m_specColor = DD_PreviewGUILayout.ColorField("Specular Color", m_specColor);
                                m_gloss = DD_PreviewGUILayout.Slider("Gloss", "Specular Map is read from the alpha channel", m_gloss, 0, 1, m_previewSettingsFrameRect.width - 20);
                                m_displacement = DD_PreviewGUILayout.Slider("Displacement", "", m_displacement, 0, 0.1f, m_previewSettingsFrameRect.width - 20);
                                break;

                            default:
                                break;
                        }
                        break;

                    case PREVIEWCONTENT.maps:

                        //Tab view for map preview selection
                        for (int i = 0; i < m_showMap.Length; i++)
                        {
                            if (m_showMap[i])
                            {
                                if (GUILayout.Button(m_mapNames[i], DD_EditorUtils.editorSkin.GetStyle("GenericButtonPressed"), GUILayout.Height(50)))
                                {

                                }
                            }
                            else
                            {
                                if (GUILayout.Button(m_mapNames[i], DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(50)))
                                {
                                    for (int j = 0; j < m_showMap.Length; j++) m_showMap[j] = false;
                                    m_showMap[i] = true;
                                }
                            }
                        }

                        break;

                    default:
                        break;
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            //Copy parameters for 3D preview to the material settings
            if (m_surfMat != null)
            {
                switch (m_previewShader)
                {
                    case PREVIEWSHADER.UnityStandard:
                        m_surfMat.SetTextureScale("_MainTex", new Vector2(m_tiling, m_tiling));

                        

                        if (m_masterNode.m_inputs[4].inputtingNode != null)
                        {
                            if (m_specularMapSource == SPECULARMAPSOURCE.SpecularMapAlpha)
                            {
                                m_surfMat.DisableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                                m_surfMat.SetFloat("_SmoothnessTextureChannel", 3);
                                m_surfMat.EnableKeyword("_SPECGLOSSMAP");
                            }
                            else
                            {
                                m_surfMat.EnableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                                m_surfMat.SetFloat("_SmoothnessTextureChannel", 3);
                                m_surfMat.EnableKeyword("_SPECGLOSSMAP");
                            }
                        }
                        else
                        {
                            //m_surfMat.SetFloat("_SmoothnessTextureChannel", 1);
                            m_surfMat.DisableKeyword("_SPECGLOSSMAP");
                        }

                        m_surfMat.SetFloat("_BumpScale", -m_normalStrength);
                        m_surfMat.SetFloat("_Glossiness", m_gloss);
                        m_surfMat.SetFloat("_GlossMapScale", m_gloss);
                        m_surfMat.SetColor("_SpecColor", m_specColor);
                        m_surfMat.SetFloat("_Parallax", m_displacement);
                        m_surfMat.SetFloat("_OcclusionStrength", m_ambientOcclusion);
                        break;

                    case PREVIEWSHADER.DerivePOM:
                        m_surfMat.SetTextureScale("_MainTex", new Vector2(1, 1));
                        m_surfMat.SetFloat("_Tiling", m_tiling);

                        if (m_masterNode.m_inputs[1].inputtingNode == null) m_surfMat.SetFloat("_NormalStrength", 0);
                        else m_surfMat.SetFloat("_NormalStrength", m_normalStrength);

                        if (m_masterNode.m_inputs[4].inputtingNode != null)
                        {
                            if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 1);
                            }
                            else
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 1);
                                m_surfMat.SetFloat("_SpecularMapPresent", 1);
                            }
                        }
                        else
                        {
                            if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 0);
                            }
                            else
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 0);
                            }
                        }

                        m_surfMat.SetColor("_SpecularColor", m_specColor);
                        m_surfMat.SetFloat("_Gloss", m_gloss);

                        if (m_masterNode.m_inputs[2].inputtingNode == null) m_surfMat.SetFloat("_Displacement", 0);
                        else m_surfMat.SetFloat("_Displacement", m_displacement);

                        if (m_masterNode.m_inputs[3].inputtingNode == null) m_surfMat.SetFloat("_AmbientOcclusion", 0);
                        else m_surfMat.SetFloat("_AmbientOcclusion", m_ambientOcclusion);
                        break;

                    case PREVIEWSHADER.DeriveTessellated:
                        m_surfMat.SetTextureScale("_MainTex", new Vector2(1, 1));
                        m_surfMat.SetFloat("_Tiling", m_tiling);

                        if (m_masterNode.m_inputs[1].inputtingNode == null) m_surfMat.SetFloat("_NormalStrength", 0);
                        else m_surfMat.SetFloat("_NormalStrength", m_normalStrength);

                        if (m_masterNode.m_inputs[4].inputtingNode != null)
                        {
                            if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 1);
                            }
                            else
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 1);
                                m_surfMat.SetFloat("_SpecularMapPresent", 1);
                            }
                        }
                        else
                        {
                            if (m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 0);
                            }
                            else
                            {
                                m_surfMat.SetFloat("_SpecularfromRGB", 0);
                                m_surfMat.SetFloat("_SpecularMapPresent", 0);
                            }
                        }

                        m_surfMat.SetColor("_SpecularColor", m_specColor);
                        m_surfMat.SetFloat("_Gloss", m_gloss);

                        if (m_masterNode.m_inputs[2].inputtingNode == null) m_surfMat.SetFloat("_Displacement", 0);
                        else m_surfMat.SetFloat("_Displacement", m_displacement);

                        if (m_masterNode.m_inputs[3].inputtingNode == null) m_surfMat.SetFloat("_AmbientOcclusion", 0);
                        else m_surfMat.SetFloat("_AmbientOcclusion", m_ambientOcclusion);

                        m_surfMat.SetFloat("_Tessellation", m_tessellation);
                        break;

                    case PREVIEWSHADER.DeriveMobile:
                        //m_surfMat.SetTextureScale("_MainTex", new Vector2(1, 1));
                        m_surfMat.SetFloat("_Tiling", m_tiling);

                        if (m_masterNode.m_inputs[1].inputtingNode == null) m_surfMat.SetFloat("_NormalStrength", 0);
                        else m_surfMat.SetFloat("_NormalStrength", m_normalStrength);

                        if (m_masterNode.m_inputs[4].inputtingNode != null) m_surfMat.SetFloat("_SpecularMapPresent", 1);
                        else  m_surfMat.SetFloat("_SpecularMapPresent", 0);

                        m_surfMat.SetColor("_SpecColor", m_specColor);
                        m_surfMat.SetFloat("_Gloss", m_gloss);

                        if (m_masterNode.m_inputs[2].inputtingNode == null) m_surfMat.SetFloat("_Displacement", 0);
                        else m_surfMat.SetFloat("_Displacement", m_displacement);
                        break;

                    default:
                        break;
                }
            }

            //Adjust camera rect to the preview area for optimal usage of space
            m_camera.rect = new Rect(0, 0, Mathf.Max(m_previewTextureRect.width / m_previewTextureRect.height, 1), Mathf.Max(1, m_previewTextureRect.height / m_previewTextureRect.width));

            //Draw the preview of the maps inputted into the master node
            //Draw null texture (black) if nothing is inputted into an input
            if (m_previewContent == PREVIEWCONTENT.maps)
            {
                //Unlike the 3D preview the preview of the maps is always squared to avoid distortion
                //The shortest edge of the preview area serves as the width and height of the preview texture
                float resolution = Mathf.Min(m_previewTextureRect.width, m_previewTextureRect.height);
                Rect previewMapRect = new Rect(m_previewTextureRect.position + new Vector2(m_previewTextureRect.width / 2 - resolution / 2, m_previewTextureRect.height / 2 - resolution / 2), new Vector2(resolution, resolution));

                //Draw preview texture for selected map
                for (int i = 0; i < m_showMap.Length; i++)
                {
                    if (m_showMap[i])
                    {
                        if (m_masterNode != null)
                        {
                            if (m_masterNode.m_inputs[i].inputtingNode == null)
                            {
                                EditorGUI.DrawPreviewTexture(previewMapRect, m_nullTexture);
                            }
                            else
                            {
                                EditorGUI.DrawPreviewTexture(previewMapRect, m_masterNode.m_inputs[i].inputtingNode.m_outputs[m_masterNode.m_inputs[i].outputIndex].outputTexture);
                            }
                            GUI.Box(previewMapRect, "", DD_EditorUtils.editorSkin.GetStyle("TextureFrame"));
                        }
                    }
                }
            }
            else
            {
                //Draw the actual 3D preview texture
                if (m_targetFinal != null)
                    EditorGUI.DrawPreviewTexture(m_previewTextureRect, m_targetFinal);

                GUI.Box(m_previewTextureRect, "", DD_EditorUtils.editorSkin.GetStyle("TextureFrame"));

                //Sets a switch in the material based on whether the albedo input of the master node is connected
                //If the albedo input is not connected the material will display a white diffuse color instead of a texture
                //This is to avoid a black surface whenever no texture is inputted
                if (m_masterNode != null)
                {
                    if(m_previewShader != PREVIEWSHADER.UnityStandard)
                    {
                        for (int i = 0; i < m_masterNode.m_inputs.Count; i++)
                        {
                            if (m_masterNode.m_inputs[i].inputtingNode == null)
                            {
                                m_surfMat.SetTexture(m_shaderPropertiesDerive[i], m_nullTexture);
                                if (i == 0) m_surfMat.SetFloat("_AlbedoPresent", 0);
                            }
                            else
                            {
                                m_surfMat.SetTexture(m_shaderPropertiesDerive[i], m_masterNode.m_inputs[i].inputtingNode.m_outputs[m_masterNode.m_inputs[i].outputIndex].outputTexture);
                                if (i == 0) m_surfMat.SetFloat("_AlbedoPresent", 1);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_masterNode.m_inputs.Count; i++)
                        {
                            if (m_masterNode.m_inputs[i].inputtingNode != null)
                            {
                                m_surfMat.SetTexture(m_shaderPropertiesUnityStandard[i], m_masterNode.m_inputs[i].inputtingNode.m_outputs[m_masterNode.m_inputs[i].outputIndex].outputTexture);
                            }
                            else m_surfMat.SetTexture(m_shaderPropertiesUnityStandard[i], null);
                        }
                    }
                }
            }

            ProcessEvents();
        }

        void ProcessEvents()
        {
            ///<summary>
            ///Inside the preview the following can be done:
            ///Left mouse button drag: rotate render object
            ///Right mouse button drag: pan view
            ///Middle mouse button drag: rotate light
            ///Mouse wheel scroll: zoom in & out
            ///DURING THESE OPERATIONS PANNING THE CANVASS AND SELECTING NODES IS NOT POSSIBLE!
            /// </summary>
            if (m_previewTextureRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {
                    if (DD_EditorUtils.currentEvent.button == 0)
                    {
                        m_rotateObject = true;
                        DD_EditorUtils.allowGridOffset = false;
                        DD_EditorUtils.allowSelection = false;
                        DD_EditorUtils.allowSelectionRectRender = false;
                    }

                    if (DD_EditorUtils.currentEvent.button == 2)
                    {
                        m_rotateLight = true;
                        DD_EditorUtils.allowGridOffset = false;
                        DD_EditorUtils.allowSelection = false;
                        DD_EditorUtils.allowSelectionRectRender = false;
                    }

                    if (DD_EditorUtils.currentEvent.button == 1)
                    {
                        m_panCamera = true;
                        DD_EditorUtils.allowGridOffset = false;
                        DD_EditorUtils.allowSelection = false;
                        DD_EditorUtils.allowSelectionRectRender = false;
                    }
                }

                if (DD_EditorUtils.currentEvent.type == EventType.ScrollWheel)
                {
                    if (DD_EditorUtils.currentEvent.delta.y < 0)
                    {
                        m_fOV = Mathf.Max(m_fOV - 10, 10);
                        m_zoomIn = true;
                    }
                    else
                    {
                        m_fOV = Mathf.Min(m_fOV + 10, 70);
                        m_zoomOut = true;
                    }
                }
            }

            if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
            {
                m_rotateObject = false;
                m_rotateLight = false;
                m_panCamera = false;

                DD_EditorUtils.allowGridOffset = true;
                DD_EditorUtils.allowSelection = true;
                DD_EditorUtils.allowSelectionRectRender = true;
            }
        }
        #endregion

        #region utility methods
        /// <summary>
        /// Checks if the required objects are set up correctly and sets them up if required
        /// </summary>
        void CheckObjectSetup()
        {
            if (m_deriveObj == null) m_deriveObj = DD_PreviewUtils.AddDeriveObj(m_deriveObj, m_cameraObj, m_lightObj, m_renderObj, m_gizmoCameraObj, m_gizmoLightObj, m_gizmoFlashLight);

            if (m_cameraObj == null)
            {
                m_cameraObj = DD_PreviewUtils.AddCamera(m_cameraObj, m_deriveObj);
                m_camera = m_cameraObj.GetComponent<Camera>();
            }

            if (m_lightObj == null)
            {
                m_lightObj = DD_PreviewUtils.AddLight(m_lightObj, m_deriveObj, m_lightObjectRotation, 1);
                m_lightObjectRotation = m_lightObj.transform.eulerAngles;
                m_light = m_lightObj.GetComponent<Light>();
            }

            if(m_refProbeObj == null)
            {
                m_refProbeObj = DD_PreviewUtils.AddRefProbe(m_refProbeObj, m_deriveObj);
                m_refProbe = m_refProbeObj.GetComponent<ReflectionProbe>();
            }

            if (m_renderObj == null) m_renderObj = DD_PreviewUtils.AddRenderObject(m_renderObj, m_deriveObj, m_renderObjectRotation);

            if (m_gizmoCameraObj == null)
            {
                m_gizmoCameraObj = DD_PreviewUtils.AddGizmoCamera(m_gizmoCameraObj, m_deriveObj);
                m_renderObjectRotation = m_renderObj.transform.eulerAngles;
                m_gizmoCamera = m_gizmoCameraObj.GetComponent<Camera>();
            }

            if (m_gizmoLightObj == null)
            {
                m_gizmoLightObj = DD_PreviewUtils.AddGizmoLight(m_gizmoLightObj, m_deriveObj);
                m_gizmoLight = m_gizmoLightObj.GetComponent<Light>();
            }

            if (m_gizmoFlashLight == null) m_gizmoFlashLight = DD_PreviewUtils.AddGizmo(m_gizmoFlashLight, m_deriveObj, m_lightObjectRotation);
        }

        /// <summary>
        /// Checks if the required RenderTextures are set up correctly and sets them up if required
        /// Conditions:
        /// 1. One of the textures is null
        /// 2. Preview area rect has been changed. In that case the textures are set up again with adjusted width and height
        /// </summary>
        void CheckRenderTextureSetup()
        {
            if (m_target == null || m_targetGizmos == null || m_targetFinal == null)
            {
                if (m_previewTextureRect.width > 0 && m_previewTextureRect.height > 0 && m_previewTextureRect.width > 0 && m_previewTextureRect.height > 0)
                {
                    m_target = RenderTexture.GetTemporary((int)(m_previewTextureRect.width * m_zoomFactor), (int)m_previewTextureRect.height, 0, RenderTextureFormat.ARGB32);
                    m_targetGizmos = RenderTexture.GetTemporary((int)(m_previewTextureRect.height * 0.2f * m_zoomFactor), (int)(m_previewTextureRect.height * 0.2f), 0, RenderTextureFormat.ARGB32);
                    m_targetFinal = RenderTexture.GetTemporary((int)(m_previewTextureRect.width * m_zoomFactor), (int)m_previewTextureRect.height, 0, RenderTextureFormat.ARGB32);
                }
            }

            if (m_target != null && m_targetGizmos != null && m_targetFinal != null)
            {
                if (m_target.width != (int)m_previewTextureRect.width || m_target.height != (int)m_previewTextureRect.height)
                {
                    if (m_previewTextureRect.width > 0 && m_previewTextureRect.height > 0 && m_previewTextureRect.width > 0 && m_previewTextureRect.height > 0)
                    {
                        RenderTexture.ReleaseTemporary(m_target);
                        RenderTexture.ReleaseTemporary(m_targetGizmos);
                        RenderTexture.ReleaseTemporary(m_targetFinal);

                        m_target = RenderTexture.GetTemporary((int)(m_previewTextureRect.width * m_zoomFactor), (int)m_previewTextureRect.height, 0, RenderTextureFormat.ARGB32);
                        m_targetGizmos = RenderTexture.GetTemporary((int)(m_previewTextureRect.height * 0.2f * m_zoomFactor), (int)(m_previewTextureRect.height * 0.2f), 0, RenderTextureFormat.ARGB32);
                        m_targetFinal = RenderTexture.GetTemporary((int)(m_previewTextureRect.width * m_zoomFactor), (int)m_previewTextureRect.height, 0, RenderTextureFormat.ARGB32);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the rotation and position of the objects for the preview
        /// Can be resetted
        /// See description of ProcessEvents()-method in this class for more information
        /// </summary>
        void UpdateTransforms()
        {
            if (m_rotateObject)
            {
                m_renderObj.transform.Rotate(new Vector3(-DD_EditorUtils.currentEvent.delta.y / (4 * 60 / m_camera.fieldOfView), 0, 0), Space.World);
                m_renderObj.transform.Rotate(new Vector3(0, -DD_EditorUtils.currentEvent.delta.x / (4 * 60 / m_camera.fieldOfView), 0), Space.Self);

                m_renderObjectRotation = m_renderObj.transform.eulerAngles;
            }

            if (m_rotateLight)
            {
                m_lightObj.transform.Rotate(new Vector3(-DD_EditorUtils.currentEvent.delta.y / 4, 0, 0), Space.World);
                m_lightObj.transform.Rotate(new Vector3(0, -DD_EditorUtils.currentEvent.delta.x / 4, 0), Space.World);

                m_gizmoFlashLight.transform.Rotate(new Vector3(-DD_EditorUtils.currentEvent.delta.y / 4, 0, 0), Space.World);
                m_gizmoFlashLight.transform.Rotate(new Vector3(0, -DD_EditorUtils.currentEvent.delta.x / 4, 0), Space.World);

                m_lightObjectRotation = m_lightObj.transform.eulerAngles;
            }

            if (m_panCamera)
            {
                m_cameraPosition = new Vector3(Mathf.Min(Mathf.Max(m_cameraPosition.x - DD_EditorUtils.currentEvent.delta.x / (400 * 60 / m_camera.fieldOfView), -1), 1), Mathf.Min(Mathf.Max(m_cameraPosition.y + DD_EditorUtils.currentEvent.delta.y / (400 * 60 / m_camera.fieldOfView), -1), 1), -3);
            }

            m_renderObj.transform.eulerAngles = m_renderObjectRotation;
            m_gizmoFlashLight.transform.eulerAngles = m_lightObjectRotation;
            m_lightObj.transform.eulerAngles = m_lightObjectRotation;
            m_cameraObj.transform.localPosition = m_cameraPosition;
        }

        /// <summary>
        /// Captures 2 images, one of the render object and one of the flash light gizmo that shows which direction the light currently comes from
        /// In a second step the rendered images are processed into a single texture using a post processing shader to get the final result
        /// The final result is shows as the actual preview
        /// </summary>
        void CaptureAndProcessImage()
        {
            m_camera.targetTexture = m_target;
            RenderTexture.active = m_target;
            m_camera.Render();

            m_gizmoCamera.targetTexture = m_targetGizmos;
            RenderTexture.active = m_targetGizmos;
            m_gizmoCamera.Render();

            if (m_PostProcessingMat == null) m_PostProcessingMat = new Material(Shader.Find("Hidden/Derive/Post Processing/Preview Post Processing"));

            m_PostProcessingMat.SetTexture("_Tex", m_target);
            m_PostProcessingMat.SetTexture("_Tex2", m_targetGizmos);
            m_PostProcessingMat.SetFloat("_gizmoTexTilingX", 5 * m_previewTextureRect.width / m_previewTextureRect.height);

            if (m_target != null)
                m_PostProcessingMat.SetFloat("_gizmoTexOffset", 4 * (float)m_target.width / (float)m_target.height);

            Graphics.Blit(m_targetFinal, m_targetFinal, m_PostProcessingMat);

            RenderTexture.active = null;
        }

        /// <summary>
        /// This makes sure the rener object has the desired mesh on it's mesh filter component
        /// Either one of the preset meshes or a custom mesh
        /// </summary>
        void UpdateMesh()
        {
            if (m_renderObj != null)
                switch (m_previewMesh)
                {
                    case PREVIEWMESH.plane:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/plane.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.uVSphere:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/uVSphere.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.torus:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/torus.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.roundedCube:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/roundedCube.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.cloth1:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/Cloth1.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.cloth2:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/Cloth2.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.goldBar:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/Goldbar.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.pipe:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/Pipe.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.bridge:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/Bridge.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.d_Cube:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/D-Cube.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.d_Cylinder:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/D-Cylinder.fbx", typeof(Mesh));
                        break;

                    case PREVIEWMESH.customMesh:
                        m_renderObj.GetComponent<MeshFilter>().sharedMesh = m_mesh;
                        break;

                    default:
                        break;
                }

            if(m_refProbeObj.activeSelf)
                m_refProbe.RenderProbe();
        }

        /// <summary>
        /// Adjusts the size of the render object to fit preciesely into the preview area
        /// </summary>
        void AdjustRenderObjectSize()
        {
            Mesh tmpMesh = m_renderObj.GetComponent<MeshFilter>().sharedMesh;

            float scale = 1;

            if (tmpMesh != null)
                scale = Mathf.Min(1 / tmpMesh.bounds.extents.x, 1 / tmpMesh.bounds.extents.y, 1 / tmpMesh.bounds.extents.z);

            m_renderObj.transform.localScale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Restores the previous state of the scene by removing layers specific for the preview and destroying the derive object together with all it's children.
        /// </summary>
        public void OnDestroy()
        {
            GameObject.DestroyImmediate(m_deriveObj);
            DD_PreviewUtils.RemoveLayer("Derive", m_maxLayers);
            DD_PreviewUtils.RemoveLayer("Derive Gizmos", m_maxLayers);
            Tools.visibleLayers = -1;
        }
        #endregion
    }
}
#endif