// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using DeriveUtils;
using UnityEditor;
using System.IO;
using System;

namespace Derive
{
    public enum RESOLUTIONS
    {
        tiny256x256,
        small512x512,
        medium1024x1024,
        large2048x2048,
        veryLarge4096x4096
    }

    public enum OUTPUTFORMAT
    {
        JPEG,
        PNG,
        TIFF,
        TGA,
        EXR,
        Bitmap
    }

    public class DD_NodeMaster : DD_NodeBase
    {
        #region Public Variables
        public bool m_showResolutionSettings = true;
        public RESOLUTIONS m_editorResolution = RESOLUTIONS.small512x512;
        public RESOLUTIONS m_exportResolution = RESOLUTIONS.large2048x2048;
        public OUTPUTFORMAT[] m_outputFormats = new OUTPUTFORMAT[5] { OUTPUTFORMAT.PNG, OUTPUTFORMAT.PNG, OUTPUTFORMAT.PNG, OUTPUTFORMAT.PNG, OUTPUTFORMAT.PNG };

        public bool m_showExportSettings = true;

        public bool[] m_exportMaps;             //Bool[] which maps to export
        public bool[] m_inputOccupied = new bool[5] { false, false, false, false, false };
        public string[] m_exportMapNames;
        public string[] m_shaderPropNamesUnity;
        public string[] m_shaderPropNamesDerive;

        public string m_outputName;
        public string m_outputPath;
        public bool m_createSampleMaterials = true;
        #endregion

        #region privateVariables
        bool m_runExport = false;
        bool m_waitForRefresh = false;

        string m_pathSubstitute = "../";
        string m_pathcache;
        #endregion

        #region Constructor
        public DD_NodeMaster()
        {
            m_inputs = new List<DD_InputConnector>(2) { new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector() };
            m_outputs = new List<DD_OutputConnector>(0);

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBrown";
        }
        #endregion

        #region Main Methods
        public override void InitNode()
        {
            base.InitNode();

            m_nodeType = NodeType.Master;

            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Albedo";
            m_inputs[1].inputLabel = "Normal Map";
            m_inputs[2].inputLabel = "Displacement Map";
            m_inputs[3].inputLabel = "Ambient Occlusion Map";
            m_inputs[4].inputLabel = "Specular Map";

            m_hasNodePreview = false;

            m_exportMaps = new bool[5] { true, true, true, true, true };
            m_exportMapNames = new string[5] { "Albedo", "Normal Map", "Displacement Map", "Ambient Occlusion Map", "Specular Map" };
            m_shaderPropNamesUnity = new string[5] { "_MainTex", "_BumpMap", "_ParallaxMap", "_OcclusionMap", "_SpecGlossMap" };
            m_shaderPropNamesDerive = new string[5] { "_MainTex", "_NormalMap", "_DisplacementMap", "_AmbientOcclusionMap", "_SpecularMap" };

            m_outputName = DD_EditorUtils.currentProject.name;
            m_outputPath = DD_EditorUtils.GetDerivePath();
            m_outputPath = m_outputPath.Replace("Derive/", "");
            m_pathcache = Application.dataPath;
            m_pathcache = Application.dataPath;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            ///<summary>
            /// Makes sure that newly connected inputs are registered for exporting
            /// </summary>
            if (m_redoCalculation)
            {
                for (int i = 0; i < m_inputs.Count; i++)
                {
                    if (m_inputs[i].isOccupied && !m_inputOccupied[i])
                    {
                        m_inputOccupied[i] = m_inputs[i].isOccupied;
                        m_exportMaps[i] = true;
                        m_inputs[i].inputtingNode.m_outputHasChanged = false;
                    }
                    if (m_inputs[i].inputtingNode == null && m_inputOccupied[i])
                    {
                        m_inputOccupied[i] = false;
                        m_exportMaps[i] = false;
                    }
                }
            }

            m_redoCalculation = false;      //The Master node performs in sequences and different from the other nodes, hence redoCalculation can always be false

            ///<summary>
            ///Editor resolution and export resolution are different
            ///When export is run, the first step is to set the resolution to the export value 
            ///and set all nodes to recalculate to get the final result in the desired resolution for export.
            /// </summary>
            if (m_runExport)
            {
                switch (m_exportResolution)
                {
                    case RESOLUTIONS.tiny256x256:
                        m_parentProject.m_projectSettings.resolution = 256;
                        foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                        break;

                    case RESOLUTIONS.small512x512:
                        m_parentProject.m_projectSettings.resolution = 512;
                        foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                        break;

                    case RESOLUTIONS.medium1024x1024:
                        m_parentProject.m_projectSettings.resolution = 1024;
                        foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                        break;

                    case RESOLUTIONS.large2048x2048:
                        m_parentProject.m_projectSettings.resolution = 2048;
                        foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                        break;

                    case RESOLUTIONS.veryLarge4096x4096:
                        m_parentProject.m_projectSettings.resolution = 4096;
                        foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                        break;

                    default:
                        break;
                }

                //Can't remember why I left this in here, but I guess it was neccessary for some reason :S
                foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                
                m_runExport = false;
                m_waitForRefresh = true;
            }


            //This makes sure, exportung only continues after all nodes have finished recalculating
            if (m_waitForRefresh)
            {
                bool allNodesRefreshed = true;

                foreach (DD_NodeBase node in m_parentProject.m_nodes)
                    if (node.m_redoCalculation || node.m_outputHasChanged) allNodesRefreshed = false;

                if (allNodesRefreshed)
                {
                    ExportMaps();
                    m_waitForRefresh = false;
                }
            }
        }
        #endregion

        #region utility methods
        public override void DrawProperties()
        {
            base.DrawProperties();

            DD_GUILayOut.TitleLabel("Project Settings");

            m_showResolutionSettings = DD_GUILayOut.FoldOut(m_showResolutionSettings, "Resolution Settings");

            if (m_showResolutionSettings)
            {
                EditorGUILayout.Space(10);

                DD_GUILayOut.TitleLabel("Resolutions");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Editor Resolution"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(130), GUILayout.Height(25));
                EditorGUI.BeginChangeCheck();
                m_editorResolution = (RESOLUTIONS)EditorGUILayout.EnumPopup(new GUIContent(" "), m_editorResolution, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 180), GUILayout.Height(25));
                if (EditorGUI.EndChangeCheck())
                {
                    switch (m_editorResolution)
                    {
                        case RESOLUTIONS.tiny256x256:
                            m_parentProject.m_projectSettings.resolution = 256;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.small512x512:
                            m_parentProject.m_projectSettings.resolution = 512;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.medium1024x1024:
                            m_parentProject.m_projectSettings.resolution = 1024;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.large2048x2048:
                            m_parentProject.m_projectSettings.resolution = 2048;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.veryLarge4096x4096:
                            
                            if (EditorUtility.DisplayDialog("Warning: Very High Resolution", 
                                "You are about to set resolution to 4k by 4k pixels. This will require extreme amounts of memory. If you don't have enough, the editor might freeze. Are you sure you want to continue?", 
                                "Continue Anyway", 
                                "Revert to 512x512"))
                            {
                                m_parentProject.m_projectSettings.resolution = 4096;
                                foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            }
                            else
                            {
                                m_editorResolution = RESOLUTIONS.small512x512;
                            }
                            
                            break;

                        default:
                            break;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(20);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Export Resolution"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(130), GUILayout.Height(25));
                m_exportResolution = (RESOLUTIONS)EditorGUILayout.EnumPopup(new GUIContent(" "), m_exportResolution, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 180), GUILayout.Height(25));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(10);

                if (m_exportResolution == RESOLUTIONS.veryLarge4096x4096)
                {
                    GUILayout.Space(5);
                    GUILayout.Label("Attention:\nYou have set the export resolution to 4k by 4k pixels.This will require extreme amounts of memory. If you don't have enough, the editor might freeze during export.", DD_EditorUtils.editorSkin.GetStyle("AttentionLabel"), GUILayout.Width(DD_EditorUtils.propertyRect.width - 40));
                    GUILayout.Space(10);
                }
            }

            m_showExportSettings = DD_GUILayOut.FoldOut(m_showExportSettings, "Export Settings");

            if (m_showExportSettings)
            {
                DD_GUILayOut.TitleLabel("Export");

                DD_GUILayOut.Label("Select Maps to export");

                for(int i = 0; i < m_exportMaps.Length; i++)
                {
                    if(m_inputs[i].inputtingNode != null)
                    {
                        m_exportMaps[i] = DD_GUILayOut.BoolField(m_exportMapNames[i], m_exportMaps[i], true);
                    }
                    else
                    {
                        m_exportMaps[i] = DD_GUILayOut.BoolField(m_exportMapNames[i], m_exportMaps[i], false);
                        m_exportMaps[i] = false;
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Output Format"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(130));
                    m_outputFormats[i] = (OUTPUTFORMAT)EditorGUILayout.EnumPopup(new GUIContent(" "), m_outputFormats[i], DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 180));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(10);
                }

                if(m_outputFormats[0] == OUTPUTFORMAT.JPEG || m_outputFormats[0] == OUTPUTFORMAT.Bitmap ||
                    m_outputFormats[1] == OUTPUTFORMAT.JPEG || m_outputFormats[1] == OUTPUTFORMAT.Bitmap ||
                    m_outputFormats[2] == OUTPUTFORMAT.JPEG || m_outputFormats[2] == OUTPUTFORMAT.Bitmap ||
                    m_outputFormats[3] == OUTPUTFORMAT.JPEG || m_outputFormats[3] == OUTPUTFORMAT.Bitmap ||
                    m_outputFormats[4] == OUTPUTFORMAT.JPEG || m_outputFormats[4] == OUTPUTFORMAT.Bitmap)
                {
                    GUILayout.Space(10);
                    GUILayout.Label("Attention:\n\nYou are exporting at least one file with no alpha-channel (Bitmap or JPEG). Before exporting, make sure you are not storing important texture data in the alpha channel of a texture that does not have one.", DD_EditorUtils.editorSkin.GetStyle("AttentionLabel"), GUILayout.Width(DD_EditorUtils.propertyRect.width - 40));
                    GUILayout.Space(10);
                }

                EditorGUILayout.Space(10);
                m_createSampleMaterials = DD_GUILayOut.BoolField("Create Sample Materials", m_createSampleMaterials, true);
                EditorGUILayout.Space(20);

                m_outputName = DD_GUILayOut.TextField("Output Name", m_outputName);
                DD_GUILayOut.DoubleLabel("Output Folder", m_pathSubstitute + m_outputPath);

                GUILayout.BeginHorizontal();
                GUILayout.Space(4);
                if(GUILayout.Button("Select Output Folder", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Width(DD_EditorUtils.propertyRect.width - 25), GUILayout.Height(25)))
                {
                    m_outputPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");

                    if (m_outputPath == "") m_outputPath = m_pathcache;

                    int appPathLength = 0;
                    if (m_outputPath.Contains(Application.dataPath))
                    {
                        appPathLength = Application.dataPath.Length;
                        m_pathSubstitute = "../";
                    }
                    else m_pathSubstitute = "";

                    m_pathcache = m_outputPath;

                    m_outputPath = m_outputPath.Substring(appPathLength - 6);      // -6 for minus ASSETS folder
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(30);


                GUILayout.BeginHorizontal();
                GUILayout.Space(4);
                if (GUILayout.Button("Export", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Width(Mathf.Min(DD_EditorUtils.propertyRect.width - 4, 200)), GUILayout.Height(30)))
                {
                    if (m_outputName != "") m_runExport = true;
                    else EditorUtility.DisplayDialog("Error", "Please specify a name for the output.", "Ok");
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
        }

        /// <summary>
        /// Actually exports the maps (or not, based on if maps already exist and are not to be overwritten)
        /// and restores the editor resolution
        /// Optionally creates a set of sample materials with the same settings as in the preview
        /// </summary>
        void ExportMaps()
        {
            if (Directory.Exists(m_outputPath + "/" + m_outputName))
            {
                if (EditorUtility.DisplayDialog("Warning", "A project with the name '" + m_outputName + "' already exists at that path. Do you wish to replace it?", "Replace", "Cancel"))
                {

                }
                else
                {
                    switch (m_editorResolution)
                    {
                        case RESOLUTIONS.tiny256x256:
                            m_parentProject.m_projectSettings.resolution = 256;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.small512x512:
                            m_parentProject.m_projectSettings.resolution = 512;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.medium1024x1024:
                            m_parentProject.m_projectSettings.resolution = 1024;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.large2048x2048:
                            m_parentProject.m_projectSettings.resolution = 2048;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        case RESOLUTIONS.veryLarge4096x4096:
                            m_parentProject.m_projectSettings.resolution = 4096;
                            foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                            break;

                        default:
                            break;
                    }
                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(m_outputPath + "/" + m_outputName);
            }

            for(int i = 0; i < m_exportMaps.Length; i++)
            {
                if (m_exportMaps[i])
                {
                    bool isNormal = false;
                    if (i == 1) isNormal = true;
                    SaveTexture(m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputTexture, m_outputPath + "/" + m_outputName, m_outputName, i, isNormal, m_outputFormats[i]);
                }
            }

            AssetDatabase.Refresh();
                        
            if (m_createSampleMaterials) CreateSampleMaterials();

            switch (m_editorResolution)
            {
                case RESOLUTIONS.tiny256x256:
                    m_parentProject.m_projectSettings.resolution = 256;
                    foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                    break;

                case RESOLUTIONS.small512x512:
                    m_parentProject.m_projectSettings.resolution = 512;
                    foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                    break;

                case RESOLUTIONS.medium1024x1024:
                    m_parentProject.m_projectSettings.resolution = 1024;
                    foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                    break;

                case RESOLUTIONS.large2048x2048:
                    m_parentProject.m_projectSettings.resolution = 2048;
                    foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                    break;

                case RESOLUTIONS.veryLarge4096x4096:
                    m_parentProject.m_projectSettings.resolution = 4096;
                    foreach (DD_NodeBase node in m_parentProject.m_nodes) node.m_redoCalculation = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Simple method that saves the exported file to the disk, adjusts the import settings and refreshes the database
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="mapIndex"></param>
        /// <param name="isNormalMap"></param>
        void SaveTexture(Texture2D tex, string path, string name, int mapIndex, bool isNormalMap, OUTPUTFORMAT outputFormat)
        {
            TextureImporter importer = new TextureImporter();
            byte[] bytes;
            string fullPath = "";

            switch (outputFormat)
            {
                case OUTPUTFORMAT.JPEG:
                    bytes = tex.EncodeToJPG();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".jpg", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".jpg";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".jpg");
                    break;

                case OUTPUTFORMAT.PNG:
                    bytes = tex.EncodeToPNG();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".png", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".png";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".png");
                    break;

                case OUTPUTFORMAT.TIFF:
                    bytes = tex.EncodeToPNG();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tiff", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tiff";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tiff");
                    break;

                case OUTPUTFORMAT.TGA:
                    bytes = tex.EncodeToTGA();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tga", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tga";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".tga");
                    break;

                case OUTPUTFORMAT.EXR:
                    bytes = tex.EncodeToEXR();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".exr", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".exr";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".exr");
                    break;

                case OUTPUTFORMAT.Bitmap:
                    bytes = tex.EncodeToPNG();
                    File.WriteAllBytes(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".bmp", bytes);
                    fullPath = path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".bmp";
                    AssetDatabase.Refresh();
                    importer = (TextureImporter)TextureImporter.GetAtPath(path + "/" + name + "_" + m_exportMapNames[mapIndex] + ".bmp");
                    break;

                default:
                    break;
            }

            importer.maxTextureSize = m_parentProject.m_projectSettings.resolution;
            importer.textureCompression = TextureImporterCompression.Uncompressed;

            if (isNormalMap) importer.textureType = TextureImporterType.NormalMap;

            AssetDatabase.ImportAsset(fullPath, ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// If sample materials are to be created, this method will do so.
        /// </summary>
        void CreateSampleMaterials()
        {
            //Creates the directory for the sample materials and one material for each shader
            Directory.CreateDirectory(m_outputPath + "/" + m_outputName + "/Sample Materials");

            Material sampleMat1 = new Material(Shader.Find("Standard (Specular setup)"));
            sampleMat1.EnableKeyword("_NORMALMAP");
            sampleMat1.EnableKeyword("_SPECGLOSSMAP");
            sampleMat1.EnableKeyword("_PARALLAXMAP");

            Material sampleMat2 = new Material(Shader.Find("Derive/POM"));
            Material sampleMat3 = new Material(Shader.Find("Derive/Tessellation"));
            Material sampleMat4 = new Material(Shader.Find("Derive/POM (Mobile)"));

            //Assigns the exported textures and the parameters set in the preview settings to the material properties
            for (int i = 0; i < m_exportMaps.Length; i++)
            {
                if (m_exportMaps[i])
                {
                    string fileExtension = "";

                    switch (m_outputFormats[i])
                    {
                        case OUTPUTFORMAT.JPEG:
                            fileExtension = ".jpg";
                            break;

                        case OUTPUTFORMAT.PNG:
                            fileExtension = ".png";
                            break;

                        case OUTPUTFORMAT.TIFF:
                            fileExtension = ".tiff";
                            break;

                        case OUTPUTFORMAT.TGA:
                            fileExtension = ".tga";
                            break;

                        case OUTPUTFORMAT.EXR:
                            fileExtension = ".exr";
                            break;

                        case OUTPUTFORMAT.Bitmap:
                            fileExtension = ".bmp";
                            break;

                        default:
                            break;
                    }

                    sampleMat1.SetTexture(m_shaderPropNamesUnity[i], (Texture2D)AssetDatabase.LoadAssetAtPath(m_outputPath + "/" + m_outputName + "/" + m_outputName + "_" + m_exportMapNames[i] + fileExtension, typeof(Texture2D))); ;
                    sampleMat2.SetTexture(m_shaderPropNamesDerive[i], (Texture2D)AssetDatabase.LoadAssetAtPath(m_outputPath + "/" + m_outputName + "/" + m_outputName + "_" + m_exportMapNames[i] + fileExtension, typeof(Texture2D)));
                    sampleMat3.SetTexture(m_shaderPropNamesDerive[i], (Texture2D)AssetDatabase.LoadAssetAtPath(m_outputPath + "/" + m_outputName + "/" + m_outputName + "_" + m_exportMapNames[i] + fileExtension, typeof(Texture2D)));
                    sampleMat4.SetTexture(m_shaderPropNamesDerive[i], (Texture2D)AssetDatabase.LoadAssetAtPath(m_outputPath + "/" + m_outputName + "/" + m_outputName + "_" + m_exportMapNames[i] + fileExtension, typeof(Texture2D)));

                    sampleMat1.SetTextureScale("_MainTex", new Vector2(m_parentProject.m_preview.m_tiling, m_parentProject.m_preview.m_tiling));
                    //sampleMat2.SetTextureScale("_MainTex", new Vector2(m_parentProject.m_preview.m_tiling, m_parentProject.m_preview.m_tiling));
                    //sampleMat3.SetTextureScale("_MainTex", new Vector2(m_parentProject.m_preview.m_tiling, m_parentProject.m_preview.m_tiling));
                    //sampleMat4.SetTextureScale("_MainTex", new Vector2(m_parentProject.m_preview.m_tiling, m_parentProject.m_preview.m_tiling));

                    sampleMat2.SetFloat("_Tiling", m_parentProject.m_preview.m_tiling);
                    sampleMat2.SetTextureScale("_MainTex", new Vector2(1, 1));

                    sampleMat3.SetFloat("_Tiling", m_parentProject.m_preview.m_tiling);

                    sampleMat4.SetFloat("_Tiling", m_parentProject.m_preview.m_tiling);
                    sampleMat4.SetTextureScale("_MainTex", new Vector2(1, 1));

                    sampleMat3.SetFloat("_Tessellation", m_parentProject.m_preview.m_tessellation);



                    sampleMat1.SetFloat("_BumpScale", -m_parentProject.m_preview.m_normalStrength);
                    sampleMat2.SetFloat("_NormalStrength", m_parentProject.m_preview.m_normalStrength);
                    sampleMat3.SetFloat("_NormalStrength", m_parentProject.m_preview.m_normalStrength);
                    sampleMat4.SetFloat("_NormalStrength", m_parentProject.m_preview.m_normalStrength);

                    sampleMat1.SetFloat("_Parallax", m_parentProject.m_preview.m_displacement);
                    sampleMat2.SetFloat("_Displacement", m_parentProject.m_preview.m_displacement);
                    sampleMat3.SetFloat("_Displacement", m_parentProject.m_preview.m_displacement);
                    sampleMat4.SetFloat("_Displacement", m_parentProject.m_preview.m_displacement);

                    sampleMat1.SetFloat("_OcclusionStrength", m_parentProject.m_preview.m_ambientOcclusion);
                    sampleMat2.SetFloat("_AmbientOcclusion", m_parentProject.m_preview.m_ambientOcclusion);
                    sampleMat3.SetFloat("_AmbientOcclusion", m_parentProject.m_preview.m_ambientOcclusion);

                    if (m_parentProject.m_preview.m_specularMapSource == SPECULARMAPSOURCE.SpecularMapAlpha)
                    {
                        sampleMat1.DisableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                        sampleMat1.SetFloat("_SmoothnessTextureChannel", 0);
                    }
                    else
                    {
                        sampleMat1.EnableKeyword("_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A");
                        sampleMat1.SetFloat("_SmoothnessTextureChannel", 1);
                    }

                    sampleMat1.SetFloat("_Glossiness", m_parentProject.m_preview.m_gloss);
                    sampleMat1.SetFloat("_GlossMapScale", m_parentProject.m_preview.m_gloss);

                    if (m_parentProject.m_preview.m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                    {
                        sampleMat2.SetFloat("_SpecularfromRGB", 0);
                        sampleMat2.SetFloat("_SpecularMapPresent", 1);
                    }
                    else
                    {
                        sampleMat2.SetFloat("_SpecularfromRGB", 1);
                        sampleMat2.SetFloat("_SpecularMapPresent", 1);
                    }

                    sampleMat2.SetColor("_SpecularColor", m_parentProject.m_preview.m_specColor);
                    sampleMat2.SetFloat("_Gloss", m_parentProject.m_preview.m_gloss);

                    if (m_parentProject.m_preview.m_SpecularColorSource == SPECULARCOLORSOURCE.uniformColor)
                    {
                        sampleMat3.SetFloat("_SpecularfromRGB", 0);
                        sampleMat3.SetFloat("_SpecularMapPresent", 1);
                    }
                    else
                    {
                        sampleMat3.SetFloat("_SpecularfromRGB", 1);
                        sampleMat3.SetFloat("_SpecularMapPresent", 1);
                    }

                    sampleMat3.SetColor("_SpecularColor", m_parentProject.m_preview.m_specColor);
                    sampleMat3.SetFloat("_Gloss", m_parentProject.m_preview.m_gloss);

                    sampleMat4.SetFloat("_SpecularMapPresent", 1);

                    sampleMat4.SetFloat("_SpecularStrength", m_parentProject.m_preview.m_specStrength);
                    sampleMat4.SetFloat("_Gloss", m_parentProject.m_preview.m_gloss);

                }
            }

            //Actually create the sample materials and refreshes the assets
            AssetDatabase.CreateAsset(sampleMat1, m_outputPath + "/" + m_outputName + "/Sample Materials/" + m_outputName + "_Standard.mat");
            AssetDatabase.CreateAsset(sampleMat2, m_outputPath + "/" + m_outputName + "/Sample Materials/" + m_outputName + "_POM.mat");
            AssetDatabase.CreateAsset(sampleMat3, m_outputPath + "/" + m_outputName + "/Sample Materials/" + m_outputName + "_Tessellated.mat");
            AssetDatabase.CreateAsset(sampleMat4, m_outputPath + "/" + m_outputName + "/Sample Materials/" + m_outputName + "_Mobile.mat");

            AssetDatabase.Refresh();
        }
        #endregion
    }
}
#endif