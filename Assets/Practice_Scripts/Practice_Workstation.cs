using System;
using UnityEngine;

public class Practice_Workstation : MonoBehaviour ,IPracticeGrabbable
{
    // Cache the MeshRenderer component for better performance.
    private MeshRenderer _meshRenderer;
    
     
    // This is our public "radio channel". Any script can listen for this event.
    public static event Action<GameObject> OnWorkstationCollided;
    
    private void Awake()
    {
        // Get the component once and store it in our private variable.
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnGrab(Transform grabPoint)
    {
        // Check the current state from the manager.
        if (Practice_Manager.instance.currentState == Practice_Manager.ProcedureState.ReadyToWork)
        {
            Debug.Log("<color=green>SUCCESS: Workstation activated. You are following the procedure!</color>");
            _meshRenderer.material.color = Color.green;
        }
        else // This 'else' block will catch AwaitingPPE and any other future states.
        {
            Debug.LogError("DANGER: Cannot use workstation without wearing PPE first!");
            _meshRenderer.material.color = Color.red;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // The workstation's only job is to announce that something hit it.
        // The '?' is a safety check to make sure at least one script is listening.
        OnWorkstationCollided?.Invoke(other.gameObject);
    }

    // The OnRelease method must exist to satisfy the interface, but it doesn't need to do anything for this object.
    public void OnRelease() { }
}
