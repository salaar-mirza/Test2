using UnityEngine;

/// <summary>
/// Listens for raw player input and publishes abstract game events.
/// It does not know what the actions will do.
/// </summary>
public class InputService : IService, ITickable
{
    public void OnTick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventBus<PrimaryActionStartedEvent>.Publish(new PrimaryActionStartedEvent());
        }
    }
}