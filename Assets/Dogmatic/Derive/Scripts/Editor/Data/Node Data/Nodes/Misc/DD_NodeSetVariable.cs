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
    public class DD_NodeSetVariable : DD_NodeBase
    {
        #region Public Variables
        public DD_NodeBase m_registeredNode;
        #endregion

        #region private variables
        bool m_baseSettings = true;
        bool m_showOutput = true;
        #endregion

        #region Constructor
        public DD_NodeSetVariable()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(1) { new DD_InputConnector() };

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

            m_nodeType = NodeType.SetVariable;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "In";
            m_outputs[0].outputLabel = "Out";

            m_outputs[0].outputDataType = DataType.Float;

            DD_EditorUtils.HandleDuplicateNames(m_nodeName, this);
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

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            //Highlights the node, if a node that references it, is selected
            if (m_highlightReference && !m_isSelected) GUI.Box(m_absNodeRect, "", DD_EditorUtils.editorSkin.GetStyle("NodeSelectionHighlight_Reference"));

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

            DD_GUILayOut.TitleLabel(NodeType.SetVariable.ToString());

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                EditorGUI.BeginChangeCheck();
                m_nodeName = DD_GUILayOut.TextField("Variable Name", m_nodeName);
                if (EditorGUI.EndChangeCheck()) m_nodeName = DD_EditorUtils.HandleDuplicateNames(m_nodeName, this);
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Performs the actual node operation
        /// Grabs the data of the inputting node and outputs it, also using the same data type
        /// 
        /// </summary>
        void Perform()
        {
            if (m_inputs[0].inputtingNode == null)
            {
                m_outputs[0].outputTexture.Reinitialize(1, 1);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputDataType = DataType.Float;
            }
            else
            {
                Texture2D inputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                inputTexture = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

                DD_NodeUtils.TexToTex(inputTexture, m_outputs[0].outputTexture, false, false);
                m_outputs[0].outputDataType = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputDataType;

                if (!m_parentProject.m_registeredNodes.Contains(this)) m_parentProject.m_registeredNodes.Add(this);
            }

            m_outputs[0].outputTexture.Apply();

            m_nodeName = DD_EditorUtils.HandleDuplicateNames(m_nodeName, this);

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif