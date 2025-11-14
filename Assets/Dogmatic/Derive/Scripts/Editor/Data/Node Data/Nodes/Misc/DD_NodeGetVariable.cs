// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DeriveUtils;
using System;

namespace Derive
{
    [Serializable]
    public class DD_NodeGetVariable : DD_NodeBase
    {

        #region Public Variables
        public DD_NodeBase m_registeredNode;
        #endregion

        #region private variables
        bool m_baseSettings = true;
        bool m_showOutput = true;
        #endregion

        #region Constructor
        public DD_NodeGetVariable()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderPurple";
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Initialize everything specific to this node type
        /// </summary>
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.GetVariable;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_nodeName = " ";

            m_outputs[0].outputLabel = "Out";

            m_outputs[0].outputDataType = DataType.Float;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputDataType = DataType.Float;
                m_outputs[0].outputTexture.Apply();

                m_outputHasChanged = true;
            }

            if (m_redoCalculation) Perform();

            ///<summary>
            ///Handles whether the node is referencing an existing node
            ///If yes, the name will be set to the name of the referenced node with an asterisk
            ///Otherwise, the name is set to an empty string
            /// </summary>
            if (m_sourceNodeIndex < m_parentProject.m_registeredVariableNames.Length) 
            {
                m_nodeName = m_parentProject.m_registeredVariableNames[m_sourceNodeIndex] + "*";
                m_outputs[0].outputDataType = m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_outputs[0].outputDataType;
            }
            else
            {
                m_nodeName = "";
                m_redoCalculation = true;
            }

            //Handles highlighting of the referenced node and checks if the output of the referenced node has changed
            if(m_sourceNodeIndex < m_parentProject.m_registeredNodes.Count)
            {
                if (m_parentProject.m_registeredNodes[m_sourceNodeIndex] != null)
                {
                    if (m_isSelected) m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_highlightReference = true;
                    else m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_highlightReference = false;

                    if (m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_outputHasChanged) m_outputHasChanged = true;
                }
            }

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            EditorUtility.SetDirty(this);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// DrawProperties draws the node's properties into the property view
        /// This method is called from the property view!!
        /// </summary>
        public override void DrawProperties()
        {
            Rect rt = EditorGUILayout.BeginVertical();

            DD_GUILayOut.TitleLabel(NodeType.GetVariable.ToString());

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                EditorGUILayout.Space(10);
                GUILayout.BeginHorizontal();

                GUILayout.Label(new GUIContent("Reference"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                m_sourceNodeIndex = EditorGUILayout.Popup(m_sourceNodeIndex, m_parentProject.m_registeredVariableNames, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                if (EditorGUI.EndChangeCheck())
                {
                    m_redoCalculation = true;
                    for (int i = 0; i < m_parentProject.m_registeredNodes.Count; i++) m_parentProject.m_registeredNodes[i].m_highlightReference = false;
                }

                GUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// This is run when a referencing node has been deleted
        /// This node will try to reassign itself to the same node as before, finding it by name
        /// If it cannot find it, it means it has been deleted - the source node index is then set to well outside the range of the array of referencing images, so it shows as missing a reference.
        /// </summary>
        public override void UpdateNodeReference()
        {
            base.UpdateNodeReference();

            string name = m_nodeName.Replace("*", "");

            if (Array.IndexOf(m_parentProject.m_registeredVariableNames, name) > -1)
                m_sourceNodeIndex = Array.IndexOf(m_parentProject.m_registeredVariableNames, name);
            else
                m_sourceNodeIndex = 10000;

            m_redoCalculation = true;
        }

        /// <summary>
        /// Performs the actual node operation
        /// Will show the output of the referenced node with the same data type
        /// If there is no referenced node it will show black as datatype float
        /// </summary>
        void Perform()
        {
            if (m_sourceNodeIndex < m_parentProject.m_registeredNodes.Count)
            {
                m_outputs[0].outputTexture = m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_outputs[0].outputTexture;
                m_outputs[0].outputDataType = m_parentProject.m_registeredNodes[m_sourceNodeIndex].m_outputs[0].outputDataType;
            }
            else
            {
                m_nodeName = "";

                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputDataType = DataType.Float;
            }

            if(m_outputs[0].outputTexture != null)
            {
                m_outputs[0].outputTexture.Apply();
                m_redoCalculation = false;
                m_outputHasChanged = true;
            }
                
        }
        #endregion
    }
}
#endif