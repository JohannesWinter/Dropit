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
    public class DD_NodePlasma : DD_NodeBase
    {
        #region public variables
        public int m_seed = 0;
        public float m_scale = 1;
        public int m_octaves = 4;
        public Vector2 m_tiling = Vector2.one;

        public Color m_redChannel = Color.red;
        public Color m_greenChannel = Color.green;
        public Color m_blueChannel = Color.blue;
        #endregion

        #region private variables
        bool m_showOutput = true;
        bool m_showPlasmaSettings = true;
        #endregion

        #region constructors
        public DD_NodePlasma()
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

            m_nodeType = NodeType.Plasma;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputLabel = "Output";

            m_outputs[0].outputDataType = DataType.RGBA;

            m_seed = (int)UnityEngine.Random.Range(0, 2147483647 / 100);
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

            DD_GUILayOut.TitleLabel(NodeType.Plasma.ToString());

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            m_showPlasmaSettings = DD_GUILayOut.FoldOut(m_showPlasmaSettings, "Noise Settings");

            if (m_showPlasmaSettings)
            {
                EditorGUI.BeginChangeCheck();

                m_seed = (int)DD_GUILayOut.IntField("Seed", m_seed);

                EditorGUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Randomize", DD_EditorUtils.editorSkin.GetStyle("GenericButton"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.size.x - 25), GUILayout.Height(30)))
                {
                    m_seed = (int)UnityEngine.Random.Range(0, 2147483647 / 100);
                }

                m_seed = Mathf.Max(0, m_seed);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(20);

                m_scale = DD_GUILayOut.FloatField("Scale", m_scale);
                m_octaves = (int)DD_GUILayOut.Slider("Octaves", m_octaves, 1, 10);
                m_tiling = DD_GUILayOut.Vector2Field("Tiling", m_tiling);

                EditorGUILayout.Space(20);
                DD_GUILayOut.Label("Color Portions");

                m_redChannel = DD_GUILayOut.ColorField("Red Portion", m_redChannel);
                m_greenChannel = DD_GUILayOut.ColorField("Green Portion", m_greenChannel);
                m_blueChannel = DD_GUILayOut.ColorField("Blue Portion", m_blueChannel);

                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

                EditorGUILayout.Space(10);
            }
        }

        void Perform()
        {
            Texture2D step1red = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D step1green = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D step1blue = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            DD_NodeUtils.Noise(step1red, m_scale, m_seed, m_tiling, m_octaves, NOISETYPE.SolidNoise);
            DD_NodeUtils.Noise(step1green, m_scale, m_seed / 2, m_tiling, m_octaves, NOISETYPE.SolidNoise);
            DD_NodeUtils.Noise(step1blue, m_scale, m_seed / 4, m_tiling, m_octaves, NOISETYPE.SolidNoise);

            step1red.Apply();
            step1green.Apply();
            step1blue.Apply();

            Texture2D correctionGreen = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            correctionGreen.SetPixel(0, 0, new Color(2, 2, 2, 2));
            correctionGreen.Apply();

            Texture2D correctionBlue = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            correctionBlue.SetPixel(0, 0, new Color(3, 3, 3, 3));
            correctionBlue.Apply();

            DD_NodeUtils.TextureMath(step1green, correctionGreen, step1green, MATHNODETYPE.Divide);
            DD_NodeUtils.TextureMath(step1blue, correctionBlue, step1blue, MATHNODETYPE.Divide);

            step1green.Apply();
            step1blue.Apply();

            Texture2D redChannelTex = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D greenChannelTex = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D blueChannelTex = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            redChannelTex.SetPixel(0, 0, m_redChannel);
            greenChannelTex.SetPixel(0, 0, m_greenChannel);
            blueChannelTex.SetPixel(0, 0, m_blueChannel);

            redChannelTex.Apply();
            greenChannelTex.Apply();
            blueChannelTex.Apply();

            Texture2D step2red = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D step2green = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D step2blue = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            DD_NodeUtils.TextureMath(step1red, redChannelTex, step2red, MATHNODETYPE.Multiply);
            DD_NodeUtils.TextureMath(step1green, greenChannelTex, step2green, MATHNODETYPE.Multiply);
            DD_NodeUtils.TextureMath(step1blue, blueChannelTex, step2blue, MATHNODETYPE.Multiply);

            step2red.Apply();
            step2green.Apply();
            step2blue.Apply();

            Texture2D step3rg = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            Texture2D step3rgb = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

            DD_NodeUtils.TextureMath(step2red, step2green, step3rg, MATHNODETYPE.Add);
            step3rg.Apply();

            DD_NodeUtils.TextureMath(step3rg, step2blue, step3rgb, MATHNODETYPE.Add);
            step3rgb.Apply();

            Vector3 redVector = new Vector3(m_redChannel.r, m_redChannel.g, m_redChannel.b);
            Vector3 greenVector = new Vector3(m_greenChannel.r, m_greenChannel.g, m_greenChannel.b);
            Vector3 blueVector = new Vector3(m_blueChannel.r, m_blueChannel.g, m_blueChannel.b);

            Vector3 resultingColor = redVector + greenVector + blueVector;

            float toOldFloat = Mathf.Max(resultingColor.x, resultingColor.y, resultingColor.z);

            //Remap from 0-1 to 0.2-1
            Texture2D fromOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            fromOld.SetPixel(0, 0, new Color(0, 0, 0, 0));
            fromOld.Apply();

            Texture2D toOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            toOld.SetPixel(0, 0, new Color(toOldFloat, toOldFloat, toOldFloat, toOldFloat));
            toOld.Apply();

            Texture2D fromNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            fromNew.SetPixel(0, 0, new Color(0, 0, 0, 0));
            fromNew.Apply();

            Texture2D toNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
            toNew.SetPixel(0, 0, new Color(1, 1, 1, 1));
            toNew.Apply();

            DD_NodeUtils.Remap(step3rgb, fromOld, toOld, fromNew, toNew, m_outputs[0].outputTexture);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif