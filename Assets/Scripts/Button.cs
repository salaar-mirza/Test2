using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class Button : MonoBehaviour, IHovarabel,IIntractable
{
    private Material _material;
    private Color _originalColor;
    public Color hoverColor;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
        
    }


    public void OnHoverEnter()
    {
        _material.color = hoverColor;
    }

    public void OnHoverExit()
    {
        _material.color = _originalColor;
    }

    public void OnGrab(Transform interactor)
    {
        Debug.Log(interactor.name);
    }
    public void OnRelease(Vector3 releaseVelocity)
    {}
}
