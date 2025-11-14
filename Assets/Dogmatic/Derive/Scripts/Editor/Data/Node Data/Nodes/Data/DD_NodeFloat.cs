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
    public class DD_NodeFloat : DD_NodeBase
    {
        #region Public Variables
        public float m_floatValue = 0;
        #endregion

        #region private variables
        bool m_baseSettings = true;
        bool m_showOutput = true;
        #endregion

        #region Constructor
        public DD_NodeFloat()
        {
            m_inputs = null;

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderRed";
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

            m_nodeType = NodeType.Float;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputDataType = DataType.Float;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat));
                m_outputs[0].outputTexture.Apply();

                m_outputHasChanged = true;
            }

            if (m_redoCalculation) Perform();

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            ///<summary>
            ///This area draws a float input field directly on the node
            ///and sets "redoCalculation" to true, when the float value is changed
            /// </summary>
            Rect textFieldRect = new Rect(m_scaledNodeRect.position.x + 8 * DD_EditorUtils.zoomFactor - DD_EditorUtils.viewRect_propertyView.width, m_outputs[0].outputRect.position.y, m_scaledNodeRect.width - m_outputs[0].outputRect.width - 16 * DD_EditorUtils.zoomFactor, m_outputs[0].outputRect.height);

            GUIStyle floatInputStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("TextInput"));
            floatInputStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("TextInput").fontSize * DD_EditorUtils.zoomFactor);

            float labelWidthCache = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 5;

            GUILayout.BeginArea(textFieldRect);

            EditorGUI.BeginChangeCheck();

            m_floatValue = EditorGUILayout.FloatField(new GUIContent(" "), m_floatValue, floatInputStyle);
            m_outputs[0].outputFloat = m_floatValue;

            if (EditorGUI.EndChangeCheck())
            {
                m_redoCalculation = true;
                DD_EditorUtils.allowSelection = false;
            }

            EditorGUIUtility.labelWidth = labelWidthCache;

            GUILayout.EndArea();

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

            DD_GUILayOut.TitleLabel(NodeType.Float.ToString());

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                m_nodeName = DD_GUILayOut.TextField("Node Name", m_nodeName);

                EditorGUI.BeginChangeCheck();
                m_floatValue = DD_GUILayOut.FloatField("Output", m_floatValue);
                m_outputs[0].outputFloat = m_floatValue;
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        void Perform()
        {
            m_outputs[0].outputTexture.SetPixel(0, 0, new Color(m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat));
            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif