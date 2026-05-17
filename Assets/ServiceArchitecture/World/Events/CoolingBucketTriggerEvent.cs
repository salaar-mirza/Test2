using UnityEngine;

namespace ServiceArchitecture.World.Events
{
    /// <summary>
    /// Published when any object enters a cooling bucket's trigger.
    /// </summary>
    public struct CoolingBucketTriggerEvent
    {
        public GameObject CollidingObject;
    }
}