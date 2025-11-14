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
    public class DD_NodeShadowFilter : DD_NodeBase
    {
        #region public variables
        public float m_area = 0;
        public Texture2D m_areaOfEffect;

        public float m_filterStrength = 1;
        public float m_smoothness = 0.2f;
        #endregion

        #region private variables
        bool m_showOutput = true;
        bool m_showFilterSettings = true;
        #endregion

        #region constructors
        public DD_NodeShadowFilter()
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

            m_nodeType = NodeType.ShadowFilter;
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

            if (m_areaOfEffect == null)
            {
                m_areaOfEffect = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_areaOfEffect.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_areaOfEffect.Apply();
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

            DD_GUILayOut.TitleLabel("Shadow Filter");

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            Rect rt2 = EditorGUILayout.BeginVertical();

            if (m_showOutput) 
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            m_showFilterSettings = DD_GUILayOut.FoldOut(m_showFilterSettings, "Filter Settings");

            EditorGUILayout.EndVertical();

            if (m_showFilterSettings)
            {
                Rect rt3 = EditorGUILayout.BeginVertical();
                DD_GUILayOut.TitleLabel("Area of Effect");
                EditorGUILayout.EndVertical();

                Rect rt4 = new Rect(rt2.position, new Vector2(rt2.width, rt2.height + rt3.height));

                DD_GUILayOut.DrawTexture(m_areaOfEffect, rt4);

                EditorGUI.BeginChangeCheck();
                m_area = DD_GUILayOut.Slider("Area of Effect", m_area, 0, 1);
                m_filterStrength = DD_GUILayOut.Slider("Filter Strength", m_filterStrength, 0, 5);
                m_smoothness = DD_GUILayOut.Slider("Smoothness", m_smoothness, 0, 1);
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;
            }
        }

        /// <summary>
        /// Performs a variety of operations attempting to remove shadows from the input texture
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
                fromOld.SetPixel(0, 0, Color.black);
                fromOld.Apply();

                Texture2D toOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                toOld.SetPixel(0, 0, Color.white);
                toOld.Apply();

                Texture2D fromNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                fromNew.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 0.2f));
                fromNew.Apply();

                Texture2D toNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                toNew.SetPixel(0, 0, Color.white);
                toNew.Apply();

                Texture2D step1 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Remap(inputTexture, fromOld, toOld, fromNew, toNew, step1);
                step1.Apply();

                //Get the grayscale of the remapped texture
                Texture2D step2 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Grayscale(step1, step2);
                step2.Apply();

                //Step operation to get the darker areas
                Texture2D areaThreshold = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                areaThreshold.SetPixel(0, 0, new Color(m_area, m_area, m_area, m_area));
                areaThreshold.Apply();

                Texture2D step3 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Step(step2, areaThreshold, step3);
                step3.Apply();

                //Multiply area of effect with input texture to show area of effect in the filter settings
                DD_NodeUtils.TextureMath(inputTexture, step3, m_areaOfEffect, MATHNODETYPE.Multiply);
                m_areaOfEffect.Apply();

                //Blur stepped texture to use as shadow mask later
                Texture2D step4 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Blur(step3, step4, m_smoothness / 250);
                step4.Apply();

                //Brighten area of effect
                Texture2D step5 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Brightness(step1, step5, m_filterStrength);
                step5.Apply();

                //Remap back from 0.2-1 to 0-1
                fromOld.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 0.2f));
                fromOld.Apply();

                toOld.SetPixel(0, 0, Color.white);
                toOld.Apply();

                fromNew.SetPixel(0, 0, Color.black);
                fromNew.Apply();

                toNew.SetPixel(0, 0, Color.white);
                toNew.Apply();

                Texture2D step6 = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.Remap(step5, fromOld, toOld, fromNew, toNew, step6);
                step6.Apply();

                //Lerp between brightened and unbrightened texture based on shadow mask and store result in the output texture
                DD_NodeUtils.Lerp(inputTexture, step6, step4, m_outputs[0].outputTexture);

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