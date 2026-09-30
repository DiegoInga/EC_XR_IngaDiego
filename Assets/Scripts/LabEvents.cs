using System;

public static class LabEvents
{
    public static event Action<LabTask> TaskCompleted;

    public static void Report(LabTask task)
    {
        TaskCompleted?.Invoke(task);
    }
}
