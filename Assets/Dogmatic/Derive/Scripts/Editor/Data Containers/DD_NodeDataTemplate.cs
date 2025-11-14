// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using Derive;
using UnityEditor;

namespace DeriveUtils
{
    [Serializable]
    public struct TooltipData
    {
        public string title;
        public string tooltipContent;
        public string url;
    }



    [Serializable]
    //[CreateAssetMenu(fileName = "Node Data", menuName = "Node Data Container")]
    public class DD_NodeDataTemplate : ScriptableObject
    {
        public NodeType[] nodeTypes;
        public TooltipData[] tooltipData;
    }

    [CustomEditor(typeof(DD_NodeDataTemplate))]
    public class DD_NodeDataInspector : Editor
    {
        public override void OnInspectorGUI()
        {

        }
    }
}
#endif