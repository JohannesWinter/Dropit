// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_NodeTooltip
    {
        #region public variables
        public string m_title = "Float";
        public string m_content = "Node Info";
        public string m_targetURL = "https://derive.dogmatic.tech/knowledge-base/node-reference";
        public bool m_activateTimer = false;
        public Vector2 m_mousePos;
        #endregion

        #region private variables
        public Rect m_contextBoxRect;
        public Rect m_contentRect;
        public Rect m_linkButtonRect;
        float t = 0;
        bool m_renderNodeContext = false;
        bool captureMousePos = false;
        #endregion

        #region main methods
        public void Update()
        {
            if (m_activateTimer)
            {
                t += Time.deltaTime;
            }
            else
            {
                captureMousePos = true;
                t = 0;
            }

            if (t > 1)
            {
                if (captureMousePos)
                {
                    float contextBoxWidth = Mathf.Max(120, DD_EditorUtils.editorSkin.GetStyle("NodeContextBox").CalcSize(new GUIContent(m_content)).x + 20);

                    m_contextBoxRect = new Rect(DD_EditorUtils.mousePosInEditor - new Vector2(DD_EditorUtils.viewRect_propertyView.width, DD_EditorUtils.viewRect_headerView.height) - new Vector2(contextBoxWidth / 2, 100), new Vector2(contextBoxWidth, 80));
                    m_contentRect = new Rect(m_contextBoxRect.position.x, m_contextBoxRect.position.y + 34, m_contextBoxRect.width, 20);
                    m_linkButtonRect = new Rect(m_contextBoxRect.position.x, m_contextBoxRect.position.y + 58, m_contextBoxRect.width, 20);
                    captureMousePos = false;
                }

                m_renderNodeContext = true;
            }

            if (Vector2.Distance(m_contextBoxRect.position + new Vector2(m_contextBoxRect.width/2, -m_contextBoxRect.height/2),
                DD_EditorUtils.mousePosInEditor - new Vector2(DD_EditorUtils.viewRect_propertyView.width, DD_EditorUtils.viewRect_headerView.height)) > 300) m_renderNodeContext = false;
        }

        public void OnNodeContextGUI()
        {
            if (m_renderNodeContext)
            {
                //Make sure only one node context is rendered at a time
                for(int i = 0; i < DD_EditorUtils.currentProject.m_nodes.Count; i++)
                {
                    if (DD_EditorUtils.currentProject.m_nodes[i].m_nodeTooltip == this) continue;

                    DD_EditorUtils.currentProject.m_nodes[i].m_nodeTooltip.m_renderNodeContext = false;
                }

                GUI.Box(m_contextBoxRect, m_title, DD_EditorUtils.editorSkin.GetStyle("NodeContextBox"));
                GUI.Box(m_contentRect, m_content, DD_EditorUtils.editorSkin.GetStyle("NodeContextContent"));
                GUI.Box(m_linkButtonRect, "Online Reference", DD_EditorUtils.editorSkin.GetStyle("LinkButton"));
            }

            ProcessEvents();
        }
        #endregion

        #region utility methods
        void ProcessEvents()
        {
            if (m_renderNodeContext && !m_contextBoxRect.Contains(DD_EditorUtils.mousePosInEditor - new Vector2(DD_EditorUtils.viewRect_propertyView.width, DD_EditorUtils.viewRect_headerView.height)))
                m_activateTimer = false;

            if (m_renderNodeContext)
                if (m_linkButtonRect.Contains(DD_EditorUtils.mousePosInEditor - new Vector2(DD_EditorUtils.viewRect_propertyView.width, DD_EditorUtils.viewRect_headerView.height)))
                    if (DD_EditorUtils.currentEvent.button == 0)
                        if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                            Application.OpenURL(m_targetURL);

            if (DD_EditorUtils.currentEvent.rawType == EventType.MouseDown) m_renderNodeContext = false;
        }
        #endregion
    }
}
#endif