using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableSpawner : MonoBehaviour
{
    private const string CounterPrefix = "Samples generated: ";
    private const string SpawnedNamePrefix = "Sample_";
    private const float SpawnedObjectMass = 0.3f;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Text counterLabel;
    [SerializeField] private Vector3 sampleScale = new Vector3(0.05f, 0.08f, 0.05f);
    [SerializeField] private Color[] palette =
    {
        new Color(0.2f, 0.9f, 0.4f),
        new Color(0.9f, 0.2f, 0.6f),
        new Color(0.2f, 0.6f, 1f),
        new Color(1f, 0.6f, 0.1f)
    };

    private int spawnedCount;

    public void Configure(Transform point, Text label)
    {
        spawnPoint = point;
        counterLabel = label;
    }

    private void Start()
    {
        RefreshCounter();
    }

    public void Spawn()
    {
        GameObject sample = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        sample.name = SpawnedNamePrefix + spawnedCount;
        sample.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        sample.transform.localScale = sampleScale;
        Color sampleColor = palette[spawnedCount % palette.Length];
        sample.GetComponent<Renderer>().material.color = sampleColor;
        sample.AddComponent<ChemicalSample>().Configure(sampleColor);

        Rigidbody body = sample.AddComponent<Rigidbody>();
        body.mass = SpawnedObjectMass;
        sample.AddComponent<XRGrabInteractable>();

        spawnedCount++;
        RefreshCounter();
        LabEvents.Report(LabTask.GenerateSample);
    }

    private void RefreshCounter()
    {
        if (counterLabel != null)
        {
            counterLabel.text = CounterPrefix + spawnedCount;
        }
    }
}
