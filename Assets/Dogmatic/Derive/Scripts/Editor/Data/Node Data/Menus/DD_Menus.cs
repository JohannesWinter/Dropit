// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEditor;

namespace Derive
{
    public static class DD_Menus
    {
        [MenuItem("Window/Derive/Launch Editor")]
        public static void InitDeriveEditor()
        {
            DD_NodeEditorWindow.InitEditorWindow();
        }
    }
}
#endif
