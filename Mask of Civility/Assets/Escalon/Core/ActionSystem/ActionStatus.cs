namespace Escalon
{
    /// <summary>
    /// The state of a sequence
    /// </summary>
    public enum ActionStatus
    {
        Pending,
        Running,
        Waiting,
        Cancelled,
        Complete
    }
}