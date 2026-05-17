using System;
using UnityEngine;

public class Practice_PPE : MonoBehaviour ,IPracticeGrabbable
{
    private Rigidbody _rb;
    private Transform _originalParent;

    // This is the "PPE Equipped" radio channel.
    public static event Action OnPPEEquipped;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform grabPoint)
    {
        _rb.isKinematic = true;
        //transform.SetParent(grabPoint); // We don't need to parent it if we're destroying it.
        
        Debug.Log("PPE Grabbed and Equipped!");
        
        // Announce that PPE has been equipped.
        OnPPEEquipped?.Invoke();
        
        // Destroy the game object to simulate equipping it.
        Destroy(gameObject);
    }

    public void OnRelease()
    {
        
    }
}
