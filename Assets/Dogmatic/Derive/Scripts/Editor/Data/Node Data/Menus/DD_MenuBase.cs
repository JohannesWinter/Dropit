// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;

namespace Derive
{
    public class DD_MenuBase
    {
        #region Public Variables
        public Rect m_menuRect;
        #endregion

        #region Protected Variables
        protected GUISkin m_foldOutSkin;
        #endregion

        #region Private Variables
        #endregion

        #region Contructors
        #endregion

        #region Main Methods
        public virtual void OnMenuGUI(Rect menuRect)
        {
            m_menuRect = menuRect;
        }
        #endregion

        #region Utility Methods
        #endregion
    }
}
#endif