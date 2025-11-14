// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System;
using UnityEngine;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_FooterView : DD_ViewBase
    {
        #region Public Variables
        #endregion

        #region Protected Variables
        #endregion

        #region Constructor
        public DD_FooterView() : base("") { }
        #endregion

        #region Main Methods
        public override void OnViewGUI()
        {
            m_viewRect = DD_EditorUtils.viewRect_footerView;

            base.OnViewGUI();

            GUI.Box(DD_EditorUtils.viewRect_footerView, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("Footer_BG"));

            GUIStyle versionStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("Footer_BG"));
            versionStyle.alignment = TextAnchor.MiddleLeft;
            versionStyle.normal.background = null;
            versionStyle.fontStyle = FontStyle.Bold;

            GUIStyle copyrightStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("Footer_BG"));
            copyrightStyle.alignment = TextAnchor.MiddleRight;
            copyrightStyle.contentOffset = new Vector2(-10, 7);
            copyrightStyle.normal.background = null;
            copyrightStyle.fontStyle = FontStyle.Bold;

            GUILayout.BeginArea(m_viewRect);
            GUILayout.BeginHorizontal();

            GUILayout.Label("Derive v" + DD_EditorUtils.editorData.versionNumber + " [BETA]", versionStyle);
            GUILayout.Label("© Dogmatic 2022", copyrightStyle);

            GUILayout.EndHorizontal();
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