using UnityEngine;
using TMPro;

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
    
    // Add a new state to track the workstation's power status
    public bool isWorkstationOn = false;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    [Header("Scoring")]
    private int _totalScore = 0;
    private bool _ppeScored = false;
    private bool _powerOnScored = false;
    private bool _heatScored = false;
    private bool _coolScored = false;
    private bool _powerOffScored = false;
    
    private void OnEnable()
    {
        // Subscribe to the event when the manager is enabled.
        Practice_PPE.OnPPEEquipped += HandlePPEEquipped;
        Practice_Workstation.OnWorkstationPoweredOn += HandlePowerOn;
        Practice_Workstation.OnWorkstationPoweredOff += HandlePowerOff;
        Practice_MetalRod.OnRodHeated += HandleRodHeated;
        Practice_MetalRod.OnRodCooled += HandleRodCooled;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent errors.
        Practice_PPE.OnPPEEquipped -= HandlePPEEquipped;
        Practice_Workstation.OnWorkstationPoweredOn -= HandlePowerOn;
        Practice_Workstation.OnWorkstationPoweredOff -= HandlePowerOff;
        Practice_MetalRod.OnRodHeated -= HandleRodHeated;
        Practice_MetalRod.OnRodCooled -= HandleRodCooled;
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
        UpdateScoreUI();
        
    }

    public void AdvanceState(ProcedureState newState)
    {
        currentState = newState;
        Debug.Log("Current State is " + currentState);
    }

    private void HandlePPEEquipped()
    {
        AdvanceState(ProcedureState.ReadyToWork);
        if (!_ppeScored)
        {
            _totalScore += 20;
            _ppeScored = true;
            UpdateScoreUI();
        }
    }

    private void HandlePowerOn()
    {
        if (!_powerOnScored)
        {
            _totalScore += 20;
            _powerOnScored = true;
            UpdateScoreUI();
        }
    }

    private void HandleRodHeated()
    {
        if (!_heatScored)
        {
            _totalScore += 20;
            _heatScored = true;
            UpdateScoreUI();
        }
    }

    private void HandleRodCooled()
    {
        if (!_coolScored)
        {
            _totalScore += 20;
            _coolScored = true;
            UpdateScoreUI();
        }
    }

    private void HandlePowerOff()
    {
        // Only award points for turning it off if it was on before
        if (_powerOnScored && !_powerOffScored)
        {
            _totalScore += 20;
            _powerOffScored = true;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;

        string ppeText = $"1. Wear PPE: {(_ppeScored ? "<color=green>20</color>" : "0")} points";
        string powerOnText = $"2. Turn on Workstation: {(_powerOnScored ? "<color=green>20</color>" : "0")} points";
        string heatText = $"3. Heat Metal Rod: {(_heatScored ? "<color=green>20</color>" : "0")} points";
        string coolText = $"4. Cool Metal Rod: {(_coolScored ? "<color=green>20</color>" : "0")} points";
        string powerOffText = $"5. Turn off Workstation: {(_powerOffScored ? "<color=green>20</color>" : "0")} points";

        scoreText.text = $"{ppeText}\n{powerOnText}\n{heatText}\n{coolText}\n{powerOffText}\n--------------------\n<b>Total Score: {_totalScore}</b>";
    }
}
