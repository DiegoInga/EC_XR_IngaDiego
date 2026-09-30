using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Renderer))]
public class ReactorMixer : MonoBehaviour
{
    private Renderer liquidRenderer;

    private void Awake()
    {
        liquidRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionStay(Collision collision)
    {
        ChemicalSample sample = collision.collider.GetComponentInParent<ChemicalSample>();
        if (sample == null || IsHeld(sample))
        {
            return;
        }
        liquidRenderer.material.color = sample.SampleColor;
        Destroy(sample.gameObject);
    }

    private static bool IsHeld(ChemicalSample sample)
    {
        XRGrabInteractable grab = sample.GetComponent<XRGrabInteractable>();
        return grab != null && grab.isSelected;
    }
}
