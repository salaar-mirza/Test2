using UnityEngine;

// This attribute automatically adds a CharacterController component
// to any GameObject this script is attached to.
[RequireComponent(typeof(CharacterController))]
public class Practice_PlayerMovement : MonoBehaviour
{
    public float speed = 7.5f;
    public float mouseSensitivity = 100f;
    public float gravity = -9.81f;

    private CharacterController _characterController;
    private Transform _cameraTransform;

    private float _xRotation = 0f;
    private Vector3 _velocity;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform; // Assumes the main camera is the player's view

        // Lock and hide the cursor for a better first-person experience
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Clamp vertical rotation to prevent looking upside down
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // Apply rotation to the camera (for looking up/down) and the player body (for turning left/right)
        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D keys
        float moveZ = Input.GetAxis("Vertical");   // W/S keys

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        _characterController.Move(moveDirection * speed * Time.deltaTime);

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
    
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.red;
       
        GUI.Label(new Rect(Screen.width / 2f - 10f, Screen.height /2f - 15f,20,20), "+",style);
    }

    
}