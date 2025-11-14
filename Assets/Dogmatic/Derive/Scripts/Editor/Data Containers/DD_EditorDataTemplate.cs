// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;

namespace DeriveUtils
{
    [Serializable]
    //[CreateAssetMenu(fileName = "Editor Data", menuName = "Editor Data Container")]
    public class DD_EditorDataTemplate : ScriptableObject
    {
        public string versionNumber;
        public Texture2D workViewLogo;
    }

    [CustomEditor(typeof(DD_EditorDataTemplate))]
    public class DD_EditorDataInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            
        }
    }
}
#endif