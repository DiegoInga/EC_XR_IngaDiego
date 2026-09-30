using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public static class ECScenePrimitives
{
    private const string MaterialsFolder = "Assets/Materials";
    private const string LitShaderName = "Universal Render Pipeline/Lit";
    private const float DefaultGrabMass = 1f;

    public static GameObject Create(PrimitiveType type, string name, Vector3 position, Vector3 scale, Transform parent, Material material)
    {
        GameObject created = GameObject.CreatePrimitive(type);
        created.name = name;
        created.transform.SetParent(parent, false);
        created.transform.localPosition = position;
        created.transform.localScale = scale;
        if (material != null)
        {
            created.GetComponent<Renderer>().sharedMaterial = material;
        }
        return created;
    }

    public static GameObject CreateGrabbable(PrimitiveType type, string name, Vector3 position, Vector3 scale, Transform parent, Material material)
    {
        GameObject grabbable = Create(type, name, position, scale, parent, material);
        Rigidbody body = grabbable.AddComponent<Rigidbody>();
        body.mass = DefaultGrabMass;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        XRGrabInteractable grab = grabbable.AddComponent<XRGrabInteractable>();
        grab.throwOnDetach = true;
        return grabbable;
    }

    public static Material GetOrCreateMaterial(string name, Color color)
    {
        if (!AssetDatabase.IsValidFolder(MaterialsFolder))
        {
            AssetDatabase.CreateFolder("Assets", Path.GetFileName(MaterialsFolder));
        }
        string path = MaterialsFolder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            material = new Material(Shader.Find(LitShaderName));
            AssetDatabase.CreateAsset(material, path);
        }
        material.color = color;
        EditorUtility.SetDirty(material);
        return material;
    }

    public static Material GetOrCreateTransparentMaterial(string name, Color color)
    {
        Material material = GetOrCreateMaterial(name, color);
        material.SetFloat("_Surface", 1f);
        material.SetFloat("_Blend", 0f);
        material.SetFloat("_SrcBlend", (float)BlendMode.One);
        material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0f);
        material.SetOverrideTag("RenderType", "Transparent");
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        material.renderQueue = (int)RenderQueue.Transparent;
        EditorUtility.SetDirty(material);
        return material;
    }

    public static Material GetOrCreateEmissiveMaterial(string name, Color color, float intensity)
    {
        Material material = GetOrCreateMaterial(name, color);
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * intensity);
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        EditorUtility.SetDirty(material);
        return material;
    }

    public static GameObject CreateVisual(PrimitiveType type, string name, Vector3 position, Vector3 scale, Transform parent, Material material)
    {
        GameObject visual = Create(type, name, position, scale, parent, material);
        Object.DestroyImmediate(visual.GetComponent<Collider>());
        return visual;
    }

    public static GameObject FindPrefab(string exactName)
    {
        foreach (string guid in AssetDatabase.FindAssets(exactName + " t:Prefab"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileNameWithoutExtension(path) == exactName)
            {
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
        }
        return null;
    }

    public static GameObject InstantiatePrefab(string exactName)
    {
        GameObject prefab = FindPrefab(exactName);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + exactName + ". Import the XR Interaction Toolkit samples.");
            return null;
        }
        return (GameObject)PrefabUtility.InstantiatePrefab(prefab);
    }
}
