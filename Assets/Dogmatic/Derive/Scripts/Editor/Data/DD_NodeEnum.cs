// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
namespace Derive
{
    public enum NodeType
    {
        #region Data nodes
        Color,
        Float,
        RGBAVector,
        Texture,
        #endregion

        #region Math nodes
        Abs,
        Clamp,
        Clamp01,
        Fract,
        Lerp,
        Math,
        MinMax,
        Negate,
        OneMinus,
        Pow,
        Remap,
        Round,
        Sqrt,
        Step,
        #endregion

        #region Filter nodes
        AOFromHeight,
        Blur,
        Brightness,
        Contrast,
        Distortion,
        Grayscale,
        Level,
        NormalsFromHeight,
        Pixelate,
        Posterize,
        Saturation,
        ShadowFilter,
        #endregion

        #region Generator nodes
        BaseShape,
        Gradient,
        Noise,
        Plasma,
        Voronoi,
        #endregion

        #region Texture operation nodes
        Append,
        Blend,
        ChannelBreakup,
        ChannelMask,
        Cross,
        Dot,
        Length,
        Normalize,
        #endregion

        #region Mapping nodes
        Inlay,
        Mirror,
        Rotate,
        SeamlessMapping,
        Shrink,
        TilingOffset,
        #endregion

        #region Misc nodes
        GetVariable,
        Relay,
        SetVariable,
        #endregion

        Master
    }

    public enum DataType
    {
        Float,
        RGBA
    }
}
#endif
