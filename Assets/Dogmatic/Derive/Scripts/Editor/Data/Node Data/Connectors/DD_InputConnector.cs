// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using System;

namespace Derive
{
    [Serializable]
    public class DD_InputConnector
    {
        public bool isOccupied = false;
        public DD_NodeBase inputtingNode;
        public int outputIndex;
        public Rect inputRect;

        public string inputLabel = "";
    }
}
#endif