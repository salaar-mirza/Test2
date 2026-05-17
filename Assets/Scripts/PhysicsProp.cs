using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProp : MonoBehaviour, IIntractable
{
    [Tooltip("The local position of the object when held by the player.")]
    public Vector3 heldPositionOffset = new Vector3(0f, 0f, 2f);

    [Tooltip("The maximum speed the object can be thrown at.")]
    public float maxThrowSpeed = 15f;
    
    private Rigidbody _rb;
    private Material _material;
    private Color _originalEmissionColor;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _material = GetComponent<Renderer>().material;
        _originalEmissionColor = _material.GetColor("_EmissionColor");
    }

    public void OnGrab(Transform interactor)
    {
        _rb.isKinematic = true;
        transform.SetParent(interactor);
        // Use the configurable offset instead of a hard-coded "magic number".
        transform.localPosition = heldPositionOffset;
        Debug.Log($"[PhysicsProp] {gameObject.name}grabbed!]");
        // Provide immediate visual feedback to the player that the grab was successful.
        _material.SetColor("_EmissionColor", Color.yellow * 0.5f); // Make it glow yellow
        
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        // Clamp the release velocity to prevent objects from being thrown at uncontrollable speeds.
        _rb.linearVelocity = Vector3.ClampMagnitude(releaseVelocity, maxThrowSpeed);
        // Restore the object's original visual state on release.
        _material.SetColor("_EmissionColor", _originalEmissionColor); // Reset the glow
        Debug.Log($"[PhysicsProp] {gameObject.name}released!");
        
        
    }
}
