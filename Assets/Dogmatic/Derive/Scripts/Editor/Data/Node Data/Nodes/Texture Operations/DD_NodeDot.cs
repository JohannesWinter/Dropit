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
    public class DD_NodeDot : DD_NodeBase
    {
        #region public variables
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeDot()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(2) { new DD_InputConnector(), new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlack";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Dot;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "A";
            m_inputs[1].inputLabel = "B";
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

            DD_GUILayOut.TitleLabel(NodeType.Dot.ToString());

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Calculates the a dot product per each pixel of the input textures treating them as vectors and stores the result in the output texture
        /// </summary>
        void Perform()
        {
            Texture2D textureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D textureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if (m_inputs[0].inputtingNode == null)
            {
                textureA.SetPixel(0, 0, new Color(0, 0, 0, 0));
                textureA.Apply();
            }
            else textureA = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if (m_inputs[1].inputtingNode == null)
            {
                textureB.SetPixel(0, 0, new Color(0, 0, 0, 0));
                textureB.Apply();
            }
            else textureB = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            DD_NodeUtils.Dot(textureA, textureB, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif