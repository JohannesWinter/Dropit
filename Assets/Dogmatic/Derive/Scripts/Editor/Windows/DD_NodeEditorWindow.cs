// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DeriveUtils;

namespace Derive
{
    public class DD_NodeEditorWindow : EditorWindow
    {

        #region Variables
        public static DD_NodeEditorWindow m_currentWindow;

        public DD_PropertyView m_propertyView;
        public DD_PreviewView m_previewView;
        public DD_WorkView m_workView;
        public DD_ResourcesView m_resourceView;
        public DD_HeaderView m_headerView;
        public DD_FooterView m_footerView;

        public float m_headerHeight = 30;
        public float m_footerHeight = 30;

        public DD_ProjectTemplate m_currentProject = null;

        GUIStyle m_objectFieldStyle;
        GUIStyle m_numberFieldStyle;
        GUIStyle m_textFieldStyle;
        #endregion

        #region Main Methods
        public static void InitEditorWindow(bool openingBlankEditor = true)
        {
            m_currentWindow = CreateWindow<DD_NodeEditorWindow>("Derive", typeof(DD_NodeEditorWindow));

            //Make sure the editor starts with a min value, but remove that limitation afterwards
            m_currentWindow.minSize = new Vector2(1280, 768);
            m_currentWindow.minSize = new Vector2(600, 460);

            m_currentWindow.Show();

            m_currentWindow.wantsMouseMove = true;

            CreateViews();

            if (openingBlankEditor) DD_ProjectManagementWindow.InitPopupWindow(m_currentWindow.position);
        }

        private void OnEnable()
        {
            ///EMPTY EXCEPTION HANDLER!!! 
            ///WON'T SPEW OUT CONSOLE ERRORS, BUT IF THE CODE INSIDE PRODUCES ERRORS, IT WILL LEAD TO INEXPLICABLE BEHAVIOUR!!! 
            ///REMOVE TRY-CATCH-STATEMENT IN CASE OF DOUBT!!!
            try
            {
                if (m_currentWindow != null)
                    m_currentWindow.wantsMouseMove = true;

                DD_EditorUtils.currentEvent = null;
                DD_EditorUtils.allowGridOffset = true;
                DD_EditorUtils.allowSelection = true;
                DD_EditorUtils.allowSelectionRectRender = true;
                DD_EditorUtils.preventNodeMovement = false;

                DD_EditorUtils.resourcesData = (DD_ResourcesDataTemplate)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "/Data Containers/Resource Data.asset", typeof(DD_ResourcesDataTemplate));
                DD_EditorUtils.editorData = (DD_EditorDataTemplate)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "/Data Containers/Editor Data.asset", typeof(DD_EditorDataTemplate));
                DD_EditorUtils.nodeData = (DD_NodeDataTemplate)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "/Data Containers/Node Data.asset", typeof(DD_NodeDataTemplate));
                DD_EditorUtils.projectManagementData = (DD_ProjectListTemplate)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "/Data Containers/Project Management Data.asset", typeof(DD_ProjectListTemplate));

                DD_ResourcesUtils.UpdateNews();
                DD_ResourcesUtils.UpdateResourceFeed();

                m_objectFieldStyle = EditorStyles.objectField;
                m_numberFieldStyle = EditorStyles.numberField;
                m_textFieldStyle = EditorStyles.textField;
            }
            catch { }
        }

        private void Update()
        {
            if (DD_EditorUtils.currentEvent == null)
                return;

            if(m_currentWindow == null)
            {
                m_currentWindow = GetWindow<DD_NodeEditorWindow>("Derive", typeof(DD_NodeEditorWindow));
                m_currentWindow.wantsMouseMove = true;
            }

            DD_EditorUtils.currentEvent.mousePosition = new Vector2(DD_EditorUtils.currentEvent.mousePosition.x, DD_EditorUtils.currentEvent.mousePosition.y - 22);
            DD_EditorUtils.GetMousePosInEditor();

            m_workView.UpdateView();
            m_headerView.UpdateView();
            m_footerView.UpdateView();
            m_previewView.UpdateView();
            m_propertyView.UpdateView();
            m_resourceView.UpdateView();

            if (m_currentProject == null) titleContent = new GUIContent("< No Project >");
            else titleContent = new GUIContent(m_currentProject.name);

            EditorStyles.objectField.normal = m_objectFieldStyle.normal;
            EditorStyles.objectField.fontStyle = m_objectFieldStyle.fontStyle;

            EditorStyles.numberField.normal = m_numberFieldStyle.normal;
            EditorStyles.numberField.fontStyle = m_numberFieldStyle.fontStyle;

            EditorStyles.textField.normal = m_textFieldStyle.normal;
            EditorStyles.textField.fontStyle = m_textFieldStyle.fontStyle;
        }

        private void OnGUI()
        {
            if (DD_EditorUtils.editorSkin == null)
            {
                GetEditorSkin();
            }

            //Check if views exist and create them if they don't
            if (m_propertyView == null || m_workView == null || m_resourceView == null || m_headerView == null || m_footerView == null || m_previewView == null)
            {
                CreateViews();
                return;
            }

            //Get and process Events
            DD_EditorUtils.currentEvent = Event.current;
            ProcessEvents();

            DD_EditorUtils.windowRect = position;
            DD_EditorUtils.viewRect_footerView = new Rect(0, position.height - m_footerHeight, position.width, m_footerHeight);
            DD_EditorUtils.viewRect_headerView = new Rect(m_propertyView.m_viewWidth, 0, position.width - m_propertyView.m_viewWidth - m_previewView.m_viewWidth, m_headerHeight);
            DD_EditorUtils.viewRect_propertyView = new Rect(0, 0, m_propertyView.m_viewWidth, position.height - m_footerHeight);
            DD_EditorUtils.viewRect_workView = new Rect(m_propertyView.m_viewWidth, m_headerHeight, position.width - m_propertyView.m_viewWidth - m_previewView.m_viewWidth, position.height - m_headerHeight - m_footerHeight);
            DD_EditorUtils.viewRect_resourcesView = new Rect(m_propertyView.m_viewWidth + m_workView.m_viewRect.width, m_previewView.m_viewHeight, m_previewView.m_viewWidth, position.height - m_footerHeight - m_previewView.m_viewHeight);
            DD_EditorUtils.viewRect_previewView = new Rect(m_propertyView.m_viewWidth + m_workView.m_viewRect.width, 0, m_previewView.m_viewWidth, m_previewView.m_viewHeight);


            DD_EditorUtils.currentProject = m_currentProject;

            //Update Views
            m_workView.OnViewGUI();
            m_headerView.OnViewGUI();
            m_footerView.OnViewGUI();
            m_previewView.OnViewGUI();
            m_propertyView.OnViewGUI();
            m_resourceView.OnViewGUI();

            Repaint();

            //if (DD_EditorUtils.allowEventUsage)
            //    if (DD_EditorUtils.currentEvent.type == EventType.MouseUp || DD_EditorUtils.currentEvent.type == EventType.MouseDown)
            //        if (!DD_EditorUtils.showNodeMenu) DD_EditorUtils.currentEvent.Use();
        }
        #endregion

        #region Utility Methods
        void GetEditorSkin()
        {
            DD_EditorUtils.editorSkin = (GUISkin)Resources.Load("Editor/Skins/Derive_Standard");
            GUI.skin = (GUISkin)Resources.Load("Editor/Skins/Derive_Standard");
        }

        //Instantiate views as objects of their classes
        private static void CreateViews()
        {
            if (m_currentWindow != null)
            {
                m_currentWindow.m_propertyView = new DD_PropertyView();
                m_currentWindow.m_workView = new DD_WorkView();
                m_currentWindow.m_workView.SetUpContextMenu();
                m_currentWindow.m_resourceView = new DD_ResourcesView();
                m_currentWindow.m_headerView = new DD_HeaderView();
                m_currentWindow.m_footerView = new DD_FooterView();
                m_currentWindow.m_previewView = new DD_PreviewView();
            }
            else
            {
                m_currentWindow = (DD_NodeEditorWindow)EditorWindow.GetWindow<DD_NodeEditorWindow>();
            }
        }

        void ProcessEvents()
        {

        }

        private void OnDisable()
        {
            if (DD_EditorUtils.currentProject != null)
                if (DD_EditorUtils.currentProject.m_preview != null)
                    DD_EditorUtils.currentProject.m_preview.OnDestroy();
        }
        #endregion

    }
}
#endif
