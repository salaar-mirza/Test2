using UnityEngine;
using ServiceArchitecture.Core;
using ServiceArchitecture.Interaction;
using ServiceArchitecture.Simulation.Events;

namespace ServiceArchitecture.Simulation
{
    
    /// <summary>
    /// A test object that represents a piece of PPE.
    /// </summary>
    public class Test_PPE : MonoBehaviour, IInteractable
    {
        public void OnInteract(Transform interactor)
        {
            EventBus<PPE_EquippedEvent>.Publish(new PPE_EquippedEvent());
            Destroy(gameObject);
        }
    }
}