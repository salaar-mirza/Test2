using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabSimulator : MonoBehaviour
{
   public float grabDistance = 10f;
   public float mouseSensitivity = .2f;
   public LayerMask interactLayer = ~0;
   
   private IIntractable _currentintractable;
   private IHovarabel _currentHovered;

   private float _CameraPitch = 0f;
   private float _CameraYaw = 0f;

   private Vector3 _previousPosition;
   private Vector3 _handVelocity;

   private void Update()
   {
       if (Mouse.current == null) return;

       Vector2 mouseDelta = Mouse.current.delta.ReadValue();
       _CameraYaw += mouseDelta.x * mouseSensitivity;
       _CameraPitch -= mouseDelta.y * mouseSensitivity;

       _CameraPitch = Mathf.Clamp(_CameraPitch, -90f, 90f);
       transform.localRotation = Quaternion.Euler(_CameraPitch, _CameraYaw, 0f);

       Debug.DrawRay(transform.position, transform.forward * grabDistance, Color.red);

       if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hoverHit, grabDistance, interactLayer))
       {
           IHovarabel hoverable = hoverHit.collider.GetComponentInParent<IHovarabel>();
           if (hoverable != null)
           {
               if (_currentHovered != hoverable)
               {
                   if (_currentHovered != null) _currentHovered.OnHoverExit();
                   _currentHovered = hoverable;
                   _currentHovered.OnHoverEnter();

               }
              

           }
           else if (_currentHovered != null)
           {
               _currentHovered.OnHoverExit();
               _currentHovered = null;
           }
       }
       else if (_currentHovered != null)
       {
           _currentHovered.OnHoverExit();
           _currentHovered = null;
       }


       if (Mouse.current.leftButton.wasPressedThisFrame)
       {
          
           if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDistance, interactLayer))
           {
               Debug.Log($"[GrabSimulator] Raycast hit: {hit.collider.gameObject.name}");
               
               IIntractable intractable = hit.collider.GetComponentInParent<IIntractable>();
               if (intractable != null)
               {
                   _currentintractable = intractable;

                   _currentintractable.OnGrab(transform);

                   _previousPosition = transform.position + transform.forward * 2f;
               }
               else
               {
                   Debug.LogWarning($"[GrabSimulator] {hit.collider.gameObject.name} does not have an IIntractable component!");
               }
           }
           else
           {
               Debug.LogWarning("[GrabSimulator] Raycast did not hit anything within grabDistance.");
           }
       }


       if (_currentintractable != null)
       {
           Vector3 currentpos = transform.position +  transform.forward * 2f ;
           _handVelocity = (currentpos - _previousPosition) / Time.deltaTime; 
           _previousPosition = currentpos;
       }


       if (Mouse.current.leftButton.wasReleasedThisFrame && _currentintractable != null)
       {
           _currentintractable.OnRelease(_handVelocity);
           _currentintractable = null;
       }



   }

   private void OnGUI()
   {
       GUIStyle style = new GUIStyle();
       style.fontSize = 30;
       style.normal.textColor = Color.red;
       
       GUI.Label(new Rect(Screen.width / 2f - 10f, Screen.height /2f - 15f,20,20), "+",style);
   }



}
