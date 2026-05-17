namespace ServiceArchitecture.Core
{
    /// <summary>
    /// Represents a service that needs to be updated every frame.
    /// </summary>
    public interface ITickable
    {
        void OnTick();
    }
}