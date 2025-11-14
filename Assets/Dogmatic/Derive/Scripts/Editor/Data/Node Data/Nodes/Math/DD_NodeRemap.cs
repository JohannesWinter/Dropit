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
    public class DD_NodeRemap : DD_NodeBase
    {
        #region public variables
        public float m_fromOld = 0;
        public float m_toOld = 1;
        public float m_fromNew = 0;
        public float m_toNew = 1;
        #endregion

        #region private variables
        bool m_showOutput = true;
        #endregion

        #region constructors
        public DD_NodeRemap()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };
            m_inputs = new List<DD_InputConnector>(5) { new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector(), new DD_InputConnector() };

            m_connectorStyles = new string[2] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected" };

            m_nodeStyle = "NodeHeaderBlue";
        }
        #endregion

        #region main methods
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>(1) { new DD_OutputConnector() };

            base.InitNode();

            m_nodeType = NodeType.Remap;

            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_inputs[0].inputLabel = "Input";
            m_inputs[1].inputLabel = "From (Old)";
            m_inputs[2].inputLabel = "To (Old)";
            m_inputs[3].inputLabel = "From (New)";
            m_inputs[4].inputLabel = "To (New)";

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

            DD_GUILayOut.TitleLabel(NodeType.Remap.ToString());

            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            if (m_inputs[1].inputtingNode == null)
                m_fromOld = DD_GUILayOut.FloatField("From (Old)", m_fromOld);

            if (m_inputs[2].inputtingNode == null)
                m_toOld = DD_GUILayOut.FloatField("To (Old)", m_toOld);

            if (m_inputs[3].inputtingNode == null)
                m_fromNew = DD_GUILayOut.FloatField("From (New)", m_fromNew);

            if (m_inputs[4].inputtingNode == null)
                m_toNew = DD_GUILayOut.FloatField("To (New)", m_toNew);
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
            if (m_inputs[0].inputtingNode == null)
            {
                m_outputs[0].outputTexture.Reinitialize(1, 1);
                m_outputs[0].outputTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
                m_outputs[0].outputDataType = DataType.Float;
            }
            else
            {
                Texture2D texInput = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D texFromOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D texToOld = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D texFromNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                Texture2D texToNew = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                texInput = m_inputs[0].inputtingNode.m_outputs[m_inputs[0].outputIndex].outputTexture;

                if (m_inputs[1].inputtingNode == null)
                {
                    texFromOld.SetPixel(0, 0, new Color(m_fromOld, m_fromOld, m_fromOld, m_fromOld));
                    texFromOld.Apply();
                }
                else texFromOld = m_inputs[1].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

                if (m_inputs[2].inputtingNode == null)
                {
                    texToOld.SetPixel(0, 0, new Color(m_toOld, m_toOld, m_toOld, m_toOld));
                    texToOld.Apply();
                }
                else texToOld = m_inputs[2].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

                if (m_inputs[3].inputtingNode == null)
                {
                    texFromNew.SetPixel(0, 0, new Color(m_fromNew, m_fromNew, m_fromNew, m_fromNew));
                    texFromNew.Apply();
                }
                else texFromNew = m_inputs[3].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

                if (m_inputs[4].inputtingNode == null)
                {
                    texToNew.SetPixel(0, 0, new Color(m_toNew, m_toNew, m_toNew, m_toNew));
                    texToNew.Apply();
                }
                else texToNew = m_inputs[4].inputtingNode.m_outputs[m_inputs[1].outputIndex].outputTexture;

                DD_NodeUtils.Remap(texInput, texFromOld, texToOld, texFromNew, texToNew, m_outputs[0].outputTexture);
            }




            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif