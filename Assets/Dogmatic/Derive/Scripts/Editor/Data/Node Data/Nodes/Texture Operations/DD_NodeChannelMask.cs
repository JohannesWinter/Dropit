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
    public class DD_NodeChannelMask : DD_NodeBase
    {
        #region public variables
        public bool m_r = false;
        public bool m_g = false;
        public bool m_b = false;
        public bool m_a = false;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeChannelMask()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(1) { new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlack";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.ChannelMask;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Input";
            m_outputs[0].outputLabel = "Output";

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

            DD_GUILayOut.TitleLabel("Channel Mask");

            EditorGUI.BeginChangeCheck();

            m_r = DD_GUILayOut.BoolField("Red Channel", m_r, true);
            m_g = DD_GUILayOut.BoolField("Green Channel", m_g, true);
            m_b = DD_GUILayOut.BoolField("Blue Channel", m_b, true);
            m_a = DD_GUILayOut.BoolField("Alpha Channel", m_a, true);

            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

            EditorGUILayout.Space(10);

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Masks out one or more color channels and outputs only the selected channels - the result is stored in the output texture
        /// </summary>
        void Perform()
        {
            if (m_inputs[0].inputtingNode == null)
            {
                m_outputs[0].outputTexture.Reinitialize(1, 1);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputDataType = DataType.RGBA;
            }
            else
            {
                Texture2D inputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                Texture2D inputTextureR = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D inputTextureG = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D inputTextureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D inputTextureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                inputTextureR.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTextureG.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTextureB.SetPixel(0, 0, new Color(0, 0, 0, 0));
                inputTextureA.SetPixel(0, 0, new Color(0, 0, 0, 0));

                inputTextureR.Apply();
                inputTextureG.Apply();
                inputTextureB.Apply();
                inputTextureA.Apply();

                inputTexture = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

                if (m_r)
                {
                    DD_NodeUtils.TextureComponent(inputTexture, inputTextureR, 1, 0, 0, 0);
                    inputTextureR.Apply();
                }

                if (m_g)
                {
                    DD_NodeUtils.TextureComponent(inputTexture, inputTextureG, 0, 1, 0, 0);
                    inputTextureG.Apply();
                }

                if (m_b)
                {
                    DD_NodeUtils.TextureComponent(inputTexture, inputTextureB, 0, 0, 1, 0);
                    inputTextureB.Apply();
                }

                if (m_a)
                {
                    DD_NodeUtils.TextureComponent(inputTexture, inputTextureA, 0, 0, 0, 1);
                    inputTextureA.Apply();
                }

                DD_NodeUtils.Append(inputTextureR, inputTextureG, inputTextureB, inputTextureA, m_outputs[0].outputTexture);
            }

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif