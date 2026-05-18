using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabSimulator : MonoBehaviour
{
   public float grabDistance = 10f;
   public LayerMask interactLayer = ~0;
   
   private IIntractable _currentintractable;
   private IHovarabel _currentHovered;

   private Vector3 _previousPosition;
   private Vector3 _handVelocity;

   private void Update()
   {
       Debug.DrawRay(transform.position, transform.forward * grabDistance, Color.red);

       if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hoverHit, grabDistance, interactLayer))
       {
           // We hit something. Check if it's a hoverable object.
           IHovarabel hoverable = hoverHit.collider.GetComponentInParent<IHovarabel>();
           if (hoverable != null)
           {
               // Case 1: We are looking at a new hoverable object.w
               if (_currentHovered != hoverable)
               {
                   // If we were previously hovering over a different object, tell it to exit first.
                   if (_currentHovered != null) _currentHovered.OnHoverExit();
                   // Set the new object as the current one and tell it to enter.
                   _currentHovered = hoverable;
                   _currentHovered.OnHoverEnter();

               }
               // If we are still looking at the same object, do nothing.
           }
           else if (_currentHovered != null)
           {
               // Case 2: We hit something, but it's not hoverable (e.g., a wall).
               // Clear the previously hovered object.
               _currentHovered.OnHoverExit();
               _currentHovered = null;
           }
       }
       else if (_currentHovered != null)
       {
           // Case 3: The raycast hit nothing at all.
           // This is the critical fix for the "stuck hover" bug.
           // If we were previously hovering over an object, clear its state.
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
                   // Successfully found a grabbable object.
                   _currentintractable = intractable;

                   _currentintractable.OnGrab(transform);
                   // Initialize the position for velocity tracking.
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
           // CRITICAL FIX: Calculate velocity in units per SECOND, not units per FRAME.
           // This is done by dividing the distance traveled by the time it took (Time.deltaTime).
           // This ensures throwing physics are consistent on all computers, regardless of frame rate.
           _handVelocity = (currentpos - _previousPosition) / Time.deltaTime; 
           // Update the previous position for the next frame's calculation.
           _previousPosition = currentpos;
       }


       if (Mouse.current.leftButton.wasReleasedThisFrame && _currentintractable != null)
       {
           _currentintractable.OnRelease(_handVelocity);
           // Clear the reference to the held object.
           _currentintractable = null;
       }


   }
}
