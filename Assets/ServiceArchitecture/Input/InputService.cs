using UnityEngine;
using ServiceArchitecture.Core;
using ServiceArchitecture.Input.Events;

namespace ServiceArchitecture.Input
{
    /// <summary>
    /// Listens for raw player input and publishes abstract game events.
    /// It does not know what the actions will do.
    /// </summary>
    public class InputService : IService, ITickable
    {
        public void OnTick()
        {
            // Fully qualify UnityEngine.Input to resolve namespace ambiguity.
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                EventBus<PrimaryActionStartedEvent>.Publish(new PrimaryActionStartedEvent());
            }
        }
    }
}