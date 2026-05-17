using ServiceArchitecture.Core;
using ServiceArchitecture.Scoring.Events;
using ServiceArchitecture.Simulation.Events;
using ServiceArchitecture.World.Events;
using UnityEngine;

namespace ServiceArchitecture.Scoring
{
    /// <summary>
    /// Listens for gameplay events and manages the player's score.
    /// </summary>
    public class ScoringService : IService
    {
        private int _totalScore = 0;

        public ScoringService()
        {
            EventBus<PPE_EquippedEvent>.Subscribe(OnPPEEquipped);
            EventBus<PropHeatedEvent>.Subscribe(OnPropHeated);
            EventBus<PropCooledEvent>.Subscribe(OnPropCooled);
        }

        private void OnPPEEquipped(PPE_EquippedEvent payload)
        {
            _totalScore += 20;
            EventBus<ScoreUpdatedEvent>.Publish(new ScoreUpdatedEvent { NewTotalScore = _totalScore });
            Debug.Log($"<color=yellow>ScoringService: PPE Equipped! New Score: {_totalScore}</color>");
        }
        
        private void OnPropHeated(PropHeatedEvent payload)
        {
            _totalScore += 20;
            EventBus<ScoreUpdatedEvent>.Publish(new ScoreUpdatedEvent { NewTotalScore = _totalScore });
            Debug.Log($"<color=yellow>ScoringService: Prop Heated! New Score: {_totalScore}</color>");
        }
         
        private void OnPropCooled(PropCooledEvent payload)
        {
            _totalScore += 20;
            EventBus<ScoreUpdatedEvent>.Publish(new ScoreUpdatedEvent { NewTotalScore = _totalScore });
            Debug.Log($"<color=yellow>ScoringService: Prop Cooled! New Score: {_totalScore}</color>");
        }
    }
}