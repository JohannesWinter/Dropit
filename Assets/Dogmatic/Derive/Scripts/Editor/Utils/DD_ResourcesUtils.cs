// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Linq;


namespace DeriveUtils
{
    public static class DD_ResourcesUtils
    {
        public static void CaptureScreenshot(string path)
        {
            RenderTexture rt = RenderTexture.GetTemporary(1920, 1080, 0, RenderTextureFormat.ARGB32);
            RenderTexture rt2 = RenderTexture.GetTemporary(1920, 1080, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = rt;

            Camera cam = DD_EditorUtils.currentProject.m_preview.m_camera;
            cam.targetTexture = rt;
            cam.Render();

            Material waterMarkMat = new Material(Shader.Find("Hidden/Derive/Post Processing/Watermark Post Processing"));
            Texture2D waterMarkTex = (Texture2D)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Textures/Logos/DD_FullSizeWatermark.png", typeof(Texture2D));

            waterMarkMat.SetTexture("_Tex", rt);
            waterMarkMat.SetTexture("_Tex2", waterMarkTex);

            Graphics.Blit(rt, rt2, waterMarkMat);

            rt.Release();
            RenderTexture.active = rt2;

            Texture2D output = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
            output.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);

            byte[] bytes;
            bytes = output.EncodeToJPG();
            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();

            RenderTexture.active = null;
            rt2.Release();
            UnityEngine.Object.DestroyImmediate(output);
        }

        public static void CaptureEditorScreenshot(string path)
        {
            Color[] colors = UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(DD_EditorUtils.windowRect.position, (int)DD_EditorUtils.windowRect.width, (int)DD_EditorUtils.windowRect.height);

            Texture2D output = new Texture2D((int)DD_EditorUtils.windowRect.width, (int)DD_EditorUtils.windowRect.height, TextureFormat.ARGB32, false);
            output.SetPixels(colors);

            byte[] bytes;
            bytes = output.EncodeToJPG();
            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();

            UnityEngine.Object.DestroyImmediate(output);
        }

        public static void UpdateNews()
        {
            string fullContent = "-";

            using (UnityWebRequest www = UnityWebRequest.Get(DD_EditorUtils.resourcesData.newsFeedUrl + "?" + DateTime.Now))
            {
                www.SendWebRequest();

                float timestamp = Time.realtimeSinceStartup;

                while (www.responseCode == 0)
                {
                    //Prevents Unity from crashing because of the loop
                    System.Threading.Thread.Sleep(1000);
                    if ((Time.realtimeSinceStartup - timestamp) > 3) return;
                }
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {

                    if (!fullContent.Equals(www.downloadHandler.text))
                    {
                        fullContent = www.downloadHandler.text;
                    }

                    try
                    {
                        string checkdate = fullContent.Remove(8);
                        if(checkdate != DD_EditorUtils.resourcesData.checkdateNews)
                        {
                            DD_EditorUtils.resourcesData.checkdateNews = checkdate;

                            string nONewsElementsString = "";
                            string nONewsElementsStringStart = "#NONEWSSTART";
                            string nONewsElementsStringEnd = "#NONEWSEND";

                            nONewsElementsString = fullContent.Substring(fullContent.IndexOf(nONewsElementsStringStart) + nONewsElementsStringStart.Length);
                            nONewsElementsString = nONewsElementsString.Remove(nONewsElementsString.IndexOf(nONewsElementsStringEnd));

                            int nONewsElements = 0;
                            int.TryParse(nONewsElementsString, out nONewsElements);

                            DD_EditorUtils.resourcesData.newsFeedObjects = new NewsFeedObject[nONewsElements];

                            for (int i = 1; i <= nONewsElements; i++)
                            {
                                string title = "";
                                string titleStart = "#TITLESTART" + i.ToString();
                                string titleEnd = "#TITLEEND" + i.ToString();

                                string date = "";
                                string dateStart = "#DATESTART" + i.ToString();
                                string dateEnd = "#DATEEND" + i.ToString();

                                string text = "";
                                string textStart = "#TEXTSTART" + i.ToString();
                                string textEnd = "#TEXTEND" + i.ToString();

                                string url = "";
                                string urlStart = "#URLSTART" + i.ToString();
                                string urlEnd = "#URLEND" + i.ToString();

                                title = fullContent.Substring(fullContent.IndexOf(titleStart) + titleStart.Length);
                                title = title.Remove(title.IndexOf(titleEnd));

                                date = fullContent.Substring(fullContent.IndexOf(dateStart) + dateStart.Length);
                                date = date.Remove(date.IndexOf(dateEnd));

                                text = fullContent.Substring(fullContent.IndexOf(textStart) + textStart.Length);
                                text = text.Remove(text.IndexOf(textEnd));

                                url = fullContent.Substring(fullContent.IndexOf(urlStart) + urlStart.Length);
                                url = url.Remove(url.IndexOf(urlEnd));

                                DD_EditorUtils.resourcesData.newsFeedObjects[i-1].title = title;
                                DD_EditorUtils.resourcesData.newsFeedObjects[i-1].date = date;
                                DD_EditorUtils.resourcesData.newsFeedObjects[i-1].content = text;
                                DD_EditorUtils.resourcesData.newsFeedObjects[i-1].url = url;
                            }

                            DD_EditorUtils.resourcesData.highlightNewsTab = true;
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.Log("Failed to update news feed - " + e.Message);
                    }

                    UnityWebRequest.Delete(DD_EditorUtils.resourcesData.newsFeedUrl); 
                    www.Dispose();
                    www.downloadHandler.Dispose();
                }
            }
        }

        public static void UpdateResourceFeed()
        {
            string fullContent = "-";

            using (UnityWebRequest www = UnityWebRequest.Get(DD_EditorUtils.resourcesData.resourcesFeedUrl + "?" + DateTime.Now))
            {
                www.SendWebRequest();

                float timestamp = Time.realtimeSinceStartup;

                while (www.responseCode == 0)
                {
                    //Prevents Unity from crashing because of the loop
                    System.Threading.Thread.Sleep(1000);
                    if ((Time.realtimeSinceStartup - timestamp) > 3) return;
                }
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {

                    if (!fullContent.Equals(www.downloadHandler.text))
                    {
                        fullContent = www.downloadHandler.text;
                    }

                    try
                    {
                        string checkdate = fullContent.Remove(8);
                        if (checkdate != DD_EditorUtils.resourcesData.checkdateResources)
                        {
                            DD_EditorUtils.resourcesData.checkdateResources = checkdate;

                            string nOResourcePackagesString = "";
                            string nOResourcePackagesStringStart = "#NORESOURCEPACKAGESSTART";
                            string nOResourcePackagesStringEnd = "#NORESOURCEPACKAGESEND";

                            nOResourcePackagesString = fullContent.Substring(fullContent.IndexOf(nOResourcePackagesStringStart) + nOResourcePackagesStringStart.Length);
                            nOResourcePackagesString = nOResourcePackagesString.Remove(nOResourcePackagesString.IndexOf(nOResourcePackagesStringEnd));

                            int nOResourcePackages = 0;
                            int.TryParse(nOResourcePackagesString, out nOResourcePackages);

                            DD_EditorUtils.resourcesData.resourcePackageObjects = new ResourcePackageObject[nOResourcePackages];

                            for (int i = 1; i <= nOResourcePackages; i++)
                            {
                                string title = "";
                                string titleStart = "#TITLESTART" + i.ToString();
                                string titleEnd = "#TITLEEND" + i.ToString();

                                string url = "";
                                string urlStart = "#URLSTART" + i.ToString();
                                string urlEnd = "#URLEND" + i.ToString();

                                title = fullContent.Substring(fullContent.IndexOf(titleStart) + titleStart.Length);
                                title = title.Remove(title.IndexOf(titleEnd));

                                url = fullContent.Substring(fullContent.IndexOf(urlStart) + urlStart.Length);
                                url = url.Remove(url.IndexOf(urlEnd));

                                DD_EditorUtils.resourcesData.resourcePackageObjects[i - 1].title = title;
                                DD_EditorUtils.resourcesData.resourcePackageObjects[i - 1].url = url;
                            }

                            DD_EditorUtils.resourcesData.highlightResourcesTab = true;
                        }
                    }
                    catch (Exception e)
                    {

                        Debug.Log("Failed to update resource feed - " + e.Message);
                    }

                    UnityWebRequest.Delete(DD_EditorUtils.resourcesData.newsFeedUrl);
                    www.Dispose();
                    www.downloadHandler.Dispose();
                }
            }
        }

        public static ResourceTextureData GetResourceTextures()
        {
            ResourcePackageObject[] resourcePackageObjects = DD_EditorUtils.resourcesData.resourcePackageObjects;

            FileInfo[] fullFileInfosPatterns = new FileInfo[0];
            FileInfo[] fullFileInfosHeightMaps = new FileInfo[0];
            FileInfo[] fullFileInfosAlbedoMaps = new FileInfo[0];

            for (int i = 0; i < resourcePackageObjects.Length; i++)
            {
                string dogmaticResourcesFolder = DD_EditorUtils.GetDerivePath().Replace("Derive", "Derive Resources");
                string fullPath = Application.dataPath.Replace("Assets", "");

                fullPath = fullPath + dogmaticResourcesFolder + resourcePackageObjects[i].title;

                if (Directory.Exists(fullPath))
                {
                    string patternsPath = fullPath + "/Patterns";
                    string heightMapPath = fullPath + "/Height Maps";
                    string albedoPath = fullPath + "/Albedo Maps";

                    if (Directory.Exists(patternsPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(patternsPath);
                        FileInfo[] fileInfos = directoryInfo.GetFiles("*.png");

                        fullFileInfosPatterns = fullFileInfosPatterns.Concat(fileInfos).ToArray();
                    }

                    if (Directory.Exists(heightMapPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(heightMapPath);
                        FileInfo[] fileInfos = directoryInfo.GetFiles("*.png");

                        fullFileInfosHeightMaps = fullFileInfosHeightMaps.Concat(fileInfos).ToArray();
                    }

                    if (Directory.Exists(albedoPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(albedoPath);
                        FileInfo[] fileInfos = directoryInfo.GetFiles("*.png");

                        fullFileInfosAlbedoMaps = fullFileInfosAlbedoMaps.Concat(fileInfos).ToArray();
                    }
                }
            }

            //Order full file info arrays alphabetically by file name(not path!)
            Array.Sort(fullFileInfosPatterns, (x, y) => String.Compare(x.Name, y.Name));
            Array.Sort(fullFileInfosHeightMaps, (x, y) => String.Compare(x.Name, y.Name));
            Array.Sort(fullFileInfosAlbedoMaps, (x, y) => String.Compare(x.Name, y.Name));

            ResourceTextureData resourceTextureData = new ResourceTextureData();

            resourceTextureData.resourceTextures = new Texture2D[3][] { new Texture2D[fullFileInfosPatterns.Length], new Texture2D[fullFileInfosHeightMaps.Length], new Texture2D[fullFileInfosAlbedoMaps.Length] };
            resourceTextureData.resourceTextureNames = new string[3][] { new string[fullFileInfosPatterns.Length], new string[fullFileInfosHeightMaps.Length], new string[fullFileInfosAlbedoMaps.Length] };
            resourceTextureData.resourceTextureFullNames = new string[3][] { new string[fullFileInfosPatterns.Length], new string[fullFileInfosHeightMaps.Length], new string[fullFileInfosAlbedoMaps.Length] };
            resourceTextureData.resourceTextureRects = new Rect[3][] { new Rect[fullFileInfosPatterns.Length], new Rect[fullFileInfosHeightMaps.Length], new Rect[fullFileInfosAlbedoMaps.Length] };

            for (int i = 0; i < fullFileInfosPatterns.Length; i++)
            {
                string texturePath = fullFileInfosPatterns[i].FullName.Substring(fullFileInfosPatterns[i].FullName.IndexOf("Assets"));
                resourceTextureData.resourceTextures[0][i] = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                resourceTextureData.resourceTextureNames[0][i] = fullFileInfosPatterns[i].Name;
                resourceTextureData.resourceTextureFullNames[0][i] = fullFileInfosPatterns[i].FullName;
            }

            for (int i = 0; i < fullFileInfosHeightMaps.Length; i++)
            {
                string texturePath = fullFileInfosHeightMaps[i].FullName.Substring(fullFileInfosHeightMaps[i].FullName.IndexOf("Assets"));
                resourceTextureData.resourceTextures[1][i] = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                resourceTextureData.resourceTextureNames[1][i] = fullFileInfosHeightMaps[i].Name;
                resourceTextureData.resourceTextureFullNames[1][i] = fullFileInfosHeightMaps[i].FullName;
            }

            for (int i = 0; i < fullFileInfosAlbedoMaps.Length; i++)
            {
                string texturePath = fullFileInfosAlbedoMaps[i].FullName.Substring(fullFileInfosAlbedoMaps[i].FullName.IndexOf("Assets"));
                resourceTextureData.resourceTextures[2][i] = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                resourceTextureData.resourceTextureNames[2][i] = fullFileInfosAlbedoMaps[i].Name;
                resourceTextureData.resourceTextureFullNames[2][i] = fullFileInfosAlbedoMaps[i].FullName;
            }

            return resourceTextureData;
        }

        /// <summary>
        /// Sets the alpha value of all pixels in the input texture to 1
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void RemoveAlphaFromTexture(Texture2D src, Texture2D dst)
        {
            RenderTexture rt = RenderTexture.GetTemporary(128, 128, 0, RenderTextureFormat.ARGB32);
            RenderTexture.active = rt;

            Material mat = new Material(Shader.Find("Hidden/Derive/Post Processing/Remove Alpha Post Processing"));

            mat.SetTexture("_TextureInput", src);

            Graphics.Blit(rt, rt, mat);

            if (dst == null)
                dst = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            else
                dst.Reinitialize(128, 128, TextureFormat.RGBA32, false);

            dst.ReadPixels(new Rect(0, 0, 128, 128), 0, 0);

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}
#endif