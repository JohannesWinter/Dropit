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
    public enum MATHNODETYPE
    {
        Add = 0,
        Subtract = 1,
        Multiply = 2,
        Divide = 3,
    }

    [Serializable]
    public class DD_NodeMath : DD_NodeBase
    {
        #region Public Variables
        //public float m_outputValue;
        //public List<float> m_inputValues;
        public MATHNODETYPE m_mathNodeType;
        public bool m_initMathNodeType = true;

        public float m_inputFloatA = 0;
        public float m_inputFloatB = 0;

        //[NonSerialized]
        //public Texture2D m_textureA;
        //[NonSerialized]
        //public Texture2D m_textureB;
        #endregion

        #region privateVariables
        bool m_baseSettings = true;
        bool m_showOutput = true;
        #endregion

        #region Constructor
        public DD_NodeMath()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(2) { new DD_InputConnector(), new DD_InputConnector() };

            //m_inputValues = new List<float>(2) { 0, 0 };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region Main Methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Math;

            if (m_initMathNodeType)
                m_mathNodeType = MATHNODETYPE.Add;

            m_initMathNodeType = false;

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
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat, m_outputs[0].outputFloat));
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

            for(int i = 0; i < m_inputs.Count; i++)
            {
                if (m_inputs[i].isOccupied)
                    if(m_inputs[i].inputtingNode != null)
                        if (m_inputs[i].inputtingNode.m_outputs[m_inputs[i].outputIndex].outputDataType == DataType.RGBA) textureDataPresent = true;
            }

            if (textureDataPresent) m_outputs[0].outputDataType = DataType.RGBA;
            else m_outputs[0].outputDataType = DataType.Float;

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// DrawProperties draws the node's properties into the property view
        /// This method is called from the property view!!
        /// </summary>
        public override void DrawProperties()
        {
            Rect rt = EditorGUILayout.BeginVertical();

            DD_GUILayOut.TitleLabel(NodeType.Math.ToString());
            DD_GUILayOut.TitleLabel(m_nodeName);

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                EditorGUILayout.Space(10);
                GUILayout.BeginHorizontal();

                GUILayout.Label(new GUIContent("Operation"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                m_mathNodeType = (MATHNODETYPE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_mathNodeType, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateNodeName();
                    m_redoCalculation = true;
                }

                GUILayout.EndHorizontal();
                EditorGUILayout.Space(10);

                EditorGUI.BeginChangeCheck();
                if (m_inputs[0].inputtingNode == null)
                    m_inputFloatA = DD_GUILayOut.FloatField("Input A", m_inputFloatA);

                if (m_inputs[1].inputtingNode == null)
                    m_inputFloatB = DD_GUILayOut.FloatField("Input B", m_inputFloatB);
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            EditorGUI.BeginChangeCheck();
        }

        void UpdateNodeName()
        {
            switch (m_mathNodeType)
            {
                case MATHNODETYPE.Add:
                    m_nodeName = "Add";
                    break;

                case MATHNODETYPE.Subtract:
                    m_nodeName = "Subtract";
                    break;

                case MATHNODETYPE.Multiply:
                    m_nodeName = "Multiply";
                    break;

                case MATHNODETYPE.Divide:
                    m_nodeName = "Divide";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Performs the actual node operation based on the inputs
        /// </summary>
        void Perform()
        {
            Texture2D textureA = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D textureB = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            if(m_inputs[0].inputtingNode == null)
            {
                Color colorA = new Color(m_inputFloatA, m_inputFloatA, m_inputFloatA, m_inputFloatA);
                textureA.SetPixel(0, 0, colorA);
                textureA.Apply();
            }
            else textureA = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

            if(m_inputs[1].inputtingNode == null)
            {
                Color colorB = new Color(m_inputFloatB, m_inputFloatB, m_inputFloatB, m_inputFloatB);
                textureB.SetPixel(0, 0, colorB);
                textureB.Apply();
            }
            else textureB = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

            DD_NodeUtils.TextureMath(textureA, textureB, m_outputs[0].outputTexture, m_mathNodeType);
            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif