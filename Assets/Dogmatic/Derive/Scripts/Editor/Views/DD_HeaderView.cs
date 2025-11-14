// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System;
using UnityEngine;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_HeaderView : DD_ViewBase
    {
        #region Public Variables
        #endregion

        #region Private Variables
        #endregion

        #region Constructor
        public DD_HeaderView() : base("") { }
        #endregion

        #region Main Methods
        public override void UpdateView()
        {
            base.UpdateView();

            m_viewRect = DD_EditorUtils.viewRect_headerView;
        }

        public override void OnViewGUI()
        {

            base.OnViewGUI();

            GUI.Box(DD_EditorUtils.viewRect_headerView, m_viewTitle, DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            GUILayout.BeginArea(m_viewRect);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create", DD_EditorUtils.editorSkin.GetStyle("MenuInbound"), GUILayout.Width(90), GUILayout.Height(30)))
                MenuCallback("Create");

            GUILayout.Box("", DD_EditorUtils.editorSkin.GetStyle("HorizontalSeparator"), GUILayout.Width(3), GUILayout.Height(30));

            if (GUILayout.Button("Load", DD_EditorUtils.editorSkin.GetStyle("MenuInbound"), GUILayout.Width(90), GUILayout.Height(30)))
                MenuCallback("Load");

            GUILayout.Box("", DD_EditorUtils.editorSkin.GetStyle("HorizontalSeparator"), GUILayout.Width(3), GUILayout.Height(30));

            if (DD_EditorUtils.currentProject != null)
            {
                if (GUILayout.Button("Close Project", DD_EditorUtils.editorSkin.GetStyle("MenuInbound"), GUILayout.Width(130), GUILayout.Height(30)))
                    MenuCallback("Close");
            }
            else
            {
                if (GUILayout.Button("Close Project", DD_EditorUtils.editorSkin.GetStyle("MenuInboundInactive"), GUILayout.Width(130), GUILayout.Height(30))) { Debug.Log("Weird"); }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();
        }
        #endregion

        #region Utils
        void MenuCallback(string task)
        {
            DD_EditorUtils.showNodeMenu = false;

            switch (task)
            {
                case "Create":
                    DD_NodePopupWindow.InitPopupWindow();
                    break;

                case "Load":
                    DD_EditorUtils.LoadProject();
                    break;

                case "Close":
                    DD_EditorUtils.UnloadProject();
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
#endif