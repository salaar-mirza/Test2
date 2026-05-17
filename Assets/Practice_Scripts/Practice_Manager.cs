using UnityEngine;


public class Practice_Manager : MonoBehaviour
{
    public static Practice_Manager instance;
    
    public enum ProcedureState
    {
        AwaitingPPE,
        ReadyToWork,
        TaskComplete
    }
    
    public ProcedureState currentState { get; private set; }
    
    private void OnEnable()
    {
        // Subscribe to the event when the manager is enabled.
        Practice_PPE.OnPPEEquipped += HandlePPEEquipped;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent errors.
        Practice_PPE.OnPPEEquipped -= HandlePPEEquipped;
    }

    
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        currentState = ProcedureState.AwaitingPPE;
        Debug.Log("MANAGER INITIALIZED. Current State: " + currentState);
        
    }

    public void AdvanceState(ProcedureState newState)
    {
        currentState = newState;
        Debug.Log("Current State is " + currentState);
    }

    private void HandlePPEEquipped()
    {
        AdvanceState(ProcedureState.ReadyToWork);
    }
}
