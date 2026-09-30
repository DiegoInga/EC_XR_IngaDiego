using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableSpawner : MonoBehaviour
{
    private const string CounterPrefix = "Spawned objects: ";
    private const float SpawnedObjectScale = 0.2f;
    private const float SpawnedObjectMass = 0.5f;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Text counterLabel;
    [SerializeField] private Color[] palette = { Color.red, Color.green, Color.blue, Color.yellow };

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
        GameObject spawned = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spawned.name = "SpawnedCube_" + spawnedCount;
        spawned.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        spawned.transform.localScale = Vector3.one * SpawnedObjectScale;
        spawned.GetComponent<Renderer>().material.color = palette[spawnedCount % palette.Length];

        Rigidbody body = spawned.AddComponent<Rigidbody>();
        body.mass = SpawnedObjectMass;
        spawned.AddComponent<XRGrabInteractable>();

        spawnedCount++;
        RefreshCounter();
    }

    private void RefreshCounter()
    {
        if (counterLabel != null)
        {
            counterLabel.text = CounterPrefix + spawnedCount;
        }
    }
}
