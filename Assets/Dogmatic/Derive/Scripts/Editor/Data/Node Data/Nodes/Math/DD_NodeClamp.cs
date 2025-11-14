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
    public class DD_NodeClamp : DD_NodeBase
    {
        #region public variables
        public float m_value = 0;
        public float m_min = 0;
        public float m_max = 1;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeClamp()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(3) { new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Clamp;

            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Input";
            m_inputs[1].inputLabel = "Min";
            m_inputs[2].inputLabel = "Max";

            m_outputs[0].outputLabel = "Output";

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
            ///These lines make sure that the data type of the output is right. The data type is float if both inputting types are float
            ///If one or both input types are textures, the output data will also be of type 'texture'.
            /// </summary>
            bool textureDataPresent = false;

            for (int i = 0; i < m_inputs.Count; i++)
            {
                if (m_inputs[i].isOccupied)
                    if (m_inputs[i].inputtingNode != null)
                        if (m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputDataType == DataType.RGBA) textureDataPresent = true;
            }

            if (textureDataPresent) m_outputs[0].outputDataType = DataType.RGBA;
            else m_outputs[0].outputDataType = DataType.Float;

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

            DD_GUILayOut.TitleLabel(NodeType.Clamp.ToString());

            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            if (m_inputs[0].inputtingNode == null)
                m_value = DD_GUILayOut.FloatField("Value", m_value);

            if (m_inputs[1].inputtingNode == null)
                m_min = DD_GUILayOut.FloatField("Min", m_min);

            if (m_inputs[2].inputtingNode == null)
                m_max = DD_GUILayOut.FloatField("Max", m_max);
            if (EditorGUI.EndChangeCheck())
            {
                m_redoCalculation = true;
            }

            EditorGUILayout.Space(10);

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            if (m_showOutput)
            {
                EditorGUILayout.EndVertical();
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
            }
        }

        /// <summary>
        /// Performs the actual node operation returning the power of input 0 and 1
        /// </summary>
        void Perform()
        {
            Texture2D texInput = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D texMin = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D texMax = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if (m_inputs[0].inputtingNode == null)
            {
                texInput.SetPixel(0, 0, new Color(m_value, m_value, m_value, m_value));
                texInput.Apply();
            }
            else texInput = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if (m_inputs[1].inputtingNode == null)
            {
                texMin.SetPixel(0, 0, new Color(m_min, m_min, m_min, m_min));
                texMin.Apply();
            }
            else texMin = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            if (m_inputs[2].inputtingNode == null)
            {
                texMax.SetPixel(0, 0, new Color(m_max, m_max, m_max, m_max));
                texMax.Apply();
            }
            else texMax = m_inputs[2].inputtingNode.m_outputs[m_inputs[2].outputIndex].outputTexture;

            DD_NodeUtils.Clamp(texInput, texMin, texMax, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion

    }
}
#endif