using System;
using UnityEngine;

public class Practice_Workstation : MonoBehaviour ,IPracticeGrabbable
{
    // Cache the MeshRenderer component for better performance.
    private MeshRenderer _meshRenderer;
    
    public Practice_MetalRod _rodMaterial;


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
        if (other.gameObject.GetComponent<Practice_MetalRod>())
        {
            _rodMaterial = other.gameObject.GetComponent<Practice_MetalRod>();
            if (Practice_Manager.instance.currentState == Practice_Manager.ProcedureState.ReadyToWork)
            {
                _rodMaterial.isHeated = true;
                Debug.Log("the Road is Hot now");
            }
            else
            {
                _rodMaterial.isHeated = false;
                Debug.LogError("Were Gloves");
            }
        }
    }

    // The OnRelease method must exist to satisfy the interface, but it doesn't need to do anything for this object.
    public void OnRelease() { }
}
