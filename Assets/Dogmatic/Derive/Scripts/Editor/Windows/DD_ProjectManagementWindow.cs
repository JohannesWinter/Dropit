// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using DeriveUtils;

namespace Derive
{
    public class DD_ProjectManagementWindow : EditorWindow
    {
        #region public variables
        #endregion

        #region private variables
        static DD_ProjectManagementWindow m_currentPopup;
        static DD_ProjectListTemplate m_projectList;
        Vector2 m_scrollPos;
        string m_selectedPath = "";
        
        float m_t = 0;
        bool m_startTimer = false;
        float m_timeStamp = 0;
        #endregion

        #region main methods
        /// <summary>
        /// Initialize window and position it in the middle of the main window
        /// </summary>
        /// <param name="position"></param>
        public static void InitPopupWindow(Rect position)
        {
            m_currentPopup = (DD_ProjectManagementWindow)EditorWindow.GetWindow(typeof(DD_ProjectManagementWindow), true, "Project Manager");
            m_currentPopup.maxSize = new Vector2(600, 500);
            m_currentPopup.minSize = new Vector2(600, 500);
            m_currentPopup.position = new Rect(position.position + position.size / 2 - new Vector2(m_currentPopup.position.size.x / 2, position.size.y * 0.25f), m_currentPopup.position.size);                   
            m_currentPopup.Show();

            DD_EditorUtils.projectManagementWindow = m_currentPopup;
            m_projectList = DD_EditorUtils.projectManagementData;
        }

        private void Update()
        {
            RemoveNullProjects();

            //Timing functionality for double clicking projects from the list
            if (m_startTimer)
            {
                m_t = (float)EditorApplication.timeSinceStartup - m_timeStamp;
            }

            if (m_t >= 0.3f)
            {
                m_startTimer = false;
                m_t = 0;
            }
        }

        private void OnGUI()
        {
            //Make sure focus is only forced as long as Unity editor has focus, or else user can't unfocus Unity.
            if (UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                Focus();

            //Only continue, when the skin is loaded
            if (DD_EditorUtils.editorSkin == null) return;
            
            //Draw the background
            GUI.Box(new Rect(0, 0, position.width, position.height), "", DD_EditorUtils.editorSkin.GetStyle("MenuBox_BG"));

            //Calculate the content rect based on the window rect
            Rect contentRect = new Rect(20, 20, position.width - 40, position.height - 40);

            //Content area without the project list!
            GUILayout.BeginArea(contentRect);

            GUI.Box(EditorGUILayout.GetControlRect(GUILayout.Height(DD_EditorUtils.editorData.workViewLogo.height)), new GUIContent(DD_EditorUtils.editorData.workViewLogo), GUIStyle.none);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Project Manager", DD_EditorUtils.editorSkin.GetStyle("BigLabel"));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New Project", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(40)))
            {
                DD_NodePopupWindow.InitPopupWindow(true);
                m_currentPopup.Close();
            }

            if (GUILayout.Button("Load Project", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Height(40)))
            {
                DD_EditorUtils.LoadProject();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.LabelField("Recently Edited", DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));
            GUILayout.Space(10);

            //Rect for the box in which the list of recently edited projects is displayed
            Rect projectListRect = EditorGUILayout.GetControlRect(GUILayout.Height(190));

            GUI.Box(projectListRect, "", DD_EditorUtils.editorSkin.GetStyle("PropertyFrame"));

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            
            //Do this to display the 'Open' button on the right side
            EditorGUILayout.Space(300);

            string buttonStyle = "GenericButton";

            //Make sure, the button only works if a project is selected
            if (m_selectedPath == "") buttonStyle = "GenericButtonDisabled";

            if (GUILayout.Button("Open", DD_EditorUtils.editorSkin.GetStyle(buttonStyle), GUILayout.Height(30)))
            {
                DD_EditorUtils.LoadProject(m_selectedPath);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
            
            //This is where the project list is actually displayed
            GUILayout.BeginArea(new Rect(20, 248, position.width - 50, 180));

            //Make the list scrollable
            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

            //If the project list exists...
            if (m_projectList != null)
            {
                //...iterate through the given file paths in the list
                for (int i = m_projectList.projectPaths.Count - 1; i >= 0; i--)
                {
                    string listElementStyle = "ProjectListButton";

                    //If the project is selected in the list, use a different style to have it highlighted
                    if (m_selectedPath == m_projectList.projectPaths[i]) listElementStyle = "ProjectListButtonActive";

                    EditorGUILayout.BeginHorizontal();

                    //Display the Derive file icon in front of each project name
                    EditorGUILayout.LabelField("", DD_EditorUtils.editorSkin.GetStyle("ProjectListButtonIcon"), GUILayout.Width(14), GUILayout.Height(14));

                    //Every list element is a button that...
                    if (GUILayout.Button(GetProjectName(m_projectList.projectPaths[i]), DD_EditorUtils.editorSkin.GetStyle(listElementStyle)))
                    {
                        //assigns the selected path to a member - if the member is not null or empty, the project is selected and highlighted
                        m_selectedPath = m_projectList.projectPaths[i];

                        //Set a timestamp from which the time since the click is counted
                        m_timeStamp = (float)EditorApplication.timeSinceStartup;

                        //If the list element is clicked again in less than 0.3 seconds after the first click it counts as a double click and the project is opened
                        if (m_startTimer) DD_EditorUtils.LoadProject(m_selectedPath);

                        //Start the timer when the list element is clicked
                        m_startTimer = true;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        #endregion

        #region utility methods
        /// <summary>
        /// Gets the name of the project at a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetProjectName(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path, typeof(DD_ProjectTemplate)).name;
        }

        /// <summary>
        /// Iterates through the list of recently edited projects and checks if the projects still exists at their registered paths
        /// If no project is found at a given path, it is removed from the list
        /// </summary>
        void RemoveNullProjects()
        {
            if (m_projectList != null)
            {
                for (int i = m_projectList.projectPaths.Count - 1; i >= 0; i--)
                {
                    if ((DD_ProjectTemplate)AssetDatabase.LoadAssetAtPath(m_projectList.projectPaths[i], typeof(DD_ProjectTemplate)) == null)
                        m_projectList.projectPaths.RemoveAt(i);
                }
            }
                
        }
        #endregion
    }
}
#endif