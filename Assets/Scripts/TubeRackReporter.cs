using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TubeRackReporter : MonoBehaviour
{
    private XRSocketInteractor[] sockets;

    private void Awake()
    {
        sockets = GetComponentsInChildren<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.AddListener(OnTubePlaced);
        }
    }

    private void OnDisable()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnTubePlaced);
        }
    }

    private void OnTubePlaced(SelectEnterEventArgs args)
    {
        LabEvents.Report(LabTask.PlaceTubeInRack);
    }
}
