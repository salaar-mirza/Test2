using ServiceArchitecture.Core;
using ServiceArchitecture.Simulation;
using ServiceArchitecture.Simulation.Events;
using ServiceArchitecture.World.Data;
using ServiceArchitecture.World.View;
using ServiceArchitecture.World.Events;
using UnityEngine;

namespace ServiceArchitecture.World.Controller
{
    public class PhysicsPropController : IPropController
    {
        private readonly PhysicsPropView _view;
        private bool _isHeld;
        private bool _isHeated;
        private Transform _hand;

        public PhysicsPropController(PhysicsPropConfig config, PhysicsPropView view)
        {
            _view = view;
            _view.Rb.mass = config.mass;
            _view.Rb.linearDamping = config.drag;
            // Subscribe to all relevant world events
            EventBus<WorkstationCollisionEvent>.Subscribe(OnWorkstationCollision);
            EventBus<CoolingBucketTriggerEvent>.Subscribe(OnCoolingBucketCollision);
        }
        public void Dispose()
        {
            // Unsubscribe from all events to prevent memory leaks
            EventBus<WorkstationCollisionEvent>.Unsubscribe(OnWorkstationCollision);
            EventBus<CoolingBucketTriggerEvent>.Unsubscribe(OnCoolingBucketCollision);
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
        
         
        private void OnWorkstationCollision(WorkstationCollisionEvent payload)
        {
            // Ignore the event if it wasn't this object that collided or if it's already heated
            if (payload.CollidingObject != _view.gameObject || _isHeated) return;
 
            // Safety Gate: Check the SOP state from the service.
            var sopService = GameService.Get<SOP_Service>();
            if (sopService.CurrentState == ProcedureState.ReadyToWork)
            {
                _isHeated = true;
                _view.GetComponent<MeshRenderer>().material.color = Color.red;
                EventBus<PropHeatedEvent>.Publish(new PropHeatedEvent());
                Debug.Log("<color=orange>Prop has been heated!</color>");
            }
            else
            {
                Debug.LogWarning("Cannot heat prop: PPE not equipped!");
            }
        }
        
         
        private void OnCoolingBucketCollision(CoolingBucketTriggerEvent payload)
        {
            // Ignore if it's not this object or if it's not heated
            if (payload.CollidingObject != _view.gameObject || !_isHeated) return;
 
            _isHeated = false;
            Debug.Log("<color=cyan>Prop has been cooled!</color>");
 
            // Announce that the prop has been cooled
            EventBus<PropCooledEvent>.Publish(new PropCooledEvent());
 
            Dispose(); // Clean up event subscriptions
            Object.Destroy(_view.gameObject); // Destroy the view object
        }
        
    }
}