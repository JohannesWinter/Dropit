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
    public class DD_PreviewView : DD_ViewBase
    {
        #region Public Variables
        // Variables needed for view resizing
        public float m_viewWidth = 300;
        public float m_maxWidth = 500;
        public float m_viewHeight = 400;
        public float m_maxHeight = 500;
        public float m_leftSideWidth = 300;
        #endregion

        #region Protected Variables
        #endregion

        #region Private Variables
        // Variables needed for view resizing
        Rect m_resizeRect;
        bool m_collapsedHorizontal = false;
        bool m_collapsedVertical = false;
        bool m_maxedVertical = false;
        public bool m_dragging = false;
        float m_viewWidthCache = 300;
        float m_viewHeightCache = 400;
        #endregion

        #region Constructor
        public DD_PreviewView() : base("Preview") { }
        #endregion

        #region Main Methods
        public override void UpdateView()
        {
            base.UpdateView();

            if (DD_EditorUtils.currentProject != null)
                if (DD_EditorUtils.currentProject.m_preview != null)
                    DD_EditorUtils.currentProject.m_preview.Update();
        }

        public override void OnViewGUI()
        {
            base.OnViewGUI();

            m_viewRect = DD_EditorUtils.viewRect_previewView;
            m_leftSideWidth = DD_EditorUtils.viewRect_propertyView.width;
            m_resizeRect = new Rect(m_viewRect.x, m_viewRect.y + m_viewRect.height - 24, 24, 24);

            if (m_viewWidth > 150) GUI.Box(DD_EditorUtils.viewRect_previewView, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));
            else GUI.Box(m_viewRect, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            GUI.Box(m_resizeRect, "", DD_EditorUtils.editorSkin.GetStyle("Resizer"));
            EditorGUIUtility.AddCursorRect(m_resizeRect, MouseCursor.MoveArrow);

            ResizeView();

            GUILayout.BeginArea(m_viewRect);

            GUILayout.EndArea();

            if (!m_collapsedHorizontal && !m_collapsedVertical)
                if (DD_EditorUtils.currentProject != null)
                    if (DD_EditorUtils.currentProject.m_preview != null)
                        DD_EditorUtils.currentProject.m_preview.OnPreviewGUI();
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();

            //Handles view resizing
            if (m_resizeRect.Contains(DD_EditorUtils.currentEvent.mousePosition) && DD_EditorUtils.currentEvent.button == 0)
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {
                    DD_EditorUtils.preventNodeMovement = true;
                    m_dragging = true;
                    DD_EditorUtils.allowSelection = false;
                    DD_EditorUtils.allowSelectionRectRender = false;
                }

                DD_EditorUtils.allowSelectionRectRender = false;
            }

            if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp || DD_EditorUtils.currentEvent.rawType == EventType.MouseMove)
            {
                m_dragging = false;
                DD_EditorUtils.preventNodeMovement = false;
                DD_EditorUtils.allowSelection = true;
                DD_EditorUtils.allowSelectionRectRender = true;
            }

        }
        #endregion

        #region Utils

        /// <summary>
        /// Further handles view resizing for a smooth and intuitive control
        /// </summary>
        void ResizeView()
        {

            if (m_collapsedHorizontal) m_viewWidth = 16;
            else m_viewWidth = Mathf.Min(Mathf.Max(m_viewWidthCache, 200), m_maxWidth);

            if (m_collapsedVertical) m_viewHeight = 30;
            else if (m_maxedVertical) m_viewHeight = DD_EditorUtils.windowRect.height - 60;
            else m_viewHeight = Mathf.Min(Mathf.Max(m_viewHeightCache, 200), DD_EditorUtils.windowRect.height - 260);


            if (m_dragging)
            {
                m_viewWidthCache = DD_EditorUtils.windowRect.width - DD_EditorUtils.currentEvent.mousePosition.x;
                m_viewHeightCache = DD_EditorUtils.currentEvent.mousePosition.y;

                if (m_viewWidthCache >= 100) m_collapsedHorizontal = false;
                else m_collapsedHorizontal = true;

                if (m_viewHeightCache >= 100) m_collapsedVertical = false;
                else m_collapsedVertical = true;

                if (m_viewHeightCache < DD_EditorUtils.windowRect.height - 160) m_maxedVertical = false;
                else m_maxedVertical = true;
            }

            m_maxWidth = DD_EditorUtils.windowRect.width - 400;      //window width minus min left menu width and min canvas width (200 each);

            if (m_viewWidthCache > m_maxWidth) m_viewWidthCache = m_maxWidth;
        }
        #endregion
    }
}
#endif