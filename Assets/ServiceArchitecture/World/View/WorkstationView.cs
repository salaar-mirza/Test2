using ServiceArchitecture.Core;
using ServiceArchitecture.World.Events;
using UnityEngine;

namespace ServiceArchitecture.World.View
{
    public class WorkstationView : MonoBehaviour
    {
        // Switched to OnTriggerEnter to detect kinematic rigidbodies
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[WorkstationView] Trigger detected with {other.gameObject.name}");
            EventBus<WorkstationCollisionEvent>.Publish(new WorkstationCollisionEvent { CollidingObject = other.gameObject });
        }
    }
}