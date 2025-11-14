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
    public class DD_NodeLerp : DD_NodeBase
    {
        #region public variables
        public float m_a;
        public float m_b;
        public float m_alpha = 0;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeLerp()
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

            m_nodeType = NodeType.Lerp;

            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "A";
            m_inputs[1].inputLabel = "B";
            m_inputs[2].inputLabel = "Alpha";

            m_outputs[0].outputLabel = "Result";

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

            DD_GUILayOut.TitleLabel(NodeType.Lerp.ToString());

            EditorGUI.BeginChangeCheck();
            if (!m_inputs[0].isOccupied)
                m_a = DD_GUILayOut.FloatField("A", m_a);

            if (!m_inputs[1].isOccupied)
                m_b = DD_GUILayOut.FloatField("B", m_b);

            if (!m_inputs[2].isOccupied)
            {
                EditorGUILayout.Space(10);
                m_alpha = DD_GUILayOut.Slider("Alpha", m_alpha, 0, 1);
                EditorGUILayout.Space(10);
            }
            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
            {
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
            }
        }

        /// <summary>
        /// Performs the actual node operation lerping between inputs A & B via the alpha value
        /// </summary>
        void Perform()
        {
            Texture2D textureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D textureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D alphaTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if (m_inputs[0].inputtingNode == null)
            {
                textureA.SetPixel(0, 0, new Color(m_a, m_a, m_a, m_a));
                textureA.Apply();
            }
            else textureA = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if(m_inputs[1].inputtingNode == null)
            {
                textureB.SetPixel(0, 0, new Color(m_b, m_b, m_b, m_b));
                textureB.Apply();
            }
            else textureB = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            if (m_inputs[2].inputtingNode == null)
            {
                alphaTexture.SetPixel(0, 0, new Color(m_alpha, m_alpha, m_alpha, m_alpha));
                alphaTexture.Apply();
            }
            else alphaTexture = m_inputs[2].inputtingNode.m_outputs[m_inputs[2].outputIndex].outputTexture;

            DD_NodeUtils.Lerp(textureA, textureB, alphaTexture, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif