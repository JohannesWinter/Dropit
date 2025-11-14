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
    public class DD_WorkView : DD_ViewBase
    {
        #region Public Variables
        public Rect m_windowRect;
        #endregion

        #region Protected Variables
        #endregion

        #region Private Variables
        DD_NodeMenu m_nodeMenu;
        bool m_showSelectionBox;
        bool m_allowShowNodeMenu;
        bool m_allowPanning = false;

        float m_time = 0;
        bool m_zoomIn = false;
        bool m_zoomOut = false;
        float m_zoomDuration = 0.1f;
        float m_targetZoomFactor;
        #endregion

        #region Constructor
        public DD_WorkView() : base("< No Project >") { }
        #endregion

        public override void UpdateView()
        {
            base.UpdateView();

            if (DD_EditorUtils.currentProject != null)
                DD_EditorUtils.currentProject.UpdateProject();

            ///<summary>
            ///Smooth zooming by lerping the zoom factor along time
            /// </summary>
            if (m_zoomIn || m_zoomOut)
            {
                m_time += Time.deltaTime;

                if (DD_EditorUtils.currentProject == null) DD_EditorUtils.zoomFactor = Mathf.Lerp(DD_EditorUtils.zoomFactor, m_targetZoomFactor, m_time / m_zoomDuration);
                else DD_EditorUtils.currentProject.m_zoomFactor = Mathf.Lerp(DD_EditorUtils.zoomFactor, m_targetZoomFactor, m_time / m_zoomDuration);

                if (m_time >= m_zoomDuration)
                {
                    m_zoomIn = false;
                    m_zoomOut = false;
                    m_time = 0;
                }
            }

            /*if(m_allowPanning)
            {
                if (DD_EditorUtils.currentProject != null && DD_EditorUtils.allowGridOffset)
                    DD_EditorUtils.currentProject.m_canvasOffset += 4 * DD_EditorUtils.currentEvent.delta / DD_EditorUtils.zoomFactor;

                if (DD_EditorUtils.allowGridOffset)
                    DD_EditorUtils.gridOffset -= 4 * DD_EditorUtils.currentEvent.delta;

                if (DD_EditorUtils.currentEvent.delta.x != 0 || DD_EditorUtils.currentEvent.delta.y != 0) m_allowShowNodeMenu = false;
            }*/
        }

        #region Main Methods
        public override void OnViewGUI()
        {
            if (m_allowPanning)
            {
                if (DD_EditorUtils.currentProject != null && DD_EditorUtils.allowGridOffset)
                    DD_EditorUtils.currentProject.m_canvasOffset += 0.5f * DD_EditorUtils.currentEvent.delta / DD_EditorUtils.zoomFactor;

                if (DD_EditorUtils.allowGridOffset)
                    DD_EditorUtils.gridOffset -= 0.5f * DD_EditorUtils.currentEvent.delta;

                if (DD_EditorUtils.currentEvent.delta.x != 0 || DD_EditorUtils.currentEvent.delta.y != 0) m_allowShowNodeMenu = false;
            }

            m_viewRect = DD_EditorUtils.viewRect_workView;

            if (m_nodeMenu == null) SetUpContextMenu();

            base.OnViewGUI();

            if (DD_EditorUtils.currentProject != null) m_viewTitle = DD_EditorUtils.currentProject.name;
            else m_viewTitle = "< No Project >";

            GUI.Box(m_viewRect, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("WorkView_BG"));

            //Draw Grid
            DD_EditorUtils.DrawGrid(m_viewRect, 200, 0.06f, Color.white);
            DD_EditorUtils.DrawGrid(m_viewRect, 40, 0.03f, Color.white);

            //Alternatively draw dark grid
            //DD_EditorUtils.DrawGrid(m_viewRect, 150, 0.3f, Color.black);
            //DD_EditorUtils.DrawGrid(m_viewRect, 30, 0.15f, Color.black);

            GUILayout.BeginArea(m_viewRect);

            if (DD_EditorUtils.currentProject != null)
            {
                DD_EditorUtils.currentProject.OnProjectGUI();

                //Handle Undo operation for the entire project file
                try
                {
                    Undo.RecordObject(DD_EditorUtils.currentProject, DD_EditorUtils.currentProject.name);
                }
                catch (Exception e)
                {
                    e.GetType();
                }
                               
            }

            GUILayout.EndArea();


            if (m_showSelectionBox && DD_EditorUtils.allowSelectionRectRender)
                GUI.Box(DD_EditorUtils.selectionRect, "", DD_EditorUtils.editorSkin.GetStyle("SelectionBox"));

            //Show node menu, if allowed
            if (DD_EditorUtils.showNodeMenu)
            {
                DD_EditorUtils.nodeMenuRect.size = new Vector2(250, 250);

                if (m_viewRect.x + m_viewRect.width < DD_EditorUtils.nodeMenuRect.x + DD_EditorUtils.nodeMenuRect.width)
                    DD_EditorUtils.nodeMenuRect.x = m_viewRect.x + m_viewRect.width - 270;

                if (m_viewRect.x > DD_EditorUtils.nodeMenuRect.x)
                    DD_EditorUtils.nodeMenuRect.x = m_viewRect.x + 20;

                if (m_viewRect.y + m_viewRect.height < DD_EditorUtils.nodeMenuRect.y + DD_EditorUtils.nodeMenuRect.height)
                    DD_EditorUtils.nodeMenuRect.y = m_viewRect.y + m_viewRect.height - 270;

                if (m_viewRect.Contains(new Vector2(DD_EditorUtils.nodeMenuRect.x + DD_EditorUtils.nodeMenuRect.width, DD_EditorUtils.nodeMenuRect.y + DD_EditorUtils.nodeMenuRect.height)))
                {
                    m_nodeMenu.m_propertyViewWidth = m_viewRect.x;
                    m_nodeMenu.OnMenuGUI(new Rect(DD_EditorUtils.nodeMenuRect));
                }
            }

            //This should be in ProcessEvents, but RMB + mouseUp doesn't trigger if Events are processed in Update rather than OnGUI - Don't know why.
            //Handles opening of the node menu
            if (m_viewRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                if (DD_EditorUtils.currentEvent.button == 1)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                    {
                        if (m_allowShowNodeMenu)
                        {
                            m_nodeMenu.m_selectSearchString = true;
                            DD_EditorUtils.showNodeMenu = true;
                            DD_EditorUtils.nodeMenuRect.position = DD_EditorUtils.currentEvent.mousePosition;
                        }
                        else m_allowShowNodeMenu = true;
                    }
                }
            }

            //This should be in ProcessEvents, but RMB + mouseUp doesn't trigger if Events are processed in Update rather than OnGUI - Don't know why.
            //Handles opening of the node menu for connections awaiting nodes
            if (DD_EditorUtils.triggerSearchStringSelection)
            {
                if (m_allowShowNodeMenu)
                {
                    m_nodeMenu.m_selectSearchString = true;
                    DD_EditorUtils.showNodeMenu = true;
                    DD_EditorUtils.nodeMenuRect.position = DD_EditorUtils.mousePosInEditor;//.currentEvent.mousePosition;
                    DD_EditorUtils.triggerSearchStringSelection = false;
                }
                else m_allowShowNodeMenu = true;
            }

            GUI.Box(new Rect(m_viewRect.position.x + 10, m_viewRect.position.y + m_viewRect.height - 80, 473, 70), DD_EditorUtils.editorData.workViewLogo, GUIStyle.none);
        }

        public override void ProcessEvents()
        {
            if (m_viewRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                //Panning the canvass
                if (DD_EditorUtils.currentEvent.button == 1 || DD_EditorUtils.currentEvent.button == 2)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseDown) m_allowPanning = true;
                    DD_EditorUtils.allowSelectionRectRender = false;
                }

                //Zooming
                if (DD_EditorUtils.currentEvent.type == EventType.ScrollWheel && !DD_EditorUtils.showNodeMenu)
                {
                    if (DD_EditorUtils.currentEvent.delta.y < 0)
                    {
                        m_targetZoomFactor = Mathf.Min(DD_EditorUtils.zoomFactor + 0.1f, 1);
                        m_zoomIn = true;
                    }
                    else
                    {
                        m_targetZoomFactor = Mathf.Max(DD_EditorUtils.zoomFactor - 0.1f, 0.4f);
                        m_zoomOut = true;
                    }
                }
            }

            if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp) m_allowPanning = false;

            ///<summary>
            ///Handles selection box
            ///Makes sure selection box select from any direction to any direction
            /// </summary>
            if (DD_EditorUtils.currentEvent.button == 0)
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown && DD_EditorUtils.viewRect_workView.Contains(DD_EditorUtils.mousePosInEditor)) m_showSelectionBox = true;
                if (DD_EditorUtils.currentEvent.type == EventType.MouseUp) DD_EditorUtils.allowSelectionRectRender = true;

                if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                {
                    if (m_currentProject != null)
                    {
                        if (DD_EditorUtils.selectionRect.position.x < DD_EditorUtils.selectionRect.position.x + DD_EditorUtils.selectionRect.width)
                        {
                            if (DD_EditorUtils.selectionRect.position.y < DD_EditorUtils.selectionRect.position.y + DD_EditorUtils.selectionRect.height)
                                m_currentProject.m_selectionRect = DD_EditorUtils.selectionRect;
                            else
                                m_currentProject.m_selectionRect = new Rect(DD_EditorUtils.selectionRect.x, DD_EditorUtils.selectionRect.y + DD_EditorUtils.selectionRect.height, DD_EditorUtils.selectionRect.width, -DD_EditorUtils.selectionRect.height);
                        }
                        else
                        {
                            if (DD_EditorUtils.selectionRect.position.y < DD_EditorUtils.selectionRect.position.y + DD_EditorUtils.selectionRect.height)
                                m_currentProject.m_selectionRect = new Rect(DD_EditorUtils.selectionRect.position.x + DD_EditorUtils.selectionRect.width, DD_EditorUtils.selectionRect.position.y, -DD_EditorUtils.selectionRect.width, DD_EditorUtils.selectionRect.height);
                            else
                                m_currentProject.m_selectionRect = new Rect(DD_EditorUtils.selectionRect.position.x + DD_EditorUtils.selectionRect.width, DD_EditorUtils.selectionRect.y + DD_EditorUtils.selectionRect.height, -DD_EditorUtils.selectionRect.width, -DD_EditorUtils.selectionRect.height);
                        }

                        if (m_showSelectionBox)
                        {
                            m_showSelectionBox = false;
                            m_currentProject.m_boxSelect = true;
                            DD_EditorUtils.selectionRect.position = DD_EditorUtils.currentEvent.mousePosition;
                            DD_EditorUtils.selectionRect.size = Vector2.zero;
                        }
                    }
                }

                if (DD_EditorUtils.currentEvent.type == EventType.MouseDrag && DD_EditorUtils.allowSelectionRectRender)
                    DD_EditorUtils.selectionRect.size = DD_EditorUtils.currentEvent.mousePosition - DD_EditorUtils.selectionRect.position;
            }

            if (DD_EditorUtils.currentEvent.rawType == EventType.MouseMove)
            {
                m_showSelectionBox = false;
                DD_EditorUtils.selectionRect.position = DD_EditorUtils.currentEvent.mousePosition;
                DD_EditorUtils.selectionRect.size = Vector2.zero;
            }

            //Closes the node menu, if clicked outside of it
            if (DD_EditorUtils.currentEvent.button == 0)
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {
                    if (!DD_EditorUtils.nodeMenuRect.Contains(DD_EditorUtils.currentEvent.mousePosition) && DD_EditorUtils.showNodeMenu)
                    {
                        DD_EditorUtils.showNodeMenu = false;
                        if (DD_EditorUtils.currentProject != null)
                            if (DD_EditorUtils.currentProject.m_connectionAttemptingNode != null)
                                if (DD_EditorUtils.currentProject.m_connectionAttemptingNode.m_outputs != null)
                                    if (DD_EditorUtils.currentProject.m_connectionAttemptingNode.m_outputs.Count > 0)
                                        DD_EditorUtils.currentProject.m_connectionAttemptingNode.m_outputs[DD_EditorUtils.currentProject.m_outputIndex].isOccupied = false;
                    }
                }
            }
        }
        #endregion

        #region Utils
        public void SetUpContextMenu()
        {
            m_nodeMenu = new DD_NodeMenu();
        }
        #endregion
    }
}
#endif