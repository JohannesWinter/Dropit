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
    public enum MAPPINGMODE
    {
        stretchToSquare,
        cropToSquare,
    }

    /// <summary>
    /// Unlike with other nodes, this node must perform it's calculation inside OnGUI and cannot do it in Update
    /// While technically possible, running the node operation in the Update method leads the result to lag behind in the editor
    /// </summary>
    [Serializable]
    public class DD_NodeTexture : DD_NodeBase
    {
        #region Public Variables
        #endregion

        #region private variables
        public bool m_baseSettings = true;
        public bool m_showOutput = true;
        public bool m_mappingSettings = true;

        public MAPPINGMODE m_mappingMode;

        public Texture2D m_Texture;

        public Texture2D m_TextureDark;

        float m_tilingX = 1;
        float m_tilingY = 1;
        float m_offsetX = 0;
        float m_offsetY = 0;


        public Rect m_nonCropRect;
        public Rect m_cropRect;
        public Vector2 m_CropRectOffset;

        [Range(0, 1)]
        public float m_CropRectScaleFactor = 1;

        bool m_cropRectDragging = false;

        #endregion

        #region Constructor
        public DD_NodeTexture()
        {
            m_inputs = null;

            m_connectorStyles = new string[10] { "ConnectorWhiteUnconnected", "ConnectorWhiteConnected",
            "ConnectorRedUnconnected", "ConnectorRedConnected",
            "ConnectorGreenUnconnected", "ConnectorGreenConnected",
            "ConnectorBlueUnconnected", "ConnectorBlueConnected",
            "ConnectorGrayUnconnected", "ConnectorGrayConnected" };

            m_nodeStyle = "NodeHeaderRed";
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// In this node connector drawings in the base class are overridden and the connectors are drawn here
        /// This is because this node uses connectors with different colors
        /// </summary>
        public override void InitNode()
        {
            m_outputs = new List<DD_OutputConnector>() { new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector(), new DD_OutputConnector() };

            m_TextureDark = null;

            base.InitNode();

            m_nodeType = NodeType.Texture;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = true;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputLabel = "RGBA";
            m_outputs[1].outputLabel = "R";
            m_outputs[2].outputLabel = "G";
            m_outputs[3].outputLabel = "B";
            m_outputs[4].outputLabel = "A";

            m_outputs[0].outputDataType = DataType.RGBA;
            m_outputs[1].outputDataType = DataType.Float;
            m_outputs[2].outputDataType = DataType.Float;
            m_outputs[3].outputDataType = DataType.Float;
            m_outputs[4].outputDataType = DataType.Float;

            m_expandNodePreview = true;

            m_CropRectOffset = Vector2.zero;
            m_CropRectScaleFactor = 1;
        }

        public override void UpdateNode()
        {
            base.UpdateNode();

            if (m_outputs[0].outputTexture == null)
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[1].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[2].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[3].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[4].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);

                if (m_Texture != null)
                {
                    if (m_mappingMode == MAPPINGMODE.stretchToSquare)
                        DD_NodeUtils.TexToTex(m_Texture, m_outputs[0].outputTexture, false, false);
                    else
                        DD_NodeUtils.TexToTex(m_Texture, m_outputs[0].outputTexture, m_tilingX, m_tilingY, m_offsetX, m_offsetY);
                }
                else
                {
                    m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                    m_outputs[0].outputTexture.SetPixel(0, 0, Color.black);
                }

                m_outputs[0].outputTexture.Apply();

                m_outputHasChanged = true;
            }

            //No need to process m_TextureDark, if m_Texture == null, since it depends on it.
            if (m_Texture == null) return;

            if (m_TextureDark == null)
            {
                m_TextureDark = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                DD_NodeUtils.TexToTex(m_Texture, m_TextureDark, true, true);

                m_TextureDark.Apply();
            }

            if (m_redoCalculation) Perform();

            EditorUtility.SetDirty(this);
        }

        public override void OnNodeGUI(float propertyViewWidth, float headerViewHeight)
        {
            base.OnNodeGUI(propertyViewWidth, headerViewHeight);

            ///<summary>
            ///This area draws a texture input field directly on the node
            ///and sets "redoCalculation" to true, when the texture reference is changed
            /// </summary>
            Rect textFieldRect = new Rect(m_scaledNodeRect.position.x + 8 * DD_EditorUtils.zoomFactor - DD_EditorUtils.viewRect_propertyView.width, m_outputs[0].outputRect.position.y, m_scaledNodeRect.width - m_outputs[0].outputRect.width - 60 * DD_EditorUtils.zoomFactor, m_outputs[0].outputRect.height);

            GUILayout.BeginArea(textFieldRect);

            ///<summary>
            /// Set node operation to perform if texture reference is changed
            /// Reset scale and offset and get a darkened background texture from referenced texture to use for cropping visualization
            /// </summary>
            EditorGUI.BeginChangeCheck();
            m_Texture = DD_GUILayOut.Texture2DFieldOnNode(m_Texture);
            if (EditorGUI.EndChangeCheck())
            {
                m_redoCalculation = true;

                m_CropRectOffset = Vector2.zero;
                m_CropRectScaleFactor = 1;

                m_tilingX = m_tilingY = 1;
                m_offsetX = m_offsetY = 0;

                if (m_TextureDark != null)
                {
                    DD_NodeUtils.TexToTex(m_Texture, m_TextureDark, true, true);
                    m_TextureDark.Apply();
                }
            }

            GUILayout.EndArea();

            /////////------------------------------> if (m_redoCalculation) Perform();

            //Limit crop rect position by relocating it when it exceeds the boundary area
            if (m_cropRect.position.y + m_cropRect.height > m_nonCropRect.position.y + m_nonCropRect.height)
                m_CropRectOffset -= new Vector2(0, (m_cropRect.position.y + m_cropRect.height) - (m_nonCropRect.position.y + m_nonCropRect.height));

            if (m_cropRect.position.x + m_cropRect.width > m_nonCropRect.position.x + m_nonCropRect.width)
                m_CropRectOffset -= new Vector2((m_cropRect.position.x + m_cropRect.width) - (m_nonCropRect.position.x + m_nonCropRect.width), 0);

            //Output connectors of texture node have a different color scheme, thus we override the drawing of the connectors in node base and draw them here instead.
            GUIStyle outputLabelStyle = new GUIStyle(DD_EditorUtils.editorSkin.GetStyle("OutputLabel"));
            outputLabelStyle.fontSize = (int)(DD_EditorUtils.editorSkin.GetStyle("OutputLabel").fontSize * DD_EditorUtils.zoomFactor);

            float outputSize = 24 * DD_EditorUtils.zoomFactor;
            float topSpace = 16 * DD_EditorUtils.zoomFactor;
            float spaceBetweenOutputs = 32 * DD_EditorUtils.zoomFactor;

            for (int i = 0; i < m_outputs.Count; i++)
            {
                m_outputs[i].outputRect = new Rect(m_absNodeBodyRect.x + m_absNodeBodyRect.width - outputSize, m_absNodeBodyRect.y + topSpace + spaceBetweenOutputs * i, outputSize, outputSize);

                if (!m_outputs[i].isOccupied) GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[i * 2]));
                else GUI.Box(m_outputs[i].outputRect, "", DD_EditorUtils.editorSkin.GetStyle(m_connectorStyles[i * 2 + 1]));

                float labelWidth = outputLabelStyle.CalcSize(new GUIContent(m_outputs[i].outputLabel)).x;

                Rect labelRect = new Rect(m_outputs[i].outputRect.position - new Vector2(labelWidth, 0), new Vector2(labelWidth, outputSize));
                GUI.Label(labelRect, m_outputs[i].outputLabel, outputLabelStyle);
            }

            EditorUtility.SetDirty(this);
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();

            if (m_cropRectDragging)
            {
                if (m_TextureDark == null) return;

                float xScaleFactor = m_TextureDark.width / m_nonCropRect.width;
                float yScaleFactor = m_TextureDark.height / m_nonCropRect.height;

                float xOffset = Mathf.Max(0, Mathf.Min(m_CropRectOffset.x + xScaleFactor * Event.current.delta.x / 2, xScaleFactor * (m_nonCropRect.width - m_cropRect.width)));
                float yOffset = Mathf.Max(0, Mathf.Min(m_CropRectOffset.y + yScaleFactor * Event.current.delta.y / 2, yScaleFactor * (m_nonCropRect.height - m_cropRect.height)));

                m_CropRectOffset = new Vector2(xOffset, yOffset);

                m_redoCalculation = true;
            }

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

            DD_GUILayOut.TitleLabel(NodeType.Texture.ToString());

            m_baseSettings = DD_GUILayOut.FoldOut(m_baseSettings, "Base Settings");

            if (m_baseSettings)
            {
                m_nodeName = DD_GUILayOut.TextField("Node Name", m_nodeName);

                ///<summary>
                /// Set node operation to perform if texture reference is changed
                /// Reset scale and offset and get a darkened background texture from referenced texture to use for cropping visualization
                /// </summary>
                EditorGUI.BeginChangeCheck();
                m_Texture = DD_GUILayOut.Texture2DField("Texture", m_Texture);
                if (EditorGUI.EndChangeCheck())
                {
                    m_redoCalculation = true;

                    m_CropRectOffset = Vector2.zero;
                    m_CropRectScaleFactor = 1;

                    m_tilingX = m_tilingY = 1;
                    m_offsetX = m_offsetY = 0;

                    if (m_TextureDark != null)
                    {
                        DD_NodeUtils.TexToTex(m_Texture, m_TextureDark, true, true);
                        m_TextureDark.Apply();
                    }
                }
            }

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            Rect rt2 = EditorGUILayout.BeginVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            EditorGUILayout.EndVertical();

            //Method is aborted here when m_Texture is null, the rest is not required to run!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (m_Texture == null) return;

            m_mappingSettings = DD_GUILayOut.FoldOut(m_mappingSettings, "Mapping");

            if (m_mappingSettings)
            {
                Rect rt3 = EditorGUILayout.BeginVertical();

                EditorGUILayout.Space(10);
                GUILayout.BeginHorizontal();

                GUILayout.Label(new GUIContent("Mapping"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                m_mappingMode = (MAPPINGMODE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_mappingMode, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

                GUILayout.EndHorizontal();
                EditorGUILayout.Space(10);

                EditorGUILayout.EndVertical();

                ///<summary>
                ///Draw squared texture if mapping mode is set to stretchToSquare
                ///Otherwise draw dropping setup with darkened reference texture and squared crop area
                ///The darkened reference texture keeps it's aspect ratio
                ///Tiling and offset define the crop area
                /// </summary>
                if (m_mappingMode == MAPPINGMODE.stretchToSquare)
                    DD_GUILayOut.DrawRectTexture(m_Texture, new Rect(rt.position + new Vector2(0, 20), new Vector2(rt.width, rt.height + rt2.height + rt3.height)));
                else
                {
                    m_nonCropRect = DD_GUILayOut.DrawRectTexture(m_TextureDark, new Rect(rt.position + new Vector2(0, 20), new Vector2(rt.width, rt.height + rt2.height + rt3.height)));

                    if (m_TextureDark == null)
                    {
                        DD_NodeUtils.TexToTex(m_Texture, m_TextureDark, true, true);
                        if (m_TextureDark != null) m_TextureDark.Apply();
                    }

                    if (m_TextureDark != null)
                        m_cropRect.position = m_nonCropRect.position + m_CropRectOffset * (m_nonCropRect.size / new Vector2(m_TextureDark.width, m_TextureDark.height));

                    float cropRectSize = Mathf.Min((float)m_nonCropRect.width, (float)m_nonCropRect.height) * m_CropRectScaleFactor;

                    m_cropRect.size = new Vector2(cropRectSize, cropRectSize);

                    m_tilingX = m_cropRect.width / m_nonCropRect.width;
                    m_tilingY = m_cropRect.height / m_nonCropRect.height;

                    m_offsetX = (m_cropRect.position.x - m_nonCropRect.position.x) / m_nonCropRect.width;
                    m_offsetY = (m_nonCropRect.position.y + m_nonCropRect.height - (m_cropRect.position.y + m_cropRect.height)) / m_nonCropRect.height;

                    if (m_outputs[0].outputTexture != null)
                        EditorGUI.DrawPreviewTexture(m_cropRect, m_outputs[0].outputTexture);

                    GUI.Box(m_cropRect, "", DD_EditorUtils.editorSkin.GetStyle("CropRect"));

                    EditorGUI.BeginChangeCheck();
                    m_CropRectScaleFactor = DD_GUILayOut.Slider("Crop Size", m_CropRectScaleFactor, 0, 1);
                    if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

                    ProcessPropertyEvents();
                }
            }
        }

        void ProcessPropertyEvents()
        {
            //Drag the crop rect
            if (new Rect(m_cropRect.position - m_scrollPos, m_cropRect.size).Contains(DD_EditorUtils.mousePosInEditor - new Vector2(10, 40)))
            {
                if (Event.current.button == 0)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        m_cropRectDragging = true;
                    }
                }
            }

            if (Event.current.rawType == EventType.MouseUp)
            {
                if (m_cropRectDragging)
                {
                    m_cropRectDragging = false;
                    DD_EditorUtils.currentEvent.Use();
                }
            }
        }

        ///<summary>
        ///If calculation is set to be redone the following is done:
        ///Texture reference is drawn into a texture associated to the output connector
        ///output is either stretched to be a square or cropped using tiling and offset values
        ///The other output connectors of the node are associated with the result's color channels (RGBA)
        /// </summary>
        void Perform()
        {
            if (m_Texture != null)
            {
                if (m_mappingMode == MAPPINGMODE.stretchToSquare)
                    DD_NodeUtils.TexToTex(m_Texture, m_outputs[0].outputTexture, false, false);
                else
                    DD_NodeUtils.TexToTex(m_Texture, m_outputs[0].outputTexture, m_tilingX, m_tilingY, m_offsetX, m_offsetY);
            }
            else
            {
                m_outputs[0].outputTexture = new Texture2D(1, 1, TextureFormat.RGBAHalf, false);
                m_outputs[0].outputTexture.SetPixel(0, 0, Color.black);
            }

            if (m_outputs[0].outputTexture != null)
            {
                m_outputs[0].outputTexture.Apply();

                DD_NodeUtils.TextureComponent(m_outputs[0].outputTexture, m_outputs[1].outputTexture, 1, 0, 0, 0);
                DD_NodeUtils.TextureComponent(m_outputs[0].outputTexture, m_outputs[2].outputTexture, 0, 1, 0, 0);
                DD_NodeUtils.TextureComponent(m_outputs[0].outputTexture, m_outputs[3].outputTexture, 0, 0, 1, 0);
                DD_NodeUtils.TextureComponent(m_outputs[0].outputTexture, m_outputs[4].outputTexture, 0, 0, 0, 1);

                m_outputs[1].outputTexture.Apply();
                m_outputs[2].outputTexture.Apply();
                m_outputs[3].outputTexture.Apply();
                m_outputs[4].outputTexture.Apply();
            }

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif