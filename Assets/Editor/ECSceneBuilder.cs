using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public static class ECSceneBuilder
{
    public const string ScenePath = "Assets/Scenes/EC_XR_IngaDiego.unity";

    private const float RoomHalfSize = 5f;
    private const float WallHeight = 3f;
    private const float WallThickness = 0.2f;
    private const float RoomLightRange = 12f;
    private const float RoomLightIntensity = 3f;
    private const float SunIntensity = 1f;

    private static readonly Vector3 SunRotation = new Vector3(50f, -30f, 0f);
    private static readonly Vector3 TablePosition = new Vector3(0f, 0.4f, 1.5f);
    private static readonly Vector3 TableScale = new Vector3(2f, 0.8f, 0.8f);
    private static readonly Vector3 SmallObjectScale = Vector3.one * 0.2f;
    private static readonly Vector3 ToolScale = new Vector3(0.08f, 0.15f, 0.08f);

    [MenuItem("EC XR/Build Scene")]
    public static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Transform environment = new GameObject("Environment").transform;
        Transform interactables = new GameObject("Interactables").transform;

        BuildLighting(environment);
        BuildRoom(environment);
        BuildGrabbables(interactables);
        Light roomLight = GameObject.Find("Room Light").GetComponent<Light>();
        BuildRayInteractables(interactables, roomLight);
        ECSceneUI.BuildSpawnerPanel(interactables);
        BuildRig();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuild();
        Debug.Log("EC XR scene built at " + ScenePath);
    }

    private static void BuildLighting(Transform parent)
    {
        GameObject sun = new GameObject("Directional Light");
        sun.transform.SetParent(parent, false);
        sun.transform.rotation = Quaternion.Euler(SunRotation);
        Light sunLight = sun.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.intensity = SunIntensity;
        sunLight.shadows = LightShadows.Soft;

        GameObject room = new GameObject("Room Light");
        room.transform.SetParent(parent, false);
        room.transform.localPosition = new Vector3(0f, WallHeight - 0.5f, 0f);
        Light roomLight = room.AddComponent<Light>();
        roomLight.type = LightType.Point;
        roomLight.range = RoomLightRange;
        roomLight.intensity = RoomLightIntensity;
        roomLight.color = new Color(1f, 0.9f, 0.7f);
    }

    private static void BuildRoom(Transform parent)
    {
        Material floorMaterial = ECScenePrimitives.GetOrCreateMaterial("Floor", new Color(0.35f, 0.35f, 0.4f));
        Material wallMaterial = ECScenePrimitives.GetOrCreateMaterial("Wall", new Color(0.75f, 0.8f, 0.85f));

        GameObject floor = ECScenePrimitives.Create(PrimitiveType.Plane, "Floor", Vector3.zero, Vector3.one, parent, floorMaterial);
        floor.AddComponent<TeleportationArea>();

        float wallY = WallHeight / 2f;
        Vector3 wallX = new Vector3(RoomHalfSize * 2f, WallHeight, WallThickness);
        Vector3 wallZ = new Vector3(WallThickness, WallHeight, RoomHalfSize * 2f);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall North", new Vector3(0f, wallY, RoomHalfSize), wallX, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall South", new Vector3(0f, wallY, -RoomHalfSize), wallX, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall East", new Vector3(RoomHalfSize, wallY, 0f), wallZ, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall West", new Vector3(-RoomHalfSize, wallY, 0f), wallZ, parent, wallMaterial);

        Material tableMaterial = ECScenePrimitives.GetOrCreateMaterial("Table", new Color(0.45f, 0.3f, 0.2f));
        ECScenePrimitives.Create(PrimitiveType.Cube, "Table", TablePosition, TableScale, parent, tableMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cylinder, "Pillar", new Vector3(-3f, 1f, -3f), new Vector3(0.5f, 1f, 0.5f), parent, wallMaterial);
    }

    private static void BuildGrabbables(Transform parent)
    {
        float topY = TablePosition.y + TableScale.y / 2f + SmallObjectScale.y;
        Material red = ECScenePrimitives.GetOrCreateMaterial("GrabRed", Color.red);
        Material blue = ECScenePrimitives.GetOrCreateMaterial("GrabBlue", Color.blue);
        Material yellow = ECScenePrimitives.GetOrCreateMaterial("GrabYellow", Color.yellow);
        ECScenePrimitives.CreateGrabbable(PrimitiveType.Cube, "Grab Cube", new Vector3(-0.6f, topY, TablePosition.z), SmallObjectScale, parent, red);
        ECScenePrimitives.CreateGrabbable(PrimitiveType.Sphere, "Grab Sphere", new Vector3(0f, topY, TablePosition.z), SmallObjectScale, parent, blue);
        ECScenePrimitives.CreateGrabbable(PrimitiveType.Cylinder, "Grab Tool", new Vector3(0.6f, topY, TablePosition.z), ToolScale, parent, yellow);
    }

    private static void BuildRayInteractables(Transform parent, Light roomLight)
    {
        Material switchMaterial = ECScenePrimitives.GetOrCreateMaterial("LightSwitch", Color.yellow);
        GameObject lightSwitch = ECScenePrimitives.Create(PrimitiveType.Cube, "Light Switch (Ray)", new Vector3(-2.5f, 1.3f, 3f), Vector3.one * 0.3f, parent, switchMaterial);
        lightSwitch.AddComponent<XRSimpleInteractable>();
        lightSwitch.AddComponent<RayLightSwitch>().Configure(roomLight, lightSwitch.GetComponent<Renderer>());

        Material colorMaterial = ECScenePrimitives.GetOrCreateMaterial("ColorChanger", Color.white);
        GameObject colorChanger = ECScenePrimitives.Create(PrimitiveType.Capsule, "Color Changer (Ray)", new Vector3(2.5f, 1f, 3f), Vector3.one * 0.5f, parent, colorMaterial);
        colorChanger.AddComponent<XRSimpleInteractable>();
        colorChanger.AddComponent<RayColorChanger>();
    }

    private static void BuildRig()
    {
        GameObject setup = ECScenePrimitives.InstantiatePrefab("XR Interaction Setup");
        if (setup == null)
        {
            setup = ECScenePrimitives.InstantiatePrefab("XR Origin (XR Rig)");
        }
        if (setup != null)
        {
            setup.transform.position = new Vector3(0f, 0f, -1f);
        }
        ECScenePrimitives.InstantiatePrefab("XR Device Simulator");
        ECSceneUI.EnsureEventSystem();
    }

    private static void AddSceneToBuild()
    {
        var scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).ToList();
        scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
