// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Derive;
using System.IO;


namespace DeriveUtils
{
    public struct ResourceTextureData
    {
        public Texture2D[][] resourceTextures;
        public string[][] resourceTextureNames;
        public string[][] resourceTextureFullNames;
        public Rect[][] resourceTextureRects;
    }

    public class DD_Resources
    {
        #region public variables

        #endregion

        #region private variables
        Rect m_resourcesFrameRect = Rect.zero;
        Rect m_resourcesRect = Rect.zero;
        Vector2 m_scrollPos = Vector2.zero;

        bool m_showLatestNews = false;
        bool m_showShareYourWork = true;
        bool m_showKnowledgeBase = false;
        bool m_showFeedback = false;

        bool m_showResources = false;
        bool m_showAvailableResources = false;
        bool m_showResourcesPatterns = false;
        bool m_showResourcesHeightMaps = false;
        bool m_showResourcesAlbedoMaps = false;
        bool m_showResourcesForDownload = false;

        bool m_showHelpAndSupport = false;

        bool m_showChangeLog = false;

        ResourceTextureData m_resourceTextureData;
        bool m_getResourceTextures = true;

        Texture2D m_dragTexture;
        //bool m_drawDragTexture = false;
        string m_dragTexturePath;
        #endregion

        #region main methods
        public void Update()
        {
            if (m_dragTexture == null)
            {
                m_dragTexture = new Texture2D(1, 1);
                m_dragTexture.Apply();
            }
        }


        public void OnResourcesGUI()
        {
            GUI.Box(DD_EditorUtils.resourcesFrameRect, "", DD_EditorUtils.editorSkin.GetStyle("PropertyFrame"));

            m_resourcesRect = new Rect(DD_EditorUtils.viewRect_resourcesView.position + new Vector2(18, 40), DD_EditorUtils.viewRect_resourcesView.size - new Vector2(32, 56));
            DD_EditorUtils.resourcesRect = m_resourcesRect;

            //Set up Layout area and scroll view
            GUILayout.BeginArea(m_resourcesRect);
            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

            m_resourcesFrameRect = EditorGUILayout.BeginVertical();

            m_showLatestNews = DD_ResourcesGUILayout.FoldOut(m_showLatestNews, "Latest News", DD_EditorUtils.resourcesData.highlightNewsTab);
            if (m_showLatestNews) DrawLatestNews();

            m_showShareYourWork = DD_ResourcesGUILayout.FoldOut(m_showShareYourWork, "Share your work", false);
            if (m_showShareYourWork) DrawShareYourWork();

            m_showKnowledgeBase = DD_ResourcesGUILayout.FoldOut(m_showKnowledgeBase, "Knowledge Base", false);
            if (m_showKnowledgeBase) DrawKnowledgeBase();

            m_showFeedback = DD_ResourcesGUILayout.FoldOut(m_showFeedback, "Feedback", false);
            if (m_showFeedback) DrawFeedback();

            m_showResources = DD_ResourcesGUILayout.FoldOut(m_showResources, "Resources", DD_EditorUtils.resourcesData.highlightResourcesTab);
            if (m_showResources) DrawResources();
            else m_getResourceTextures = true;

            m_showHelpAndSupport = DD_ResourcesGUILayout.FoldOut(m_showHelpAndSupport, "Help and Support", false);
            if (m_showHelpAndSupport) DrawHelpAndSupport();

            m_showChangeLog = DD_ResourcesGUILayout.FoldOut(m_showChangeLog, "ChangeLog", false);
            if (m_showChangeLog) DrawChangeLog();

            EditorGUILayout.EndVertical();

            if (m_resourcesFrameRect.size != Vector2.zero)
            {
                float frameRectWidth = m_resourcesRect.width+4;
                float frameRectHeight = Mathf.Min(m_resourcesFrameRect.height + 2, m_resourcesRect.height + 2);

                DD_EditorUtils.resourcesFrameRect = new Rect(m_resourcesRect.position - new Vector2(2, 2), new Vector2(frameRectWidth, frameRectHeight));
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            if (DD_EditorUtils.drawDragTexture)
            {
                GUI.Box(new Rect(DD_EditorUtils.mousePosInEditor - new Vector2(64, 64), new Vector2(128, 128)), m_dragTexture, DD_EditorUtils.editorSkin.GetStyle("DragTextureFrame"));
            }

            ProcessEvents();
        }

        void ProcessEvents()
        {
            if (m_resourceTextureData.resourceTextureRects == null) return;
            //if (!m_showAvailableResources) return;

            Vector2 mousePosInResourcesRect = new Vector2(DD_EditorUtils.mousePosInEditor.x - DD_EditorUtils.viewRect_workView.width - DD_EditorUtils.viewRect_propertyView.width - (m_resourcesRect.position.x - DD_EditorUtils.viewRect_resourcesView.position.x),
                        DD_EditorUtils.mousePosInEditor.y - DD_EditorUtils.viewRect_headerView.height - DD_EditorUtils.viewRect_previewView.height - 13);

            if (DD_EditorUtils.currentEvent.button == 0)
            {
                if(DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                {

                    if (m_resourceTextureData.resourceTextureRects[0] != null)
                    {
                        for (int i = 0; i < m_resourceTextureData.resourceTextureRects[0].Length; i++)
                        {
                            Rect resourceTextureRect = new Rect(m_resourceTextureData.resourceTextureRects[0][i].position - new Vector2(0, m_scrollPos.y), m_resourceTextureData.resourceTextureRects[0][i].size);

                            if (resourceTextureRect.Contains(mousePosInResourcesRect))
                            {
                                m_dragTexturePath = m_resourceTextureData.resourceTextureFullNames[0][i].Substring(m_resourceTextureData.resourceTextureFullNames[0][i].IndexOf("Assets"));

                                Texture2D tempTex = (Texture2D)AssetDatabase.LoadAssetAtPath(m_dragTexturePath, typeof(Texture2D));
                                DD_ResourcesUtils.RemoveAlphaFromTexture(tempTex, m_dragTexture);
                                m_dragTexture.Apply();

                                DD_EditorUtils.drawDragTexture = true;
                            }
                        }
                    }

                    if (m_resourceTextureData.resourceTextureRects[1] != null)
                    {
                        for (int i = 0; i < m_resourceTextureData.resourceTextureRects[1].Length; i++)
                        {
                            Rect resourceTextureRect = new Rect(m_resourceTextureData.resourceTextureRects[1][i].position - new Vector2(0, m_scrollPos.y), m_resourceTextureData.resourceTextureRects[1][i].size);

                            if (resourceTextureRect.Contains(mousePosInResourcesRect))
                            {
                                m_dragTexturePath = m_resourceTextureData.resourceTextureFullNames[1][i].Substring(m_resourceTextureData.resourceTextureFullNames[1][i].IndexOf("Assets"));

                                Texture2D tempTex = (Texture2D)AssetDatabase.LoadAssetAtPath(m_dragTexturePath, typeof(Texture2D));
                                DD_ResourcesUtils.RemoveAlphaFromTexture(tempTex, m_dragTexture);
                                m_dragTexture.Apply();

                                DD_EditorUtils.drawDragTexture = true;
                            }
                        }
                    }

                    if (m_resourceTextureData.resourceTextureRects[2] != null)
                    {
                        for (int i = 0; i < m_resourceTextureData.resourceTextureRects[2].Length; i++)
                        {
                            Rect resourceTextureRect = new Rect(m_resourceTextureData.resourceTextureRects[2][i].position - new Vector2(0, m_scrollPos.y), m_resourceTextureData.resourceTextureRects[2][i].size);

                            if (resourceTextureRect.Contains(mousePosInResourcesRect))
                            {
                                m_dragTexturePath = m_resourceTextureData.resourceTextureFullNames[2][i].Substring(m_resourceTextureData.resourceTextureFullNames[2][i].IndexOf("Assets"));

                                Texture2D tempTex = (Texture2D)AssetDatabase.LoadAssetAtPath(m_dragTexturePath, typeof(Texture2D));
                                DD_ResourcesUtils.RemoveAlphaFromTexture(tempTex, m_dragTexture);
                                m_dragTexture.Apply();

                                DD_EditorUtils.drawDragTexture = true;
                            }
                        }
                    }
                }

                if (Event.current.rawType == EventType.MouseUp && DD_EditorUtils.drawDragTexture)
                {
                    DD_EditorUtils.drawDragTexture = false;

                    if (DD_EditorUtils.viewRect_workView.Contains(DD_EditorUtils.mousePosInEditor))
                    {
                        DD_EditorUtils.CreateNode(DD_EditorUtils.currentProject, NodeType.Texture, DD_EditorUtils.mousePosInEditor - new Vector2(64, 64), DD_EditorUtils.currentProject.m_connectionAwaitingNewNode, DD_EditorUtils.currentProject.m_inverseConnection, DD_EditorUtils.currentProject.m_outputIndex);

                        DD_NodeTexture textureNode = (DD_NodeTexture)DD_EditorUtils.currentProject.m_nodes[DD_EditorUtils.currentProject.m_nodes.Count - 1] as DD_NodeTexture;
                        textureNode.m_Texture = (Texture2D)AssetDatabase.LoadAssetAtPath(m_dragTexturePath, typeof(Texture2D));

                        DD_EditorUtils.currentEvent.Use();
                    }
                }
            }
        }
        #endregion

        #region utility methods
        void DrawLatestNews()
        {
            NewsFeedObject[] newsFeedObjects = DD_EditorUtils.resourcesData.newsFeedObjects;

            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.Label("News Feed");

            for (int i = 0; i < newsFeedObjects.Length; i++)
            {
                DD_ResourcesGUILayout.HorizontalSeparator(20);
                DD_ResourcesGUILayout.TextBox(newsFeedObjects[i].date);
                DD_ResourcesGUILayout.Label(newsFeedObjects[i].title);                
                DD_ResourcesGUILayout.TextBox(newsFeedObjects[i].content);

                if (GUILayout.Button(new GUIContent("Continue reading >>", "Read the full article on the Derive blog."), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
                {
                    if(newsFeedObjects[i].url.Length >= DD_EditorUtils.resourcesData.newsFeedCheckUrl.Length)
                        if(newsFeedObjects[i].url.Substring(0, DD_EditorUtils.resourcesData.newsFeedCheckUrl.Length) == DD_EditorUtils.resourcesData.newsFeedCheckUrl)
                            Application.OpenURL(newsFeedObjects[i].url);
                }
            }

            EditorGUILayout.Space(10);

            DD_EditorUtils.resourcesData.highlightNewsTab = false;
        }

        void DrawShareYourWork()
        {
            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.Label("Step 1:"); 

            DD_ResourcesGUILayout.TextBox("Capture a screenshot of the current preview. Screenshots will automatically be taken at 1920 x 1080 pixels, you don't need to adjust your preview area.");

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Capture", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Width(Mathf.Min(200, m_resourcesRect.width)), GUILayout.Height(32)))
            {
                string path = EditorUtility.SaveFilePanel("Save Screenshot", Application.dataPath, DD_EditorUtils.currentProject.name, "jpg");

                DD_ResourcesUtils.CaptureScreenshot(path);
            }

            EditorGUILayout.Space(25);

            DD_ResourcesGUILayout.Label("Step 2:");

            DD_ResourcesGUILayout.TextBox("Select the platform on which to upload and attach the captured image to your post.");

            int columns = 0;
            int rows = 0;

            columns = Mathf.Min(DD_EditorUtils.resourcesData.socialButtonObjects.Length, Mathf.FloorToInt(m_resourcesRect.width / 42));
            rows = Mathf.CeilToInt(DD_EditorUtils.resourcesData.socialButtonObjects.Length / (float)columns);

            int buttonCounter = 0;

            for(int i = 0; i < rows; i++)
            {
                if (DD_EditorUtils.resourcesData.socialButtonObjects == null) break;

                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < columns; j++)
                {
                    if (buttonCounter == DD_EditorUtils.resourcesData.socialButtonObjects.Length) break;

                    ButtonObject socialButtonObject = DD_EditorUtils.resourcesData.socialButtonObjects[buttonCounter];

                    if (GUILayout.Button(new GUIContent(socialButtonObject.icon, socialButtonObject.tooltip), DD_EditorUtils.editorSkin.GetStyle("SocialButton"), GUILayout.Width(42), GUILayout.Height(42)))
                    {
                        if(socialButtonObject.url.Length>=socialButtonObject.urlCheck.Length)
                            if(socialButtonObject.url.Substring(0, socialButtonObject.urlCheck.Length) == socialButtonObject.urlCheck)
                                Application.OpenURL(socialButtonObject.url);
                    }

                    buttonCounter++;
                }
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.Space(15);
            DD_ResourcesGUILayout.Label("- OR -");
            EditorGUILayout.Space(10);

            
            DD_ResourcesGUILayout.Label("Share it with the makers!");

            DD_ResourcesGUILayout.TextBox("You made some nice artwork? That's great, we would love to see it! Show us and get showcased on the Derive homepage!");
            EditorGUILayout.Space(10);

            ButtonObject submissionButton = DD_EditorUtils.resourcesData.deriveSubmissionObject;

            if (GUILayout.Button(new GUIContent(submissionButton.text, submissionButton.tooltip), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(200), GUILayout.Height(42)))
            {
                if(submissionButton.url.Length >= submissionButton.urlCheck.Length)
                    if(submissionButton.url.Substring(0, submissionButton.urlCheck.Length) == submissionButton.urlCheck)
                        Application.OpenURL(submissionButton.url);
            }

            EditorGUILayout.Space(10);
        }

        void DrawKnowledgeBase()
        {
            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.TextBox("Here you can find all basic information on the derive editor, including the manual and beginner's guides for general information on usage setup and navigation, the node reference for information on each node's individual functionality, example node trees to learn basic techniques in texture design and video tutorials.");
            EditorGUILayout.Space(10);

            int columns = 0;
            int rows = 0;

            columns = Mathf.Min(DD_EditorUtils.resourcesData.knowledgeBaseObjects.Length, Mathf.Max(1, Mathf.FloorToInt(m_resourcesRect.width / 210)));
            rows = Mathf.CeilToInt(DD_EditorUtils.resourcesData.knowledgeBaseObjects.Length / (float)columns);

            int buttonCounter = 0;

            for (int i = 0; i < rows; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < columns; j++)
                {
                    if (buttonCounter == DD_EditorUtils.resourcesData.knowledgeBaseObjects.Length) break;

                    ButtonObject knowledgeBaseButton = DD_EditorUtils.resourcesData.knowledgeBaseObjects[buttonCounter];

                    if (GUILayout.Button(new GUIContent(knowledgeBaseButton.text), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
                    {
                        if(knowledgeBaseButton.url.Length >= knowledgeBaseButton.urlCheck.Length)
                            if(knowledgeBaseButton.url.Substring(0, knowledgeBaseButton.urlCheck.Length) == knowledgeBaseButton.urlCheck)
                                Application.OpenURL(knowledgeBaseButton.url);
                    }

                    buttonCounter++;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(10);
        }

        void DrawFeedback()
        {
            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.Label("We value your feedback!");
            DD_ResourcesGUILayout.TextBox("Derive lives for and from it's community. We value your opinion and your feedback. If you can spare a minute or two, please feel free to leave a review for Derive on the Unity Asset Store.");
            EditorGUILayout.Space(10);

            ButtonObject feedbackButton = DD_EditorUtils.resourcesData.feedbackObject;

            if (GUILayout.Button(new GUIContent(feedbackButton.text), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
            {
                if (feedbackButton.url.Length >= feedbackButton.urlCheck.Length)
                    if (feedbackButton.url.Substring(0, feedbackButton.urlCheck.Length) == feedbackButton.urlCheck)
                        Application.OpenURL(feedbackButton.url);
            }

            EditorGUILayout.Space(10);
        }

        void DrawResources()
        {
            if (m_getResourceTextures)
            {
                m_getResourceTextures = false;
                m_resourceTextureData = DD_ResourcesUtils.GetResourceTextures();
            }

            //if (GUILayout.Button("Update Resources")) DD_ResourcesUtils.UpdateResourceFeed();

            int columns = 0;
            int rows = 0;
            int buttonCounter = 0;
            
            m_showAvailableResources = DD_ResourcesGUILayout.FoldOutLevel2(m_showAvailableResources, "Available Resources", false);

            if (m_showAvailableResources)
            {
                if (m_resourceTextureData.resourceTextureFullNames[0].Length == 0 && m_resourceTextureData.resourceTextureFullNames[1].Length == 0 && m_resourceTextureData.resourceTextureFullNames[2].Length == 0)
                {
                    EditorGUILayout.Space(10);
                    DD_ResourcesGUILayout.Label("No Resource Packages imported.");
                    EditorGUILayout.Space(10);
                }
                else
                {
                    m_showResourcesPatterns = DD_ResourcesGUILayout.FoldOutLevel3(m_showResourcesPatterns, "Patterns");

                    if (m_showResourcesPatterns)
                    {
                        EditorGUILayout.Space(10);

                        columns = Mathf.Min(m_resourceTextureData.resourceTextures[0].Length, Mathf.FloorToInt(m_resourcesRect.width / 138));
                        rows = Mathf.CeilToInt(m_resourceTextureData.resourceTextures[0].Length / (float)columns);

                        for (int i = 0; i < rows; i++)
                        {
                            if (m_resourceTextureData.resourceTextures[0][i] == null) continue;

                            EditorGUILayout.BeginHorizontal();
                            for (int j = 0; j < columns; j++)
                            {
                                if (buttonCounter == m_resourceTextureData.resourceTextures[0].Length) break;

                                m_resourceTextureData.resourceTextureRects[0][buttonCounter] = EditorGUILayout.GetControlRect(false, 138, GUILayout.Width(138), GUILayout.Height(138));
                                m_resourceTextureData.resourceTextureRects[0][buttonCounter].size = new Vector2(128, 128);
                                m_resourceTextureData.resourceTextureRects[0][buttonCounter].position = new Vector2(m_resourceTextureData.resourceTextureRects[0][buttonCounter].position.x + 10, m_resourceTextureData.resourceTextureRects[0][buttonCounter].position.y);
                                GUI.Box(m_resourceTextureData.resourceTextureRects[0][buttonCounter], new GUIContent(m_resourceTextureData.resourceTextures[0][buttonCounter], m_resourceTextureData.resourceTextureNames[0][buttonCounter]), DD_EditorUtils.editorSkin.GetStyle("ResourceTextureFrame"));

                                buttonCounter++;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }



                    columns = 0;
                    rows = 0;
                    buttonCounter = 0;

                    m_showResourcesHeightMaps = DD_ResourcesGUILayout.FoldOutLevel3(m_showResourcesHeightMaps, "Height Maps");

                    if (m_showResourcesHeightMaps)
                    {
                        EditorGUILayout.Space(10);

                        columns = Mathf.Min(m_resourceTextureData.resourceTextures[1].Length, Mathf.FloorToInt(m_resourcesRect.width / 138));
                        rows = Mathf.CeilToInt(m_resourceTextureData.resourceTextures[1].Length / (float)columns);

                        for (int i = 0; i < rows; i++)
                        {
                            if (m_resourceTextureData.resourceTextures[1][i] == null) continue;

                            EditorGUILayout.BeginHorizontal();
                            for (int j = 0; j < columns; j++)
                            {
                                if (buttonCounter == m_resourceTextureData.resourceTextures[1].Length) break;

                                m_resourceTextureData.resourceTextureRects[1][buttonCounter] = EditorGUILayout.GetControlRect(false, 138, GUILayout.Width(138), GUILayout.Height(138));
                                m_resourceTextureData.resourceTextureRects[1][buttonCounter].size = new Vector2(128, 128);
                                m_resourceTextureData.resourceTextureRects[1][buttonCounter].position = new Vector2(m_resourceTextureData.resourceTextureRects[1][buttonCounter].position.x + 10, m_resourceTextureData.resourceTextureRects[1][buttonCounter].position.y);
                                GUI.Box(m_resourceTextureData.resourceTextureRects[1][buttonCounter], new GUIContent(m_resourceTextureData.resourceTextures[1][buttonCounter], m_resourceTextureData.resourceTextureNames[1][buttonCounter]), DD_EditorUtils.editorSkin.GetStyle("ResourceTextureFrame"));

                                buttonCounter++;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }



                    columns = 0;
                    rows = 0;
                    buttonCounter = 0;

                    m_showResourcesAlbedoMaps = DD_ResourcesGUILayout.FoldOutLevel3(m_showResourcesAlbedoMaps, "Albedo Maps");

                    if (m_showResourcesAlbedoMaps)
                    {
                        EditorGUILayout.Space(10);

                        columns = Mathf.Min(m_resourceTextureData.resourceTextures[2].Length, Mathf.FloorToInt(m_resourcesRect.width / 138));
                        rows = Mathf.CeilToInt(m_resourceTextureData.resourceTextures[2].Length / (float)columns);

                        for (int i = 0; i < rows; i++)
                        {
                            if (m_resourceTextureData.resourceTextures[2][i] == null) continue;

                            EditorGUILayout.BeginHorizontal();
                            for (int j = 0; j < columns; j++)
                            {
                                if (buttonCounter == m_resourceTextureData.resourceTextures[2].Length) break;

                                m_resourceTextureData.resourceTextureRects[2][buttonCounter] = EditorGUILayout.GetControlRect(false, 138, GUILayout.Width(138), GUILayout.Height(138));
                                m_resourceTextureData.resourceTextureRects[2][buttonCounter].size = new Vector2(128, 128);
                                m_resourceTextureData.resourceTextureRects[2][buttonCounter].position = new Vector2(m_resourceTextureData.resourceTextureRects[2][buttonCounter].position.x + 10, m_resourceTextureData.resourceTextureRects[2][buttonCounter].position.y);
                                GUI.Box(m_resourceTextureData.resourceTextureRects[2][buttonCounter], new GUIContent(m_resourceTextureData.resourceTextures[2][buttonCounter], m_resourceTextureData.resourceTextureNames[2][buttonCounter]), DD_EditorUtils.editorSkin.GetStyle("ResourceTextureFrame"));

                                buttonCounter++;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }

            m_showResourcesForDownload = DD_ResourcesGUILayout.FoldOutLevel2(m_showResourcesForDownload, "Download Resources", DD_EditorUtils.resourcesData.highlightResourcesTab);

            if (m_showResourcesForDownload)
            {
                EditorGUILayout.Space(10);

                if(DD_EditorUtils.resourcesData.resourcePackageObjects != null)
                {
                    ResourcePackageObject[] resourcePackageObjects = DD_EditorUtils.resourcesData.resourcePackageObjects;

                    if (resourcePackageObjects.Length==0)
                    {
                        EditorGUILayout.Space(10);
                        DD_ResourcesGUILayout.Label("Coming Soon...");
                        EditorGUILayout.Space(10);
                    }

                    for (int i = 0; i< resourcePackageObjects.Length; i++)
                    {
                        DD_ResourcesGUILayout.Label(resourcePackageObjects[i].title);

                        string dogmaticResourcesFolder = DD_EditorUtils.GetDerivePath().Replace("Derive", "Derive Resources");
                        string fullPath = Application.dataPath.Replace("Assets", "");

                        fullPath = fullPath + dogmaticResourcesFolder + resourcePackageObjects[i].title;

                        if (Directory.Exists(fullPath))
                        {
                            DD_ResourcesGUILayout.Label("- Imported -");
                        }
                        else
                        {
                            if (GUILayout.Button(new GUIContent("Download", resourcePackageObjects[i].title), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
                            {
                                if(resourcePackageObjects[i].url.Length >= DD_EditorUtils.resourcesData.resourceFeedCheckUrl.Length)
                                    if(resourcePackageObjects[i].url.Substring(0, DD_EditorUtils.resourcesData.resourceFeedCheckUrl.Length) == DD_EditorUtils.resourcesData.resourceFeedCheckUrl)
                                        Application.OpenURL(resourcePackageObjects[i].url);
                            }
                        }
                        EditorGUILayout.Space(20);
                    }
                }

                EditorGUILayout.Space(10);

                DD_EditorUtils.resourcesData.highlightResourcesTab = false;
            }
        }

        void DrawHelpAndSupport()
        {
            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.Label("Need Help?");
            DD_ResourcesGUILayout.TextBox("If you experience an issue or have questions, the FAQ may be a good place to start, the answer might already be there.");
            EditorGUILayout.Space(10);

            ButtonObject fAQButton = DD_EditorUtils.resourcesData.deriveFAQObject;

            if (GUILayout.Button(new GUIContent(fAQButton.text, fAQButton.tooltip), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
            {
                if(fAQButton.url.Length >= fAQButton.urlCheck.Length)
                    if(fAQButton.url.Substring(0, fAQButton.urlCheck.Length) == fAQButton.urlCheck)
                        Application.OpenURL(fAQButton.url);
            }

            DD_ResourcesGUILayout.HorizontalSeparator(20);
             
            DD_ResourcesGUILayout.Label("Nothing in the FAQ?");
            DD_ResourcesGUILayout.TextBox("Please feel free to create a post on the Unity forum or on Derive's community board.");
            EditorGUILayout.Space(10);

            int columns = 0;
            int rows = 0;

            columns = Mathf.Min(DD_EditorUtils.resourcesData.supportForumObjects.Length, Mathf.Max(1, Mathf.FloorToInt(m_resourcesRect.width / 210)));
            rows = Mathf.CeilToInt(DD_EditorUtils.resourcesData.supportForumObjects.Length / (float)columns);

            int buttonCounter = 0;

            for (int i = 0; i < rows; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < columns; j++)
                {
                    if (buttonCounter == DD_EditorUtils.resourcesData.supportForumObjects.Length) break;

                    ButtonObject forumButton = DD_EditorUtils.resourcesData.supportForumObjects[buttonCounter];

                    if (GUILayout.Button(new GUIContent(forumButton.text), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
                    {
                        if(forumButton.url.Length >= forumButton.urlCheck.Length)
                            if(forumButton.url.Substring(0, forumButton.urlCheck.Length) == forumButton.urlCheck)
                                Application.OpenURL(forumButton.url); 
                    }

                    buttonCounter++;
                }
                EditorGUILayout.EndHorizontal();
            }

            DD_ResourcesGUILayout.HorizontalSeparator(20);

            DD_ResourcesGUILayout.Label("Bug reports and feature requests");
            DD_ResourcesGUILayout.TextBox("You have discovered a bug or would simply like to request a feature for future releases? Please use the issue tracker to submit bug reports and feature requests.");
            EditorGUILayout.Space(10);

            ButtonObject issueTrackerButton = DD_EditorUtils.resourcesData.deriveIssueTrackerObject;

            if (GUILayout.Button(new GUIContent(issueTrackerButton.text), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
            {
                if(issueTrackerButton.url.Length >= issueTrackerButton.urlCheck.Length)
                    if(issueTrackerButton.url.Substring(0, issueTrackerButton.urlCheck.Length) == issueTrackerButton.urlCheck)
                        Application.OpenURL(issueTrackerButton.url);
            }

            DD_ResourcesGUILayout.HorizontalSeparator(20);

            DD_ResourcesGUILayout.Label("Contact us!");
            DD_ResourcesGUILayout.TextBox("Still in hot water? Please feel free to send us a message and let us know what we can help you with.");
            EditorGUILayout.Space(10);

            ButtonObject contactFormButton = DD_EditorUtils.resourcesData.deriveContactFormObject;

            if (GUILayout.Button(new GUIContent(contactFormButton.text), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
            {
                if(contactFormButton.url.Length >= contactFormButton.urlCheck.Length)
                    if(contactFormButton.url.Substring(0, contactFormButton.urlCheck.Length) == contactFormButton.urlCheck)
                        Application.OpenURL(contactFormButton.url);
            }

            DD_ResourcesGUILayout.HorizontalSeparator(20);

            DD_ResourcesGUILayout.Label("Editor Screenshot");
            DD_ResourcesGUILayout.TextBox("When you submit a request or want to post on the forum, it may sometimes be helpful to provide a screenshot of your node tree to make it easier to understand your issue. You can use the button below to do that. Please make sure that the editor shows what you want to be seen before taking a screenshot.");
            EditorGUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Capture Editor", "Captures a screenshot of the Derive editor as it is."), DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Width(Mathf.Min(200, m_resourcesRect.width)), GUILayout.Height(32)))
            {
                string path = EditorUtility.SaveFilePanel("Save Screenshot", Application.dataPath, DD_EditorUtils.currentProject.name + "_EditorScreen", "jpg");
                DD_ResourcesUtils.CaptureEditorScreenshot(path);
            }
            
            EditorGUILayout.Space(10);
        }

        void DrawChangeLog()
        {
            EditorGUILayout.Space(10);
            DD_ResourcesGUILayout.Label("Change Log for version 0.19 beta");

            DD_ResourcesGUILayout.TextBox("-Bugfix: Fixed an issue that caused a compiler error, when trying to create a new project from within the Derive editor.\r\n\r\n" +
                "- Revised seamless mapping node: The smooth collage technique was replaced by a variety of other mapping modes, that can now be selected from. Depending on the type of texture, one mode might work better than others.\r\n\r\n" +
                "- Revised the Gradient node: Added smooth gradients to choose from (previously 3, now 16). In addition to linear gradients, you can now choose from function based gradients based on sine waves, logistic growth, as well as cylindrical and hemispherical shapes. The gradients can now also be rotated without creating repetition and scaled before output.\r\n\r\n" +
                "- Added a project manager: When opening the editor from the 'Window' menu, instead of getting the blank editor, the user is now prompted to create a new project, load a project or open a project from a list of recently edited projects\r\n\r\n" +
                "- Added the change log tab to the resources area. The changes made in the current version can now be viewed in the resources area.");
            EditorGUILayout.Space(10);

            ButtonObject changelogButton = DD_EditorUtils.resourcesData.changelogObject;

            if (GUILayout.Button(new GUIContent(changelogButton.text, changelogButton.tooltip), DD_EditorUtils.editorSkin.GetStyle("OnlineButton"), GUILayout.Width(210), GUILayout.Height(42)))
            {
                if (changelogButton.url.Length >= changelogButton.urlCheck.Length)
                    if (changelogButton.url.Substring(0, changelogButton.urlCheck.Length) == changelogButton.urlCheck)
                        Application.OpenURL(changelogButton.url);
            }

            EditorGUILayout.Space(10);
        }
        #endregion
    }
}
#endif