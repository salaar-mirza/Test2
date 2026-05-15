using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed =3f;
    public Transform cameraTransform;

    private void Update()
    {
        if(Keyboard.current == null) return;

        float moveX = 0f;
        float moveZ = 0f;
        
        if(Keyboard.current.wKey.isPressed) moveZ += 1f;
        if(Keyboard.current.sKey.isPressed) moveZ -= 1f;
        if(Keyboard.current.aKey.isPressed) moveX -= 1f;
        if(Keyboard.current.dKey.isPressed) moveX += 1f;
        
        Vector3 inputDirection = new Vector3(moveX, 0f, moveZ).normalized;
        if (inputDirection.magnitude > 0.1f)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            
            Vector3 moveVector = (forward * inputDirection.z + right * inputDirection.x) * movespeed * Time.deltaTime;
            transform.position += moveVector;
        }


    }
}
