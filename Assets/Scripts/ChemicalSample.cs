using UnityEngine;

public class ChemicalSample : MonoBehaviour
{
    [SerializeField] private Color sampleColor = Color.white;

    public Color SampleColor => sampleColor;

    public void Configure(Color color)
    {
        sampleColor = color;
    }
}
