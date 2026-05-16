using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Practice_Grabber : MonoBehaviour
{
    private float RayLength = 10f;
    private IPracticeGrabbable _grabbable;
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * RayLength, Color.red);

      TryGrab();


    }
    
    void TryGrab()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, RayLength))
            {
                // Try to find a grabbable component on the object we hit.
                IPracticeGrabbable foundGrabbable = hit.collider.GetComponent<IPracticeGrabbable>();
                if (foundGrabbable != null)
                {
                    _grabbable = foundGrabbable;
                    _grabbable.OnGrab(this.transform); // Only grab if we found something valid.
                }

            }
        }


        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(_grabbable != null)
            {
                _grabbable.OnRelease();
                _grabbable = null; // IMPORTANT: Clear the reference after releasing.
            }
        }
    }
    
    
}
 
