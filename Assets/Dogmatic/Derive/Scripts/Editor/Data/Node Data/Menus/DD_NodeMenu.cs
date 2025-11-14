// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DeriveUtils;

namespace Derive
{
    public class DD_NodeMenu : DD_MenuBase
    {
        #region Public Variables
        public float m_propertyViewWidth;

        public bool m_selectSearchString = false;
        #endregion

        #region private Variables
        string m_searchString = "";

        bool m_showDataNodes = false;
        bool m_showMathNodes = false;
        bool m_showFilterNodes = false;
        bool m_showGeneratorNodes = false;
        bool m_showTextureOperationNodes = false;
        bool m_showMappingNodes = false;
        bool m_showMiscNodes = false;

        Vector2 scrollPos;
        bool resetFoldouts = false;

        /// <summary>
        /// Arrays representing node groups
        /// Each node group contains nodes of the group
        /// </summary>
        string[] m_dataNodes = new string[] { "Float", "Color", "RGBA Vector", "Texture" };
        string[] m_mathNodes = new string[] { "Abs", "Clamp", "Clamp 0-1", "Fract", "Lerp", "Math", "MinMax", "Negate", "One Minus", "Power", "Remap", "Round", "Sqrt", "Step",};
        string[] m_filterNodes = new string[] { "AO From Height", "Blur", "Brightness", "Contrast", "Distortion", "Grayscale", "Level", "Normals From Height", "Pixelate", "Posterize", "Saturation", "Shadow Filter" };
        string[] m_generatorNodes = new string[] { "Base Shape", "Gradient", "Noise", "Plasma", "Voronoi" };
        string[] m_textureOperationNodes = new string[] { "Append", "Blend", "Channel Breakup", "Channel Mask", "Cross", "Dot", "Length", "Normalize" };
        string[] m_mappingNodes = new string[] { "Inlay", "Mirror", "Rotate", "Seamless Mapping", "Shrink", "Tiling & Offset" };
        string[] m_miscNodes = new string[] { "Get Variable", "Relay", "Set Variable" };

        TextEditor textEditor;
        #endregion

        #region Constructor
        #endregion

        #region Main Methods
        public override void OnMenuGUI(Rect menuRect)
        {
            base.OnMenuGUI(menuRect);

            if (textEditor == null) textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            Rect menuGUIRect = new Rect(menuRect.x + 6, menuRect.y + 6, menuRect.width - 12, menuRect.height - 12);

            GUI.Box(menuRect, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            GUILayout.BeginArea(menuGUIRect);

            EditorGUILayout.BeginHorizontal();

            //Draw search field
            GUI.SetNextControlName("Label");
            EditorGUILayout.LabelField("", DD_EditorUtils.editorSkin.GetStyle("SearchIcon"), GUILayout.Width(20), GUILayout.Height(20));
            if (m_selectSearchString) GUI.FocusControl("Label");

            GUI.SetNextControlName("SearchField");
            m_searchString = EditorGUILayout.TextField(m_searchString, DD_EditorUtils.editorSkin.GetStyle("TextInput"), GUILayout.Height(20));
            if (m_selectSearchString) EditorGUI.FocusTextInControl("SearchField");// GUI.FocusControl("SearchField");

            ///<summary>
            ///Make sure input is always highlighted when node menu opens
            ///makes it quicker to delete previous entry, but starts with it in case the same node a the previous one needs to be added again
            /// </summary>
            if (m_selectSearchString)
            {
                m_selectSearchString = false;
                textEditor.OnFocus();
                textEditor.cursorIndex = 0; //CursorStartPosition;
                textEditor.selectIndex = m_searchString.Length;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            if (m_searchString.Length > 0)
            {
                m_showDataNodes = m_showMathNodes = m_showFilterNodes = m_showMappingNodes = true;
                resetFoldouts = true;
            }
            else
            {
                if (resetFoldouts)
                {
                    resetFoldouts = false;
                    m_showDataNodes = m_showMathNodes = m_showFilterNodes = m_showMappingNodes = false;
                }
            }

            m_showDataNodes = DrawNodeList(m_dataNodes, m_showDataNodes, "Data");
            m_showMathNodes = DrawNodeList(m_mathNodes, m_showMathNodes, "Math");
            m_showFilterNodes = DrawNodeList(m_filterNodes, m_showFilterNodes, "Filters");
            m_showGeneratorNodes = DrawNodeList(m_generatorNodes, m_showGeneratorNodes, "Generators");
            m_showTextureOperationNodes = DrawNodeList(m_textureOperationNodes, m_showTextureOperationNodes, "Texture Operations");
            m_showMappingNodes = DrawNodeList(m_mappingNodes, m_showMappingNodes, "Mapping");
            m_showMiscNodes = DrawNodeList(m_miscNodes, m_showMiscNodes, "Misc");

            EditorGUILayout.EndScrollView();

            GUILayout.EndArea();

            ProcessEvents(menuRect);
        }

        void ProcessEvents(Rect menuRect)
        {
            if (!menuRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                if (DD_EditorUtils.currentEvent.button == 0)
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                        if (DD_EditorUtils.currentProject != null)
                            DD_EditorUtils.currentProject.m_connectionAwaitingNewNode = false;
        }
        #endregion

        #region Utility Methods

        bool DrawNodeList(string[] nodes, bool foldOut, string foldoutName)
        {
            GUIStyle foldOutStyle = new GUIStyle();
            foldOutStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
            foldOutStyle.onNormal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
            foldOutStyle.fontSize = 14;
            foldOutStyle.fontStyle = FontStyle.Bold;

            ///<summary>
            ///Controls how node list should behave when
            ///1. search string is not empty
            ///2. search string is empty
            ///Aim at creating a convenient intuitive navigation
            /// </summary>
            if (m_searchString.Length > 0)
            {
                resetFoldouts = true;

                foldOut = true;

                int numberOfEntries = 0;

                foreach (string entry in nodes)
                    if (entry.Contains(m_searchString) || entry.ToLower().Contains(m_searchString)) numberOfEntries++;

                if (numberOfEntries > 0)
                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("", DD_EditorUtils.editorSkin.FindStyle("ArrowDown"), GUILayout.Width(20)))
                        foldOut = false;

                    EditorGUILayout.LabelField(foldoutName, foldOutStyle, GUILayout.Width(140));

                    EditorGUILayout.EndHorizontal();

                    if (foldOut)
                        foreach (string entry in nodes)
                            if (entry.Contains(m_searchString) || entry.ToLower().Contains(m_searchString))
                                if (GUILayout.Button(entry, DD_EditorUtils.editorSkin.GetStyle("MenuItemInbound"), GUILayout.Width(220), GUILayout.Height(20)))
                                {
                                    MenuCallback(entry);
                                    DD_EditorUtils.showNodeMenu = false;
                                    DD_EditorUtils.currentProject.m_connectionAwaitingNewNode = false;
                                }
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                if (!foldOut)
                {
                    if (GUILayout.Button("", DD_EditorUtils.editorSkin.FindStyle("ArrowRight"), GUILayout.Width(20)))
                        foldOut = true;
                }
                else
                {
                    if (GUILayout.Button("", DD_EditorUtils.editorSkin.FindStyle("ArrowDown"), GUILayout.Width(20)))
                        foldOut = false;
                }

                EditorGUILayout.LabelField(foldoutName, foldOutStyle, GUILayout.Width(140));

                EditorGUILayout.EndHorizontal();

                if (foldOut)
                    foreach (string entry in nodes)
                        if (GUILayout.Button(entry, DD_EditorUtils.editorSkin.GetStyle("MenuItemInbound"), GUILayout.Width(220), GUILayout.Height(20)))
                        {
                            MenuCallback(entry);
                            DD_EditorUtils.showNodeMenu = false;
                            DD_EditorUtils.currentProject.m_connectionAwaitingNewNode = false;
                        }
            }

            return foldOut;
        }

        /// <summary>
        /// Calls create node in the editor utilities for the selected node type
        /// </summary>
        /// <param name="entry"></param>
        void MenuCallback(string entry)
        {

            if (DD_EditorUtils.currentProject == null)
            {
                EditorUtility.DisplayDialog("Error", "Cannot add node to empty project. You must create or load a project first.", "Ok");
                return;
            }

            Vector2 mousePosInCanvas = DD_EditorUtils.mousePosInEditor;

            switch (entry)
            {
                #region Data Nodes
                case "Color":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Color, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Float":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Float, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "RGBA Vector":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.RGBAVector, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Texture":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Texture, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Math Nodes
                case "Abs":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Abs, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Clamp":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Clamp, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Clamp 0-1":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Clamp01, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Fract":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Fract, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Lerp":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Lerp, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Math":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Math, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "MinMax":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.MinMax, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Negate":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Negate, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "One Minus":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.OneMinus, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Power":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Pow, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Remap":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Remap, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Round":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Round, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Sqrt":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Sqrt, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Step":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Step, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Filter Nodes
                case "AO From Height":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.AOFromHeight, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Blur":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Blur, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Brightness":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Brightness, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Contrast":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Contrast, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Distortion":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Distortion, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Grayscale":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Grayscale, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Level":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Level, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Normals From Height":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.NormalsFromHeight, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Pixelate":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Pixelate, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Posterize":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Posterize, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Saturation":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Saturation, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Shadow Filter":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.ShadowFilter, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Generator Nodes
                case "Base Shape":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.BaseShape, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Gradient":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Gradient, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Noise":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Noise, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Plasma":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Plasma, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Voronoi":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Voronoi, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Texture operation Nodes
                case "Append":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Append, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Blend":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Blend, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Channel Breakup":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.ChannelBreakup, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Channel Mask":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.ChannelMask, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Cross":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Cross, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Dot":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Dot, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Length":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Length, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Normalize":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Normalize, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Mapping Nodes
                case "Inlay":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Inlay, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Mirror":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Mirror, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Rotate":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Rotate, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Seamless Mapping":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.SeamlessMapping, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Shrink":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Shrink, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Tiling & Offset":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.TilingOffset, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                #region Misc Nodes
                case "Get Variable":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.GetVariable, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Relay":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Relay, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;

                case "Set Variable":
                    DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.SetVariable, mousePosInCanvas, DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);
                    break;
                #endregion

                default:
                    break;
            }
        }
        #endregion
    }
}
#endif