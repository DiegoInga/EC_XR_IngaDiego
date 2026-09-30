using System.IO;
using UnityEditor;
using UnityEngine;
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
