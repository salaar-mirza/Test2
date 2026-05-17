using ServiceArchitecture.Core;
using ServiceArchitecture.Scoring.Events;
using ServiceArchitecture.Simulation.Events;
using TMPro;

namespace ServiceArchitecture.UI
{
    /// <summary>
    /// Listens for game state events and updates the UI display.
    /// </summary>
    public class UIService : IService
    {
        private readonly TextMeshProUGUI _statusText;

        public UIService(TextMeshProUGUI statusText)
        {
            _statusText = statusText;
            EventBus<ProcedureStateChangedEvent>.Subscribe(OnProcedureStateChanged);
            EventBus<ScoreUpdatedEvent>.Subscribe(OnScoreUpdated);
        }

        private void OnProcedureStateChanged(ProcedureStateChangedEvent payload) => 
            UpdateText($"State: {payload.NewState}");

        private void OnScoreUpdated(ScoreUpdatedEvent payload) => 
            UpdateText($"Score: {payload.NewTotalScore}");

        private void UpdateText(string content) => _statusText.text = content;
    }
}