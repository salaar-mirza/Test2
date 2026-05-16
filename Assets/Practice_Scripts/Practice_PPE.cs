using UnityEngine;

public class Practice_PPE : MonoBehaviour ,IPracticeGrabbable
{
    private Rigidbody _rb;
    private Transform _originalParent;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform grabPoint)
    {
        _rb.isKinematic = true;
        //transform.SetParent(grabPoint); // We don't need to parent it if we're destroying it.
        
        Debug.Log("PPE Grabbed and Equipped!");
        
        // Use the public static instance to access the manager
        Practice_Manager.instance.AdvanceState(Practice_Manager.ProcedureState.ReadyToWork);
        
        // Destroy the game object to simulate equipping it.
        Destroy(gameObject);
    }

    public void OnRelease()
    {
        
    }
}
