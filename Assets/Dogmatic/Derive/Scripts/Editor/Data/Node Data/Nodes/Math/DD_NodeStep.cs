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
    public class DD_NodeStep : DD_NodeBase
    {
        #region public variables
        public float m_input1 = 0;
        public float m_input2 = 0;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeStep()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(2) { new DD_InputConnector(), new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region main Methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Step;

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

            DD_GUILayOut.TitleLabel("Step");

            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            if (m_inputs[0].inputtingNode == null)
                m_input1 = DD_GUILayOut.FloatField("A", m_input1);

            if (m_inputs[1].inputtingNode == null)
                m_input2 = DD_GUILayOut.FloatField("B", m_input2);
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
            Texture2D textureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D textureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if(m_inputs[0].inputtingNode == null)
            {
                textureA.SetPixel(0, 0, new Color(m_input1, m_input1, m_input1, m_input1));
                textureA.Apply();
            }
            else textureA = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if (m_inputs[1].inputtingNode == null)
            {
                textureB.SetPixel(0, 0, new Color(m_input2, m_input2, m_input2, m_input2));
                textureB.Apply();
            }
            else textureB = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            DD_NodeUtils.Step(textureA, textureB, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif