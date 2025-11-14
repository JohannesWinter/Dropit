// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System;
using UnityEngine;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_ViewBase
    {
        #region Public Variables
        public Rect m_viewRect;
        #endregion

        #region Protected Variables
        protected string m_viewTitle;
        protected DD_ProjectTemplate m_currentProject;
        #endregion

        #region Constructors
        public DD_ViewBase(string title) { m_viewTitle = title; }
        #endregion

        #region Main Methods
        public virtual void UpdateView()
        {
            
        }

        public virtual void OnViewGUI()
        {
            //Set the current view graph
            this.m_currentProject = DD_EditorUtils.currentProject;

            if (DD_EditorUtils.currentEvent != null)
                ProcessEvents();
        }

        public virtual void ProcessEvents()
        {
            /*if (m_viewRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                    DD_EditorUtils.allowSelectionRectRender = false;

                if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp)
                    DD_EditorUtils.allowSelectionRectRender = true;
            }

            if (DD_EditorUtils.currentEvent.type == EventType.MouseMove)
                DD_EditorUtils.allowSelectionRectRender = true;*/
        }
        #endregion

        #region Utility Methods
        #endregion
    }
}
#endif