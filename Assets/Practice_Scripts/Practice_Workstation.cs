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
        // Safety Gate: You can't operate the workstation without PPE.
        if (Practice_Manager.instance.currentState != Practice_Manager.ProcedureState.ReadyToWork)
        {
            Debug.LogError("DANGER: Cannot use workstation without wearing PPE first!");
            _meshRenderer.material.color = Color.red;
            return;
        }

        // If PPE is equipped, this action toggles the power state in the manager.
        Practice_Manager.instance.isWorkstationOn = !Practice_Manager.instance.isWorkstationOn;

        // Update visuals and log feedback based on the new power state.
        if (Practice_Manager.instance.isWorkstationOn)
        {
            Debug.Log("<color=green>Workstation POWER ON.</color>");
            _meshRenderer.material.color = Color.green;
        }
        else
        {
            Debug.Log("Workstation POWER OFF.");
            _meshRenderer.material.color = Color.white; // Use white to show it's ready but off
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
