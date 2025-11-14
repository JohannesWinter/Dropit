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
    public enum VORONOITYPE
    {
        Soft,
        Stucco,
        Simple
    }

    public enum DISTANCETYPE
    {
        Euclidian,
        Manhattan
    }

    [Serializable]
    public class DD_NodeVoronoi : DD_NodeBase
    {
        #region public variables
        public VORONOITYPE m_voronoiType = VORONOITYPE.Soft;
        public DISTANCETYPE m_distanceType = DISTANCETYPE.Euclidian;

        public int m_seed = 0;
        public float m_scale = 1;
        public int m_octaves = 4;
        public Vector2 m_tiling = Vector2.one;
        #endregion

        #region private variables
        bool m_showOutput = true;
        bool m_showVoronoiSettings = true;
        #endregion

        #region constructors
        public DD_NodeVoronoi()
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

            m_nodeType = NodeType.Voronoi;
            m_nodeRect = new Rect(10, 10, 160, 32);

            m_overrideInputConnectorRendering = false;
            m_overrideOutputConnectorRendering = false;

            TooltipData toolTip = DD_EditorUtils.nodeData.tooltipData[Array.IndexOf(DD_EditorUtils.nodeData.nodeTypes, m_nodeType)];

            m_nodeTooltip.m_title = toolTip.title;
            m_nodeTooltip.m_content = toolTip.tooltipContent;
            m_nodeTooltip.m_targetURL = toolTip.url;

            m_outputs[0].outputLabel = "Output";

            m_outputs[0].outputDataType = DataType.Float;

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

            DD_GUILayOut.TitleLabel(NodeType.Voronoi.ToString());

            m_showOutput = DD_GUILayOut.FoldOut(m_showOutput, "Output Preview");

            EditorGUILayout.EndVertical();

            if (m_showOutput)
                DD_GUILayOut.DrawTexture(m_outputs[0].outputTexture, rt);

            m_showVoronoiSettings = DD_GUILayOut.FoldOut(m_showVoronoiSettings, "Voronoi Settings");

            if (m_showVoronoiSettings)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Voronoi Type"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
                m_voronoiType = (VORONOITYPE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_voronoiType, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);

                if (m_voronoiType != VORONOITYPE.Simple)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Distance Type"), DD_EditorUtils.editorSkin.GetStyle("DefaultLabel"), GUILayout.Width(100));
                    m_distanceType = (DISTANCETYPE)EditorGUILayout.EnumPopup(new GUIContent(" "), m_distanceType, DD_EditorUtils.editorSkin.GetStyle("EnumPopup"), GUILayout.Width(DD_EditorUtils.viewRect_propertyView.width - 150));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(20);
                }
                

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

                if (EditorGUI.EndChangeCheck()) m_redoCalculation = true;

                EditorGUILayout.Space(5);
            }
        }

        /// <summary>
        /// Generates a Foronoi texture and stores it in the output texture
        /// </summary>
        void Perform()
        {
            DD_NodeUtils.Voronoi(m_outputs[0].outputTexture, m_scale, m_seed, m_tiling, m_octaves, m_voronoiType, m_distanceType);

            m_outputs[0].outputTexture.Apply();

            m_redoCalculation = false;
            m_outputHasChanged = true;
        }
        #endregion
    }
}
#endif