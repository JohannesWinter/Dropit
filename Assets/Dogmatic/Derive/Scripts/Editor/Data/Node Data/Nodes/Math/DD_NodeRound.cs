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
    public enum ROUNDINGOPERATION
    {
        Round,
        Ceil,
        Floor
    }

    [Serializable]
    public class DD_NodeRound : DD_NodeBase
    {
        #region public variables
        public ROUNDINGOPERATION m_roundingOperation = ROUNDINGOPERATION.Round;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeRound()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(1) { new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Round;
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

            DD_GUILayOut.TitleLabel(NodeType.Round.ToString());

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Operation"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

            EditorGUI.BeginChangeCheck();
            m_roundingOperation = (ROUNDINGOPERATION)EditorGUILayout.EnumPopup(new GUIContent(" "), m_roundingOperation, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
            if (EditorGUI.EndChangeCheck())
            {
                m_redoCalculation = true;
                UpdateNodeName();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
            {
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
            }

        }

        void UpdateNodeName()
        {
            switch (m_roundingOperation)
            {
                case ROUNDINGOPERATION.Ceil:
                    m_nodeName = "Ceil";
                    break;

                case ROUNDINGOPERATION.Floor:
                    m_nodeName = "Floor";
                    break;

                case ROUNDINGOPERATION.Round:
                    m_nodeName = "Round";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Performs the actual node operation rounting the input UP to the next integer
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

                DD_NodeUtils.Round(inputTexture, m_outputs[0].outputTexture, m_roundingOperation);

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