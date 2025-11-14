// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using Derive;

namespace DeriveUtils
{
    /// <summary>
    /// Contains static methods for texture operations
    /// mostly performed with shaders executed via Graphics.Blit
    /// </summary>
    public static class DD_NodeUtils
    {
        /// <summary>
        /// Performs basic math operations on textures (+ - * /)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="dst"></param>
        /// <param name="mathNodeType"></param>
        public static void TextureMath(Texture2D a, Texture2D b, Texture2D dst, MATHNODETYPE mathNodeType)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Math"));

            switch (mathNodeType)
            {
                case MATHNODETYPE.Add:
                    mat.SetFloat("_MulDiv", 0);
                    mat.SetFloat("_Subtract", 0);
                    break;

                case MATHNODETYPE.Subtract:
                    mat.SetFloat("_MulDiv", 0);
                    mat.SetFloat("_Subtract", 1);
                    break;

                case MATHNODETYPE.Multiply:
                    mat.SetFloat("_MulDiv", 1);
                    mat.SetFloat("_Divide", 0);
                    break;

                case MATHNODETYPE.Divide:
                    mat.SetFloat("_MulDiv", 1);
                    mat.SetFloat("_Divide", 1);
                    break;

                default:
                    break;
            }

            mat.SetTexture("_TextureInput1", a);
            mat.SetTexture("_TextureInput2", b);

            Graphics.Blit(rt, rt, mat);

            dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);
            dst.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Writes from one texture to another with some changing parameters
        /// darkening the texture will divide the pixel values by 2 in the shader - used for the background of the culling operation in the editor
        /// using the input resolution will set the output resolution equal to the input resolution to keep the aspect ratio - otherwise the texture is stretched to a square
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="darken"></param>
        /// <param name="useInputResolution"></param>
        public static void TexToTex(Texture2D src, Texture2D dst, bool darken, bool useInputResolution)
        {
            RenderTexture rt;
            //Texture2D output;

            if (useInputResolution)
            {
                rt = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.ARGBHalf);

                RenderTexture.active = rt;

                Material mat = new Material(Shader.Find("Hidden/Derive/Data/TexToTex"));

                mat.SetTexture("_Texture", src);

                if (darken)
                    mat.SetFloat("_Darken", 1);
                else
                    mat.SetFloat("_Darken", 0);

                Graphics.Blit(rt, rt, mat);


                if (dst == null)
                    dst = new Texture2D(src.width, src.height, TextureFormat.RGBAHalf, false);
                else
                    dst.Reinitialize(src.width, src.height, TextureFormat.RGBAHalf, false);

                dst.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
            }

            else
            {
                rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);

                RenderTexture.active = rt;

                Material mat = new Material(Shader.Find("Hidden/Derive/Data/TexToTex"));

                mat.SetTexture("_Texture", src);

                if (darken)
                    mat.SetFloat("_Darken", 1);
                else
                    mat.SetFloat("_Darken", 0);

                Graphics.Blit(rt, rt, mat);

                if (dst == null)
                    dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
                else
                    dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

                dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);
            }

            RenderTexture.active = null;

            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Same as above but with different overload
        /// Tiling and offset are passed to the shader.
        /// The shader processes the values in order to perform a cropping operation as specified in the editor
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="tilingX"></param>
        /// <param name="tilingY"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void TexToTex(Texture2D src, Texture2D dst, float tilingX, float tilingY, float offsetX, float offsetY)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Data/TexToTex"));

            mat.SetTexture("_Texture", src);
            mat.SetFloat("_Crop", 1);
            mat.SetFloat("_TilingX", tilingX);
            mat.SetFloat("_TilingY", tilingY);
            mat.SetFloat("_OffsetX", offsetX);
            mat.SetFloat("_OffsetY", offsetY);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Performs a component mask effect on the input texture
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public static void TextureComponent(Texture2D src, Texture2D dst, float r, float g, float b, float a)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Texture Component"));

            mat.SetTexture("_Texture", src);
            mat.SetFloat("_R", r);
            mat.SetFloat("_G", g);
            mat.SetFloat("_B", b);
            mat.SetFloat("_A", a);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Performs a grayscale operation on the input texture
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Grayscale(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Grayscale"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Extracts a normal map from a grayscale height map and stores it in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="normalStrength"></param>
        /// <param name="overdetermination"></param>
        /// <param name="bias"></param>
        public static void NormalsFromHeight(Texture2D src, Texture2D dst, float normalStrength, float overdetermination, float bias)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Normals From Height"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_NormalStrength", normalStrength);
            mat.SetFloat("_Overdetermination", overdetermination);
            mat.SetFloat("_Bias", bias);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Performs a blur operation on the input texture
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="blurStrength"></param>
        public static void Blur(Texture2D src, Texture2D dst, float blurStrength)
        {
            RenderTexture rt1 = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture rt2 = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt1;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Blur"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_NormalStrength", blurStrength);
            mat.SetFloat("_Strength", blurStrength);

            for (int i = 0; i < 100; i++)
            {
                if(i == 0)
                {
                    Graphics.Blit(rt1, rt1, mat);
                    continue;
                }
                else
                {
                    switch (9 % i)
                    {
                        case 0:
                            mat.SetTexture("_TextureInput", rt1);
                            RenderTexture.active = rt2;
                            Graphics.Blit(rt2, rt2, mat);
                            break;

                        case 1:
                            mat.SetTexture("_TextureInput", rt2);
                            RenderTexture.active = rt1;
                            Graphics.Blit(rt1, rt1, mat);
                            break;

                        default:
                            break;
                    }
                }
            }

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }

        /// <summary>
        /// Lerps between src1 and src2 based on alpha
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="alpha"></param>
        /// <param name="dst"></param>
        public static void Lerp(Texture2D src1, Texture2D src2, Texture2D alpha, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Lerp"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);
            mat.SetTexture("_TextureInput3", alpha);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the abolute value of the input
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Abs(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Abs"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the fractional remainder of the input value
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Fract(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Fract"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the square root of the input value
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Sqrt(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Sqrt"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Rounds the src UP or DOWN to the next integer
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="roundingOperation"></param>
        public static void Round(Texture2D src, Texture2D dst, ROUNDINGOPERATION roundingOperation)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Round"));

            mat.SetTexture("_TextureInput", src);

            switch (roundingOperation)
            {
                case ROUNDINGOPERATION.Ceil:
                    mat.SetFloat("_Floor", 0);
                    mat.SetFloat("_Round", 0);
                    break;

                case ROUNDINGOPERATION.Floor:
                    mat.SetFloat("_Floor", 1);
                    mat.SetFloat("_Round", 0);
                    break;

                case ROUNDINGOPERATION.Round:
                    mat.SetFloat("_Floor", 0);
                    mat.SetFloat("_Round", 1);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the minimum or the maximum of src1 & src2 inputted values
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="dst"></param>
        /// <param name="minMaxOperation"></param>
        public static void MinMax(Texture2D src1, Texture2D src2, Texture2D dst, MINMAXOPERATION minMaxOperation)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/MinMax"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            if(minMaxOperation == MINMAXOPERATION.Min) mat.SetFloat("_Max", 0);
            else mat.SetFloat("_Max", 1);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the power of input base and exponent
        /// </summary>
        /// <param name="src1">Base</param>
        /// <param name="src2">Exponent</param>
        /// <param name="dst">Target texture</param>
        public static void Pow(Texture2D src1, Texture2D src2, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Power"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns 1 if src1 <= src2
        /// Returns 0 if src1 > src2
        /// </summary>
        /// <param name="src1">Base</param>
        /// <param name="src2">Exponent</param>
        /// <param name="dst">Target texture</param>
        public static void Step(Texture2D src1, Texture2D src2, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Step"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Clamps the input between min & Max
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="dst"></param>
        public static void Clamp(Texture2D input, Texture2D min, Texture2D max, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Clamp"));

            mat.SetTexture("_TextureInput", input);
            mat.SetTexture("_Min", min);
            mat.SetTexture("_Max", max);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Remaps the input from the old range to the new range
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fromOld"></param>
        /// <param name="toOld"></param>
        /// <param name="fromNew"></param>
        /// <param name="toNew"></param>
        /// <param name="dst"></param>
        public static void Remap(Texture2D input, Texture2D fromOld, Texture2D toOld, Texture2D fromNew, Texture2D toNew, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Math/Remap"));

            mat.SetTexture("_TextureInput", input);
            mat.SetTexture("_FromOld", fromOld);
            mat.SetTexture("_ToOld", toOld);
            mat.SetTexture("_FromNew", fromNew);
            mat.SetTexture("_ToNew", toNew);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Extracts an ambient occlusion map from a grayscale height map and stores it in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        /// <param name="bias"></param>
        public static void AOFromHeight(Texture2D src, Texture2D dst, float strength, float bias)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/AO From Height"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Strength", strength);
            mat.SetFloat("_Bias", bias);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Manipulates the contrast of the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        public static void Contrast(Texture2D src, Texture2D dst, float strength)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Contrast"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Strength", strength);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Manipulates the saturation of the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        public static void Saturation(Texture2D src, Texture2D dst, float strength)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Saturation"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Strength", strength);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Manipulates the brightness of the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        public static void Brightness(Texture2D src, Texture2D dst, float strength)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Brightness"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Strength", strength);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Manipulates the saturation of the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        public static void Distort(Texture2D src1, Texture2D src2, Texture2D dst, float strength)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Distortion"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);
            mat.SetFloat("_Strength", strength);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Applies a posterizing effect on the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="dst"></param>
        /// <param name="strength"></param>
        public static void Posterize(Texture2D src, Texture2D dst, float strength)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Posterize"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Strength", strength);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Applies a pixelating effect on the input and stores the resulting texture into dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="size"></param>
        public static void Pixelate(Texture2D src, Texture2D dst, float size)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Filters/Pixelate"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Size", size);
            mat.SetFloat("_Resolution", (float)DD_EditorUtils.currentProject.m_projectSettings.resolution);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Tiles and offsets the input texture accorting to Vector2 inputs and stores the new texture in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="tiling"></param>
        /// <param name="offset"></param>
        public static void TilingAndOffset(Texture2D src, Texture2D dst, Vector2 tiling, Vector2 offset)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Tiling and Offset"));

            mat.SetTexture("_TextureInput", src);
            mat.SetVector("_Tiling", tiling);
            mat.SetVector("_Offset", offset);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Maps the input seamlessly and stores the result in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="falloff"></param>
        public static void SeamlessMapping(Texture2D src, Texture2D dst, float falloff, SEAMLESSMAPPINGMODE mode)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Seamless Mapping"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Falloff", falloff);

            switch (mode)
            {
                case (SEAMLESSMAPPINGMODE.CircularBlend):
                    mat.SetFloat("_CircularBlend", 1);
                    mat.SetFloat("_InverseCircularBlend", 0);
                    mat.SetFloat("_DiamondBlend", 0);
                    mat.SetFloat("_PolygonalBlend", 0);
                    mat.SetFloat("_MirroredEdges", 0);
                    break;

                case (SEAMLESSMAPPINGMODE.InverseCircularBlend):
                    mat.SetFloat("_CircularBlend", 0);
                    mat.SetFloat("_InverseCircularBlend", 1);
                    mat.SetFloat("_DiamondBlend", 0);
                    mat.SetFloat("_PolygonalBlend", 0);
                    mat.SetFloat("_MirroredEdges", 0);
                    break;

                case (SEAMLESSMAPPINGMODE.DiamondBlend):
                    mat.SetFloat("_CircularBlend", 0);
                    mat.SetFloat("_InverseCircularBlend", 0);
                    mat.SetFloat("_DiamondBlend", 1);
                    mat.SetFloat("_PolygonalBlend", 0);
                    mat.SetFloat("_MirroredEdges", 0);
                    break;

                case (SEAMLESSMAPPINGMODE.PolygonalBlend):
                    mat.SetFloat("_CircularBlend", 0);
                    mat.SetFloat("_InverseCircularBlend", 0);
                    mat.SetFloat("_DiamondBlend", 0);
                    mat.SetFloat("_PolygonalBlend", 1);
                    mat.SetFloat("_MirroredEdges", 0);
                    break;

                case (SEAMLESSMAPPINGMODE.MirroredEdges):
                    mat.SetFloat("_CircularBlend", 0);
                    mat.SetFloat("_InverseCircularBlend", 0);
                    mat.SetFloat("_DiamondBlend", 0);
                    mat.SetFloat("_PolygonalBlend", 0);
                    mat.SetFloat("_MirroredEdges", 1);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Rotates the input around the center and stores the result in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="rotation"></param>
        public static void Rotate(Texture2D src, Texture2D dst, float degrees)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Rotate"));

            mat.SetTexture("_TextureInput", src);
            mat.SetFloat("_Degrees", degrees);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Rotates the input around the center and stores the result in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="rotation"></param>
        public static void Mirror(Texture2D src, Texture2D dst, bool horizontal, bool vertical)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Mirror"));

            mat.SetTexture("_TextureInput", src);

            if (horizontal) mat.SetFloat("_Horizontal", 1);
            else mat.SetFloat("_Horizontal", 0);

            if (vertical) mat.SetFloat("_Vertical", 1);
            else mat.SetFloat("_Vertical", 0);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Creates an inlay of the input texture mapping it as a pattern and storing it in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="tilingX"></param>
        /// <param name="tilingY"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="extentX"></param>
        /// <param name="extentY"></param>
        public static void Inlay(Texture2D src, Texture2D dst, int tilingX, int tilingY, int offsetX, int offsetY, int extentX, int extentY)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Inlay"));

            mat.SetTexture("_TextureInput", src);

            mat.SetInt("_TilingX", tilingX);
            mat.SetInt("_TilingY", tilingY);

            mat.SetInt("_OffsetX", offsetX);
            mat.SetInt("_OffsetY", offsetY);

            mat.SetInt("_ExtentX", extentX);
            mat.SetInt("_ExtentY", extentY);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Uses the inlay shader to shrink the input. The result is stored in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="tilingX"></param>
        /// <param name="tilingY"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="extentX"></param>
        /// <param name="extentY"></param>
        public static void Shrink(Texture2D src, Texture2D dst, float tilingX, float tilingY, float offsetX, float offsetY, float extentX, float extentY)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Mapping/Shrink"));

            mat.SetTexture("_TextureInput", src);

            mat.SetFloat("_TilingX", tilingX);
            mat.SetFloat("_TilingY", tilingY);

            mat.SetFloat("_OffsetX", offsetX);
            mat.SetFloat("_OffsetY", offsetY);

            mat.SetFloat("_ExtentX", extentX);
            mat.SetFloat("_ExtentY", extentY);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Generates a tiled simplex noise texture and stores it in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="scale"></param>
        /// <param name="seed"></param>
        /// <param name="tiling"></param>
        /// <param name="octaves"></param>
        public static void Noise(Texture2D dst, float scale, float seed, Vector2 tiling, int octaves, NOISETYPE noiseType)
        {
            RenderTexture rt1 = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture rt2 = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);

            Material mat = new Material(Shader.Find("Hidden/Derive/Generators/Noise"));

            mat.SetFloat("_Seed", seed);
            mat.SetVector("_Tiling", tiling);

            if (noiseType == NOISETYPE.SolidNoise)
            {
                mat.SetFloat("_Turbulence", 0);

                for (int i = 1; i <= octaves; i++)
                {
                    float portion = 0;
                    float portionScale = scale * (Mathf.Pow(2, i - 1));

                    if (i == octaves) portion = 1 / Mathf.Pow(2, i - 1);
                    else portion = 1 / Mathf.Pow(2, i);

                    mat.SetFloat("_Scale", portionScale);
                    mat.SetFloat("_Portion", portion);

                    if (i == 1) mat.SetFloat("_FirstIteration", 1);
                    else mat.SetFloat("_FirstIteration", 0);

                    if (i % 2 == 1)
                    {
                        RenderTexture.active = rt1;
                        mat.SetTexture("_TextureInput", rt2);
                        Graphics.Blit(rt1, rt1, mat);
                    }
                    else
                    {
                        RenderTexture.active = rt2;
                        mat.SetTexture("_TextureInput", rt1);
                        Graphics.Blit(rt2, rt2, mat);
                    }
                }
            }
            else
            {
                mat.SetFloat("_Turbulence", 1);
                mat.SetFloat("_Scale", scale);

                RenderTexture.active = rt1;
                Graphics.Blit(rt1, rt1, mat);
            }
            
            

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }

        /// <summary>
        /// Generates a tiled voronoi texture and stores it in dst
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="scale"></param>
        /// <param name="seed"></param>
        /// <param name="tiling"></param>
        /// <param name="octaves"></param>
        /// <param name="voronoiType"></param>
        /// <param name="distanceType"></param>
        public static void Voronoi(Texture2D dst, float scale, float seed, Vector2 tiling, int octaves, VORONOITYPE voronoiType, DISTANCETYPE distanceType)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Generators/Voronoi"));

            mat.SetFloat("_Seed", seed);
            mat.SetVector("_Tiling", tiling);
            mat.SetFloat("_Scale", scale);
            mat.SetInt("_Octaves", octaves);

            switch (voronoiType)
            {
                case VORONOITYPE.Soft:
                    if (distanceType == DISTANCETYPE.Euclidian)
                    {
                        mat.SetFloat("_EuclidianSoft", 1);
                        mat.SetFloat("_EuclidianStucco", 0);
                        mat.SetFloat("_ManhattanSoft", 0);
                        mat.SetFloat("_ManhattanStucco", 0);
                        mat.SetFloat("_Simple", 0);
                    }
                    else
                    {
                        mat.SetFloat("_EuclidianSoft", 0);
                        mat.SetFloat("_EuclidianStucco", 0);
                        mat.SetFloat("_ManhattanSoft", 1);
                        mat.SetFloat("_ManhattanStucco", 0);
                        mat.SetFloat("_Simple", 0);
                    }
                    break;

                case VORONOITYPE.Stucco:
                    if (distanceType == DISTANCETYPE.Euclidian)
                    {
                        mat.SetFloat("_EuclidianSoft", 0);
                        mat.SetFloat("_EuclidianStucco", 1);
                        mat.SetFloat("_ManhattanSoft", 0);
                        mat.SetFloat("_ManhattanStucco", 0);
                        mat.SetFloat("_Simple", 0);
                    }
                    else
                    {
                        mat.SetFloat("_EuclidianSoft", 0);
                        mat.SetFloat("_EuclidianStucco", 0);
                        mat.SetFloat("_ManhattanSoft", 0);
                        mat.SetFloat("_ManhattanStucco", 1);
                        mat.SetFloat("_Simple", 0);
                    }
                    break;

                case VORONOITYPE.Simple:
                    mat.SetFloat("_EuclidianSoft", 0);
                    mat.SetFloat("_EuclidianStucco", 0);
                    mat.SetFloat("_ManhattanSoft", 0);
                    mat.SetFloat("_ManhattanStucco", 0);
                    mat.SetFloat("_Simple", 1);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Generates a gradient texture and stores it in dst
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="gradientType"></param>
        public static void Gradient(Texture2D dst, GRADIENTTYPE gradientType, float size, float rotation)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Generators/Gradient"));

            mat.SetFloat("_LinearAsymmetrical", 0);
            mat.SetFloat("_LinearSymmetrical", 0);
            mat.SetFloat("_LinearSquare", 0);
            mat.SetFloat("_SmoothDiamond", 0);
            mat.SetFloat("_SinAsymmetrical", 0);
            mat.SetFloat("_SinSymmetrical", 0);
            mat.SetFloat("_SinSquare", 0);
            mat.SetFloat("_SinSmoothSquare", 0);
            mat.SetFloat("_LogisticAsymmetrical", 0);
            mat.SetFloat("_LogisticSymmetrical", 0);
            mat.SetFloat("_LogisticSquare", 0);
            mat.SetFloat("_LogisticSmoothSquare", 0);
            mat.SetFloat("_QuarterCylinder", 0);
            mat.SetFloat("_HalfCylinder", 0);
            mat.SetFloat("_SphereToSquare", 0);
            mat.SetFloat("_SphereToSmoothSquare", 0);

            mat.SetFloat("_Rotation", rotation);
            mat.SetFloat("_Size", size);

            switch (gradientType)
            {
                case GRADIENTTYPE.LinearAsymmetrical:
                    mat.SetFloat("_LinearAsymmetrical", 1);
                    break;

                case GRADIENTTYPE.LinearSymmetrical:
                    mat.SetFloat("_LinearSymmetrical", 1);
                    break;

                case GRADIENTTYPE.LinearSquare:
                    mat.SetFloat("_LinearSquare", 1);
                    break;

                case GRADIENTTYPE.SmoothDiamond:
                    mat.SetFloat("_SmoothDiamond", 1);
                    break;

                case GRADIENTTYPE.SinAsymmetrical:
                    mat.SetFloat("_SinAsymmetrical", 1);
                    break;

                case GRADIENTTYPE.SinSymmetrical:
                    mat.SetFloat("_SinSymmetrical", 1);
                    break;

                case GRADIENTTYPE.SinSquare:
                    mat.SetFloat("_SinSquare", 1);
                    break;

                case GRADIENTTYPE.SinSmoothSquare:
                    mat.SetFloat("_SinSmoothSquare", 1);
                    break;

                case GRADIENTTYPE.LogisticAsymmetrical:
                    mat.SetFloat("_LogisticAsymmetrical", 1);
                    break;

                case GRADIENTTYPE.LogisticSymmetrical:
                    mat.SetFloat("_LogisticSymmetrical", 1);
                    break;

                case GRADIENTTYPE.LogisticSquare:
                    mat.SetFloat("_LogisticSquare", 1);
                    break;

                case GRADIENTTYPE.LogisticSmoothSquare:
                    mat.SetFloat("_LogisticSmoothSquare", 1);
                    break;

                case GRADIENTTYPE.QuarterCylinder:
                    mat.SetFloat("_QuarterCylinder", 1);
                    break;

                case GRADIENTTYPE.HalfCylinder:
                    mat.SetFloat("_HalfCylinder", 1);
                    break;

                case GRADIENTTYPE.SphereToSquare:
                    mat.SetFloat("_SphereToSquare", 1);
                    break;

                case GRADIENTTYPE.SphereToSmoothSquare:
                    mat.SetFloat("_SphereToSmoothSquare", 1);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Generates a gradient texture and stores it in dst
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="gradientType"></param>
        public static void BaseShape(Texture2D dst, SHAPE shape, float radius, float falloff, float ridge)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Generators/BaseShape"));

            mat.SetFloat("_Square", 0);
            mat.SetFloat("_Circle", 0);
            mat.SetFloat("_RoundedSquare", 0);
            mat.SetFloat("_Octagon", 0);
            mat.SetFloat("_Hexagon", 0);
            mat.SetFloat("_Triangle", 0);

            switch (shape)
            {
                case SHAPE.Square:
                    mat.SetFloat("_Square", 1);
                    break;

                case SHAPE.Circle:
                    mat.SetFloat("_Circle", 1);
                    break;

                case SHAPE.RoundedSquare:
                    mat.SetFloat("_RoundedSquare", 1);
                    mat.SetFloat("_Radius", radius);
                    break;

                case SHAPE.Octagon:
                    mat.SetFloat("_Octagon", 1);
                    mat.SetFloat("_Ridge", ridge);
                    break;

                case SHAPE.Hexagon:
                    mat.SetFloat("_Hexagon", 1);
                    mat.SetFloat("_HexagonFalloff", falloff);
                    break;

                case SHAPE.Triangle:
                    mat.SetFloat("_Triangle", 1);
                    mat.SetFloat("_TriangleFalloff", falloff);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Normalizes the value of each pixel treating them as vectors and stores the result in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Normalize(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Normalize"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the length of each pixel treating them as vectors and stores the result in dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Length(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Length"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the a cross product per each pixel of src1 & src2 treating them as vectors and stores the result in dst
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="dst"></param>
        public static void Cross(Texture2D src1, Texture2D src2, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Cross"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Returns the a dot product per each pixel of src1 & src2 treating them as vectors and stores the result in dst
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="dst"></param>
        public static void Dot(Texture2D src1, Texture2D src2, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Dot"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Appends input textures that are treated as float to an RGBA-Texture and stores the result in dst
        /// Textures of type float have the same value for each channel in each pixel
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="src3"></param>
        /// <param name="src4"></param>
        /// <param name="dst"></param>
        public static void Append(Texture2D src1, Texture2D src2, Texture2D src3, Texture2D src4, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Append"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);
            mat.SetTexture("_TextureInput3", src3);
            mat.SetTexture("_TextureInput4", src4);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Blends src1 & src2 based on the blendMode and stores the result in dst
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="dst"></param>
        /// <param name="blendMode"></param>
        public static void Blend(Texture2D src1, Texture2D src2, Texture2D dst, BLENDMODE blendMode)
        {
            RenderTexture rt = RenderTexture.GetTemporary(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, 0, RenderTextureFormat.ARGBHalf);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Texture Operations/Blend"));

            mat.SetTexture("_TextureInput1", src1);
            mat.SetTexture("_TextureInput2", src2);

            mat.SetFloat("_Overlay", 0);
            mat.SetFloat("_Burn", 0);
            mat.SetFloat("_Dodge", 0);
            mat.SetFloat("_Exclude", 0);
            mat.SetFloat("_Darken", 0);
            mat.SetFloat("_Lighten", 0);
            mat.SetFloat("_SoftLight", 0);
            mat.SetFloat("_HardLight", 0);
            mat.SetFloat("_PinLight", 0);
            mat.SetFloat("_VividLight", 0);
            mat.SetFloat("_HardMix", 0);
            mat.SetFloat("_Difference", 0);
            mat.SetFloat("_Subtraction", 0);
            mat.SetFloat("_Multiplication", 0);
            mat.SetFloat("_Division", 0);

            switch (blendMode)
            {
                case BLENDMODE.Overlay:
                    mat.SetFloat("_Overlay", 1);
                    break;

                case BLENDMODE.Burn:
                    mat.SetFloat("_Burn", 1);
                    break;

                case BLENDMODE.Dodge:
                    mat.SetFloat("_Dodge", 1);
                    break;

                case BLENDMODE.Exclude:
                    mat.SetFloat("_Exclude", 1);
                    break;

                case BLENDMODE.Darken:
                    mat.SetFloat("_Darken", 1);
                    break;

                case BLENDMODE.Lighten:
                    mat.SetFloat("_Lighten", 1);
                    break;

                case BLENDMODE.SoftLight:
                    mat.SetFloat("_SoftLight", 1);
                    break;

                case BLENDMODE.HardLight:
                    mat.SetFloat("_HardLight", 1);
                    break;

                case BLENDMODE.PinLight:
                    mat.SetFloat("_PinLight", 1);
                    break;

                case BLENDMODE.VividLight:
                    mat.SetFloat("_VividLight", 1);
                    break;

                case BLENDMODE.HardMix:
                    mat.SetFloat("_HardMix", 1);
                    break;

                case BLENDMODE.Difference:
                    mat.SetFloat("_Difference", 1);
                    break;

                case BLENDMODE.Subtraction:
                    mat.SetFloat("_Subtraction", 1);
                    break;

                case BLENDMODE.Multiplication:
                    mat.SetFloat("_Multiplication", 1);
                    break;

                case BLENDMODE.Division:
                    mat.SetFloat("_Division", 1);
                    break;

                default:
                    break;
            }

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);
            else
                dst.Reinitialize(DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution, TextureFormat.RGBAHalf, false);

            dst.ReadPixels(new Rect(0, 0, DD_EditorUtils.currentProject.m_projectSettings.resolution, DD_EditorUtils.currentProject.m_projectSettings.resolution), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}
#endif