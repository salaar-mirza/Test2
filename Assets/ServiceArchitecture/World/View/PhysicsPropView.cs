using ServiceArchitecture.Interaction;
using ServiceArchitecture.World.Controller;
using UnityEngine;

namespace ServiceArchitecture.World.View
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PhysicsPropView : MonoBehaviour, IInteractable
    {
        public Rigidbody Rb { get; private set; }
        
        private IPropController _controller;

        public void Initialize(IPropController controller) => _controller = controller;

        private void Awake() => Rb = GetComponent<Rigidbody>();

        // The View's only job is to forward the interaction to its Controller.
        public void OnInteract(Transform interactor) => _controller?.OnInteract(interactor);
    }
}