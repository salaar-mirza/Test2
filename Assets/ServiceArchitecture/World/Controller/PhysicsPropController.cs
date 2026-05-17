using ServiceArchitecture.World.Data;
using ServiceArchitecture.World.View;
using UnityEngine;

namespace ServiceArchitecture.World.Controller
{
    public class PhysicsPropController : IPropController
    {
        private readonly PhysicsPropView _view;
        private bool _isHeld;
        private Transform _hand;

        public PhysicsPropController(PhysicsPropConfig config, PhysicsPropView view)
        {
            _view = view;
            _view.Rb.mass = config.mass;
            _view.Rb.linearDamping = config.drag;
        }

        public void OnInteract(Transform interactor)
        {
            _isHeld = !_isHeld;
            if (_isHeld)
            {
                _hand = interactor;
                _view.Rb.isKinematic = true;
                _view.transform.SetParent(_hand);
            }
            else
            {
                _view.Rb.isKinematic = false;
                _view.transform.SetParent(null);
            }
        }
    }
}