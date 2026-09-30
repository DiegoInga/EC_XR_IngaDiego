using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RayColorChanger : MonoBehaviour
{
    [SerializeField] private Color[] palette = { Color.red, Color.green, Color.blue, Color.magenta, Color.cyan };

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
