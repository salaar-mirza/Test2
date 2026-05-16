using System;
using UnityEngine;

public class Practice_MetalRod : MonoBehaviour, IPracticeGrabbable
{
    
    private Rigidbody _rodBody;
    public bool isHeated = false;

    private void Awake()
    {
        _rodBody = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform pivot)
    {
        _rodBody.isKinematic = true;
        transform.SetParent(pivot);
        transform.localPosition = new Vector3(0f,0f,2f);
    }
    

    public void OnRelease()
    {
        transform.SetParent(null);
        _rodBody.isKinematic = false;
    }
    
    
}
