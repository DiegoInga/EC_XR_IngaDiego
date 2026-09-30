using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public static class ECLabProps
{
    public static readonly Vector3 BenchPosition = new Vector3(0f, 0.45f, 1.5f);
    public static readonly Vector3 BenchScale = new Vector3(2.4f, 0.9f, 0.9f);

    private const float BenchTopThickness = 0.04f;
    private const float BurnerLightRange = 4f;
    private const float BurnerLightIntensity = 2f;
    private const float FlameGlow = 4f;
    private const float ShelfWallOffset = 4.75f;
    private const float RackOffsetX = 0.75f;
    private const float RackBaseHeight = 0.04f;
    private const float SocketHeight = 0.09f;
    private const float SocketRadius = 0.08f;
    private static readonly float[] RackSlots = { -0.15f, 0f, 0.15f };

    private static readonly Color FlameColor = new Color(1f, 0.55f, 0.1f);
    private static readonly Vector3 BurnerStandPosition = new Vector3(-2.2f, 0.45f, 2.5f);
    private static readonly Vector3 BurnerStandScale = new Vector3(0.8f, 0.9f, 0.8f);
    private static readonly Vector3 ReactorPosition = new Vector3(2.2f, 0f, 2.5f);
    private static readonly float[] ShelfHeights = { 1.2f, 1.7f };
    private static readonly float[] ShelfSlots = { -0.6f, 0f, 0.6f };

    public static float BenchTopY => BenchPosition.y + BenchScale.y / 2f + BenchTopThickness;

    public static void BuildFurniture(Transform parent)
    {
        Material benchMaterial = ECScenePrimitives.GetOrCreateMaterial("BenchBase", new Color(0.2f, 0.25f, 0.3f));
        Material topMaterial = ECScenePrimitives.GetOrCreateMaterial("BenchTop", new Color(0.92f, 0.94f, 0.95f));
        Material cabinetMaterial = ECScenePrimitives.GetOrCreateMaterial("SafetyCabinet", new Color(1f, 0.8f, 0.1f));

        ECScenePrimitives.Create(PrimitiveType.Cube, "Lab Bench", BenchPosition, BenchScale, parent, benchMaterial);
        Vector3 topPosition = new Vector3(BenchPosition.x, BenchTopY - BenchTopThickness / 2f, BenchPosition.z);
        Vector3 topScale = new Vector3(BenchScale.x + 0.1f, BenchTopThickness, BenchScale.z + 0.1f);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Lab Bench Top", topPosition, topScale, parent, topMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Burner Stand", BurnerStandPosition, BurnerStandScale, parent, benchMaterial);
        ECScenePrimitives.Create(PrimitiveType.Cube, "Safety Cabinet", new Vector3(3.9f, 1f, -3.5f), new Vector3(1f, 2f, 0.6f), parent, cabinetMaterial);
        BuildShelf(parent, benchMaterial);
    }

    public static GameObject BuildGlassware(Transform parent)
    {
        float y = BenchTopY;
        float z = BenchPosition.z;
        Material beaker = ECScenePrimitives.GetOrCreateTransparentMaterial("GlassBeaker", new Color(0.4f, 0.8f, 1f, 0.55f));
        Material tube = ECScenePrimitives.GetOrCreateTransparentMaterial("GlassTestTube", new Color(0.3f, 1f, 0.5f, 0.6f));
        Material flask = ECScenePrimitives.GetOrCreateTransparentMaterial("GlassFlask", new Color(1f, 0.4f, 0.7f, 0.6f));
        ECScenePrimitives.CreateGrabbable(PrimitiveType.Cylinder, "Beaker (Grab)", new Vector3(-0.7f, y + 0.09f, z), new Vector3(0.12f, 0.08f, 0.12f), parent, beaker);
        ECScenePrimitives.CreateGrabbable(PrimitiveType.Cylinder, "Test Tube (Grab)", new Vector3(-0.3f, y + 0.1f, z), new Vector3(0.04f, 0.09f, 0.04f), parent, tube);
        return ECScenePrimitives.CreateGrabbable(PrimitiveType.Sphere, "Flask (Grab)", new Vector3(0.1f, y + 0.09f, z), Vector3.one * 0.16f, parent, flask);
    }

    public static Light BuildBurner(Transform parent)
    {
        Material metal = ECScenePrimitives.GetOrCreateMaterial("BurnerMetal", new Color(0.6f, 0.62f, 0.65f));
        Material flameMaterial = ECScenePrimitives.GetOrCreateEmissiveMaterial("BurnerFlame", FlameColor, FlameGlow);

        GameObject burner = new GameObject("Bunsen Burner (Ray)");
        burner.transform.SetParent(parent, false);
        burner.transform.localPosition = new Vector3(BurnerStandPosition.x, BurnerStandPosition.y + BurnerStandScale.y / 2f, BurnerStandPosition.z);
        BoxCollider hitArea = burner.AddComponent<BoxCollider>();
        hitArea.center = new Vector3(0f, 0.2f, 0f);
        hitArea.size = new Vector3(0.3f, 0.4f, 0.3f);

        GameObject baseDisk = ECScenePrimitives.CreateVisual(PrimitiveType.Cylinder, "Base", new Vector3(0f, 0.03f, 0f), new Vector3(0.18f, 0.03f, 0.18f), burner.transform, metal);
        ECScenePrimitives.CreateVisual(PrimitiveType.Cylinder, "Tube", new Vector3(0f, 0.15f, 0f), new Vector3(0.05f, 0.1f, 0.05f), burner.transform, metal);
        GameObject flame = ECScenePrimitives.CreateVisual(PrimitiveType.Capsule, "Flame", new Vector3(0f, 0.3f, 0f), new Vector3(0.06f, 0.08f, 0.06f), burner.transform, flameMaterial);

        GameObject lightObject = new GameObject("Flame Light");
        lightObject.transform.SetParent(burner.transform, false);
        lightObject.transform.localPosition = new Vector3(0f, 0.35f, 0f);
        Light flameLight = lightObject.AddComponent<Light>();
        flameLight.type = LightType.Point;
        flameLight.color = FlameColor;
        flameLight.range = BurnerLightRange;
        flameLight.intensity = BurnerLightIntensity;
        flameLight.enabled = false;

        burner.AddComponent<XRSimpleInteractable>();
        burner.AddComponent<RayLightSwitch>().Configure(flameLight, baseDisk.GetComponent<Renderer>(), flame);
        return flameLight;
    }

    public static void BuildReactor(Transform parent)
    {
        Material glass = ECScenePrimitives.GetOrCreateTransparentMaterial("ReactorGlass", new Color(0.8f, 0.95f, 1f, 0.25f));
        Material liquid = ECScenePrimitives.GetOrCreateMaterial("ReactorLiquid", new Color(0.2f, 0.9f, 0.4f));
        Material metal = ECScenePrimitives.GetOrCreateMaterial("BurnerMetal", new Color(0.6f, 0.62f, 0.65f));

        GameObject reactor = new GameObject("Chemical Reactor");
        reactor.transform.SetParent(parent, false);
        reactor.transform.localPosition = ReactorPosition;
        ECScenePrimitives.Create(PrimitiveType.Cube, "Reactor Base", new Vector3(0f, 0.02f, 0f), new Vector3(0.7f, 0.04f, 0.7f), reactor.transform, metal);
        ECScenePrimitives.CreateVisual(PrimitiveType.Cylinder, "Reactor Glass", new Vector3(0f, 0.64f, 0f), new Vector3(0.5f, 0.6f, 0.5f), reactor.transform, glass);

        GameObject liquidObject = ECScenePrimitives.Create(PrimitiveType.Cylinder, "Reactor Liquid (Ray)", new Vector3(0f, 0.41f, 0f), new Vector3(0.44f, 0.35f, 0.44f), reactor.transform, liquid);
        liquidObject.AddComponent<XRSimpleInteractable>();
        liquidObject.AddComponent<RayColorChanger>();
        liquidObject.AddComponent<ReactorMixer>();
    }

    public static void BuildTubeRack(Transform parent)
    {
        Material rackMaterial = ECScenePrimitives.GetOrCreateMaterial("TubeRack", new Color(0.55f, 0.35f, 0.2f));
        Vector3 rackCenter = new Vector3(RackOffsetX, BenchTopY, BenchPosition.z);

        GameObject rack = new GameObject("Test Tube Rack");
        rack.transform.SetParent(parent, false);
        rack.transform.localPosition = rackCenter;
        rack.AddComponent<TubeRackReporter>();
        ECScenePrimitives.Create(PrimitiveType.Cube, "Rack Base", new Vector3(0f, RackBaseHeight / 2f, 0f), new Vector3(0.5f, RackBaseHeight, 0.14f), rack.transform, rackMaterial);

        Material slotMaterial = ECScenePrimitives.GetOrCreateMaterial("TubeSlotMarker", new Color(0.1f, 0.1f, 0.12f));
        foreach (float slotX in RackSlots)
        {
            ECScenePrimitives.CreateVisual(PrimitiveType.Cylinder, "Slot Marker", new Vector3(slotX, RackBaseHeight + 0.005f, 0f), new Vector3(0.08f, 0.01f, 0.08f), rack.transform, slotMaterial);
            GameObject slot = new GameObject("Tube Slot (Socket)");
            slot.transform.SetParent(rack.transform, false);
            slot.transform.localPosition = new Vector3(slotX, RackBaseHeight + SocketHeight, 0f);
            SphereCollider zone = slot.AddComponent<SphereCollider>();
            zone.isTrigger = true;
            zone.radius = SocketRadius;
            slot.AddComponent<XRSocketInteractor>();
        }
    }

    private static void BuildShelf(Transform parent, Material boardMaterial)
    {
        Color[] bottleColors = { new Color(0.9f, 0.3f, 0.3f, 0.7f), new Color(0.3f, 0.5f, 1f, 0.7f), new Color(0.9f, 0.8f, 0.2f, 0.7f) };
        for (int level = 0; level < ShelfHeights.Length; level++)
        {
            float boardY = ShelfHeights[level];
            ECScenePrimitives.Create(PrimitiveType.Cube, "Shelf Board " + level, new Vector3(-ShelfWallOffset, boardY, 0f), new Vector3(0.4f, 0.04f, 2f), parent, boardMaterial);
            for (int slot = 0; slot < ShelfSlots.Length; slot++)
            {
                Material bottle = ECScenePrimitives.GetOrCreateTransparentMaterial("Bottle" + slot, bottleColors[slot]);
                Vector3 position = new Vector3(-ShelfWallOffset, boardY + 0.12f, ShelfSlots[slot]);
                ECScenePrimitives.Create(PrimitiveType.Cylinder, "Bottle " + level + "-" + slot, position, new Vector3(0.1f, 0.1f, 0.1f), parent, bottle);
            }
        }
    }
}
