using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RayLightSwitch : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private Renderer indicator;
    [SerializeField] private Color onColor = Color.yellow;
    [SerializeField] private Color offColor = Color.gray;

    private XRSimpleInteractable interactable;

    public void Configure(Light light, Renderer indicatorRenderer)
    {
        targetLight = light;
        indicator = indicatorRenderer;
    }

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void Start()
    {
        RefreshIndicator();
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Toggle();
    }

    public void Toggle()
    {
        targetLight.enabled = !targetLight.enabled;
        RefreshIndicator();
    }

    private void RefreshIndicator()
    {
        if (indicator == null || targetLight == null)
        {
            return;
        }
        indicator.material.color = targetLight.enabled ? onColor : offColor;
    }
}
