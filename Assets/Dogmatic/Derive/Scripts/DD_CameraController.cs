// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace DeriveUtils
{
    /// <summary>
    /// Adjusts lighting settings before rendering and reverts them after rendering.
    /// </summary>
    [ExecuteAlways]
    public class DD_CameraController : MonoBehaviour
    {
        AmbientMode m_ambientModeCache;
        Color m_ambientColorCache;

        private void OnPreRender()
        {
            m_ambientModeCache = RenderSettings.ambientMode;
            m_ambientColorCache = RenderSettings.ambientLight;

            RenderSettings.ambientMode = AmbientMode.Flat;

            if(DD_EditorUtils.currentProject != null)
            {
                if (DD_EditorUtils.currentProject.m_preview.m_lightSetup == LIGHTSETUP.DirectionalAndAmbient)
                    RenderSettings.ambientLight = new Color(0.15f, 0.15f, 0.15f, 1);
                else
                    RenderSettings.ambientLight = new Color(0, 0, 0, 1);
            }
                
            
        }

        private void OnPostRender()
        {
            RenderSettings.ambientMode = m_ambientModeCache;
            RenderSettings.ambientLight = m_ambientColorCache;
        }
    }
}
#endif
