using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProp : MonoBehaviour, IIntractable
{
    private  Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform interactor)
    {
        _rb.isKinematic = true;
        transform.SetParent(interactor);
        transform.localPosition = new Vector3(0f, 0f, 2f);
        Debug.Log($"[PhysicsProp] {gameObject.name}grabbed!]");
        
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.linearVelocity = releaseVelocity;
        Debug.Log($"[PhysicsProp] {gameObject.name}released!");
        
        
    }
}
