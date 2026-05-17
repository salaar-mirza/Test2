using UnityEngine;
using UnityEngine.InputSystem;

public class Practice_Grabber : MonoBehaviour
{
    public float grabDistance = 10f;
    private IPracticeGrabbable _heldObject;

    private void Update()
    {
        // This check handles cases where the held object is destroyed by another script.
        // Unity overloads the '==' operator, so a destroyed object will evaluate to null.
        if (_heldObject as Object == null)
        {
            _heldObject = null;
        }

        Debug.DrawRay(transform.position, transform.forward * grabDistance, Color.red);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_heldObject == null)
            {
                // Not holding anything, so try to grab.
                TryToGrab();
            }
            else
            {
                // Already holding something, so release it.
                _heldObject.OnRelease();
                _heldObject = null;
            }
        }
    }

    private void TryToGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            // Check if the hit object implements the grabbable interface.
            IPracticeGrabbable grabbable = hit.collider.GetComponent<IPracticeGrabbable>();
            if (grabbable != null)
            {
                _heldObject = grabbable;
                _heldObject.OnGrab(this.transform);
            }
        }
    }
}
