using UnityEngine;

namespace ServiceArchitecture.World.Controller
{
    // An interface for any prop controller to unify interaction.
    public interface IPropController
    {
        void OnInteract(Transform interactor);
    }
}