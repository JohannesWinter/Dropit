// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DeriveUtils;

namespace Derive
{
    [Serializable]
    public class DD_NodeBase : ScriptableObject
    {
        #region Public Variables
        public bool m_isSelected = false;
        public bool m_highlightReference = false;
        public string m_nodeName;
        public DD_ProjectTemplate m_parentProject;

        public Rect m_nodeRect;
        public Rect m_scaledNodeRect;

        public List<DD_InputConnector> m_inputs;
        public List<DD_OutputConnector> m_outputs;

        public List<DD_ConnectionRelay> m_connectionRelays;
        public bool doubleClicked = false;                              //Will be set to true by project template, if double click occurred
        public DD_NodeBase m_originNode;                                //Here the node will store itself when it's copy-pasted. It's required, to keep track which node was copied, to be able to copy node trees with existing connections

        public int m_sourceNodeIndex = 10000;                           //Only used for setting and getting local variables
        #endregion

        #region Protected Variables
        public GUISkin m_nodeSkin;
        public string m_nodeStyle;
        public string[] m_connectorStyles;                           //Usually only 2 styles (one connected, one unconnected, but can be overridden, if other styles ought to be used)
        public NodeType m_nodeType;

        public bool m_overrideInputConnectorRendering = false;
        public bool m_overrideOutputConnectorRendering = false;

        public DD_NodeTooltip m_nodeTooltip;

        public Vector2 m_scrollPos;

        protected float m_nodeBodyHeight;
        #endregion

        #region Private Variables
        /// <summary>
        /// Required rects to translate the node rect
        /// into the work view correctly and to add body,
        /// shadow and preview in dependence to the node rect
        /// </summary>
        Rect m_absNodeHeaderRect;
        protected Rect m_absNodeBodyRect;
        Rect m_absNodeShadowRect;
        protected Rect m_absNodeRect;

        public Rect m_scaledNodeRectTotal;

        Rect m_absNodePreviewRect;
        Rect m_absNodePreviewShadowRect;
        Rect m_absNodeExpandTriangleRect;
        Rect m_absPreviewTextureRect;
        public bool m_expandNodePreview = false;
        public bool m_hasNodePreview = true;

        public bool m_checkForRecursion = false;
        public DD_NodeBase m_recursionCandidate;
        public DD_InputConnector m_pendingConnector;

        public bool m_outputHasChanged = false;
        public bool m_redoCalculation = true;
        #endregion


        #region Main Methods
        public virtual void InitNode()
        {
            m_nodeTooltip = new DD_NodeTooltip();
        }

        /// <summary>
        /// Updates the node every frame
        /// </summary>
        public virtual void UpdateNode()
        {
            m_outputHasChanged = false;

            if (m_checkForRecursion) CheckForRecursion();

            if (m_connectionRelays == null) m_connectionRelays = new List<DD_ConnectionRelay>();

            //Calculate the height of the node body depending on the amount of inputs and outputs
            int x = 0;
            if (m_outputs != null && m_inputs != null) x = Mathf.Max(m_outputs.Count, m_inputs.Count);
            if (m_outputs != null && m_inputs == null) x = m_outputs.Count;
            if (m_outputs == null && m_inputs != null) x = m_inputs.Count;

            float topSpace = 16 * DD_EditorUtils.zoomFactor;
            float spaceBetweenInputs = 32 * DD_EditorUtils.zoomFactor;

            m_nodeBodyHeight = topSpace + spaceBetweenInputs * x;

            //Run every relay's Update method
            if (m_connectionRelays != null) foreach (DD_ConnectionRelay relay in m_connectionRelays) relay.Update();

            //Remove every relay that has the delete flag set
            if (m_connectionRelays != null)
                for (int i = 0; i < m_connectionRelays.Count; i++)
                    if (m_connectionRelays[i].m_readyToDelete) m_connectionRelays.Remove(m_connectionRelays[i]);

            //Recalculate the output startpoints
            for (int i = 0; i < m_outputs.Count; i++)
                m_outputs[i].connectionStartPoint = new Vector2(m_outputs[i].outputRect.x + m_outputs[i].outputRect.width, m_outputs[i].outputRect.y + m_outputs[i].outputRect.height / 2);

            if (m_nodeTooltip != null)
                m_nodeTooltip.Update();

            //Check if any inputting node has changed
            if (m_inputs != null)
                if (m_inputs.Count > 0)
                    for (int i = 0; i < m_inputs.Count; i++)
                        if (m_inputs[i].inputtingNode != null)
                            if (m_inputs[i].inputtingNode.m_outputHasChanged) m_redoCalculation = true;

            EditorUtility.SetDirty(this);
        }

        private void OnEnable()
        {
            if(m_inputs == null)
            {
                m_redoCalculation = true;
            }
            else
            {
                if (m_inputs.Count == 0)
                {
                    m_redoCalculation = true;
                }
                else
                {
                    bool noInputConnected = true;

                    foreach (DD_InputConnector input in m_inputs)
                    {
                        if (input.inputtingNode != null) noInputConnected = false;
                    }

                    if (noInputConnected)
                    {
                        m_redoCalculation = true;
                    }
                }
            }
        }

        //OnGUI Call (see Unity API Reference for more information)
        public virtual void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            if (m_nodeStyle == null) return;

            //Set all inputs that have no inputting node to 'not occupied'
            if (m_inputs != null)
                foreach (DD_InputConnector input in m_inputs)
                {
                    if (input.inputtingNode == null) input.isOccupied = false;
                    else
                    {
                        input.isOccupied = true;
                        input.inputtingNode.m_outputs[input.outputIndex].isOccupied = true;
                    }
                }

            //Calculate node rect that considers zooming
            m_scaledNodeRect = new Rect(m_nodeRect.position * DD_EditorUtils.zoomFactor, m_nodeRect.size * DD_EditorUtils.zoomFactor);

            m_scaledNodeRect.width = CalculateNodeWidth();

            //Calculate absolute node rects in relation to the window from the scaled node rect
            m_absNodeHeaderRect = new Rect(m_scaledNodeRect.x - propertyViewWidth, m_scaledNodeRect.y - headerViewHeight, m_scaledNodeRect.width, m_scaledNodeRect.height);

            m_absNodeBodyRect = new Rect(m_absNodeHeaderRect.x, m_absNodeHeaderRect.y + m_absNodeHeaderRect.height, m_absNodeHeaderRect.width, m_nodeBodyHeight);
            m_absNodeShadowRect = new Rect(m_absNodeHeaderRect.x - 6 * DD_EditorUtils.zoomFactor, m_absNodeHeaderRect.y + 6 * DD_EditorUtils.zoomFactor, m_absNodeHeaderRect.width, m_absNodeHeaderRect.height + m_absNodeBodyRect.height);
            m_absNodeRect = new Rect(m_absNodeHeaderRect.position, new Vector2(m_absNodeHeaderRect.width, m_absNodeHeaderRect.height + m_absNodeBodyRect.height));

            m_scaledNodeRectTotal = new Rect(m_scaledNodeRect.position, new Vector2(m_scaledNodeRect.size.x, m_scaledNodeRect.size.y + m_absNodeBodyRect.size.y));

            float nodePreviewWidth = 144 * DD_EditorUtils.zoomFactor;
            float nodePreviewHeight;

            if (!m_expandNodePreview) nodePreviewHeight = 16 * DD_EditorUtils.zoomFactor;
            else nodePreviewHeight = 150 * DD_EditorUtils.zoomFactor;

            float expansionTriangleSize = 16 * DD_EditorUtils.zoomFactor;
            m_absNodePreviewRect = new Rect(m_absNodeBodyRect.position.x + m_absNodeBodyRect.width / 2 - nodePreviewWidth / 2, m_absNodeBodyRect.position.y + m_absNodeBodyRect.height - 2, nodePreviewWidth, nodePreviewHeight);
            m_absNodeExpandTriangleRect = new Rect(m_absNodePreviewRect.position.x + m_absNodePreviewRect.width / 2 - expansionTriangleSize / 2, m_absNodePreviewRect.position.y + nodePreviewHeight - expansionTriangleSize, expansionTriangleSize, expansionTriangleSize);

            m_absNodePreviewShadowRect = new Rect(m_absNodePreviewRect.position.x - 3 * DD_EditorUtils.zoomFactor, m_absNodePreviewRect.position.y, m_absNodePreviewRect.width, m_absNodePreviewRect.height + 3 * DD_EditorUtils.zoomFactor);

            float previewTextureSize = 128 * DD_EditorUtils.zoomFactor;
            m_absPreviewTextureRect = new Rect(m_absNodePreviewRect.position.x + m_absNodePreviewRect.width / 2 - previewTextureSize / 2, m_absNodePreviewRect.position.y + 8, previewTextureSize, previewTextureSize);

            //Copy style to adjust font size according to zoom factor
            GUIStyle nodeHeaderSkin = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle(m_nodeStyle));
            nodeHeaderSkin.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle(m_nodeStyle).fontSize * DD_EditorUtils.zoomFactor);

            //Render Node
            GUI.Box(m_absNodeShadowRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeShadow"));
            GUI.Box(m_absNodeHeaderRect, m_nodeName, nodeHeaderSkin);

            if (m_hasNodePreview)
            {
                GUI.Box(m_absNodePreviewShadowRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeShadow"));
                GUI.Box(m_absNodePreviewRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeBody"));
            }

            GUI.Box(m_absNodeBodyRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeBody"));

            if (m_hasNodePreview)
            {
                if (!m_expandNodePreview) GUI.Box(m_absNodeExpandTriangleRect, "", DD_EditorUtils.editorSkin.GetStyle("ArrowDownFlat"));
                else
                {
                    GUI.Box(m_absNodeExpandTriangleRect, "", DD_EditorUtils.editorSkin.GetStyle("ArrowUpFlat"));
                    //GUI.Box(m_absPreviewTextureRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeContextBox"));
                    if (m_outputs != null)
                        if (m_outputs.Count > 0)
                            if (m_outputs[0].outputTexture != null)
                            {
                                EditorGUI.DrawPreviewTexture(m_absPreviewTextureRect, m_outputs[0].outputTexture);
                                GUI.Box(m_absPreviewTextureRect, "", DD_EditorUtils.editorSkin.GetStyle("TextureFrame"));
                            }
                }
            }

            //Render green border if node is selected
            if (m_isSelected) GUI.Box(m_absNodeRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeSelectionHighlight"));

            GUIStyle inputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("InputLabel"));
            inputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("InputLabel").fontSize * DD_EditorUtils.zoomFactor);

            GUIStyle outputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("OutputLabel"));
            outputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("OutputLabel").fontSize * DD_EditorUtils.zoomFactor);

            //Calculate size and position of each input rect
            if (m_inputs != null)
            {
                if (m_inputs.Count > 0 && !m_overrideInputConnectorRendering)
                {
                    float inputSize = 24 * DD_EditorUtils.zoomFactor;
                    float topSpace = 16 * DD_EditorUtils.zoomFactor;
                    float spaceBetweenInputs = 32 * DD_EditorUtils.zoomFactor;

                    for (int i = 0; i < m_inputs.Count; i++)
                    {
                        m_inputs[i].inputRect = new Rect(m_absNodeBodyRect.x, m_absNodeBodyRect.y + topSpace + spaceBetweenInputs * i, inputSize, inputSize);

                        if (!m_inputs[i].isOccupied) GUI.Box(m_inputs[i].inputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[0]));
                        else GUI.Box(m_inputs[i].inputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[1]));

                        float labelWidth = inputLabelStyle.CalcSize(new GUIContent(m_inputs[i].inputLabel)).x;

                        Rect labelRect = new Rect(m_inputs[i].inputRect.position + new Vector2(inputSize, 0), new Vector2(labelWidth, inputSize));
                        GUI.Label(labelRect, m_inputs[i].inputLabel, inputLabelStyle);
                    }
                }
            }

            //Calculate size and position of each output rect
            if (m_outputs != null)
            {
                if (m_outputs.Count > 0 && !m_overrideOutputConnectorRendering)
                {
                    float outputSize = 24 * DD_EditorUtils.zoomFactor;
                    float topSpace = 16 * DD_EditorUtils.zoomFactor;
                    float spaceBetweenOutputs = 32 * DD_EditorUtils.zoomFactor;

                    for (int i = 0; i < m_outputs.Count; i++)
                    {
                        m_outputs[i].outputRect = new Rect(m_absNodeBodyRect.x + m_absNodeBodyRect.width - outputSize, m_absNodeBodyRect.y + topSpace + spaceBetweenOutputs * i, outputSize, outputSize);

                        if (!m_outputs[i].isOccupied) GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[0]));
                        else GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[1]));

                        float labelWidth = outputLabelStyle.CalcSize(new GUIContent(m_outputs[i].outputLabel)).x;

                        Rect labelRect = new Rect(m_outputs[i].outputRect.position - new Vector2(labelWidth, 0), new Vector2(labelWidth, outputSize));
                        GUI.Label(labelRect, m_outputs[i].outputLabel, outputLabelStyle);
                    }
                }
            }


            DrawInputConnections();

            if (m_connectionRelays != null)
                foreach (DD_ConnectionRelay relay in m_connectionRelays) relay.OnRelayGUI();

            //This is instead called from the project file after all nodes have been rendered
            //m_nodeTooltip.OnNodeContextGUI();

            ProcessEvents();

            doubleClicked = false;
        }
        #endregion

        #region Utility Methods
        public virtual void ProcessEvents()
        {
            Event e = DD_EditorUtils.currentEvent;

            //Determines that a selection box can only be drawn if the mouse is inside the work view
            if (m_scaledNodeRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown) DD_EditorUtils.allowSelectionRectRender = false;
                if (e.rawType == EventType.MouseUp) DD_EditorUtils.allowSelectionRectRender = true;
            }

            if (m_absNodeBodyRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown) DD_EditorUtils.allowSelectionRectRender = false;
                if (e.rawType == EventType.MouseUp) DD_EditorUtils.allowSelectionRectRender = true;
            }

            if (m_absNodeExpandTriangleRect.Contains(e.mousePosition))
            {
                if (e.button == 0)
                {
                    if (e.type == EventType.MouseUp)
                    {
                        if (!m_expandNodePreview) m_expandNodePreview = true;
                        else m_expandNodePreview = false;

                        m_parentProject.m_selectedNodes.Clear();
                        m_parentProject.m_selectedNodes.Add(this);
                    }
                }
            }

            //Check if an output was clicked and start the connection process
            if (m_outputs != null)
            {
                if (m_outputs.Count > 0)
                {
                    for (int i = 0; i < m_outputs.Count; i++)
                    {
                        if (m_outputs[i].outputRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                        {
                            if (DD_EditorUtils.currentEvent.button == 0)
                            {
                                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                                {
                                    if (m_parentProject != null)
                                    {
                                        m_parentProject.m_connectionStart = m_outputs[i].connectionStartPoint;

                                        if (m_outputs[i].outputDataType == DataType.Float) m_parentProject.m_connectionColor = Color.grey;
                                        else m_parentProject.m_connectionColor = new Color(0.8f, 0.2f, 1, 1);

                                        m_parentProject.m_wantsConnection = true;
                                        m_parentProject.m_connectionAttemptingNode = this;
                                        m_parentProject.m_outputIndex = i;
                                        m_parentProject.m_inverseConnection = false;
                                        m_outputs[i].isOccupied = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //This code is run when something happens while the mouse cursor is inside an input rect
            if (m_inputs != null)
            {
                if (m_inputs.Count > 0)
                {
                    for (int i = 0; i < m_inputs.Count; i++)
                    {
                        if (m_inputs[i].inputRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                        {
                            if (DD_EditorUtils.currentEvent.button == 0)
                            {
                                //When the left mouse button is released on an input...
                                if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                                {
                                    if (m_parentProject != null)
                                    {
                                        if (m_inputs[i].inputtingNode != m_parentProject.m_connectionAttemptingNode)
                                        {
                                            if (!m_parentProject.m_movingExistingConnection)
                                            {
                                                if (m_connectionRelays != null)
                                                {
                                                    for (int j = 0; j < m_connectionRelays.Count; j++)
                                                    {
                                                        if (m_connectionRelays[j].m_associatedConnector == m_inputs[i]) m_connectionRelays[j].m_readyToDelete = true;
                                                    }
                                                }
                                            }
                                        }

                                        //...establish the permanent connection by telling which output connector of which node is now connecting to it
                                        m_inputs[i].inputtingNode = m_parentProject.m_connectionAttemptingNode;
                                        m_inputs[i].outputIndex = m_parentProject.m_outputIndex;
                                        m_redoCalculation = true;

                                        //Make sure connection relays are associated with an input connector, so they don't relay another connection
                                        foreach (DD_ConnectionRelay relay in m_connectionRelays)
                                            if (relay.m_associatedConnector.inputtingNode == null) relay.m_associatedConnector = m_inputs[i];

                                        //Start Reciprocity Check
                                        m_checkForRecursion = true; ;
                                        m_recursionCandidate = this;
                                        m_pendingConnector = m_inputs[i];

                                        if (m_inputs[i].inputtingNode != null) m_inputs[i].isOccupied = true;
                                        else m_inputs[i].isOccupied = false;

                                        //Reset connection process in project file
                                        m_parentProject.m_wantsConnection = false;
                                        m_parentProject.m_connectionAttemptingNode = null;
                                        m_parentProject.m_clearAllOutputConnectors = true;
                                    }
                                }

                                //When the left mouse button is pressed on an input...
                                if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                                {
                                    if (m_parentProject != null)
                                    {
                                        //...move existing connection from associated node and it's output, if input is already connected
                                        if (m_inputs[i].inputtingNode != null)
                                        {
                                            m_parentProject.m_connectionStart = m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].connectionStartPoint;

                                            if (m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputDataType == DataType.Float) m_parentProject.m_connectionColor = Color.grey;
                                            else m_parentProject.m_connectionColor = new Color(0.8f, 0.2f, 1, 1);

                                            m_parentProject.m_wantsConnection = true;
                                            m_parentProject.m_outputIndex = m_inputs[i].outputIndex;
                                            m_parentProject.m_movingExistingConnection = true;
                                            m_parentProject.m_inverseConnection = false;
                                            m_parentProject.m_connectionAttemptingNode = m_inputs[i].inputtingNode;

                                            //Create list to store relevant connection relays into
                                            List<DD_ConnectionRelay> relevantRelays = new List<DD_ConnectionRelay>();

                                            if (m_connectionRelays.Count > 0)
                                            {
                                                //Gather relevant relays
                                                foreach (DD_ConnectionRelay relay in m_connectionRelays) if (relay.m_associatedConnector == m_inputs[i]) relevantRelays.Add(relay);
                                            }

                                            //Pass relevant relays for this connection to project file, to be used in temporary connection to mouse cursor
                                            m_parentProject.m_relayCache = relevantRelays.ToArray();

                                            m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].isOccupied = true;
                                            m_inputs[i].inputtingNode = null;
                                            m_redoCalculation = true;
                                        }
                                        //...extablish inverse connection from the input connector to another node's output connector, if input is not connected
                                        else
                                        {
                                            m_parentProject.m_connectionStart = m_inputs[i].inputRect.position + m_inputs[i].inputRect.size / 2;
                                            m_parentProject.m_wantsConnection = true;
                                            m_parentProject.m_outputIndex = m_inputs[i].outputIndex;
                                            m_parentProject.m_movingExistingConnection = false;
                                            m_parentProject.m_connectionAttemptingNode = this;
                                            m_parentProject.m_inverseConnectionAttemptingInput = m_inputs[i];
                                            m_parentProject.m_inverseConnection = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ///<summary>
            ///Runs the delayed operation method for each relay
            ///The method only checks if the connection, that the relay is associated with, still exists
            ///If it doesn't, the relay marks itself for deletion
            /// </summary>
            if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                if (DD_EditorUtils.currentEvent.button == 0)
                    foreach (DD_ConnectionRelay relay in m_connectionRelays) relay.DelayedOperation();

            if (m_absNodeHeaderRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
            {
                if (DD_EditorUtils.currentEvent.type != EventType.MouseDrag)
                    m_nodeTooltip.m_activateTimer = true;
                else m_nodeTooltip.m_activateTimer = false;
            }
            else m_nodeTooltip.m_activateTimer = false;

            if (DD_EditorUtils.currentEvent.type == EventType.MouseUp)
                DD_EditorUtils.allowSelection = true;
        }

        /// <summary>
        /// Mathod that draws connections, highlights if the mouse is hovering over a connection
        /// and adds/removes connection relays at double left-click on a connection/relay
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="input"></param>
        public void DrawConnection(Vector2 startPoint, Vector2 endPoint, DD_InputConnector input)
        {
            Handles.BeginGUI();

            Handles.color = Color.white;

            //Create list to store relevant connection relays into
            List<DD_ConnectionRelay> relevantRelays = new List<DD_ConnectionRelay>();

            if (m_connectionRelays != null && m_connectionRelays.Count > 0)
            {
                //Gather relevant relays
                foreach (DD_ConnectionRelay relay in m_connectionRelays)
                    if (relay.m_associatedConnector.inputtingNode == input.inputtingNode)
                        if (relay.m_associatedConnector.outputIndex == input.outputIndex)
                            relevantRelays.Add(relay);

                //Set the tangent direction for each relay based on the last and next connection point
                if (relevantRelays.Count > 0)
                {
                    for (int i = 0; i < relevantRelays.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (relevantRelays.Count == 1)
                                relevantRelays[i].m_tangentDirection = (endPoint - (startPoint + Vector2.right * Mathf.Min(100, Vector2.Distance(relevantRelays[i].m_center, startPoint)))).normalized;
                            else
                                relevantRelays[i].m_tangentDirection = (relevantRelays[i + 1].m_center - (startPoint + Vector2.right * Mathf.Min(100, Vector2.Distance(relevantRelays[i].m_center, startPoint)))).normalized;
                        }
                        if (i > 0)
                        {
                            if (i < relevantRelays.Count - 1)
                                relevantRelays[i].m_tangentDirection = (relevantRelays[i + 1].m_center - relevantRelays[i - 1].m_center).normalized;
                            else
                                relevantRelays[i].m_tangentDirection = (endPoint - relevantRelays[i - 1].m_center).normalized;
                        }
                    }
                }
            }

            //Tangents from connectors (always horizontal)
            Vector2 startTangent;
            Vector2 endTangent;

            float mouseDistanceToConnection;

            //Distances between outputs, inputs & relays
            float connectionDistance;
            float connectionDistanceLeft;
            float connectionDistanceRight;

            bool closeToBezier = false;

            //Check if mouse is close to the connection that arrives at this input
            //Distinct between connections from output to input and partial connections with connectoin relays
            if (relevantRelays.Count == 0)
            {
                //connectionDistance = Vector2.Distance(startPoint, endPoint);
                connectionDistance = Mathf.Abs(startPoint.x - endPoint.x);

                startTangent = startPoint + Vector2.right * connectionDistance / 2;
                endTangent = endPoint + Vector2.left * connectionDistance / 2;

                mouseDistanceToConnection = HandleUtility.DistancePointBezier(DD_EditorUtils.currentEvent.mousePosition, startPoint, endPoint, startTangent, endTangent);
                if (mouseDistanceToConnection < 10) 
                    if(DD_EditorUtils.viewRect_workView.Contains(DD_EditorUtils.mousePosInEditor))
                        closeToBezier = true;
            }
            else
            {

                //connectionDistanceLeft = Vector2.Distance(startPoint, relevantRelays[0].m_center);
                //connectionDistanceRight = Vector2.Distance(relevantRelays[relevantRelays.Count - 1].m_center, endPoint);

                connectionDistanceLeft = Mathf.Abs(startPoint.x - relevantRelays[0].m_center.x);
                connectionDistanceRight = Mathf.Abs(relevantRelays[relevantRelays.Count - 1].m_center.x - endPoint.x);

                startTangent = startPoint + Vector2.right * Mathf.Min(100, connectionDistanceLeft / 2);
                endTangent = endPoint + Vector2.left * Mathf.Min(100, connectionDistanceRight / 2);

                //float partialConnectionDistance = Vector2.Distance(startPoint, relevantRelays[0].m_center);
                float partialConnectionDistance = Mathf.Abs(startPoint.x - relevantRelays[0].m_center.x);

                Vector2 relayTangentLeft = relevantRelays[0].m_center - relevantRelays[0].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                Vector2 relayTangentRight;

                mouseDistanceToConnection = HandleUtility.DistancePointBezier(DD_EditorUtils.currentEvent.mousePosition, startPoint, relevantRelays[0].m_center, startTangent, relayTangentLeft);
                if (mouseDistanceToConnection < 10) closeToBezier = true;

                for (int i = 0; i < relevantRelays.Count - 1; i++)
                {
                    //partialConnectionDistance = Vector2.Distance(relevantRelays[i].m_center, relevantRelays[i + 1].m_center);
                    partialConnectionDistance = Mathf.Abs(relevantRelays[i].m_center.x - relevantRelays[i + 1].m_center.x);

                    relayTangentLeft = relevantRelays[i + 1].m_center - relevantRelays[i + 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                    relayTangentRight = relevantRelays[i].m_center + relevantRelays[i].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);

                    mouseDistanceToConnection = HandleUtility.DistancePointBezier(DD_EditorUtils.currentEvent.mousePosition, relevantRelays[i].m_center, relevantRelays[i + 1].m_center, relayTangentRight, relayTangentLeft);
                    if (mouseDistanceToConnection < 10) closeToBezier = true;
                }

                //partialConnectionDistance = Vector2.Distance(relevantRelays[relevantRelays.Count - 1].m_center, endPoint);
                partialConnectionDistance = Mathf.Abs(relevantRelays[relevantRelays.Count - 1].m_center.x - endPoint.x);

                relayTangentRight = relevantRelays[relevantRelays.Count - 1].m_center + relevantRelays[relevantRelays.Count - 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);

                mouseDistanceToConnection = HandleUtility.DistancePointBezier(DD_EditorUtils.currentEvent.mousePosition, relevantRelays[relevantRelays.Count - 1].m_center, endPoint, relayTangentRight, endTangent);
                if (mouseDistanceToConnection < 10) closeToBezier = true;
            }

            Color connectionColor;

            ///<summary>
            ///1. Switch color based on if the mouse is close to the connection
            ///2. Delete relay if it was double clicked
            ///3. Add relay if connection was double clicked
            /// </summary>
            if (closeToBezier)
            {
                connectionColor = new Color(0.4f, 0.8f, 0.6f, 1);

                if (doubleClicked)
                {
                    doubleClicked = false;
                    DD_ConnectionRelay relayToDelete = null;

                    foreach (DD_ConnectionRelay relay in m_connectionRelays)
                    {
                        if (relay.m_absRelayRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                        {
                            relayToDelete = relay;
                        }
                    }

                    if (relayToDelete == null)
                    {
                        DD_ConnectionRelay relay = new DD_ConnectionRelay();
                        relay.m_relayRect = new Rect(DD_EditorUtils.mousePosInEditor / DD_EditorUtils.zoomFactor - new Vector2(8, 8), new Vector2(16, 16));
                        relay.m_associatedConnector = input;
                        relay.m_tangentDirection = endPoint - startPoint;
                        relay.m_inputtingNode = relay.m_associatedConnector.inputtingNode;
                        relay.m_parentProject = m_parentProject;

                        int insertPosition = 0;

                        for (int i = 0; i < m_connectionRelays.Count; i++)
                            if (Vector2.Distance(m_connectionRelays[i].m_relayRect.position, startPoint) <= Vector2.Distance(relay.m_relayRect.position, startPoint)) insertPosition++;

                        m_connectionRelays.Insert(insertPosition, relay);
                    }
                    else
                    {
                        m_connectionRelays.Remove(relayToDelete);
                    }
                }
            }
            else
            {
                if (input.inputtingNode.m_outputs[input.outputIndex].outputDataType == DataType.Float) connectionColor = Color.grey;
                else connectionColor = new Color(0.8f, 0.2f, 1, 1);
            }

            ///<summary>
            ///Draw actual connection
            ///1. If there are no relays, draw from output to input
            ///2. If there are relays, draw from output to first relay...
            ///...from first relay to last relay connecting all relays in between...
            ///...and finally from last relay to input.
            /// </summary>
            if (relevantRelays.Count == 0)
            {
                //connectionDistance = Vector2.Distance(startPoint, endPoint);
                connectionDistance = Mathf.Abs(startPoint.x - endPoint.x);

                startTangent = startPoint + Vector2.right * connectionDistance / 2;
                endTangent = endPoint + Vector2.left * connectionDistance / 2;

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, connectionColor, null, 5);
            }
            else
            {
                //connectionDistanceLeft = Vector2.Distance(startPoint, relevantRelays[0].m_center);
                //connectionDistanceRight = Vector2.Distance(relevantRelays[relevantRelays.Count - 1].m_center, endPoint);

                connectionDistanceLeft = Mathf.Abs(startPoint.x - relevantRelays[0].m_center.x);
                connectionDistanceRight = Mathf.Abs(relevantRelays[relevantRelays.Count - 1].m_center.x - endPoint.x);

                startTangent = startPoint + Vector2.right * Mathf.Min(100, connectionDistanceLeft / 2);
                endTangent = endPoint + Vector2.left * Mathf.Min(100, connectionDistanceRight / 2);

                //float partialConnectionDistance = Vector2.Distance(startPoint, relevantRelays[0].m_center);
                float partialConnectionDistance = Mathf.Abs(startPoint.x - relevantRelays[0].m_center.x);

                Vector2 relayTangentLeft = relevantRelays[0].m_center - relevantRelays[0].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                Vector2 relayTangentRight;

                Handles.DrawBezier(startPoint, relevantRelays[0].m_center, startTangent, relayTangentLeft, connectionColor, null, 5);

                for (int i = 0; i < relevantRelays.Count - 1; i++)
                {
                    //partialConnectionDistance = Vector2.Distance(relevantRelays[i].m_center, relevantRelays[i + 1].m_center);
                    partialConnectionDistance = Mathf.Abs(relevantRelays[i].m_center.x - relevantRelays[i + 1].m_center.x);

                    relayTangentLeft = relevantRelays[i + 1].m_center - relevantRelays[i + 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                    relayTangentRight = relevantRelays[i].m_center + relevantRelays[i].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);

                    Handles.DrawBezier(relevantRelays[i].m_center, relevantRelays[i + 1].m_center, relayTangentRight, relayTangentLeft, connectionColor, null, 5);
                }

                //partialConnectionDistance = Vector2.Distance(relevantRelays[relevantRelays.Count-1].m_center, endPoint);
                partialConnectionDistance = Mathf.Abs(relevantRelays[relevantRelays.Count - 1].m_center.x - endPoint.x);

                relayTangentRight = relevantRelays[relevantRelays.Count - 1].m_center + relevantRelays[relevantRelays.Count - 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);

                Handles.DrawBezier(relevantRelays[relevantRelays.Count - 1].m_center, endPoint, relayTangentRight, endTangent, connectionColor, null, 5);
            }

            Handles.EndGUI();
        }

        /// <summary>
        /// Calls the DrawConnection method for every input connector of the node, providing start and end points.
        /// </summary>
        void DrawInputConnections()
        {
            if (m_inputs == null) return;

            for (int i = 0; i < m_inputs.Count; i++)
            {
                if (m_inputs[i].inputtingNode != null)
                {
                    Vector2 startPoint = m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].connectionStartPoint;
                    Vector2 endPoint = new Vector2(m_inputs[i].inputRect.x, m_inputs[i].inputRect.y + m_inputs[i].inputRect.height / 2);
                    DrawConnection(startPoint, endPoint, m_inputs[i]);
                }
            }
        }

        float CalculateNodeWidth()
        {
            GUIStyle nodeHeaderSkin = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle(m_nodeStyle));
            nodeHeaderSkin.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle(m_nodeStyle).fontSize * DD_EditorUtils.zoomFactor);

            float nodeNameWidth = nodeHeaderSkin.CalcSize(new GUIContent(m_nodeName)).x;

            float largestInputLabelWidth = 0;
            float largestOutputLabelWidth = 0;

            GUIStyle inputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("InputLabel"));
            inputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("InputLabel").fontSize * DD_EditorUtils.zoomFactor);

            GUIStyle outputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("OutputLabel"));
            outputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("OutputLabel").fontSize * DD_EditorUtils.zoomFactor);

            if (m_inputs != null)
                for (int i = 0; i < m_inputs.Count; i++)
                    largestInputLabelWidth = Mathf.Max(largestInputLabelWidth, inputLabelStyle.CalcSize(new GUIContent(m_inputs[i].inputLabel)).x);

            if (m_outputs != null)
                for (int i = 0; i < m_outputs.Count; i++)
                    largestOutputLabelWidth = Mathf.Max(largestOutputLabelWidth, outputLabelStyle.CalcSize(new GUIContent(m_outputs[i].outputLabel)).x);

            float nodeContentWidth = (24 * 3) * DD_EditorUtils.zoomFactor + largestInputLabelWidth + largestOutputLabelWidth;

            float largestWidth = Mathf.Max(nodeNameWidth + 40, nodeContentWidth);

            return Mathf.Max(160 * DD_EditorUtils.zoomFactor, largestWidth);
        }

        public virtual void DrawProperties()
        {

        }

        void CheckForRecursion()
        {
            m_checkForRecursion = false;

            if (m_inputs != null)
                for (int i = 0; i < m_inputs.Count; i++)
                {
                    if (m_inputs[i].inputtingNode == null) continue;

                    if (m_inputs[i].inputtingNode == m_recursionCandidate)
                    {
                        m_pendingConnector.inputtingNode = null;
                        m_parentProject.ConsoleMessage("Recursion not allowed.");
                    }
                    else
                    {
                        m_inputs[i].inputtingNode.m_recursionCandidate = m_recursionCandidate;
                        m_inputs[i].inputtingNode.m_pendingConnector = m_pendingConnector;
                        m_inputs[i].inputtingNode.m_checkForRecursion = true;
                    }
                }

            m_recursionCandidate = null;
        }

        /// <summary>
        /// Updates which node acts as the source node to reference
        /// (Only for GetVariable-Node!)
        /// </summary>
        public virtual void UpdateNodeReference()
        {

        }
        #endregion
    }
}
#endif