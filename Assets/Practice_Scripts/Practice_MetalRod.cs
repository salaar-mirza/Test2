using System;
using UnityEngine;

public class Practice_MetalRod : MonoBehaviour, IPracticeGrabbable
{
    
    private Rigidbody _rodBody;
    public bool isHeated = false;

    [Tooltip("The local position of the rod when held by the player.")]
    public Vector3 heldPositionOffset = new Vector3(0f, 0f, 2f);

    public static event Action OnRodHeated;
    public static event Action OnRodCooled;

    private void Awake()
    {
        _rodBody = GetComponent<Rigidbody>();
    }
    
     
    private void OnEnable()
    {
        // Subscribe to the event when this object is enabled.
        Practice_Workstation.OnWorkstationCollided += HandleWorkstationCollision;
        Practice_CoolingBucket.OnObjectEnteredBucket += HandleBucketCollision;
    }
 
    private void OnDisable()
    {
        // IMPORTANT: Always unsubscribe when the object is disabled to prevent errors.
        Practice_Workstation.OnWorkstationCollided -= HandleWorkstationCollision;
        Practice_CoolingBucket.OnObjectEnteredBucket -= HandleBucketCollision;
    }

    

    public void OnGrab(Transform pivot)
    {
        _rodBody.isKinematic = true;
        transform.SetParent(pivot);
        transform.localPosition = heldPositionOffset;
    }
    

    public void OnRelease()
    {
        transform.SetParent(null);
        _rodBody.isKinematic = false;
    }
    
    private void HandleWorkstationCollision(GameObject objectThatCollided)
    {
        // Check if the object that hit the workstation was this specific metal rod.
        if (objectThatCollided != this.gameObject)
        {
            return; // If not, ignore the event.
        }
 
        // All heating logic now lives on the rod itself.
        if (Practice_Manager.instance.currentState != Practice_Manager.ProcedureState.ReadyToWork)
        {
            Debug.LogError("Cannot heat rod! Safety procedures not followed (PPE not equipped).");
        }
        else if (!Practice_Manager.instance.isWorkstationOn)
        {
            // This is the new safety check for the workstation's power.
            Debug.LogWarning("Workstation is not powered on. Cannot heat rod.");
        }
        else
        {
            this.isHeated = true;
            Debug.Log("<color=orange>The Rod is now HOT!</color>");
            OnRodHeated?.Invoke();
        }
    }
    
    private void HandleBucketCollision(GameObject objectThatCollided)
    {
        // Check if the object that entered the bucket was this specific metal rod.
        if (objectThatCollided != this.gameObject)
        {
            return; // If not, ignore the event.
        }

        // If the rod is hot, cool it down and complete the task.
        if (this.isHeated)
        {
            this.isHeated = false;
            Debug.Log("<color=cyan>Sizzzzzle! Rod has been cooled.</color>");
            Practice_Manager.instance.AdvanceState(Practice_Manager.ProcedureState.TaskComplete);
            OnRodCooled?.Invoke();
            Debug.Log("<b><color=yellow>TASK COMPLETE! Well done.</color></b>");

            // Destroy the rod as the final step.
            Destroy(gameObject);
        }
    }
    
}
