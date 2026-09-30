using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabMissionBoard : MonoBehaviour
{
    private const string PendingMark = "[  ]  ";
    private const string DoneMark = "[X]  ";
    private const string ProgressFormat = "Completed: {0} / {1}";
    private const string FinishedMessage = "All missions complete!";

    private static readonly Dictionary<LabTask, string> Descriptions = new Dictionary<LabTask, string>
    {
        { LabTask.LightBurner, "Light the Bunsen burner" },
        { LabTask.HeatFlask, "Heat the flask over the flame" },
        { LabTask.GenerateSample, "Generate a sample" },
        { LabTask.PlaceTubeInRack, "Place a tube in the rack" },
        { LabTask.MixSample, "Mix a sample in the reactor" }
    };

    [SerializeField] private Text[] taskLines;
    [SerializeField] private Text progressLabel;
    [SerializeField] private Color pendingColor = Color.white;
    [SerializeField] private Color doneColor = new Color(0.4f, 1f, 0.5f);

    private readonly HashSet<LabTask> completed = new HashSet<LabTask>();

    public void Configure(Text[] lines, Text progress)
    {
        taskLines = lines;
        progressLabel = progress;
    }

    private void OnEnable()
    {
        LabEvents.TaskCompleted += OnTaskCompleted;
    }

    private void OnDisable()
    {
        LabEvents.TaskCompleted -= OnTaskCompleted;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnTaskCompleted(LabTask task)
    {
        if (completed.Add(task))
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        foreach (LabTask task in Descriptions.Keys)
        {
            int index = (int)task;
            if (index >= taskLines.Length)
            {
                continue;
            }
            bool done = completed.Contains(task);
            taskLines[index].text = (done ? DoneMark : PendingMark) + Descriptions[task];
            taskLines[index].color = done ? doneColor : pendingColor;
        }
        bool finished = completed.Count == Descriptions.Count;
        progressLabel.text = finished ? FinishedMessage : string.Format(ProgressFormat, completed.Count, Descriptions.Count);
        progressLabel.color = finished ? doneColor : pendingColor;
    }
}
