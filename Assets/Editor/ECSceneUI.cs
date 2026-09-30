using UnityEditor.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public static class ECSceneUI
{
    private const string BuiltinFontName = "LegacyRuntime.ttf";
    private const float CanvasScale = 0.005f;
    private const int TitleFontSize = 28;
    private const int BodyFontSize = 24;

    private static readonly Vector2 PanelSize = new Vector2(400f, 220f);
    private static readonly Vector2 ButtonSize = new Vector2(260f, 60f);
    private static readonly Vector3 PanelPosition = new Vector3(0f, 1.6f, 3.5f);
    private static readonly Vector3 SpawnPointPosition = new Vector3(0f, 1.3f, 1.5f);
    private static readonly Color PanelColor = new Color(0.1f, 0.1f, 0.15f, 0.85f);

    public static void BuildSpawnerPanel(Transform parent)
    {
        GameObject canvasObject = new GameObject("Spawner Panel (UI)", typeof(RectTransform));
        canvasObject.transform.SetParent(parent, false);
        canvasObject.transform.localPosition = PanelPosition;
        canvasObject.transform.localScale = Vector3.one * CanvasScale;
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<TrackedDeviceGraphicRaycaster>();
        canvasObject.GetComponent<RectTransform>().sizeDelta = PanelSize;
        canvasObject.AddComponent<Image>().color = PanelColor;

        CreateText(canvasObject.transform, "Title", "XR Training Room", TitleFontSize, new Vector2(0f, 70f));
        Text counter = CreateText(canvasObject.transform, "Counter", "Spawned objects: 0", BodyFontSize, new Vector2(0f, 25f));
        Button button = CreateButton(canvasObject.transform, "Spawn Button", "Spawn object", new Vector2(0f, -50f));

        Transform spawnPoint = new GameObject("Spawn Point").transform;
        spawnPoint.SetParent(parent, false);
        spawnPoint.localPosition = SpawnPointPosition;

        GrabbableSpawner spawner = canvasObject.AddComponent<GrabbableSpawner>();
        spawner.Configure(spawnPoint, counter);
        UnityEventTools.AddPersistentListener(button.onClick, spawner.Spawn);
    }

    public static void EnsureEventSystem()
    {
        EventSystem eventSystem = Object.FindFirstObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
        }
        foreach (BaseInputModule module in eventSystem.GetComponents<BaseInputModule>())
        {
            if (!(module is XRUIInputModule))
            {
                Object.DestroyImmediate(module);
            }
        }
        if (eventSystem.GetComponent<XRUIInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<XRUIInputModule>();
        }
    }

    private static Text CreateText(Transform parent, string name, string content, int fontSize, Vector2 position)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform));
        textObject.transform.SetParent(parent, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(PanelSize.x, ButtonSize.y);
        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>(BuiltinFontName);
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        return text;
    }

    private static Button CreateButton(Transform parent, string name, string label, Vector2 position)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform));
        buttonObject.transform.SetParent(parent, false);
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = ButtonSize;
        buttonObject.AddComponent<Image>().color = new Color(0.2f, 0.6f, 0.3f);
        Button button = buttonObject.AddComponent<Button>();
        Text text = CreateText(buttonObject.transform, "Label", label, BodyFontSize, Vector2.zero);
        text.GetComponent<RectTransform>().sizeDelta = ButtonSize;
        return button;
    }
}
