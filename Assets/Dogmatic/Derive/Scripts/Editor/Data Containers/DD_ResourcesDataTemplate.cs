// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;

namespace DeriveUtils
{
    [Serializable]
    public struct NewsFeedObject
    {
        public string date;
        public string title;
        public string content;
        public string url;
    }

    [Serializable]
    public struct ButtonObject
    {
        public string text;
        public string tooltip;
        public Texture2D icon;
        public string url;
        public string urlCheck;
    }

    [Serializable]
    public struct ResourcePackageObject
    {
        public string title;
        public string url;
    }

    [Serializable]
    //[CreateAssetMenu(fileName = "Resource Data", menuName = "Resource Data Container")]
    public class DD_ResourcesDataTemplate : ScriptableObject
    {
        #region news feed data
        public string newsFeedUrl;
        public string newsFeedCheckUrl;

        public string checkdateNews;
        public bool highlightNewsTab;
        public NewsFeedObject[] newsFeedObjects;
        #endregion

        #region share your work data
        public ButtonObject[] socialButtonObjects;
        public ButtonObject deriveSubmissionObject;
        #endregion

        #region knowledgebase data
        public ButtonObject[] knowledgeBaseObjects;
        #endregion

        #region feedback data
        public ButtonObject feedbackObject;
        #endregion

        #region resources packs data
        public string resourcesFeedUrl;
        public string resourceFeedCheckUrl;

        public string checkdateResources;
        public bool highlightResourcesTab;
        public ResourcePackageObject[] resourcePackageObjects;
        #endregion

        #region help and support data
        public ButtonObject deriveFAQObject;
        public ButtonObject[] supportForumObjects;
        public ButtonObject deriveIssueTrackerObject;
        public ButtonObject deriveContactFormObject;
        #endregion

        #region changelog data
        public ButtonObject changelogObject;
        #endregion
    }

    [CustomEditor(typeof(DD_ResourcesDataTemplate))]
    public class DD_ResourcesDataInspector : Editor
    {
        public override void OnInspectorGUI()
        {

        }
    }
}
#endif