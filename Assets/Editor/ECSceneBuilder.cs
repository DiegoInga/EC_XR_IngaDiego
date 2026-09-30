using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public static class ECSceneBuilder
{
    public const string ScenePath = "Assets/Scenes/EC_XR_IngaDiego.unity";

    private const float RoomHalfSize = 5f;
    private const float WallHeight = 3f;
    private const float WallThickness = 0.2f;
    private const float PlayAreaMargin = 0.5f;
    private const float RoomLightRange = 12f;
    private const float RoomLightIntensity = 2.5f;
    private const float SunIntensity = 1f;
    private const float SimulatorRotateSensitivity = 0.6f;
    private const float SimulatorMouseTranslateSensitivity = 0.0015f;
    private const float SimulatorKeyboardTranslateSpeed = 0.5f;

    private static readonly Vector3 SunRotation = new Vector3(50f, -30f, 0f);
    private static readonly Vector3 RigStartPosition = new Vector3(0f, 0f, -1f);

    [MenuItem("EC XR/Build Scene")]
    public static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Transform environment = new GameObject("Environment").transform;
        Transform interactables = new GameObject("Interactables").transform;

        BuildLighting(environment);
        BuildRoom(environment);
        ECLabProps.BuildFurniture(environment);
        ECLabProps.BuildGlassware(interactables);
        ECLabProps.BuildBurner(interactables);
        ECLabProps.BuildReactor(interactables);
        ECSceneUI.BuildSpawnerPanel(interactables);
        ECSceneUI.BuildWallSign(environment);
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
        roomLight.color = new Color(0.95f, 0.97f, 1f);
    }

    private static void BuildRoom(Transform parent)
    {
        Material floorMaterial = ECScenePrimitives.GetOrCreateMaterial("LabFloor", new Color(0.78f, 0.8f, 0.82f));
        Material wallMaterial = ECScenePrimitives.GetOrCreateMaterial("LabWall", new Color(0.82f, 0.93f, 0.9f));

        GameObject floor = ECScenePrimitives.Create(PrimitiveType.Plane, "Floor", Vector3.zero, Vector3.one, parent, floorMaterial);
        floor.AddComponent<TeleportationArea>();

        float wallY = WallHeight / 2f;
        Vector3 wallX = new Vector3(RoomHalfSize * 2f, WallHeight, WallThickness);
        Vector3 wallZ = new Vector3(WallThickness, WallHeight, RoomHalfSize * 2f);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall North", new Vector3(0f, wallY, RoomHalfSize), wallX, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall South", new Vector3(0f, wallY, -RoomHalfSize), wallX, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall East", new Vector3(RoomHalfSize, wallY, 0f), wallZ, parent, wallMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Wall West", new Vector3(-RoomHalfSize, wallY, 0f), wallZ, parent, wallMaterial);
    }

    private static void BuildRig()
    {
        GameObject rig = ECScenePrimitives.InstantiatePrefab("XR Origin (XR Rig)");
        if (rig != null)
        {
            rig.transform.position = RigStartPosition;
            rig.AddComponent<PlayAreaLimiter>().Configure(Vector3.zero, RoomHalfSize - PlayAreaMargin);
        }
        GameObject simulator = ECScenePrimitives.InstantiatePrefab("XR Device Simulator");
        if (simulator != null)
        {
            ConfigureSimulator(simulator.GetComponent<XRDeviceSimulator>());
        }
        ECSceneUI.EnsureEventSystem();
    }

    private static void ConfigureSimulator(XRDeviceSimulator simulator)
    {
        simulator.mouseXRotateSensitivity = SimulatorRotateSensitivity;
        simulator.mouseYRotateSensitivity = SimulatorRotateSensitivity;
        simulator.mouseXTranslateSensitivity = SimulatorMouseTranslateSensitivity;
        simulator.mouseYTranslateSensitivity = SimulatorMouseTranslateSensitivity;
        simulator.keyboardXTranslateSpeed = SimulatorKeyboardTranslateSpeed;
        simulator.keyboardYTranslateSpeed = SimulatorKeyboardTranslateSpeed;
        simulator.keyboardZTranslateSpeed = SimulatorKeyboardTranslateSpeed;
        PrefabUtility.RecordPrefabInstancePropertyModifications(simulator);
    }

    private static void AddSceneToBuild()
    {
        var scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).ToList();
        scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}
