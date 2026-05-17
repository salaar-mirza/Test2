using ServiceArchitecture.Core;
using ServiceArchitecture.World.Events;
using UnityEngine;

namespace ServiceArchitecture.World.View
{
    public class CoolingBucketView : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            EventBus<CoolingBucketTriggerEvent>.Publish(new CoolingBucketTriggerEvent { CollidingObject = other.gameObject });
        }
    }
}