// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_PropertyView : DD_ViewBase
    {
        #region Public Variables
        // Variables needed for view resizing
        public float m_viewWidth = 300;
        public float m_maxWidth = 500;
        public float m_rightSideWidth = 300;
        #endregion

        #region Protected Variables
        #endregion

        #region Private Variables
        // Variables needed for view resizing
        Rect m_resizeRect;
        bool m_collapsed = false;
        public bool m_dragging = false;
        float m_viewWidthCache = 300;

        DD_NodeBase m_lastSelectedNode;
        DD_NodeBase m_nodeDisplayingProperties;

        public float m_framerectWidth = 0;
        float m_framerectHeight = 0;
        #endregion

        #region Constructor
        public DD_PropertyView() : base("Properties") { }
        #endregion

        #region Main Methods
        public override void UpdateView()
        {
            base.UpdateView();
        }

        public override void OnViewGUI()
        {
            base.OnViewGUI();

            m_viewRect = DD_EditorUtils.viewRect_propertyView;
            m_rightSideWidth = DD_EditorUtils.viewRect_previewView.width;
            m_resizeRect = new Rect(m_viewRect.x + m_viewRect.width - 12, m_viewRect.y + m_viewRect.height / 2 - 128, 12, 256);

            if (m_viewWidth > 150) GUI.Box(m_viewRect, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));
            else GUI.Box(m_viewRect, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            EditorGUIUtility.AddCursorRect(m_resizeRect, MouseCursor.ResizeHorizontal);
            GUI.Box(m_resizeRect, "", DD_EditorUtils.editorSkin.GetStyle("HorizontalResizer"));

            ResizeView();

            ///<summary>
            ///This area looks for the currently selected node
            ///If only one node is selected, the node's DrawProperties()-method is called
            ///This will make sure, that the selected node's properties are being drawn in the property view
            /// </summary>
            if (DD_EditorUtils.currentProject != null)
            {
                if (DD_EditorUtils.currentProject.m_selectedNodes != null)
                {
                    if (DD_EditorUtils.currentProject.m_selectedNodes.Count == 1)
                    {
                        //Make sure properties lose focus when different node is selected
                        if (m_lastSelectedNode != DD_EditorUtils.currentProject.m_selectedNodes[0])
                        {
                            m_lastSelectedNode = DD_EditorUtils.currentProject.m_selectedNodes[0];
                            GUIUtility.keyboardControl = 0; 
                        }
                    }
                    else
                    {
                        //Make sure the properties of the master node are shown when more than one node is selected or when no node is selected
                        if (m_lastSelectedNode != null)
                        {
                            if (m_lastSelectedNode.m_nodeType != NodeType.Master)
                                foreach (DD_NodeBase node in DD_EditorUtils.currentProject.m_nodes) if (node.m_nodeType == NodeType.Master) m_lastSelectedNode = node;
                        }
                        else
                        {
                            foreach (DD_NodeBase node in DD_EditorUtils.currentProject.m_nodes) if (node.m_nodeType == NodeType.Master) m_lastSelectedNode = node;
                        }
                    }
                }

                if (m_lastSelectedNode != null)
                {
                    //Cache the label width, as it needs to be changed for drawing the node properties.
                    //This will be reverted after drawing the properties to avoid altering other editor windows and inspectors
                    float labelWidthCache = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 5;

                    //Calculate the size of the property rect based on the size of the property view rect and use the current event if it changed
                    EditorGUI.BeginChangeCheck();
                    DD_EditorUtils.propertyRect = new Rect(DD_EditorUtils.viewRect_propertyView.position + new Vector2(12, 40), DD_EditorUtils.viewRect_propertyView.size - new Vector2(24, 0));
                    //if (EditorGUI.EndChangeCheck()) DD_EditorUtils.currentEvent.Use();

                    //Use the current event if the node has been clicked
                    //if (DD_EditorUtils.currentEvent.type == EventType.MouseDown || DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                    //    if (m_lastSelectedNode.m_scaledNodeRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                    //        DD_EditorUtils.currentEvent.Use();

                    GUILayout.BeginArea(DD_EditorUtils.propertyRect);

                    //Calculate parameters for the scroll view inside the property rect and set up the scroll view
                    float scrollviewWidth = DD_EditorUtils.propertyRect.width;
                    float scrollviewHeight = DD_EditorUtils.propertyRect.height - DD_EditorUtils.viewRect_footerView.height - DD_EditorUtils.viewRect_headerView.height;

                    m_lastSelectedNode.m_scrollPos = EditorGUILayout.BeginScrollView(m_lastSelectedNode.m_scrollPos, GUILayout.Width(scrollviewWidth), GUILayout.Height(scrollviewHeight));

                    //Draw the background for the properties
                    GUI.Box(new Rect(0, 0, m_framerectWidth, m_framerectHeight), "", DD_EditorUtils.editorSkin.GetStyle("PropertyFrame"));

                    //Call the node's DrawProperties()-method to draw the properties into the property view
                    DD_EditorUtils.propertyFrameRect = EditorGUILayout.BeginVertical();

                    //////////////////////THIS MIGHT HAVE TO BE PLACED AFTER EndVertical()!!!!!!!!!!!!!!!
                    if (DD_EditorUtils.propertyFrameRect.width != 0)
                        m_framerectWidth = DD_EditorUtils.propertyFrameRect.width;

                    if (DD_EditorUtils.propertyFrameRect.height != 0)
                        m_framerectHeight = DD_EditorUtils.propertyFrameRect.height;

                    if (DD_EditorUtils.waitForRepaint)
                    {
                        if (DD_EditorUtils.currentEvent.type == EventType.Repaint)
                        {
                            DD_EditorUtils.waitForRepaint = false;
                        }
                    }
                    else m_lastSelectedNode.DrawProperties();
                    
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndScrollView();
                    GUILayout.EndArea();

                    //Revert label width settings
                    EditorGUIUtility.labelWidth = labelWidthCache;
                }
                    if (DD_EditorUtils.currentProject.m_selectedNodes.Count == 1)
                    {

                    }
            }
                
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();

            ///<summary>
            ///Controls the resizing of the property view area
            ///When dragging is enabled the width of the property view area will resize according to the mouse movement
            /// </summary>
            if (m_resizeRect.Contains(DD_EditorUtils.currentEvent.mousePosition) && DD_EditorUtils.currentEvent.button == 0)
            {

                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {
                    m_dragging = true;
                    DD_EditorUtils.preventNodeMovement = true;
                    DD_EditorUtils.allowSelection = false;
                    DD_EditorUtils.allowSelectionRectRender = false;
                }
            }

            if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp || DD_EditorUtils.currentEvent.rawType == EventType.MouseMove)
            {
                if (m_dragging)
                {
                    m_dragging = false;
                    DD_EditorUtils.preventNodeMovement = false;
                    DD_EditorUtils.allowSelection = true;
                    DD_EditorUtils.allowSelectionRectRender = true;

                    DD_EditorUtils.currentEvent.Use();
                }
            }

            if (m_viewRect.Contains(DD_EditorUtils.mousePosInEditor))
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {
                    DD_EditorUtils.preventNodeMovement = true;
                    DD_EditorUtils.allowSelection = false;
                    DD_EditorUtils.allowSelectionRectRender = false;
                    DD_EditorUtils.preventSelectionForOneEvent = true;
                }
            }
        }
        #endregion

        #region Utils

        /// <summary>
        /// Handles the actual resizing of the property view
        /// </summary>
        void ResizeView()
        {
            if (m_collapsed) m_viewWidth = 16;
            else m_viewWidth = Mathf.Min(Mathf.Max(m_viewWidthCache, 200), m_maxWidth);

            if (m_dragging)
            {
                m_viewWidthCache = DD_EditorUtils.currentEvent.mousePosition.x + 8;

                if (m_viewWidthCache >= 100) m_collapsed = false;
                else m_collapsed = true;
            }

            m_maxWidth = DD_EditorUtils.windowRect.width - m_rightSideWidth - 200;      //window width minus min right menu width and min canvas width (200 each);

            if (m_viewWidthCache > m_maxWidth) m_viewWidthCache = m_maxWidth;

            //if(DD_EditorUtils.currentProject != null)
            //    if (DD_EditorUtils.currentProject.m_selectedNodes != null)
            //        if (DD_EditorUtils.currentProject.m_selectedNodes.Count == 1)
            //            DD_EditorUtils.currentProject.m_selectedNodes[0].m_redoCalculation = true;
        }
        #endregion
    }
}
#endif