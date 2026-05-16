using System;
using UnityEngine;

public class Practice_Cube : MonoBehaviour , IPracticeGrabbable
{
    private Rigidbody _rb;
    private Transform _originalParent;


    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _originalParent = gameObject.transform.parent;
    }


    public void OnGrab(Transform grabPoint)
    {
        _rb.isKinematic = true;
        transform.SetParent(grabPoint);
        transform.localPosition = new Vector3(0f, 0f, 2f);
        Debug.Log(" is Grabbed "+gameObject.name);

    }

    public void OnRelease()
    {
     _rb.isKinematic = false;
     transform.SetParent(_originalParent);
     Debug.Log(" is Released "+gameObject.name);
    }
}
