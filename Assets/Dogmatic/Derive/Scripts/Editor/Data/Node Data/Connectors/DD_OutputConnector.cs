// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;

namespace Derive
{
    [Serializable]
    public class DD_OutputConnector
    {
        public bool isOccupied = false;
        public Vector2 connectionStartPoint;
        public Rect outputRect;

        public string outputLabel = "";

        public DataType outputDataType = DataType.Float;
        public float outputFloat = 0;

        public Texture2D outputTexture;

        [HideInInspector]
        public byte[] outputTextureBytes;
    }
}
#endif