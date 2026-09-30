using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(XROrigin))]
public class PlayAreaLimiter : MonoBehaviour
{
    [SerializeField] private Vector3 areaCenter = Vector3.zero;
    [SerializeField] private float halfExtent = 4.5f;
    [SerializeField] private float fallResetHeight = -2f;

    private Transform head;
    private Vector3 startPosition;

    public void Configure(Vector3 center, float extent)
    {
        areaCenter = center;
        halfExtent = extent;
    }

    private void Awake()
    {
        head = GetComponent<XROrigin>().Camera.transform;
        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (transform.position.y < fallResetHeight)
        {
            transform.position = startPosition;
            return;
        }
        transform.position += CorrectionFor(head.position);
    }

    private Vector3 CorrectionFor(Vector3 headPosition)
    {
        float clampedX = Mathf.Clamp(headPosition.x, areaCenter.x - halfExtent, areaCenter.x + halfExtent);
        float clampedZ = Mathf.Clamp(headPosition.z, areaCenter.z - halfExtent, areaCenter.z + halfExtent);
        return new Vector3(clampedX - headPosition.x, 0f, clampedZ - headPosition.z);
    }
}
