// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Derive;

namespace DeriveUtils
{
    public static class DD_EditorUtils
    {
        /// <summary>
        /// Using a set of static parameters that must be shared between multiple views and nodes
        /// </summary>
        /// <param name="path"></param>
        /// <param name="wantedName"></param>
        #region Static Variables
        public static bool waitForRepaint = false;

        public static bool triggerSearchStringSelection;
        public static bool showNodeMenu;
        public static Rect nodeMenuRect;

        public static Event currentEvent = null;
        public static Vector2 mousePosInEditor;

        public static Rect windowRect;

        public static Rect viewRect_propertyView;
        public static Rect propertyRect;
        public static Rect propertyFrameRect;

        public static Rect viewRect_workView;
        
        public static Rect viewRect_resourcesView;
        public static Rect resourcesRect;
        public static Rect resourcesFrameRect;

        public static Rect viewRect_headerView;
        public static Rect viewRect_footerView;
        public static Rect viewRect_previewView;

        public static Rect selectionRect;
        public static bool allowSelectionRectRender = true;
        public static bool allowSelection = true;
        public static bool preventSelectionForOneEvent = true;

        public static GUISkin editorSkin = null;

        public static DD_ProjectTemplate currentProject = null;

        public static bool preventNodeMovement = false;

        public static bool allowGridOffset = true;
        public static Vector2 gridOffset;
        public static float zoomFactor = 1;

        public static DD_NodeBase[] nodeBuffer;

        public static int lastControlId = 0;

        public static DD_ResourcesDataTemplate resourcesData;
        public static DD_EditorDataTemplate editorData;
        public static DD_NodeDataTemplate nodeData;
        public static DD_ProjectListTemplate projectManagementData;

        public static bool drawDragTexture;         // If this is true a resource texture is dragged from the resource view to the work view

        public static DD_ProjectManagementWindow projectManagementWindow;
        #endregion

        /// <summary>
        /// Gets the mouse position in the editor window as opposed to the current view
        /// </summary>
        public static void GetMousePosInEditor()
        {
            mousePosInEditor = currentEvent.mousePosition;
        }

        /// <summary>
        /// Creates a project and stores it at the provided path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="wantedName"></param>
        public static void CreateProject(string path, string wantedName)         //returns true, if project was created successfully
        {
            DD_ProjectTemplate currentProject = ScriptableObject.CreateInstance<DD_ProjectTemplate>();

            if (currentProject != null)
            {
                currentProject.name = wantedName;
                currentProject.InitProject();

                AssetDatabase.CreateAsset(currentProject, path + "/" + wantedName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();

                if (currentWindow != null) currentWindow.m_currentProject = currentProject;
            }
            else EditorUtility.DisplayDialog("Error!", "Unable to create project file.", "Ok");
        }

        /// <summary>
        /// Loads a project from a path
        /// </summary>
        /// <param name="promptProjectManager">Closes the project manager if a project was opened</param>
        public static void LoadProject(bool promptProjectManager = false)
        {
            DD_ProjectTemplate currentProject = null;

            string absPath = EditorUtility.OpenFilePanel("Load Project", GetDerivePath() + "My Projects/", "");
            
            int appPathLength = Application.dataPath.Length;
            string relPath = "";

            if (absPath.Length > appPathLength) relPath = absPath.Substring(appPathLength - 6);      // -6 for minus ASSETS folder

            currentProject = (DD_ProjectTemplate)AssetDatabase.LoadAssetAtPath(relPath, typeof(DD_ProjectTemplate));

            if (relPath == "") return;

            if (currentProject != null)
            {
                DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();
                if (currentWindow != null) currentWindow.m_currentProject = currentProject;

                projectManagementWindow.Close();
            }
            else EditorUtility.DisplayDialog("Error!", "Unable to load selected project file.", "Ok");
        }

        /// <summary>
        /// Loads a project from a give path without file dialog
        /// </summary>
        /// <param name="path"></param>
        public static void LoadProject(string path)
        {
            DD_ProjectTemplate currentProject = null;

            currentProject = (DD_ProjectTemplate)AssetDatabase.LoadAssetAtPath(path, typeof(DD_ProjectTemplate));

            if (path == "") return;

            if (currentProject != null)
            {
                DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();
                if (currentWindow != null) currentWindow.m_currentProject = currentProject;

                projectManagementWindow.Close();
            }
            else EditorUtility.DisplayDialog("Error!", "Unable to load selected project file.", "Ok");
        }

        /// <summary>
        /// Unloads a project from the currently open window
        /// Closes the tab if multiple tabs are open
        /// </summary>
        public static void UnloadProject()
        {
            DD_NodeEditorWindow[] dDWindows;
            dDWindows = (DD_NodeEditorWindow[])Resources.FindObjectsOfTypeAll<DD_NodeEditorWindow>();
            DD_NodeEditorWindow currentWindow = (DD_NodeEditorWindow)EditorWindow.focusedWindow;

            if (dDWindows.Length > 1)
            {
                if (currentWindow != null) currentWindow.Close();
            }
            else
            {
                if (currentWindow != null) currentWindow.m_currentProject = null;
            }

        }

        /// <summary>
        /// Draws the grid based on provided parameters
        /// </summary>
        /// <param name="viewRect"></param>
        /// <param name="gridSpacing"></param>
        /// <param name="gridOpacity"></param>
        /// <param name="gridColor"></param>
        public static void DrawGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor)
        {
            gridSpacing = gridSpacing * zoomFactor;

            int widthDivs = Mathf.CeilToInt(viewRect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(viewRect.height / gridSpacing);

            Handles.BeginGUI();

            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            for (int i = 0; i <= widthDivs; i++)
                Handles.DrawLine(new Vector2(viewRect.x + gridSpacing * i - (viewRect.x + gridOffset.x) % gridSpacing, viewRect.y), new Vector2(viewRect.x + gridSpacing * i - (viewRect.x + gridOffset.x) % gridSpacing, viewRect.y + viewRect.height));

            for (int j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector2(viewRect.x, viewRect.y + gridSpacing * j - (viewRect.y + gridOffset.y) % gridSpacing), new Vector2(viewRect.x + viewRect.width, viewRect.y + gridSpacing * j - (viewRect.y + gridOffset.y) % gridSpacing));

            Handles.color = Color.white;

            Handles.EndGUI();
        }

        /// <summary>
        /// Creates node at provided mouse position
        /// Considers various parameters including whether the node is preconnected
        /// </summary>
        /// <param name="currentProject"></param>
        /// <param name="nodeType"></param>
        /// <param name="mousePos"></param>
        /// <param name="preconnect"></param>
        /// <param name="inverseConnection"></param>
        /// <param name="outputIndex"></param>
        public static void CreateNode(DD_ProjectTemplate currentProject, NodeType nodeType, Vector2 mousePos, bool preconnect, bool inverseConnection, int outputIndex)
        {
            if (currentProject != null)
            {
                DD_NodeBase currentNode = null;

                switch (nodeType)
                {
                    #region Data Nodes
                    case NodeType.Color:
                        currentNode = (DD_NodeColor)ScriptableObject.CreateInstance<DD_NodeColor>();
                        currentNode.m_nodeName = HandleDuplicateNames("Color", currentNode);
                        break;

                    case NodeType.Float:
                        currentNode = (DD_NodeFloat)ScriptableObject.CreateInstance<DD_NodeFloat>();
                        currentNode.m_nodeName = HandleDuplicateNames("Float", currentNode);
                        break;

                    case NodeType.RGBAVector:
                        currentNode = (DD_NodeVector)ScriptableObject.CreateInstance<DD_NodeVector>();
                        currentNode.m_nodeName = HandleDuplicateNames("RGBA Vector", currentNode);
                        break;

                    case NodeType.Texture:
                        currentNode = (DD_NodeTexture)ScriptableObject.CreateInstance<DD_NodeTexture>();
                        currentNode.m_nodeName = HandleDuplicateNames("Texture", currentNode);
                        break;
                    #endregion

                    #region Math Nodes
                    case NodeType.Abs:
                        currentNode = (DD_NodeAbs)ScriptableObject.CreateInstance<DD_NodeAbs>();
                        currentNode.m_nodeName = "Abs";
                        break;

                    case NodeType.Clamp:
                        currentNode = (DD_NodeClamp)ScriptableObject.CreateInstance<DD_NodeClamp>();
                        currentNode.m_nodeName = "Clamp";
                        break;

                    case NodeType.Clamp01:
                        currentNode = (DD_NodeClamp01)ScriptableObject.CreateInstance<DD_NodeClamp01>();
                        currentNode.m_nodeName = "Clamp 0-1";
                        break;

                    case NodeType.Fract:
                        currentNode = (DD_NodeFract)ScriptableObject.CreateInstance<DD_NodeFract>();
                        currentNode.m_nodeName = "Fract";
                        break;

                    case NodeType.Lerp:
                        currentNode = (DD_NodeLerp)ScriptableObject.CreateInstance<DD_NodeLerp>();
                        currentNode.m_nodeName = "Lerp";
                        break;

                    case NodeType.Math:
                        currentNode = (DD_NodeMath)ScriptableObject.CreateInstance<DD_NodeMath>();
                        currentNode.m_nodeName = "Add";
                        break;

                    case NodeType.MinMax:
                        currentNode = (DD_NodeMinMax)ScriptableObject.CreateInstance<DD_NodeMinMax>();
                        currentNode.m_nodeName = "Min/Max";
                        break;

                    case NodeType.Negate:
                        currentNode = (DD_NodeNegate)ScriptableObject.CreateInstance<DD_NodeNegate>();
                        currentNode.m_nodeName = "Negate";
                        break;

                    case NodeType.OneMinus:
                        currentNode = (DD_NodeOneMinus)ScriptableObject.CreateInstance<DD_NodeOneMinus>();
                        currentNode.m_nodeName = "One Minus";
                        break;

                    case NodeType.Pow:
                        currentNode = (DD_NodePow)ScriptableObject.CreateInstance<DD_NodePow>();
                        currentNode.m_nodeName = "Power";
                        break;

                    case NodeType.Remap:
                        currentNode = (DD_NodeRemap)ScriptableObject.CreateInstance<DD_NodeRemap>();
                        currentNode.m_nodeName = "Remap";
                        break;

                    case NodeType.Round:
                        currentNode = (DD_NodeRound)ScriptableObject.CreateInstance<DD_NodeRound>();
                        currentNode.m_nodeName = "Round";
                        break;

                    case NodeType.Sqrt:
                        currentNode = (DD_NodeSqrt)ScriptableObject.CreateInstance<DD_NodeSqrt>();
                        currentNode.m_nodeName = "Sqrt";
                        break;

                    case NodeType.Step:
                        currentNode = (DD_NodeStep)ScriptableObject.CreateInstance<DD_NodeStep>();
                        currentNode.m_nodeName = "Step";
                        break;
                    #endregion

                    #region Filter Nodes
                    case NodeType.AOFromHeight:
                        currentNode = (DD_NodeAOFromHeight)ScriptableObject.CreateInstance<DD_NodeAOFromHeight>();
                        currentNode.m_nodeName = "AO From Height";
                        break;

                    case NodeType.Blur:
                        currentNode = (DD_NodeBlur)ScriptableObject.CreateInstance<DD_NodeBlur>();
                        currentNode.m_nodeName = "Blur";
                        break;

                    case NodeType.Brightness:
                        currentNode = (DD_NodeBrightness)ScriptableObject.CreateInstance<DD_NodeBrightness>();
                        currentNode.m_nodeName = "Brightness";
                        break;

                    case NodeType.Contrast:
                        currentNode = (DD_NodeContrast)ScriptableObject.CreateInstance<DD_NodeContrast>();
                        currentNode.m_nodeName = "Contrast";
                        break;

                    case NodeType.Distortion:
                        currentNode = (DD_NodeDistortion)ScriptableObject.CreateInstance<DD_NodeDistortion>();
                        currentNode.m_nodeName = "Distortion";
                        break;

                    case NodeType.Grayscale:
                        currentNode = (DD_NodeGrayscale)ScriptableObject.CreateInstance<DD_NodeGrayscale>();
                        currentNode.m_nodeName = "Grayscale";
                        break;

                    case NodeType.Level:
                        currentNode = (DD_NodeLevel)ScriptableObject.CreateInstance<DD_NodeLevel>();
                        currentNode.m_nodeName = "Level";
                        break;

                    case NodeType.NormalsFromHeight:
                        currentNode = (DD_NodeNormalFromHeight)ScriptableObject.CreateInstance<DD_NodeNormalFromHeight>();
                        currentNode.m_nodeName = "Normals From Height";
                        break;

                    case NodeType.Pixelate:
                        currentNode = (DD_NodePixelate)ScriptableObject.CreateInstance<DD_NodePixelate>();
                        currentNode.m_nodeName = "Pixelate";
                        break;

                    case NodeType.Posterize:
                        currentNode = (DD_NodePosterize)ScriptableObject.CreateInstance<DD_NodePosterize>();
                        currentNode.m_nodeName = "Posterize";
                        break;

                    case NodeType.Saturation:
                        currentNode = (DD_NodeSaturation)ScriptableObject.CreateInstance<DD_NodeSaturation>();
                        currentNode.m_nodeName = "Saturation";
                        break;

                    case NodeType.ShadowFilter:
                        currentNode = (DD_NodeShadowFilter)ScriptableObject.CreateInstance<DD_NodeShadowFilter>();
                        currentNode.m_nodeName = "Shadow Filter";
                        break;
                    #endregion

                    #region Generator Nodes
                    case NodeType.BaseShape:
                        currentNode = (DD_NodeBaseShape)ScriptableObject.CreateInstance<DD_NodeBaseShape>();
                        currentNode.m_nodeName = "Base Shape";
                        break;

                    case NodeType.Gradient:
                        currentNode = (DD_NodeGradient)ScriptableObject.CreateInstance<DD_NodeGradient>();
                        currentNode.m_nodeName = "Gradient";
                        break;

                    case NodeType.Noise:
                        currentNode = (DD_NodeNoise)ScriptableObject.CreateInstance<DD_NodeNoise>();
                        currentNode.m_nodeName = "Noise";
                        break;

                    case NodeType.Plasma:
                        currentNode = (DD_NodePlasma)ScriptableObject.CreateInstance<DD_NodePlasma>();
                        currentNode.m_nodeName = "Plasma";
                        break;

                    case NodeType.Voronoi:
                        currentNode = (DD_NodeVoronoi)ScriptableObject.CreateInstance<DD_NodeVoronoi>();
                        currentNode.m_nodeName = "Voronoi";
                        break;
                    #endregion

                    #region Texture Operation Nodes
                    case NodeType.Append:
                        currentNode = (DD_NodeAppend)ScriptableObject.CreateInstance<DD_NodeAppend>();
                        currentNode.m_nodeName = "Append";
                        break;

                    case NodeType.Blend:
                        currentNode = (DD_NodeBlend)ScriptableObject.CreateInstance<DD_NodeBlend>();
                        currentNode.m_nodeName = "Blend";
                        break;

                    case NodeType.ChannelBreakup:
                        currentNode = (DD_NodeChannelBreakup)ScriptableObject.CreateInstance<DD_NodeChannelBreakup>();
                        currentNode.m_nodeName = "Channel Breakup";
                        break;

                    case NodeType.ChannelMask:
                        currentNode = (DD_NodeChannelMask)ScriptableObject.CreateInstance<DD_NodeChannelMask>();
                        currentNode.m_nodeName = "Channel Mask";
                        break;

                    case NodeType.Cross:
                        currentNode = (DD_NodeCross)ScriptableObject.CreateInstance<DD_NodeCross>();
                        currentNode.m_nodeName = "Cross";
                        break;

                    case NodeType.Dot:
                        currentNode = (DD_NodeDot)ScriptableObject.CreateInstance<DD_NodeDot>();
                        currentNode.m_nodeName = "Dot";
                        break;

                    case NodeType.Length:
                        currentNode = (DD_NodeLength)ScriptableObject.CreateInstance<DD_NodeLength>();
                        currentNode.m_nodeName = "Length";
                        break;

                    case NodeType.Normalize:
                        currentNode = (DD_NodeNormalize)ScriptableObject.CreateInstance<DD_NodeNormalize>();
                        currentNode.m_nodeName = "Normalize";
                        break;
                    #endregion

                    #region Mapping Nodes
                    case NodeType.Inlay:
                        currentNode = (DD_NodeInlay)ScriptableObject.CreateInstance<DD_NodeInlay>();
                        currentNode.m_nodeName = "Inlay";
                        break;

                    case NodeType.Mirror:
                        currentNode = (DD_NodeMirror)ScriptableObject.CreateInstance<DD_NodeMirror>();
                        currentNode.m_nodeName = "Mirror";
                        break;

                    case NodeType.Rotate:
                        currentNode = (DD_NodeRotate)ScriptableObject.CreateInstance<DD_NodeRotate>();
                        currentNode.m_nodeName = "Rotate";
                        break;

                    case NodeType.SeamlessMapping:
                        currentNode = (DD_NodeSeamlessMapping)ScriptableObject.CreateInstance<DD_NodeSeamlessMapping>();
                        currentNode.m_nodeName = "Seamless Mapping";
                        break;

                    case NodeType.Shrink:
                        currentNode = (DD_NodeShrink)ScriptableObject.CreateInstance<DD_NodeShrink>();
                        currentNode.m_nodeName = "Shrink";
                        break;

                    case NodeType.TilingOffset:
                        currentNode = (DD_NodeTilingOffset)ScriptableObject.CreateInstance<DD_NodeTilingOffset>();
                        currentNode.m_nodeName = "Tiling & Offset";
                        break;
                    #endregion

                    #region Misc Nodes
                    case NodeType.GetVariable:
                        currentNode = (DD_NodeGetVariable)ScriptableObject.CreateInstance<DD_NodeGetVariable>();
                        currentNode.m_nodeName = "Get Variable";
                        break;

                    case NodeType.Relay:
                        currentNode = (DD_NodeRelay)ScriptableObject.CreateInstance<DD_NodeRelay>();
                        currentNode.m_nodeName = "Relay";
                        break;

                    case NodeType.SetVariable:
                        currentNode = (DD_NodeSetVariable)ScriptableObject.CreateInstance<DD_NodeSetVariable>();
                        currentNode.m_nodeName = HandleDuplicateNames("MyVariable", currentNode);
                        break;
                    #endregion

                    case NodeType.Master:
                        currentNode = (DD_NodeMaster)ScriptableObject.CreateInstance<DD_NodeMaster>();
                        currentNode.m_nodeName = "Master Node";
                        break;

                    default:
                        break;
                }

                if (currentProject != null)
                {
                    currentNode.name = "";
                    currentNode.InitNode();
                    currentNode.m_nodeRect.position = mousePos / zoomFactor;
                    currentNode.m_parentProject = currentProject;
                    currentProject.m_nodes.Add(currentNode);

                    if (preconnect)
                    {
                        if (!inverseConnection)
                        {
                            if (currentNode.m_inputs != null && currentNode.m_inputs.Count > 0 && currentProject.m_connectionAttemptingNode != null)
                            {
                                currentNode.m_inputs[0].inputtingNode = currentProject.m_connectionAttemptingNode;
                                currentNode.m_inputs[0].outputIndex = outputIndex;
                                currentNode.m_inputs[0].isOccupied = true;
                                currentProject.m_connectionAwaitingNewNode = false;
                            }
                        }
                        else
                        {
                            if (currentNode.m_outputs != null && currentNode.m_outputs.Count > 0 && currentProject.m_connectionAttemptingNode != null)
                            {
                                currentProject.m_inverseConnectionAttemptingInput.inputtingNode = currentNode;
                                currentProject.m_inverseConnectionAttemptingInput.isOccupied = true;
                                currentNode.m_outputs[0].isOccupied = true;
                                currentProject.m_connectionAwaitingNewNode = false;
                            }
                        }
                    }

                    currentNode.hideFlags = HideFlags.HideInHierarchy;

                    AssetDatabase.AddObjectToAsset(currentNode, currentProject);
                    //AssetDatabase.SaveAssets();
                    //AssetDatabase.Refresh();
                }
            }
        }

        /// <summary>
        /// Paste a copied node at provided position
        /// </summary>
        /// <param name="currentProject"></param>
        /// <param name="sourceNode"></param>
        /// <param name="nodePos"></param>
        /// <param name="leftestNode"></param>
        public static void PasteNode(DD_ProjectTemplate currentProject, DD_NodeBase sourceNode, Vector2 nodePos, Vector2 leftestNode)
        {
            if (currentProject != null)
            {
                DD_NodeBase currentNode = null;

                currentNode = ScriptableObject.Instantiate(sourceNode);

                if (currentProject != null)
                {
                    currentNode.name = "";

                    currentNode.InitNode();
                    currentNode.m_nodeRect.position = (nodePos + mousePosInEditor / zoomFactor) - leftestNode;
                    currentNode.m_parentProject = currentProject;
                    currentProject.m_nodes.Add(currentNode);

                    currentProject.m_selectedNodes.Add(currentNode);

                    currentNode.hideFlags = HideFlags.HideInHierarchy;

                    currentNode.m_nodeType = sourceNode.m_nodeType;
                    currentNode.m_redoCalculation = true;

                    if(currentNode.m_nodeType == NodeType.Color ||
                       currentNode.m_nodeType == NodeType.Float ||
                       currentNode.m_nodeType == NodeType.RGBAVector ||
                       currentNode.m_nodeType == NodeType.Texture)
                    {
                        currentNode.m_nodeName = HandleDuplicateNames(currentNode.m_nodeName, currentNode);
                    }

                    AssetDatabase.AddObjectToAsset(currentNode, currentProject);
                    //AssetDatabase.SaveAssets();
                    //AssetDatabase.Refresh();

                    currentEvent.Use();
                }
            }
        }


        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="nodeToDelete"></param>
        public static void DeleteNode(DD_NodeBase nodeToDelete)
        {
            if (currentProject != null)
            {
                foreach (DD_NodeBase node in currentProject.m_nodes)
                {
                    if (node == null) continue;
                    if (node.m_inputs == null) continue;

                    foreach (DD_ConnectionRelay relay in node.m_connectionRelays)
                        if (relay.m_associatedConnector.inputtingNode == nodeToDelete) relay.m_readyToDelete = true;

                    bool redo = false;
                    foreach (DD_InputConnector input in node.m_inputs)
                        if (input != null && nodeToDelete == input.inputtingNode)
                        {
                            input.inputtingNode = null;
                            redo = true;
                        }
                    if (redo) node.m_redoCalculation = true;
                }

                AssetDatabase.RemoveObjectFromAsset(nodeToDelete);
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();

                waitForRepaint = true;
            }
        }

        /// <summary>
        /// Gets the path to the derive folder from /Assets-path
        /// This way the Derive folder can be copied into other folders without losing references
        /// </summary>
        /// <returns></returns>
        public static string GetDerivePath()
        {
            string absolutePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            absolutePath = absolutePath.Replace(@"\", "/");
            absolutePath = absolutePath.Replace("Scripts/Editor/Utils/DD_EditorUtils.cs", "");
            absolutePath = absolutePath.Substring(Application.dataPath.Length - 6);

            return absolutePath;
        }

        /// <summary>
        /// Given an input string and a reference it will look through the node list to finde nodes that have the reference as name
        /// When a duplicate is found, the input is given a duplicate ID equal to the amount of duplicates found
        /// </summary>
        /// <param name="input"></param>
        /// <param name="duplicateNameToFind"></param>
        /// <returns></returns>
        public static string HandleDuplicateNames(string input, DD_NodeBase nodeToSkip)
        {
            bool checkForDuplicates = true;
            int duplicateSuffix = 0;
            while (checkForDuplicates)
            {
                bool foundDuplicate = false;

                foreach (DD_NodeBase node in currentProject.m_nodes)
                {
                    if (node == nodeToSkip) continue;

                    if (duplicateSuffix == 0)
                    {
                        if (input == node.m_nodeName)
                        {
                            duplicateSuffix++;
                            foundDuplicate = true;
                            break;
                        }
                    }
                    else
                    {
                        if (input + "(" + duplicateSuffix.ToString() + ")" == node.m_nodeName)
                        {
                            duplicateSuffix++;
                            foundDuplicate = true;
                            break;
                        }
                    }
                }

                if (!foundDuplicate)
                {
                    checkForDuplicates = false;

                    if (duplicateSuffix != 0) input = input + "(" + duplicateSuffix.ToString() + ")";
                }
            }

            return input;
        }
    }
}
#endif
