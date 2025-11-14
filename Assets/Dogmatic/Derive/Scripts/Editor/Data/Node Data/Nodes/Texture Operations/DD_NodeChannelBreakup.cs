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
    public class DD_NodeChannelBreakup : DD_NodeBase
    {
        #region public variables
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeChannelBreakup()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(1) { new DD_InputConnector() };

            m_connectorStyles = new string[10] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected",
            "ConnectorRedUnconnected", "ConnectorRedConnected",
            "ConnectorGreenUnconnected", "ConnectorGreenConnected",
            "ConnectorBlueUnconnected", "ConnectorBlueConnected",
            "ConnectorGrayUnconnected", "ConnectorGrayConnected" };

            m_nodeStyle = "NodeHeaderBlack";
        }
        #endregion

        #region main methods
        /// <summary>
        /// In this node connector drawings in the base class are overridden and the connectors are drawn here
        /// This is because this node uses connectors with different colors
        /// </summary>
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector() };


            base.InitNode();

            m_nodeType = NodeType.ChannelBreakup;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = true;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "RGBA";

            m_outputs[0].outputLabel = "R";
            m_outputs[1].outputLabel = "G";
            m_outputs[2].outputLabel = "B";
            m_outputs[3].outputLabel = "A";

            m_outputs[0].outputDataType = DataType.Float;
            m_outputs[1].outputDataType = DataType.Float;
            m_outputs[2].outputDataType = DataType.Float;
            m_outputs[3].outputDataType = DataType.Float;

            m_hasNodePreview = false;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[1].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[2].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[3].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                m_outputHasChanged = true;
            }

            if (m_redoCalculation) Perform();

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            //Output connectors of texture node have a different color scheme, thus we override the drawing of the connectors in node base and draw them here instead.
            GUIStyle outputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("OutputLabel"));
            outputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("OutputLabel").fontSize * DD_EditorUtils.zoomFactor);

            float outputSize = 24 * DD_EditorUtils.zoomFactor;
            float topSpace = 16 * DD_EditorUtils.zoomFactor;
            float spaceBetweenOutputs = 32 * DD_EditorUtils.zoomFactor;

            for (int i = 0; i < m_outputs.Count; i++)
            {
                m_outputs[i].outputRect = new Rect(m_absNodeBodyRect.x + m_absNodeBodyRect.width - outputSize, m_absNodeBodyRect.y + topSpace + spaceBetweenOutputs * i, outputSize, outputSize);

                if (!m_outputs[i].isOccupied) GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[i * 2 + 2]));
                else GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[i * 2 + 3]));

                float labelWidth = outputLabelStyle.CalcSize(new GUIContent(m_outputs[i].outputLabel)).x;

                Rect labelRect = new Rect(m_outputs[i].outputRect.position - new Vector2(labelWidth, 0), new Vector2(labelWidth, outputSize));
                GUI.Label(labelRect, m_outputs[i].outputLabel, outputLabelStyle);
            }
        }
        #endregion

        #region utility methods
        /// <summary>
        /// DrawProperties draws the node's properties into the property view
        /// This method is called from the property view!!
        /// </summary>
        public override void DrawProperties()
        {
            Rect rt1 = EditorGUILayout.BeginVertical();

            DD_GUILayOut.TitleLabel("Channel Breakup");

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
            {
                Rect rt2 = EditorGUILayout.BeginVertical();
                EditorGUILayout.Space(10);
                DD_GUILayOut.Label("Red Channel");
                EditorGUILayout.EndVertical();

                Rect rt3 = new Rect(rt1.position, new Vector2(rt1.width, rt1.height + rt2.height));

                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt3);

                Rect rt4 = EditorGUILayout.BeginVertical();
                EditorGUILayout.Space(10);
                DD_GUILayOut.Label("Green Channel");
                EditorGUILayout.EndVertical();

                DD_GUILayOut.DrawTexture(m_outputs[1].outputTexture, rt4);

                Rect rt5 = EditorGUILayout.BeginVertical();
                EditorGUILayout.Space(10);
                DD_GUILayOut.Label("Blue Channel");
                EditorGUILayout.EndVertical();

                DD_GUILayOut.DrawTexture(m_outputs[2].outputTexture, rt5);

                Rect rt6 = EditorGUILayout.BeginVertical();
                EditorGUILayout.Space(10);
                DD_GUILayOut.Label("Alpha Channel");
                EditorGUILayout.EndVertical();

                DD_GUILayOut.DrawTexture(m_outputs[3].outputTexture, rt6);
            }
                
        }

        /// <summary>
        /// Breaks the input into it's channels and outputs each channel separately (R, G, B & A)
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

                DD_NodeUtils.TextureComponent(inputTexture, m_outputs[0].outputTexture, 1, 0, 0, 0);
                DD_NodeUtils.TextureComponent(inputTexture, m_outputs[1].outputTexture, 0, 1, 0, 0);
                DD_NodeUtils.TextureComponent(inputTexture, m_outputs[2].outputTexture, 0, 0, 1, 0);
                DD_NodeUtils.TextureComponent(inputTexture, m_outputs[3].outputTexture, 0, 0, 0, 1);
            }

            m_outputs[0].outputTexture.Apply();
            m_outputs[1].outputTexture.Apply();
            m_outputs[2].outputTexture.Apply();
            m_outputs[3].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif