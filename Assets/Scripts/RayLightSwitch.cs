using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RayLightSwitch : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private Renderer indicator;
    [SerializeField] private GameObject[] toggledVisuals;
    [SerializeField] private Color onColor = Color.yellow;
    [SerializeField] private Color offColor = Color.gray;

    private XRSimpleInteractable interactable;

    public void Configure(Light light, Renderer indicatorRenderer, params GameObject[] visuals)
    {
        targetLight = light;
        indicator = indicatorRenderer;
        toggledVisuals = visuals;
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
        RefreshState();
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Toggle();
    }

    public void Toggle()
    {
        targetLight.enabled = !targetLight.enabled;
        RefreshState();
        if (targetLight.enabled)
        {
            LabEvents.Report(LabTask.LightBurner);
        }
    }

    private void RefreshState()
    {
        if (targetLight == null)
        {
            return;
        }
        foreach (GameObject visual in toggledVisuals ?? new GameObject[0])
        {
            visual.SetActive(targetLight.enabled);
        }
        if (indicator != null)
        {
            indicator.material.color = targetLight.enabled ? onColor : offColor;
        }
    }
}
