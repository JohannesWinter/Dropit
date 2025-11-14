// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using Derive;
using UnityEditor;

namespace DeriveUtils
{
    //[CreateAssetMenu(fileName = "Project Management Data", menuName = "Project List Container")]
    public class DD_ProjectListTemplate : ScriptableObject
    {
        public List<string> projectPaths;
    }

    [CustomEditor(typeof(DD_ProjectListTemplate))]
    public class DD_ProjectListInspector : Editor
    {
        public override void OnInspectorGUI()
        {

        }
    }
}
#endif