using UnityEngine;

namespace ServiceArchitecture.Interaction
{
    /// <summary>
    /// Defines any object in the world that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        void OnInteract(Transform interactor);
    }
}