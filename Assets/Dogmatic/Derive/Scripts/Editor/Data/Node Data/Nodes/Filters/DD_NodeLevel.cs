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
    public class DD_NodeLevel : DD_NodeBase
    {
        #region public variables
        public float m_min = 0;
        public float m_max = 1;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeLevel()
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

            m_nodeType = NodeType.Level;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Input";
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

            DD_GUILayOut.TitleLabel(NodeType.Level.ToString());

            EditorGUI.BeginChangeCheck();
            Vector2 range = DD_GUILayOut.MinMaxSlider("Level" , m_min, m_max, -2, 3);             
            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

            m_min = range.x;
            m_max = range.y;

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Performs the actual node operation remapping the pixel values from 0-1 to m_min-m_max
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

                //Remap from 0-1 to 0.2-1
                Texture2D fromOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                fromOld.SetPixel(0, 0, new Color(0, 0, 0, 0));
                fromOld.Apply();

                Texture2D toOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                toOld.SetPixel(0, 0, new Color(1, 1, 1, 1));
                toOld.Apply();

                Texture2D fromNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                fromNew.SetPixel(0, 0, new Color(m_min, m_min, m_min, m_min));
                fromNew.Apply();

                Texture2D toNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                toNew.SetPixel(0, 0, new Color(m_max, m_max, m_max, m_max));
                toNew.Apply();

                DD_NodeUtils.Remap(inputTexture, fromOld, toOld, fromNew, toNew, m_outputs[0].outputTexture);

                m_outputs[0].outputDataType = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputDataType;
            }

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif