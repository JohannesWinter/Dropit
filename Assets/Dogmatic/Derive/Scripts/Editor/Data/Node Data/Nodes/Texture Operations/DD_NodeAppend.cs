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
    public class DD_NodeAppend : DD_NodeBase
    {
        #region public variables
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeAppend()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(4) { new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector() };

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
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Append;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = true;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputLabel = "RGBA";

            m_inputs[0].inputLabel = "R";
            m_inputs[1].inputLabel = "G";
            m_inputs[2].inputLabel = "B";
            m_inputs[3].inputLabel = "A";

            m_outputs[0].outputDataType = DataType.RGBA;
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

            //Makes sure only outputs of type float can be connected to the inputs of the append node
            for(int i = 0; i < m_inputs.Count; i++)
            {
                if (m_inputs[i].inputtingNode != null)
                {
                    if (m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputDataType != DataType.Float)
                    {
                        m_parentProject.ConsoleMessage("Can't connect Output of type '" 
                            + m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputDataType.ToString() 
                            + "' to input of type '" + DataType.Float.ToString() 
                            + "'");
                        
                        m_inputs[i].inputtingNode = null;
                    }
                }
            }

            if (m_redoCalculation) Perform();

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            //Output connectors of texture node have a different color scheme, thus we override the drawing of the connectors in node base and draw them here instead.
            GUIStyle inputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("InputLabel"));
            inputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("InputLabel").fontSize * DD_EditorUtils.zoomFactor);

            float inputSize = 24 * DD_EditorUtils.zoomFactor;
            float topSpace = 16 * DD_EditorUtils.zoomFactor;
            float spaceBetweenOutputs = 32 * DD_EditorUtils.zoomFactor;

            for (int i = 0; i < m_inputs.Count; i++)
            {
                m_inputs[i].inputRect = new Rect(m_absNodeBodyRect.x, m_absNodeBodyRect.y + topSpace + spaceBetweenOutputs * i, inputSize, inputSize);

                if (!m_inputs[i].isOccupied) GUI.Box(m_inputs[i].inputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[2 + i * 2]));
                else GUI.Box(m_inputs[i].inputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[i * 2 + 3]));

                float labelWidth = inputLabelStyle.CalcSize(new GUIContent(m_inputs[i].inputLabel)).x;

                Rect labelRect = new Rect(m_inputs[i].inputRect.position + new Vector2(inputSize, 0), new Vector2(labelWidth, inputSize));
                GUI.Label(labelRect, m_inputs[i].inputLabel, inputLabelStyle);
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
            Rect rt = EditorGUILayout.BeginVertical();

            DD_GUILayOut.TitleLabel(NodeType.Append.ToString());

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Appends input textures that are treated as float to an RGBA-Texture and stores the result in dst
        /// Textures of type float have the same value for each channel in each pixel
        /// </summary>
        void Perform()
        {
            Texture2D inputTexture0 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D inputTexture1 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D inputTexture2 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D inputTexture3 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if (m_inputs[0].inputtingNode == null)
            {
                inputTexture0.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTexture0.Apply();
            }
            else inputTexture0 = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if (m_inputs[1].inputtingNode == null)
            {
                inputTexture1.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTexture1.Apply();
            }
            else inputTexture1 = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            if (m_inputs[2].inputtingNode == null)
            {
                inputTexture2.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTexture2.Apply();
            }
            else inputTexture2 = m_inputs[2].inputtingNode.m_outputs[m_inputs[2].outputIndex].outputTexture;

            if (m_inputs[3].inputtingNode == null)
            {
                inputTexture3.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTexture3.Apply();
            }
            else inputTexture3 = m_inputs[3].inputtingNode.m_outputs[m_inputs[3].outputIndex].outputTexture;

            DD_NodeUtils.Append(inputTexture0, inputTexture1, inputTexture2, inputTexture3, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif