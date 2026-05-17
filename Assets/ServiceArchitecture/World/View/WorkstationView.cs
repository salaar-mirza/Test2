using ServiceArchitecture.Core;
using ServiceArchitecture.World.Events;
using UnityEngine;

namespace ServiceArchitecture.World.View
{
    public class WorkstationView : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"[WorkstationView] Collision detected with {other.gameObject.name}");
            EventBus<WorkstationCollisionEvent>.Publish(new WorkstationCollisionEvent { CollidingObject = other.gameObject });
        }
    }
}