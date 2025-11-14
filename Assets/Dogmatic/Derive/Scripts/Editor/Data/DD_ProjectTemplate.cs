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
    [CreateAssetMenu(fileName = "New Project", menuName = "Derive Project")]
    public class DD_ProjectTemplate : ScriptableObject
    {
        #region Public variables
        public List<DD_NodeBase> m_nodes;

        public bool m_boxSelect = false;
        public Rect m_selectionRect;
        public List<DD_NodeBase> m_selectedNodes;

        public List<DD_NodeBase> m_registeredNodes;
        public string[] m_registeredVariableNames;

        //Variables required to establish node connections
        public bool m_wantsConnection = false;
        public bool m_connectionAwaitingNewNode = false;
        public bool m_movingExistingConnection = false;
        public bool m_inverseConnection = false;
        public bool m_clearAllOutputConnectors = false;

        public DD_NodeBase m_connectionAttemptingNode;
        public int m_outputIndex = 0;
        public Vector2 m_connectionStart;
        public Color m_connectionColor;
        public DD_InputConnector m_inverseConnectionAttemptingInput;

        //Contains the relays of a node that is attempting to move an existing connection
        public DD_ConnectionRelay[] m_relayCache;

        public float m_zoomFactor = 1;

        public bool m_enableNodeDragging = false;

        public Vector2 m_canvasOffset;

        public float m_consoleAlpha = 3;
        public string m_consoleMessage = "";

        public DD_ProjectSettings m_projectSettings;

        public DD_Preview m_preview;
        #endregion

        #region protected Variables
        #endregion

        #region private Variables
        Rect m_viewRect_propertyView;
        Rect m_viewRect_headerView;
        Rect m_viewRect_workView;

        bool m_controlSelect;
        bool m_preventSelect = false;

        //Variable for double click detection
        float m_t = 0;
        float m_firstClickTimer;
        float m_secondClickTimer;
        #endregion

        #region Constructors
        #endregion

        #region Main Methods

        /// <summary>
        /// Create neccessary lists if node list doesn't exists
        /// See Unity API reference for more infos on OnEnable call
        /// </summary>
        private void OnEnable()
        {
            if (m_nodes == null)
            {
                m_nodes = new List<DD_NodeBase>();
                m_selectedNodes = new List<DD_NodeBase>();
                m_registeredNodes = new List<DD_NodeBase>();
            }

            m_relayCache = new DD_ConnectionRelay[0];

            //Make sure to have all nodes perform their operation at startup to start with the correct data that you left off with
            //if (m_nodes != null) foreach (DD_NodeBase node in m_nodes) node.m_redoCalculation = true;
        }

        /// <summary>
        /// Initialize project by initializing all nodes
        /// </summary>
        public void InitProject()
        {
            if (m_nodes.Count > 0)
            {
                for (int i = 0; i < m_nodes.Count; i++)
                {
                    m_nodes[i].InitNode();
                }
            }

            m_projectSettings = new DD_ProjectSettings();
            m_preview = new DD_Preview();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        public void UpdateProject()
        {
            bool masterNodePresent = false;

            for (int i = 0; i < m_nodes.Count; i++)
            {
                if (m_nodes[i] == null) m_nodes.RemoveAt(i);
                if (m_nodes[i].m_nodeType == NodeType.Master) masterNodePresent = true;
            }

            if (!masterNodePresent) DD_EditorUtils.CreateNode(this, NodeType.Master, m_viewRect_workView.position + m_viewRect_workView.size / 4, false, false, 0);

            //Update registered variable names for correct handling of "local variables" in the editor
            m_registeredVariableNames = new string[m_registeredNodes.Count];
            for (int i = 0; i < m_registeredNodes.Count; i++) m_registeredVariableNames[i] = m_registeredNodes[i].m_nodeName;

            //Call Update method on all nodes and set selected nodes 'selected'
            for (int i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].UpdateNode();

                if (m_selectedNodes.Contains(m_nodes[i])) m_nodes[i].m_isSelected = true;
                else m_nodes[i].m_isSelected = false;
            }

            if (m_boxSelect)
            {
                m_boxSelect = false;
                BoxSelectNodes();
            }

            //Timer for double click detection
            m_t += Time.deltaTime;

            if (m_firstClickTimer - m_t < -0.3f)
            {
                m_firstClickTimer = 0;
                m_t = 0;
            }

            //Abort creating a connection to a new node, if node menu is closed
            if (!DD_EditorUtils.showNodeMenu) m_connectionAwaitingNewNode = false;

            //Panning across the canvass (work view)
            if (m_canvasOffset != Vector2.zero)
            {
                if (m_nodes != null)
                {
                    foreach (DD_NodeBase node in m_nodes)
                    {
                        node.m_nodeRect.position += m_canvasOffset;

                        node.m_nodeTooltip.m_contextBoxRect.position += m_canvasOffset;
                        node.m_nodeTooltip.m_contentRect.position += m_canvasOffset;
                        node.m_nodeTooltip.m_linkButtonRect.position += m_canvasOffset;

                        if (node.m_connectionRelays != null)
                            for (int i = 0; i < node.m_connectionRelays.Count; i++)
                                node.m_connectionRelays[i].m_relayRect.position += m_canvasOffset;
                    }

                    m_connectionStart += m_canvasOffset;
                }
                m_canvasOffset = Vector2.zero;
            }

            DD_EditorUtils.zoomFactor = m_zoomFactor;

            //Smooth fade out of the console message
            m_consoleAlpha = Mathf.Max(0, m_consoleAlpha - Time.deltaTime);

            //Update Project management list to make the last edited project first up the list
            if(DD_EditorUtils.projectManagementData != null)
            {
                if(DD_EditorUtils.projectManagementData.projectPaths != null)
                {
                    if (DD_EditorUtils.projectManagementData.projectPaths.Contains(AssetDatabase.GetAssetPath(this)))
                    {
                        DD_EditorUtils.projectManagementData.projectPaths.Remove(AssetDatabase.GetAssetPath(this));
                    }

                    DD_EditorUtils.projectManagementData.projectPaths.Add(AssetDatabase.GetAssetPath(this));
                }
                else
                {
                    DD_EditorUtils.projectManagementData.projectPaths = new List<string>(1);
                }
            }
                

            EditorUtility.SetDirty(this);
        }

        //OnGUI Call (see Unity API Reference for more information)
        public void OnProjectGUI()
        {
            m_viewRect_propertyView = DD_EditorUtils.viewRect_propertyView;
            m_viewRect_headerView = DD_EditorUtils.viewRect_headerView;
            m_viewRect_workView = DD_EditorUtils.viewRect_workView;

            ///<summary>
            ///Draw a temporary connection to the mouse cursor, if there is a node
            ///that wants to make a connection
            /// </summary>
            if (m_wantsConnection)
            {
                if (m_connectionAttemptingNode != null)
                {
                    Vector2 tempConnectionTarget = Vector2.zero;

                    //If connection gets close to input/output, lock connection to it, to make connecting easier
                    foreach (DD_NodeBase node in m_nodes)
                    {
                        if (!m_inverseConnection)
                        {
                            if (node.m_inputs == null) continue;

                            for (int i = 0; i < node.m_inputs.Count; i++)
                            {
                                if (Vector2.Distance(node.m_inputs[i].inputRect.position + node.m_inputs[i].inputRect.size / 2, DD_EditorUtils.currentEvent.mousePosition) < 12)
                                    tempConnectionTarget = node.m_inputs[i].inputRect.position + node.m_inputs[i].inputRect.size / 2;
                            }
                        }
                        else
                        {
                            if (node.m_outputs == null) continue;

                            for (int i = 0; i < node.m_outputs.Count; i++)
                            {
                                if (Vector2.Distance(node.m_outputs[i].outputRect.position + node.m_outputs[i].outputRect.size / 2, DD_EditorUtils.currentEvent.mousePosition) < 12)
                                {
                                    tempConnectionTarget = node.m_outputs[i].outputRect.position + node.m_outputs[i].outputRect.size / 2;
                                    m_inverseConnectionAttemptingInput.outputIndex = i;
                                }
                            }
                        }
                    }

                    //Draw temporary connection to mouse or input/output
                    if (!m_inverseConnection)
                    {
                        if (tempConnectionTarget == Vector2.zero) DrawConnection(m_connectionStart, DD_EditorUtils.currentEvent.mousePosition);
                        else DrawConnection(m_connectionStart, tempConnectionTarget);
                    }
                    else
                    {
                        if (tempConnectionTarget == Vector2.zero) DrawInverseConnection(m_connectionStart, DD_EditorUtils.currentEvent.mousePosition);
                        else DrawInverseConnection(m_connectionStart, tempConnectionTarget);
                    }
                }
            }

            ///<summary>
            ///If a connection awaits a node that doesn't yet exist
            ///it is connected to the node menu that pops up
            /// </summary>
            if (m_connectionAwaitingNewNode)
            {
                if (m_connectionAttemptingNode != null)
                {
                    if (!m_inverseConnection)
                    {
                        Vector2 tempConnectionTarget = Vector2.zero;

                        if (DD_EditorUtils.showNodeMenu) tempConnectionTarget = new Vector2(DD_EditorUtils.nodeMenuRect.position.x - DD_EditorUtils.viewRect_propertyView.width, DD_EditorUtils.nodeMenuRect.y - DD_EditorUtils.viewRect_headerView.height);
                        if (tempConnectionTarget != Vector2.zero) DrawConnection(m_connectionStart, tempConnectionTarget);
                    }
                    else
                    {
                        Vector2 tempConnectionTarget = Vector2.zero;

                        if (DD_EditorUtils.showNodeMenu) tempConnectionTarget = new Vector2(DD_EditorUtils.nodeMenuRect.position.x - DD_EditorUtils.viewRect_propertyView.width + DD_EditorUtils.nodeMenuRect.width, DD_EditorUtils.nodeMenuRect.y - DD_EditorUtils.viewRect_headerView.height);
                        if (tempConnectionTarget != Vector2.zero) DrawInverseConnection(m_connectionStart, tempConnectionTarget);
                    }
                }
            }

            //If there are nodes in the project...
            if (m_nodes.Count > 0)
            {
                //...run through all output connectors and set all to non-occupied, if they're not connected to anything...
                if (m_clearAllOutputConnectors)
                {
                    m_clearAllOutputConnectors = false;

                    for (int i = 0; i < m_nodes.Count; i++)
                        for (int j = 0; j < m_nodes[i].m_outputs.Count; j++)
                            m_nodes[i].m_outputs[j].isOccupied = false;
                }

                //...and call OnGUI on them...
                for (int i = 0; i < m_nodes.Count; i++)
                {
                    m_nodes[i].OnNodeGUI(m_viewRect_propertyView.width, m_viewRect_headerView.height);
                }

                //...and call OnNodeXontextGUI on them to be able to display the tooltip help.
                for (int i = 0; i < m_nodes.Count; i++)
                {
                    m_nodes[i].m_nodeTooltip.OnNodeContextGUI();
                }
            }

            //Draw the console
            float consolePositionX = DD_EditorUtils.viewRect_workView.position.x - DD_EditorUtils.viewRect_propertyView.width;
            float consolePositionY = DD_EditorUtils.viewRect_workView.position.y + DD_EditorUtils.viewRect_workView.height - DD_EditorUtils.viewRect_headerView.height - 24;
            Rect consoleRect = new Rect(consolePositionX, consolePositionY, DD_EditorUtils.viewRect_workView.width, 24);
            GUIStyle consoleStyle = new GUIStyle();
            consoleStyle.fontSize = 20;
            consoleStyle.normal.textColor = new Color(1, 0, 0, m_consoleAlpha);
            consoleStyle.alignment = TextAnchor.MiddleRight;

            GUI.Label(consoleRect, m_consoleMessage, consoleStyle);

            ProcessEvents();

            EditorUtility.SetDirty(this);
        }
        #endregion

        #region Utility Methods
        void ProcessEvents()
        {
            //Most events are only processed within the canvass (work view)
            if (m_viewRect_workView.Contains(DD_EditorUtils.mousePosInEditor))
            {
                //Enable control select if Ctrl is pressed
                if (DD_EditorUtils.currentEvent.keyCode == KeyCode.LeftControl || DD_EditorUtils.currentEvent.keyCode == KeyCode.RightControl)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.KeyDown) m_controlSelect = true;
                    if (DD_EditorUtils.currentEvent.type == EventType.KeyUp) m_controlSelect = false;
                }

                ///<summary>
                ///Copy nodes
                ///1. Instantiate nodes into a static node buffer
                ///2. Specify the origin node in each instantited node
                /// </summary>
                if (DD_EditorUtils.currentEvent.keyCode == KeyCode.C)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.KeyDown)
                    {
                        if (m_controlSelect && m_selectedNodes.Count != 0)
                        {
                            List<DD_NodeBase> nodeBufferList = new List<DD_NodeBase>();

                            for (int i = 0; i < m_selectedNodes.Count; i++) if (m_selectedNodes[i].m_nodeType != NodeType.Master) nodeBufferList.Add(ScriptableObject.Instantiate(m_selectedNodes[i]));
                            for (int i = 0; i < nodeBufferList.Count; i++) nodeBufferList[i].m_originNode = m_selectedNodes[i];

                            DD_EditorUtils.nodeBuffer = nodeBufferList.ToArray();
                        }
                    }
                }

                ///<summary>
                ///Paste nodes (details in the comments below)
                /// </summary>
                if (DD_EditorUtils.currentEvent.keyCode == KeyCode.V)
                {
                    if (DD_EditorUtils.currentEvent.type == EventType.KeyDown)
                    {
                        if (m_controlSelect && DD_EditorUtils.nodeBuffer != null)
                        {
                            ///<summary>
                            ///Calculate which of the copied nodes is most to the left
                            ///That node's position is set to zero, all other nodes's position is then set relative to that node
                            ///This is neccessary to paste nodes relative to the mouse cursor, starting with the 'leftest' node
                            /// </summary>
                            Vector2 leftestNode = DD_EditorUtils.windowRect.size;

                            for (int i = 0; i < DD_EditorUtils.nodeBuffer.Length; i++)
                                if (DD_EditorUtils.nodeBuffer[i].m_nodeRect.position.x < leftestNode.x)
                                    leftestNode = DD_EditorUtils.nodeBuffer[i].m_nodeRect.position;

                            m_selectedNodes.Clear();

                            for (int i = 0; i < DD_EditorUtils.nodeBuffer.Length; i++)
                                DD_EditorUtils.PasteNode(this, DD_EditorUtils.nodeBuffer[i], DD_EditorUtils.nodeBuffer[i].m_nodeRect.position, leftestNode);

                            ///<summary>
                            ///Compare the currently inputting node to the pasted nodes via originNode. If there is a match, it means, 
                            ///that a node's inputting node has been copied too and the copied node should now act as the inputting node.
                            ///This is neccessary, in order to be able to copy entire node trees including their connections.
                            ///This doesn't consider relays and will remove them.
                            /// </summary>
                            for (int i = 0; i < m_selectedNodes.Count; i++)
                            {
                                if (m_selectedNodes[i].m_inputs != null)
                                {
                                    for (int j = 0; j < m_selectedNodes[i].m_inputs.Count; j++)
                                    {
                                        DD_NodeBase currentlyInputtingNode = m_selectedNodes[i].m_inputs[j].inputtingNode;

                                        for (int k = 0; k < m_selectedNodes.Count; k++)
                                        {
                                            if (m_selectedNodes[i].m_inputs[j].inputtingNode == m_selectedNodes[k].m_originNode)
                                            {
                                                m_selectedNodes[i].m_inputs[j].inputtingNode = m_selectedNodes[k];
                                                //m_selectedNodes[k].m_originNode = null;
                                                break;
                                            }
                                        }
                                        if (m_selectedNodes[i].m_inputs[j].inputtingNode == currentlyInputtingNode || m_selectedNodes[i].m_inputs[j].inputtingNode == m_selectedNodes[i])
                                        {
                                            m_selectedNodes[i].m_inputs[j].inputtingNode = null;
                                        }
                                    }
                                }
                                m_selectedNodes[i].m_connectionRelays.Clear();
                            }
                        }
                    }
                }

                //A lot to do with the left mouse button...
                if (DD_EditorUtils.currentEvent.button == 0)
                {
                    //Call SelectNode, handle node dragging and double click on mouse down
                    if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                    {
                        if (DD_EditorUtils.allowSelection)
                        {
                            if (!DD_EditorUtils.preventSelectionForOneEvent) SelectNode(true);
                            else DD_EditorUtils.preventSelectionForOneEvent = false;
                        }

                        m_enableNodeDragging = false;

                        for (int i = m_selectedNodes.Count - 1; i >= 0; i--)
                        {
                            if (m_selectedNodes[i].m_scaledNodeRect.Contains(DD_EditorUtils.mousePosInEditor))
                            {
                                m_enableNodeDragging = true;
                            }
                        }

                        if (m_firstClickTimer == 0) m_firstClickTimer = m_t;
                        else
                        {
                            m_secondClickTimer = m_t;
                            if (m_secondClickTimer - m_firstClickTimer <= 0.3f)
                            {
                                foreach (DD_NodeBase node in m_nodes) node.doubleClicked = true;

                                m_firstClickTimer = 0;
                                m_secondClickTimer = 0;
                            }
                        }

                        //if (!DD_EditorUtils.showNodeMenu)
                        //    if (!DD_EditorUtils.drawDragTexture)
                        //        DD_EditorUtils.currentEvent.Use();
                    }

                    ///<summary>
                    ///When left mouse button is released,
                    ///1. Call SelectNode
                    ///2. Handle temporary connection
                    ///3. Clear relay cache
                    ///4. Disable node dragging mode
                    /// </summary>
                    if (DD_EditorUtils.currentEvent.rawType == EventType.MouseUp)
                    {
                        if (DD_EditorUtils.allowSelection)
                        {
                            if (!DD_EditorUtils.preventSelectionForOneEvent) SelectNode(false);
                            else DD_EditorUtils.preventSelectionForOneEvent = false;
                        }

                        if (m_wantsConnection)
                        {
                            if (m_nodes != null)
                            {
                                //Check if an input connector has been hit on mouse up
                                bool hitInputConnector = false;
                                foreach (DD_NodeBase node in m_nodes)
                                {
                                    if (node.m_inputs == null) continue;
                                    foreach (DD_InputConnector input in node.m_inputs)
                                    {
                                        if (input.inputRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                                        {
                                            hitInputConnector = true;
                                            break;
                                        }
                                    }
                                    if (hitInputConnector) break;
                                }

                                ///<summary>
                                ///If no input connector has been hit...
                                ///...Abort connection process if moving existing connection (eff. delete connection)
                                ///...Open node menu, if attempting new connection to a new node. Upon adding a new node, the connection will be established to the node's first input connector
                                /// </summary>
                                if (!hitInputConnector && !m_connectionAwaitingNewNode)
                                {
                                    if (m_movingExistingConnection)
                                    {
                                        m_movingExistingConnection = false;
                                        m_connectionAttemptingNode.m_outputs[m_outputIndex].isOccupied = false;
                                    }
                                    else
                                    {
                                        if (!m_inverseConnection)
                                        {
                                            //DD_EditorUtils.nodeMenuRect.position = DD_EditorUtils.mousePosInEditor;
                                            DD_EditorUtils.triggerSearchStringSelection = true;
                                            DD_EditorUtils.showNodeMenu = true;
                                            m_connectionAwaitingNewNode = true;

                                            //if(!DD_EditorUtils.drawDragTexture)
                                            //    DD_EditorUtils.currentEvent.Use();
                                        }
                                    }
                                }

                                ///<summary>
                                ///In case of an inverse connection, connection is attempted from an input to an output, as opposed to the other way around.
                                ///1. Connection attempting node is still the node with the output, but here it's found by inverse connection
                                ///2. Connection is established if output connector was hit
                                ///3. If no output connector was hit, open node menu to add a new node. Upon adding a new node, the connection will be established to the node's first output connector
                                ///</summary>
                                if (m_inverseConnection)
                                {
                                    bool hitOutputConnector = false;
                                    DD_NodeBase inverseConnectionAttemptingNode = null;
                                    foreach (DD_NodeBase node in m_nodes)
                                    {
                                        foreach (DD_OutputConnector output in node.m_outputs)
                                        {
                                            if (output.outputRect.Contains(DD_EditorUtils.currentEvent.mousePosition))
                                            {
                                                hitOutputConnector = true;
                                                inverseConnectionAttemptingNode = node;
                                                break;
                                            }
                                            if (hitInputConnector) break;
                                        }
                                    }

                                    if (hitOutputConnector)
                                    {
                                        m_inverseConnectionAttemptingInput.inputtingNode = inverseConnectionAttemptingNode;
                                        inverseConnectionAttemptingNode.m_redoCalculation = true;

                                        //Start Recursion Check
                                        inverseConnectionAttemptingNode.m_checkForRecursion = true; ;
                                        inverseConnectionAttemptingNode.m_recursionCandidate = inverseConnectionAttemptingNode;
                                        inverseConnectionAttemptingNode.m_pendingConnector = m_inverseConnectionAttemptingInput;
                                    }
                                    else
                                    {
                                        //DD_EditorUtils.nodeMenuRect.position = DD_EditorUtils.mousePosInEditor - new Vector2(DD_EditorUtils.nodeMenuRect.width, 0);
                                        DD_EditorUtils.triggerSearchStringSelection = true;
                                        DD_EditorUtils.showNodeMenu = true;
                                        m_connectionAwaitingNewNode = true;

                                        //if (!DD_EditorUtils.drawDragTexture)
                                        //    DD_EditorUtils.currentEvent.Use();
                                    }
                                }

                                m_wantsConnection = false;
                            }
                        }
                        m_enableNodeDragging = false;

                        m_relayCache = new DD_ConnectionRelay[0];

                        //if (!DD_EditorUtils.showNodeMenu)
                        //    if (!DD_EditorUtils.drawDragTexture)
                        //        DD_EditorUtils.currentEvent.Use();
                    }
                }

                //Delete selected nodes
                if (DD_EditorUtils.currentEvent.keyCode == KeyCode.Delete)
                    if (DD_EditorUtils.currentEvent.type == EventType.KeyDown)
                        DeleteNodes();
            }

            //Node dragging - during dragging selection is disabled
            if (DD_EditorUtils.currentEvent.type == EventType.MouseDrag && m_enableNodeDragging)
            {
                foreach (DD_NodeBase node in m_selectedNodes)
                {
                    node.m_nodeRect.position += DD_EditorUtils.currentEvent.delta / DD_EditorUtils.zoomFactor;
                    DD_EditorUtils.allowSelectionRectRender = false;
                }

                m_preventSelect = true;
            }

            if (DD_EditorUtils.currentEvent.type == EventType.MouseDown)
                if (!DD_EditorUtils.showNodeMenu)
                    DD_EditorUtils.currentEvent.Use();
        }

        /// <summary>
        /// Deselects all nodes and clears selected nodes list
        /// </summary>
        public void DeselectAllNodes()
        {
            for (int i = 0; i < m_nodes.Count; i++)
                m_nodes[i].m_isSelected = false;

            m_selectedNodes.Clear();
        }

        /// <summary>
        /// Node selection method:
        /// 1. Works only if selection is not prevented by a bool
        /// 2. Works different based on whether the associated event is mouse down or mouse up
        /// </summary>
        /// <param name="mouseDown"></param>
        void SelectNode(bool mouseDown)
        {
            if (m_preventSelect)
            {
                m_preventSelect = false;
                return;
            }

            //Handles control select
            if (m_controlSelect)
            {
                for (int i = m_nodes.Count - 1; i >= 0; i--)
                {
                    if (m_nodes[i].m_scaledNodeRectTotal.Contains(DD_EditorUtils.mousePosInEditor))
                    {
                        //At mouse down on a non-selected node, add the node to the selected nodes list and move the node to the end of the nodes list to have it render on top
                        if (!m_selectedNodes.Contains(m_nodes[i]) && mouseDown)
                        {
                            m_selectedNodes.Add(m_nodes[i]);

                            m_nodes.Add(m_nodes[i]);
                            m_nodes.RemoveAt(i);

                            m_preventSelect = true;

                            break;
                        }
                        //At mouse up on a selected node remove the node from the selected nodes list
                        if (m_selectedNodes.Contains(m_nodes[i]) && !mouseDown)
                        {
                            m_selectedNodes.Remove(m_nodes[i]);
                            break;
                        }
                    }
                }
            }
            //Handles simple select
            else
            {
                bool clickedSelected = false;

                //Check if an already selected node was clicked
                for (int i = 0; i < m_selectedNodes.Count; i++)
                    if (m_selectedNodes[i].m_scaledNodeRectTotal.Contains(DD_EditorUtils.mousePosInEditor))
                        clickedSelected = true;

                if (m_selectedNodes.Count >= 1)
                {
                    if (mouseDown && clickedSelected) return;
                    if (mouseDown && CheckIfClickHitAnyNode(DD_EditorUtils.mousePosInEditor)) return;
                }

                if (DD_EditorUtils.allowSelection)
                    DeselectAllNodes();

                //Same as control-select but all nodes are deselected first (simple select)
                for (int i = m_nodes.Count - 1; i >= 0; i--)
                {
                    if (m_nodes[i].m_scaledNodeRectTotal.Contains(DD_EditorUtils.mousePosInEditor))
                    {
                        m_selectedNodes.Add(m_nodes[i]);

                        m_nodes.Add(m_nodes[i]);
                        m_nodes.RemoveAt(i);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Simple method for box-selecting nodes
        /// </summary>
        void BoxSelectNodes()
        {

            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                if (m_selectionRect.Contains(m_nodes[i].m_scaledNodeRect.position))
                {
                    if (!m_selectedNodes.Contains(m_nodes[i]))
                        m_selectedNodes.Add(m_nodes[i]);
                }
            }
        }

        /// <summary>
        /// Deletes selected nodes
        /// </summary>
        void DeleteNodes()
        {
            for (int i = 0; i < m_selectedNodes.Count; i++)
            {
                if (m_selectedNodes[i].m_nodeType != NodeType.Master)
                {
                    DD_EditorUtils.DeleteNode(m_selectedNodes[i]);
                    m_nodes.Remove(m_selectedNodes[i]);

                    //Only for SetVariable-Nodes: Removes SetVariable-Node from the list of registered nodes. The GetVariable Nodes are reassigned to the resulting list (see some 10 lines below).
                    if (m_registeredNodes.Contains(m_selectedNodes[i]))
                        m_registeredNodes.Remove(m_selectedNodes[i]);
                }
                else
                    ConsoleMessage("Cannot delete master node");
            }

            //Recreates the array of variable names and runs the UpdateReference method on the GetVariable-Nodes
            //Each node will look for the new array id of it's name to reassign the correct SetVariable-node to itself.
            m_registeredVariableNames = new string[m_registeredNodes.Count];
            for (int i = 0; i < m_registeredNodes.Count; i++) m_registeredVariableNames[i] = m_registeredNodes[i].m_nodeName;

            for (int j = 0; j < m_nodes.Count; j++)
            {
                if (m_nodes[j].m_nodeType == NodeType.GetVariable) m_nodes[j].UpdateNodeReference();
                m_nodes[j].m_highlightReference = false;
            }

            m_selectedNodes.Clear();
            m_clearAllOutputConnectors = true;

            //Let all nodes recalculate after selected nodes have been deleted, to prevent leaking of data and remove unrequired data
            //if (m_nodes != null) foreach (DD_NodeBase node in m_nodes) node.m_redoCalculation = true;
        }

        /// <summary>
        /// Method that checks if mouse is on any node (header)
        /// Can be called arbitrarily
        /// </summary>
        /// <param name="mousePosInEditor"></param>
        /// <returns></returns>
        bool CheckIfClickHitAnyNode(Vector2 mousePosInEditor)
        {
            bool clickedOusideOfNodes = false;
            int x = 0;

            for (int i = 0; i < m_nodes.Count; i++)
                if (m_nodes[i].m_scaledNodeRect.Contains(mousePosInEditor)) x++;

            if (x == 0) clickedOusideOfNodes = true;

            return clickedOusideOfNodes;
        }

        ///<summary>
        ///Draw temporary connection
        ///1. If there are no relays, draw from output to input
        ///2. If there are relays, draw from output to first relay...
        ///...from first relay to last relay connecting all relays in between...
        ///...and finally from last relay to input.
        /// </summary>
        void DrawConnection(Vector2 startPoint, Vector2 endPoint)
        {
            Handles.BeginGUI();

            Handles.color = Color.white;
            Color connectionColor = m_connectionColor;

            float connectionDistance;

            Vector2 startTangent;
            Vector2 endTangent;

            if (m_relayCache.Length == 0)
            {
                //connectionDistance = Vector2.Distance(startPoint, endPoint);
                connectionDistance = Mathf.Abs(startPoint.x - endPoint.x);

                startTangent = startPoint + Vector2.right * connectionDistance / 2;
                endTangent = endPoint + Vector2.left * connectionDistance / 2;

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, connectionColor, null, 5);
            }
            else
            {
                //float connectionDistanceLeft = Vector2.Distance(startPoint, m_relayCache[0].m_center);
                //float connectionDistanceRight = Vector2.Distance(m_relayCache[m_relayCache.Length - 1].m_center, endPoint);

                float connectionDistanceLeft = Mathf.Abs(startPoint.x - m_relayCache[0].m_center.x);
                float connectionDistanceRight = Mathf.Abs(m_relayCache[m_relayCache.Length - 1].m_center.x - endPoint.x);

                startTangent = startPoint + Vector2.right * Mathf.Min(100, connectionDistanceLeft / 2);
                endTangent = endPoint + Vector2.left * Mathf.Min(100, connectionDistanceRight / 2);

                //float partialConnectionDistance = Vector2.Distance(startPoint, m_relayCache[0].m_center);
                float partialConnectionDistance = Mathf.Abs(startPoint.x - m_relayCache[0].m_center.x);

                Vector2 relayTangentLeft = m_relayCache[0].m_center - m_relayCache[0].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                Vector2 relayTangentRight;

                Handles.DrawBezier(startPoint, m_relayCache[0].m_center, startTangent, relayTangentLeft, connectionColor, null, 5);

                for (int i = 0; i < m_relayCache.Length - 1; i++)
                {
                    //partialConnectionDistance = Vector2.Distance(m_relayCache[i].m_center, m_relayCache[i + 1].m_center);
                    partialConnectionDistance = Mathf.Abs(m_relayCache[i].m_center.x - m_relayCache[i + 1].m_center.x);

                    relayTangentLeft = m_relayCache[i + 1].m_center - m_relayCache[i + 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                    relayTangentRight = m_relayCache[i].m_center + m_relayCache[i].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                    Handles.DrawBezier(m_relayCache[i].m_center, m_relayCache[i + 1].m_center, relayTangentRight, relayTangentLeft, connectionColor, null, 5);
                }

                //partialConnectionDistance = Vector2.Distance(m_relayCache[m_relayCache.Length - 1].m_center, endPoint);
                partialConnectionDistance = Mathf.Abs(m_relayCache[m_relayCache.Length - 1].m_center.x - endPoint.x);

                relayTangentRight = m_relayCache[m_relayCache.Length - 1].m_center + m_relayCache[m_relayCache.Length - 1].m_tangentDirection * Mathf.Min(100 * DD_EditorUtils.zoomFactor, partialConnectionDistance / 2);
                Handles.DrawBezier(m_relayCache[m_relayCache.Length - 1].m_center, endPoint, relayTangentRight, endTangent, connectionColor, null, 5);
            }

            Handles.EndGUI();
        }

        /// <summary>
        /// Same as draw connection, but from input to output as opposed to the other way around
        /// No possibility to move existing connections, so relays don't have to be accounted for.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        void DrawInverseConnection(Vector2 startPoint, Vector2 endPoint)
        {
            Handles.BeginGUI();

            Handles.color = Color.white;
            Color connectionColor = Color.grey;

            float connectionDistance = Mathf.Abs(startPoint.x - endPoint.x); ;

            Vector2 startTangent = startPoint + Vector2.left * connectionDistance / 2;
            Vector2 endTangent = endPoint + Vector2.right * connectionDistance / 2;

            Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, connectionColor, null, 5);

            Handles.EndGUI();
        }

        /// <summary>
        /// Called when something needs to be shown in the Derive console
        /// </summary>
        /// <param name="message"></param>
        public void ConsoleMessage(string message)
        {
            m_consoleMessage = message;
            m_consoleAlpha = 3;
        }
        #endregion
    }
}
#endif
