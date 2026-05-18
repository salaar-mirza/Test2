using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7.5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;

    private CharacterController _characterController;
    private float _xRotation = 0f;
    private Vector3 _verticalVelocity;

    private void Awake()
    {
        // Get the CharacterController component attached to this GameObject.
        _characterController = GetComponent<CharacterController>();

        // Lock and hide the cursor for a better first-person experience.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Separate logic into distinct methods for clarity and maintainability.
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        // Read mouse input delta. Using GetAxis is a simple way to support mouse and controller sticks.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate vertical rotation for the camera and clamp it to prevent flipping upside down.
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // Apply rotation: vertical look to the camera, horizontal look to the entire player body.
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        // Use Unity's legacy Input Manager axes for smooth, configurable input.
        float moveX = Input.GetAxis("Horizontal"); // A/D keys
        float moveZ = Input.GetAxis("Vertical");   // W/S keys

        // Calculate movement direction relative to the player's orientation.
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Use CharacterController.Move for collision-aware movement.
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Apply gravity.
        _verticalVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }
}
