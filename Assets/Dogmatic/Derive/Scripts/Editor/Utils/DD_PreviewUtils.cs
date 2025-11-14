// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Derive;

namespace DeriveUtils
{
    public static class DD_PreviewUtils
    {
        /// <summary>
        /// Deletes present derive object if it can be found
        /// Deletes all objects required for preview, so they can be created anew.
        /// Creates new derive object that holds the other objects as children
        /// </summary>
        /// <param name="deriveObj"></param>
        /// <param name="cameraObj"></param>
        /// <param name="lightObj"></param>
        /// <param name="renderObj"></param>
        /// <param name="gizmoCameraObj"></param>
        /// <param name="gizmoLightObj"></param>
        /// <param name="gizmoFlashLight"></param>
        /// <returns></returns>
        public static GameObject AddDeriveObj(GameObject deriveObj, GameObject cameraObj, GameObject lightObj, GameObject renderObj, GameObject gizmoCameraObj, GameObject gizmoLightObj, GameObject gizmoFlashLight)
        {
            GameObject.DestroyImmediate(GameObject.Find("Derive Preview Object"));
            GameObject.DestroyImmediate(cameraObj);
            GameObject.DestroyImmediate(lightObj);
            GameObject.DestroyImmediate(renderObj);
            GameObject.DestroyImmediate(gizmoCameraObj);
            GameObject.DestroyImmediate(gizmoLightObj);
            GameObject.DestroyImmediate(gizmoFlashLight);

            deriveObj = new GameObject("Derive Preview Object");
            deriveObj.layer = LayerMask.NameToLayer("Derive");
            deriveObj.hideFlags = HideFlags.HideAndDontSave;

            return deriveObj;
        }

        /// <summary>
        /// Creates a new camera object for rendering the render object
        /// </summary>
        /// <param name="cameraObj"></param>
        /// <param name="deriveObj"></param>
        /// <returns></returns>
        public static GameObject AddCamera(GameObject cameraObj, GameObject deriveObj)
        {
            cameraObj = new GameObject("Derive Preview Cam");
            cameraObj.transform.SetParent(deriveObj.transform);
            cameraObj.transform.localPosition = new Vector3(0, 0, -3);
            cameraObj.transform.eulerAngles = new Vector3(0, 0, 3);
            cameraObj.layer = LayerMask.NameToLayer("Derive");
            cameraObj.AddComponent<DD_CameraController>();

            Camera camera = cameraObj.AddComponent<Camera>();
            camera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1);
            camera.clearFlags = CameraClearFlags.Color;

            camera.cullingMask = 1 >> LayerMask.NameToLayer("Nothing");
            camera.cullingMask ^= 1 << LayerMask.NameToLayer("Derive");

            camera.renderingPath = RenderingPath.DeferredShading;

            return cameraObj;
        }

        /// <summary>
        /// Adds a camera that renders the light gizmo only
        /// </summary>
        /// <param name="gizmoCameraObj"></param>
        /// <param name="deriveObj"></param>
        /// <returns></returns>
        public static GameObject AddGizmoCamera(GameObject gizmoCameraObj, GameObject deriveObj)
        {
            gizmoCameraObj = new GameObject("Derive Gizmo Cam");
            gizmoCameraObj.transform.SetParent(deriveObj.transform);
            gizmoCameraObj.transform.localPosition = new Vector3(0, 0, -1);
            gizmoCameraObj.layer = LayerMask.NameToLayer("Derive Gizmos");

            Camera gizmoCamera = gizmoCameraObj.AddComponent<Camera>();
            gizmoCamera.backgroundColor = new Color(0, 0, 0, 0);
            gizmoCamera.clearFlags = CameraClearFlags.Color;

            gizmoCamera.cullingMask = 1 >> LayerMask.NameToLayer("Nothing");
            gizmoCamera.cullingMask ^= 1 << LayerMask.NameToLayer("Derive Gizmos");

            return gizmoCameraObj;
        }

        /// <summary>
        /// Adds a light that lightens the render object
        /// </summary>
        /// <param name="lightObj"></param>
        /// <param name="deriveObj"></param>
        /// <param name="lightObjRotation"></param>
        /// <returns></returns>
        public static GameObject AddLight(GameObject lightObj, GameObject deriveObj, Vector3 lightObjRotation, float intensity)
        {
            lightObj = new GameObject("Derive Preview Light");
            lightObj.transform.SetParent(deriveObj.transform);

            if (lightObjRotation == Vector3.zero) lightObjRotation = new Vector3(50, 10, 0);
            lightObj.transform.eulerAngles = lightObjRotation;

            lightObj.layer = LayerMask.NameToLayer("Derive");

            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1, 0.95f, 0.85f, 1);
            light.shadows = LightShadows.Soft;
            light.intensity = intensity;

            light.cullingMask = 1 >> LayerMask.NameToLayer("Nothing");
            light.cullingMask ^= 1 << LayerMask.NameToLayer("Derive");

            return lightObj;
        }

        /// <summary>
        /// Adds a reflection probe for more accurate preview lighting
        /// </summary>
        /// <param name="refProbeObj"></param>
        /// <param name="deriveObj"></param>
        /// <returns></returns>
        public static GameObject AddRefProbe(GameObject refProbeObj, GameObject deriveObj)
        {
            refProbeObj = new GameObject("Derive Reflection Probe");
            refProbeObj.transform.SetParent(deriveObj.transform);

            refProbeObj.layer = LayerMask.NameToLayer("Derive");

            ReflectionProbe refProbe = refProbeObj.AddComponent<ReflectionProbe>();
            refProbe.mode = ReflectionProbeMode.Realtime;
            refProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            refProbe.boxProjection = true;
            refProbe.clearFlags = ReflectionProbeClearFlags.SolidColor;
            refProbe.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.25f);

            refProbe.cullingMask = 1 >> LayerMask.NameToLayer("Nothing");
            refProbe.cullingMask ^= 1 << LayerMask.NameToLayer("Derive");

            return refProbeObj;
        }

        /// <summary>
        /// Adds a light that lightens the flash light gizmo
        /// This light cannot be rotated
        /// </summary>
        /// <param name="gizmoLightObj"></param>
        /// <param name="deriveObj"></param>
        /// <returns></returns>
        public static GameObject AddGizmoLight(GameObject gizmoLightObj, GameObject deriveObj)
        {
            gizmoLightObj = new GameObject("Derive Gizmo Light");
            gizmoLightObj.transform.SetParent(deriveObj.transform);
            gizmoLightObj.layer = LayerMask.NameToLayer("Derive Gizmos");

            Light gizmoLight = gizmoLightObj.AddComponent<Light>();
            gizmoLight.type = LightType.Directional;
            gizmoLight.color = new Color(1, 0.95f, 0.85f, 1);
            gizmoLight.shadows = LightShadows.Soft;

            gizmoLight.cullingMask = 1 >> LayerMask.NameToLayer("Nothing");
            gizmoLight.cullingMask ^= 1 << LayerMask.NameToLayer("Derive Gizmos");

            return gizmoLightObj;
        }

        /// <summary>
        /// Adds a render object with a plane as mesh
        /// </summary>
        /// <param name="renderObj"></param>
        /// <param name="deriveObj"></param>
        /// <param name="renderObjRotation"></param>
        /// <returns></returns>
        public static GameObject AddRenderObject(GameObject renderObj, GameObject deriveObj, Vector3 renderObjRotation)
        {
            renderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            renderObj.name = "Derive Render Object";
            renderObj.transform.SetParent(deriveObj.transform);
            renderObj.layer = LayerMask.NameToLayer("Derive");
            renderObj.GetComponent<MeshFilter>().sharedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/plane.fbx", typeof(Mesh));

            Collider.DestroyImmediate(renderObj.GetComponent<Collider>());

            if (renderObjRotation == Vector3.zero) renderObjRotation = new Vector3(-20, 10, 0);

            renderObj.transform.eulerAngles = renderObjRotation;

            return renderObj;
        }

        /// <summary>
        /// Adds a gizmo object with the flashlight mesh
        /// The gizmo is rendered in the bottom right on the preview texture
        /// </summary>
        /// <param name="gizmoFlashLight"></param>
        /// <param name="deriveObj"></param>
        /// <param name="lightObjRotation"></param>
        /// <returns></returns>
        public static GameObject AddGizmo(GameObject gizmoFlashLight, GameObject deriveObj, Vector3 lightObjRotation)
        {
            gizmoFlashLight = GameObject.Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(DD_EditorUtils.GetDerivePath() + "Resources/Editor/Models/flashLightObj.prefab", typeof(GameObject)));

            gizmoFlashLight.name = "Derive Gizmo Flashlight";
            gizmoFlashLight.transform.SetParent(deriveObj.transform);
            gizmoFlashLight.transform.localEulerAngles = lightObjRotation + new Vector3(90, 0, 0);
            gizmoFlashLight.layer = LayerMask.NameToLayer("Derive Gizmos");
            gizmoFlashLight.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Derive Gizmos");
            gizmoFlashLight.transform.GetChild(0).localEulerAngles = new Vector3(90, 0, 0);

            return gizmoFlashLight;
        }

        /// <summary>
        /// Checks if the layers required for rendering the preview exist (Derive & Derive Gizmos)
        /// If they don't exist, they are added
        /// The layers are culled from all lights and cameras, including the scene camera
        /// Only the preview cameras and lights operate on these layers
        /// The layer mask is restored when the editor is closed.
        /// </summary>
        /// <param name="maxLayers"></param>
        /// <param name="renderObjCamera"></param>
        /// <param name="gizmoObjCamera"></param>
        /// <param name="renderObjLight"></param>
        /// <param name="gizmoObjLight"></param>
        public static void RunLayerControl(int maxLayers, Camera renderObjCamera, Camera gizmoObjCamera, Light renderObjLight, Light gizmoObjLight, ReflectionProbe previewRefProbe)
        {
            AddLayer("Derive", maxLayers);
            AddLayer("Derive Gizmos", maxLayers);

            LayerMask tempMask = new LayerMask();
            tempMask.value = Tools.visibleLayers;
            tempMask &= ~(1 << LayerMask.NameToLayer("Derive"));
            tempMask &= ~(1 << LayerMask.NameToLayer("Derive Gizmos"));
            Tools.visibleLayers = tempMask.value;

            Camera[] sceneCameras = MonoBehaviour.FindObjectsByType<Camera>(FindObjectsSortMode.None);

            for (int i = 0; i < sceneCameras.Length; i++)
            {
                if (sceneCameras[i] != renderObjCamera)
                    sceneCameras[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive"));

                if (sceneCameras[i] != gizmoObjCamera)
                    sceneCameras[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive Gizmos"));
            }


            Light[] sceneLights = MonoBehaviour.FindObjectsByType<Light>(FindObjectsSortMode.None);

            for (int i = 0; i < sceneLights.Length; i++)
            {
                if (sceneLights[i] != renderObjLight)
                    sceneLights[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive"));

                if (sceneLights[i] != gizmoObjLight)
                    sceneLights[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive Gizmos"));
            }

            ReflectionProbe[] sceneRefProbes = MonoBehaviour.FindObjectsByType<ReflectionProbe>(FindObjectsSortMode.None);

            for (int i = 0; i < sceneRefProbes.Length; i++)
            {
                if (sceneRefProbes[i] != previewRefProbe)
                {
                    sceneRefProbes[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive"));
                    sceneRefProbes[i].cullingMask &= ~(1 << LayerMask.NameToLayer("Derive Gizmos"));
                }
            }
        }

        /// <summary>
        /// Adds a layer to the layer array stored in the project settings
        /// </summary>
        /// <param name="layerName"></param>
        public static void AddLayer(string layerName, int maxLayers)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layersProp = tagManager.FindProperty("layers");

            if (!PropertyExists(layersProp, 0, maxLayers, layerName))
            {
                SerializedProperty property;

                for (int i = 8; i < maxLayers; i++)
                {
                    property = layersProp.GetArrayElementAtIndex(i);

                    if (property.stringValue == "")
                    {
                        property.stringValue = layerName;
                        tagManager.ApplyModifiedProperties();
                        return;
                    }
                    if (i == 7) EditorUtility.DisplayDialog("Preview Error", "Can't add 'Derive' Layer. Preview will not be available. Please make sure, you have at least one unnamed free layer", "OK");
                }
            }
        }

        /// <summary>
        /// Checks if a serialised property in an array exists
        /// </summary>
        /// <param name="property"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool PropertyExists(SerializedProperty property, int start, int end, string value)
        {
            for (int i = start; i < end; i++)
            {
                SerializedProperty t = property.GetArrayElementAtIndex(i);

                if (t.stringValue.Equals(value)) return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a layer from the layer array stored in the project settings
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="maxLayers"></param>
        public static void RemoveLayer(string layerName, int maxLayers)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layersProp = tagManager.FindProperty("layers");

            if (PropertyExists(layersProp, 0, maxLayers, layerName))
            {
                SerializedProperty property;

                for (int i = 8; i < maxLayers; i++)
                {
                    property = layersProp.GetArrayElementAtIndex(i);

                    if (property.stringValue == layerName)
                    {
                        property.stringValue = "";
                        tagManager.ApplyModifiedProperties();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the master node from the project's node list
        /// </summary>
        /// <returns></returns>
        public static DD_NodeBase GetMasterNode()
        {
            if (DD_EditorUtils.currentProject.m_nodes != null)
                for (int i = 0; i < DD_EditorUtils.currentProject.m_nodes.Count; i++)
                    if (DD_EditorUtils.currentProject.m_nodes[i].m_nodeType == NodeType.Master)
                    {
                        return DD_EditorUtils.currentProject.m_nodes[i];
                    }

            return null;
        }
    }
}
#endif
