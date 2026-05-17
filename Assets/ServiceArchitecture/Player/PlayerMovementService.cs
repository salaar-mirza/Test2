using ServiceArchitecture.Core;
using UnityEngine;

namespace ServiceArchitecture.Player
{
    /// <summary>
    /// Manages the player's CharacterController for movement and camera for looking.
    /// </summary>
    public class PlayerMovementService : IService, ITickable
    {
        private readonly CharacterController _characterController;
        private readonly Transform _cameraTransform;
        private readonly float _speed;
        private readonly float _mouseSensitivity;
        private readonly float _gravity;

        private float _xRotation = 0f;

        public PlayerMovementService(CharacterController characterController, float speed, float sensitivity, float gravity)
        {
            _characterController = characterController;
            _cameraTransform = characterController.GetComponentInChildren<Camera>().transform;
            _speed = speed;
            _mouseSensitivity = sensitivity;
            _gravity = gravity;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnTick()
        {
            // --- Mouse Look ---
            float mouseX = UnityEngine.Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _characterController.transform.Rotate(Vector3.up * mouseX);

            // --- Movement ---
            float moveX = UnityEngine.Input.GetAxis("Horizontal"); // A/D
            float moveZ = UnityEngine.Input.GetAxis("Vertical");   // W/S

            Vector3 moveDirection = _characterController.transform.right * moveX + _characterController.transform.forward * moveZ;
            _characterController.Move(moveDirection * _speed * Time.deltaTime);

            // --- Gravity ---
            _characterController.SimpleMove(Vector3.zero); // SimpleMove applies gravity automatically
        }
    }
}