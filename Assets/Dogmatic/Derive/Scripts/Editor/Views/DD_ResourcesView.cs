// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System;
using UnityEngine;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_ResourcesView : DD_ViewBase
    {
        #region Public Variables
        #endregion

        #region Protected Variables
        #endregion

        #region Private Variables
        DD_Resources m_resources;
        bool m_collaped = false;
        #endregion

        #region Constructor
        public DD_ResourcesView() : base("Resources") { m_resources = new DD_Resources(); }
        #endregion

        #region Main Methods
        public override void UpdateView()
        {
            base.UpdateView();

            if (!m_collaped && DD_EditorUtils.currentProject != null) m_resources.Update();
        }

        public override void OnViewGUI()
        {
            base.OnViewGUI();

            m_viewRect = DD_EditorUtils.viewRect_resourcesView;

            if (m_viewRect.width > 150) GUI.Box(DD_EditorUtils.viewRect_resourcesView, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));
            else GUI.Box(DD_EditorUtils.viewRect_resourcesView, "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            if (m_viewRect.width <= 16 || m_viewRect.height <= 30) m_collaped = true;
            else m_collaped = false;

            if(!m_collaped && DD_EditorUtils.currentProject != null) m_resources.OnResourcesGUI();

            GUILayout.BeginArea(m_viewRect);

            GUILayout.EndArea();
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();
        }
        #endregion

        #region Utils
        #endregion
    }
}
#endif