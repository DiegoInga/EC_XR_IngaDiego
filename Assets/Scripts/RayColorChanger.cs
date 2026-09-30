using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RayColorChanger : MonoBehaviour
{
    [SerializeField] private Color[] palette =
    {
        new Color(0.2f, 0.9f, 0.4f),
        new Color(0.9f, 0.2f, 0.6f),
        new Color(0.2f, 0.6f, 1f),
        new Color(1f, 0.6f, 0.1f),
        new Color(0.6f, 0.3f, 1f)
    };

    private XRSimpleInteractable interactable;
    private Renderer targetRenderer;
    private int currentIndex;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        targetRenderer = GetComponent<Renderer>();
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
        targetRenderer.material.color = palette[currentIndex];
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        NextColor();
    }

    public void NextColor()
    {
        currentIndex = (currentIndex + 1) % palette.Length;
        targetRenderer.material.color = palette[currentIndex];
    }
}
