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
    public enum SHAPE
    {
        Square,
        Circle,
        RoundedSquare,
        Hexagon,
        Octagon,
        Triangle
    }

    [Serializable]
    public class DD_NodeBaseShape : DD_NodeBase
    {
        #region public variables
        public SHAPE m_shape = SHAPE.Square;

        public float m_radius = 0.5f;
        public float m_falloff = 0;
        public float m_ridge = 0.5f;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeBaseShape()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(0);

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderTeal";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.BaseShape;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

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

            DD_GUILayOut.TitleLabel("Base Shape");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Shape"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            m_shape = (SHAPE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_shape, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            if (m_shape == SHAPE.RoundedSquare)
                m_radius = DD_GUILayOut.Slider("Radius", m_radius, 0, 2);

            if (m_shape == SHAPE.Hexagon)
                m_falloff = DD_GUILayOut.Slider("Falloff", m_falloff, -45, 45);

            if (m_shape == SHAPE.Triangle)
                m_falloff = DD_GUILayOut.Slider("Falloff", m_falloff, 0, 90);

            if (m_shape == SHAPE.Octagon)
                m_ridge = DD_GUILayOut.Slider("Ridge", m_ridge, 0, 1);

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Generates a texture with a primitive shape and stores it in the output texture
        /// </summary>
        void Perform()
        {
            DD_NodeUtils.BaseShape(m_outputs[0].outputTexture, m_shape, m_radius, m_falloff, m_ridge);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif