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
    public enum MINMAXOPERATION
    {
        Min,
        Max
    }

    [Serializable]
    public class DD_NodeMinMax : DD_NodeBase
    {
        #region public variables
        public MINMAXOPERATION m_minMaxOperation = MINMAXOPERATION.Min;

        public float m_a;
        public float m_b;
        #endregion

        #region private variables
        bool m_baseSettings = true;
        bool m_showOutput = true;
        #endregion

        #region Constructors
        public DD_NodeMinMax()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(2) { new DD_InputConnector(), new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.MinMax;

            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "A";
            m_inputs[1].inputLabel = "B";

            m_outputs[0].outputLabel = "Min";

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

            DD_GUILayOut.TitleLabel("Min / Max");

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                EditorGUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(new GUIContent("Operation"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                m_minMaxOperation = (MINMAXOPERATION)EditorGUILayout.EnumPopup(new GUIContent(" "), m_minMaxOperation, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(10);

                if (!m_inputs[0].isOccupied) m_a = DD_GUILayOut.FloatField("A", m_a);
                if (!m_inputs[1].isOccupied) m_b = DD_GUILayOut.FloatField("B", m_b);

                if (EditorGUI.EndChangeCheck())
                {
                    m_redoCalculation = true;

                    if (m_minMaxOperation == MINMAXOPERATION.Min) m_nodeName = "Min";
                    else m_nodeName = "Max";
                }

                EditorGUILayout.Space(10);
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
            {
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
            }
        }

        /// <summary>
        /// Performs the actual node operation returning either the smaler or the larger of 2 values
        /// </summary>
        void Perform()
        {
            Texture2D textureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D textureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if(m_inputs[0].inputtingNode == null)
            {
                textureA.SetPixel(0, 0, new Color(m_a, m_a, m_a, m_a));
                textureA.Apply();
            }
            else textureA = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if (m_inputs[1].inputtingNode == null)
            {
                textureB.SetPixel(0, 0, new Color(m_b, m_b, m_b, m_b));
                textureB.Apply();
            }
            else textureB = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            DD_NodeUtils.MinMax(textureA, textureB, m_outputs[0].outputTexture, m_minMaxOperation);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif