using UnityEngine;

public class Practice_CoolingBucket : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Try to get the MetalRod component from the object that entered the trigger.
        Practice_MetalRod rod = other.GetComponent<Practice_MetalRod>();

        // Check if we found a rod AND if that rod is currently heated.
        if (rod != null && rod.isHeated)
        {
            Debug.Log("<color=cyan>Sizzzzzle! Rod has been cooled.</color>");
            rod.isHeated = false;
            
            // This is the final step! Advance the state manager to complete the task.
            Practice_Manager.instance.AdvanceState(Practice_Manager.ProcedureState.TaskComplete);
            Debug.Log("<b><color=yellow>TASK COMPLETE! Well done.</color></b>");
        }
    }
}
