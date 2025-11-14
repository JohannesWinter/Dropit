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
    public class DD_NodeVector : DD_NodeBase
    {
        #region Public Variables
        #endregion

        #region private variables
        bool m_baseSettings = true;
        bool m_showOutput = true;

        public Vector4 m_rgbaVector;
        #endregion

        #region Constructor
        public DD_NodeVector()
        {
            m_inputs = null;

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderRed";
        }
        #endregion

        #region Main Methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.RGBAVector;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputLabel = "RGBA";

            m_outputs[0].outputDataType = DataType.RGBA;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            m_nodeBodyHeight = 145 * DD_EditorUtils.zoomFactor;

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(m_rgbaVector.x, m_rgbaVector.y, m_rgbaVector.z, m_rgbaVector.w));
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
            ///This area draws 4 float input fields directly on the node
            ///and sets "redoCalculation" to true, when a float value is changed
            ///The floats are appended to a Vector4
            /// </summary>
            Rect textFieldRect = new Rect(m_scaledNodeRect.position.x + 8 * DD_EditorUtils.zoomFactor - DD_EditorUtils.viewRect_propertyView.width, m_outputs[0].outputRect.position.y, m_scaledNodeRect.width - m_outputs[0].outputRect.width - 50 * DD_EditorUtils.zoomFactor, 140 * DD_EditorUtils.zoomFactor);

            GUIStyle floatInputStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("TextInput"));
            floatInputStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("TextInput").fontSize * DD_EditorUtils.zoomFactor);

            GUIStyle labelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"));
            labelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("DefaultLabel").fontSize * DD_EditorUtils.zoomFactor);

            float labelWidthCache = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 20 * DD_EditorUtils.zoomFactor;

            GUILayout.BeginArea(textFieldRect);

            EditorGUI.BeginChangeCheck();

            m_rgbaVector.x = DD_GUILayOut.FloatFieldOnNode("R", m_rgbaVector.x, textFieldRect.width - 15, floatInputStyle, labelStyle);
            m_rgbaVector.y = DD_GUILayOut.FloatFieldOnNode("G", m_rgbaVector.y, textFieldRect.width - 15, floatInputStyle, labelStyle);
            m_rgbaVector.z = DD_GUILayOut.FloatFieldOnNode("B", m_rgbaVector.z, textFieldRect.width - 15, floatInputStyle, labelStyle);
            m_rgbaVector.w = DD_GUILayOut.FloatFieldOnNode("A", m_rgbaVector.w, textFieldRect.width - 15, floatInputStyle, labelStyle);

            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

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

            DD_GUILayOut.TitleLabel(NodeType.RGBAVector.ToString());

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                m_nodeName = DD_GUILayOut.TextField("Node Name", m_nodeName);

                EditorGUI.BeginChangeCheck();
                m_rgbaVector.x = DD_GUILayOut.FloatField("R", m_rgbaVector.x);
                m_rgbaVector.y = DD_GUILayOut.FloatField("G", m_rgbaVector.y);
                m_rgbaVector.z = DD_GUILayOut.FloatField("B", m_rgbaVector.z);
                m_rgbaVector.w = DD_GUILayOut.FloatField("A", m_rgbaVector.w);
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }


        /// <summary>
        /// Performs the actual node operation
        /// </summary>
        void Perform()
        {
            m_outputs[0].outputTexture.SetPixel(0, 0, new Color(m_rgbaVector.x, m_rgbaVector.y, m_rgbaVector.z, m_rgbaVector.w));
            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif