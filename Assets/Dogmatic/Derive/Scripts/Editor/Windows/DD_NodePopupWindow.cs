// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DeriveUtils;
using System.IO;

namespace Derive
{
    public class DD_NodePopupWindow : EditorWindow
    {
        #region Public Variables
        #endregion

        #region Private Variables
        static DD_NodePopupWindow m_currentPopup;
        string m_wantedName = "Enter a name...";
        bool m_invalidName = false;
        string m_targetPath;

        TextEditor m_textEditor;
        bool m_selectWantedNameString = true;

        bool m_promptProjectManager = false;
        #endregion

        #region Main Methods
        public static void InitPopupWindow(bool promptProjectManager = false)
        {
            m_currentPopup = (DD_NodePopupWindow)EditorWindow.GetWindow(typeof(DD_NodePopupWindow), true, "New Derive Project");
            m_currentPopup.maxSize = new Vector2(600, 170);
            m_currentPopup.minSize = new Vector2(600, 170);
            m_currentPopup.Show();

            if (promptProjectManager) m_currentPopup.m_promptProjectManager = true;
            else m_currentPopup.m_promptProjectManager = false;
        }

        private void OnEnable()
        {
            if (!Directory.Exists(DD_EditorUtils.GetDerivePath() + "My Projects"))
            {
                Directory.CreateDirectory(DD_EditorUtils.GetDerivePath() + "My Projects");
            }
            
            string derivePath = DD_EditorUtils.GetDerivePath() + "My Projects";
            string absPath = Application.dataPath.Replace("Assets", derivePath);
            int appPathLength = Application.dataPath.Length;
            m_targetPath = absPath.Substring(appPathLength - 6);     // -6 for minus ASSETS folder            

            m_textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
        }

        /// <summary>
        /// Simple window that creates a project file at a specified path
        /// </summary>
        private void OnGUI()
        {
            //Make sure focus is only forced as long as Unity editor has focus, or else user can't unfocus Unity.
            if (UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                Focus();

            GUI.Box(new Rect(0, 0, position.width, position.height), "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("Create New Project:", DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Target Folder", DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(120));
            EditorGUILayout.LabelField(m_targetPath, DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(350));

            if (GUILayout.Button("Select", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(20)))
            {
                string absPath = EditorUtility.OpenFolderPanel("Select Folder", DD_EditorUtils.GetDerivePath() + "My Projects/", "");
                int appPathLength = Application.dataPath.Length;

                if (!string.IsNullOrEmpty(absPath)) m_targetPath = absPath.Substring(appPathLength - 6);      // -6 for minus ASSETS folder
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enter Name: ", DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(120));

            GUI.SetNextControlName("WantedNameField");
            m_wantedName = EditorGUILayout.TextField(m_wantedName, DD_EditorUtils.editorSkin.GetStyle("TextInput"));
            GUI.FocusControl("WantedNameField");

            if (m_selectWantedNameString)
            {
                m_selectWantedNameString = false;
                m_textEditor.OnFocus();
                m_textEditor.cursorIndex = 0; //CursorStartPosition;
                m_textEditor.selectIndex = m_wantedName.Length;
            }
            GUILayout.EndHorizontal();

            if (m_invalidName)
            {
                GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
                errorStyle.normal.textColor = new Color(1, 0.5f, 0.5f, 1);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(120));
                EditorGUILayout.LabelField("Please Enter a valid project name.", errorStyle);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create Project", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(40)))
            {
                if (!string.IsNullOrEmpty(m_wantedName) && m_wantedName != "Enter a name...")
                {
                    DD_EditorUtils.CreateProject(m_targetPath, m_wantedName);
                    m_currentPopup.Close();
                }
                else
                {
                    m_invalidName = true;

                    m_currentPopup.maxSize = new Vector2(600, 190);
                    m_currentPopup.minSize = new Vector2(600, 190);
                }
            }
            if (GUILayout.Button("Cancel", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(40)))
            {
                ///<summary>
                /// If the window is closed via "Cancel" button, the project manager is opened again, if this window was originally opened from the project manager
                /// </summary>
                if (m_promptProjectManager) DD_ProjectManagementWindow.InitPopupWindow(DD_EditorUtils.windowRect);

                m_currentPopup.Close();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        #endregion

        #region Utility Methods
        #endregion
    }
}
#endif
