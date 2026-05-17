
namespace ServiceArchitecture.Simulation.Events
{
    public enum ProcedureState
    {
        AwaitingPPE,
        ReadyToWork,
        TaskComplete
    }

    /// <summary>
    /// Published when the overall simulation procedure state changes.
    /// </summary>
    public struct ProcedureStateChangedEvent
    {
        public ProcedureState NewState;
    }
}