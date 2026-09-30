using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FlaskHeater : MonoBehaviour
{
    [SerializeField] private Light flameLight;
    [SerializeField] private Transform flame;
    [SerializeField] private float heatRadius = 0.4f;
    [SerializeField] private float secondsToHeat = 3f;
    [SerializeField] private Color hotColor = new Color(1f, 0.35f, 0.1f, 0.85f);

    private Material flaskMaterial;
    private Color coldColor;
    private float heat;
    private bool reported;

    public void Configure(Light light, Transform flameTransform)
    {
        flameLight = light;
        flame = flameTransform;
    }

    private void Awake()
    {
        flaskMaterial = GetComponent<Renderer>().material;
        coldColor = flaskMaterial.color;
    }

    private void Update()
    {
        if (!IsOverLitFlame())
        {
            return;
        }
        heat = Mathf.Clamp01(heat + Time.deltaTime / secondsToHeat);
        flaskMaterial.color = Color.Lerp(coldColor, hotColor, heat);
        if (heat >= 1f && !reported)
        {
            reported = true;
            LabEvents.Report(LabTask.HeatFlask);
        }
    }

    private bool IsOverLitFlame()
    {
        return flameLight != null && flameLight.enabled && Vector3.Distance(transform.position, flame.position) <= heatRadius;
    }
}
