using UnityEngine;

namespace ServiceArchitecture.World.Events
{
    /// <summary>
    /// Published when any object collides with a workstation.
    /// </summary>
    public struct WorkstationCollisionEvent
    {
        public GameObject CollidingObject;
    }
}