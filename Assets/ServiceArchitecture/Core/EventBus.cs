using System;

namespace ServiceArchitecture.Core
{
    /// <summary>
    /// A global, static event bus for decoupled communication.
    /// Payloads should be structs to avoid GC allocation.
    /// </summary>
    public static class EventBus<T> where T : struct
    {
        private static event Action<T> OnEvent;

        public static void Subscribe(Action<T> handler)
        {
            OnEvent += handler;
        }

        public static void Unsubscribe(Action<T> handler)
        {
            OnEvent -= handler;
        }
        
        public static void Publish(T payload)
        {
            OnEvent?.Invoke(payload);
        }
    }
}