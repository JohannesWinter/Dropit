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
    public enum GRADIENTTYPE
    {
        LinearAsymmetrical,
        LinearSymmetrical,
        LinearSquare,
        SmoothDiamond,
        SinAsymmetrical,
        SinSymmetrical,
        SinSquare,
        SinSmoothSquare,
        LogisticAsymmetrical,
        LogisticSymmetrical,
        LogisticSquare,
        LogisticSmoothSquare,
        QuarterCylinder,
        HalfCylinder,
        SphereToSquare,
        SphereToSmoothSquare
    }

    [Serializable]
    public class DD_NodeGradient : DD_NodeBase
    {
        #region public variables
        public GRADIENTTYPE m_gradientType = GRADIENTTYPE.LinearAsymmetrical;
        public float m_gradientSize = 1;
        public float m_gradientRotation = 0;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeGradient()
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

            m_nodeType = NodeType.Gradient;
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

            DD_GUILayOut.TitleLabel(NodeType.Gradient.ToString());

            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Gradient Type"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
            m_gradientType = (GRADIENTTYPE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_gradientType, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            m_gradientSize = DD_GUILayOut.Slider("Size", m_gradientSize, 0, 1);
            m_gradientRotation = DD_GUILayOut.Slider("Rotation", m_gradientRotation, -180, 180);

            if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;            
            
            EditorGUILayout.Space(10);

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);
        }

        /// <summary>
        /// Generates a gradient texture and stores it in the output texture
        /// </summary>
        void Perform()
        {
            DD_NodeUtils.Gradient(m_outputs[0].outputTexture, m_gradientType, m_gradientSize, m_gradientRotation);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif