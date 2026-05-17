using UnityEngine;
using ServiceArchitecture.Core;
using ServiceArchitecture.Simulation.Events;
using ServiceArchitecture.World.Events;

namespace ServiceArchitecture.Simulation
{
    
    /// <summary>
    /// Manages the state of the Standard Operating Procedure (SOP).
    /// </summary>
    public class SOP_Service : IService
    {
        public ProcedureState CurrentState { get; private set; }

        public SOP_Service()
        {
            CurrentState = ProcedureState.AwaitingPPE;
            Debug.Log($"SOP_Service Initialized. Current State: {CurrentState}");

            EventBus<PPE_EquippedEvent>.Subscribe(OnPPEEquipped);
            EventBus<PropCooledEvent>.Subscribe(OnPropCooled);
        }

        private void OnPPEEquipped(PPE_EquippedEvent payload)
        {
            if (CurrentState == ProcedureState.AwaitingPPE)
            {
                CurrentState = ProcedureState.ReadyToWork;
                EventBus<ProcedureStateChangedEvent>.Publish(new ProcedureStateChangedEvent
                    { NewState = CurrentState });
                Debug.Log($"<color=cyan>PPE Equipped! New State: {CurrentState}</color>");
            }
        }
        
         
        private void OnPropCooled(PropCooledEvent payload)
        {
            if (CurrentState == ProcedureState.ReadyToWork)
            {
                CurrentState = ProcedureState.TaskComplete;
                EventBus<ProcedureStateChangedEvent>.Publish(new ProcedureStateChangedEvent { NewState = CurrentState });
                Debug.Log($"<color=cyan>TASK COMPLETE! Final State: {CurrentState}</color>");
            }
        }
        
        
    }
}