namespace WPF.RealTime.Infrastructure
{
    public enum TaskType
    {
        // base
        Background,
        //clock
        Periodic,
        //interrupt
        Sporadic,
        //UI
        LongRunning
    }
}
