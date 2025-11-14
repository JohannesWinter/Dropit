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
    public enum BLENDMODE
    {
        Overlay,
        Burn,
        Dodge,
        Exclude,
        Darken,
        Lighten,
        SoftLight,
        HardLight,
        PinLight,
        VividLight,
        HardMix,
        Difference,
        Subtraction,
        Multiplication,
        Division
    }

    [Serializable]
    public class DD_NodeBlend : DD_NodeBase
    {
        #region public variables
        public BLENDMODE m_blendMode = BLENDMODE.Overlay;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeBlend()
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

            m_nodeType = NodeType.Blend;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Source";
            m_inputs[1].inputLabel = "Destination";
            m_outputs[0].outputLabel = "Result";

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

            DD_GUILayOut.TitleLabel(NodeType.Blend.ToString());

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Blend Mode"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            m_blendMode = (BLENDMODE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_blendMode, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Blends input textures based on the selected blend mode and stores the result in the output texture
        /// </summary>
        void Perform()
        {
            Texture2D inputTexture0 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D inputTexture1 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

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

            DD_NodeUtils.Blend(inputTexture0, inputTexture1, m_outputs[0].outputTexture, m_blendMode);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif