// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_ConnectionRelay
    {
        #region Public Variables
        public Rect m_relayRect;

        public Rect m_absRelayRect;
        public Vector2 m_tangentDirection;

        public DD_InputConnector m_associatedConnector;
        public DD_NodeBase m_inputtingNode;
        public bool m_readyToDelete;
        public Vector2 m_center;
        public DD_ProjectTemplate m_parentProject;
        #endregion

        #region private Variables
        Rect m_scaledRelayRect;
        bool m_dragging = false;
        #endregion

        #region Main Methods
        public void Update()
        {
            if (m_associatedConnector.inputtingNode != m_inputtingNode) m_readyToDelete = true;
        }

        public void OnRelayGUI()
        {
            m_scaledRelayRect = new Rect(m_relayRect.position * DD_EditorUtils.zoomFactor, m_relayRect.size);

            if (m_dragging) m_relayRect.position += DD_EditorUtils.currentEvent.delta / 2 / DD_EditorUtils.zoomFactor;

            m_center = m_absRelayRect.position + m_relayRect.size / 2;
            m_absRelayRect = new Rect(m_scaledRelayRect.position.x - DD_EditorUtils.viewRect_propertyView.width, m_scaledRelayRect.y - DD_EditorUtils.viewRect_headerView.height, m_scaledRelayRect.width, m_scaledRelayRect.height);
            GUI.Box(m_absRelayRect, "", DD_EditorUtils.editorSkin.GetStyle("ConnectionRelay"));

            ProcessEvents();
        }

        void ProcessEvents()
        {
            if (m_absRelayRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                if (DD_EditorUtils.currentEvent.button == 0)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                    {
                        DD_EditorUtils.allowSelectionRectRender = false;
                        m_dragging = true;
                        m_parentProject.m_enableNodeDragging = false;
                    }
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseUp || DD_EditorUtils.currentEvent.type == EventType.MouseMove)
                    {
                        DD_EditorUtils.allowSelectionRectRender = false;
                        m_dragging = false;
                        m_parentProject.m_enableNodeDragging = true;
                    }
                }
            }

            if (DD_EditorUtils.currentEvent.button == 0)
            {
                if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp || DD_EditorUtils.currentEvent.type == EventType.MouseMove)
                {
                    DD_EditorUtils.allowSelectionRectRender = false;
                    m_dragging = false;
                }
            }
        }
        #endregion

        #region Utility Methods
        public void DelayedOperation()
        {
            if (m_associatedConnector.inputtingNode == null) m_readyToDelete = true;
        }
        #endregion
    }
}
#endif