namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Message
{
    public class TaskProgressMessage
    {
        public TaskProgressMessage(string taskName, int currentTask, int taskCount)
        {
            TaskName = taskName;
            CurrentTask = currentTask;
            TaskCount = taskCount;
        }

        public string TaskName { get; }

        public int CurrentTask { get; }

        public int TaskCount { get; }
    }
}
