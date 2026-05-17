using UnityEngine;
using ServiceArchitecture.Core;
using ServiceArchitecture.Input.Events;

namespace ServiceArchitecture.Interaction
{
    /// <summary>
    /// Listens for action events and performs world interactions, like raycasting.
    /// </summary>
    public class InteractionService : IService
    {
        private readonly Camera _mainCamera;
        private readonly float _interactionDistance;

        // Dependencies are passed in via the constructor.
        public InteractionService(Camera mainCamera, float interactionDistance)
        {
            _mainCamera = mainCamera;
            _interactionDistance = interactionDistance;

            EventBus<PrimaryActionStartedEvent>.Subscribe(OnPrimaryAction);
        }

        private void OnPrimaryAction(PrimaryActionStartedEvent payload)
        {
            Debug.Log("Game Initializer: Primary action started.");
            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.blue);

            if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.OnInteract(_mainCamera.transform);
            }
        }
    }
}