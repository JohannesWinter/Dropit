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
    public class DD_NodeAOFromHeight : DD_NodeBase
    {
        #region public variables
        public float m_strength = 1;
        public float m_bias = 0.002f;
        public float m_smoothness = 0.01f;
        #endregion

        #region private variables
        bool m_showOutput = true;
        bool m_showAOSettings = true;
        #endregion

        #region constructors
        public DD_NodeAOFromHeight()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(1) { new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderYellow";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.AOFromHeight;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Heightmap";
            m_outputs[0].outputLabel = "AO";

            m_outputs[0].outputDataType = DataType.Float;
        }

        public override void UpdateNode()
        {

            base.UpdateNode();

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputTexture.Apply();

                m_outputHasChanged = true;
            }

            if (m_redoCalculation) Perform();

            EditorUtility.SetDirty(this);
        }
        #endregion

        #region utility methods
        /// <summary>
        /// DrawProperties draws the node's properties into the property view
        /// This method is called from the property view!!
        /// </summary>
        public override void DrawProperties()
        {
            Rect rt = EditorGUILayout.BeginVertical();

            DD_GUILayOut.TitleLabel("Ambient Occlusion");

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            m_showAOSettings = DD_GUILayOut.FoldOut(m_showAOSettings, "AO Settings");

            if (m_showAOSettings)
            {
                EditorGUI.BeginChangeCheck();
                m_strength = DD_GUILayOut.FloatField("Strength", m_strength);
                m_bias = DD_GUILayOut.Slider("Bias", m_bias, 0.0001f, 0.02f);
                m_smoothness = DD_GUILayOut.Slider("Smoothness", m_smoothness, 0, 1);
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;
            }
        }

        /// <summary>
        /// Performs the actual node operation generating a normal map from the heightmap input
        /// </summary>
        void Perform()
        {
            if (m_inputs[0].inputtingNode == null)
            {
                m_outputs[0].outputTexture.Reinitialize(1, 1);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
            }
            else
            {
                Texture2D inputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D inputTexture2 = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

                inputTexture = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

                DD_NodeUtils.Blur(inputTexture, inputTexture2, m_smoothness / 250);
                inputTexture2.Apply();

                DD_NodeUtils.AOFromHeight(inputTexture2, m_outputs[0].outputTexture, m_strength, m_bias);
            }

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif