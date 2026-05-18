using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager instance;

    // By using an enum, we create a formal State Machine.
    // This makes the simulation flow clearer and prevents invalid states.
    public enum ProcedureState
    {
        Preparation, // Waiting for PPE
        Inspection,  // Waiting for machine inspection
        Operation,   // Ready to power on, grind, and cool
        Finished     // Task is complete
    }

    [Header("Saftey States")]
    
    public bool hasGoggles = false;
    public bool hasGloves = false;
    public bool isinspected = false;
    public bool isPowerOn = false;
    public bool isFullSpeed = false;

    // The current state of our procedure state machine.
    public ProcedureState CurrentState { get; private set; }

    [Header("Scoring")]
    
    public int totalScore = 0;
    private bool _gogglesScored = false;
    private bool _glovesScored = false;
    private bool _inspectionScored = false;
    private bool _powerOnScored = false;
    private bool _heatScored = false;
    private bool _coolScored = false;
    
    [Header("UI Refaeance")]
    public TextMeshProUGUI hudText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        CurrentState = ProcedureState.Preparation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
    }

    public void UpdateScore(int points, string reason)
    {
        totalScore += points;
        Debug.Log($"Score Update: + {points} ({reason}) | Total: {totalScore}");
    }

    public void EquipGoggles()
    {
        if (!_gogglesScored)
        {
            hasGoggles = true;
            UpdateScore(10, "Equipped Goggles");
            _gogglesScored = true;

            // If both PPE items are equipped, advance the simulation state.
            if (hasGoggles && hasGloves)
            {
                CurrentState = ProcedureState.Inspection;
            }
        }
    }
    
    public void EquipGloves()
    {
        if (!_glovesScored)
        {
            hasGloves = true;
            UpdateScore(10, "Equipped Gloves");
            _glovesScored = true;

            // If both PPE items are equipped, advance the simulation state.
            if (hasGoggles && hasGloves)
            {
                CurrentState = ProcedureState.Inspection;
            }
        }
    }
    
    public void ScoreInspection()
    {
        // Safety Gate: Can only inspect after preparing (wearing PPE).
        if (CurrentState == ProcedureState.Inspection && !_inspectionScored)
        {
            isinspected = true;
            UpdateScore(20, "Machine Inspected");
            _inspectionScored = true;
            CurrentState = ProcedureState.Operation; // Advance state after inspection.
        }
    }
    
    public void ScorePowerOn()
    {
        // Safety Gate: Can only power on during the Operation phase.
        if (CurrentState == ProcedureState.Operation && isinspected && !_powerOnScored)
        {
            UpdateScore(20, "Grinder Powered On");
            _powerOnScored = true;
        }
    }
    
    public void ScoreHeat()
    {
        // Safety Gate: Can only heat during the Operation phase.
        if (CurrentState == ProcedureState.Operation && !_heatScored)
        {
            UpdateScore(20, "Workpiece Heated Correctly");
            _heatScored = true;
        }
    }
    
    public void ScoreCool()
    {
        // Safety Gate: Can only cool during the Operation phase.
        if (CurrentState == ProcedureState.Operation && !_coolScored)
        {
            UpdateScore(20, "Workpiece Cooled");
            _coolScored = true;
            CurrentState = ProcedureState.Finished; // Final state transition.
        }
    }

    void UpdateHUD()
    {
        if (hudText == null) return;
        
        // The HUD can now derive its status text from the single CurrentState variable.
        string ppeStatus = CurrentState >= ProcedureState.Inspection ? "<color=green>EQUIPPED</color>" : $"<color=red>MISSING</color>";
        string inspectStatus = CurrentState >= ProcedureState.Operation ? "<color=green>DONE</color>" : "<color=yellow>Pending</color>";
        
        string engineStatus = isPowerOn
            ? (isFullSpeed ? "<color=green>READY</color>" : "<color=yellow>WARMING UP</color>")
            : "<color=white>OFF</color>";

        string ppeScore = $"1. Wear PPE (Goggles & Gloves): {(_gogglesScored && _glovesScored ? "<color=green>20</color>" : "0")} / 20";
        string inspectScore = $"2. Inspect Grinder: {(_inspectionScored ? "<color=green>20</color>" : "0")} / 20";
        string powerOnScore = $"3. Power On Grinder: {(_powerOnScored ? "<color=green>20</color>" : "0")} / 20";
        string heatScore = $"4. Heat Workpiece: {(_heatScored ? "<color=green>20</color>" : "0")} / 20";
        string coolScore = $"5. Cool Workpiece: {(_coolScored ? "<color=green>20</color>" : "0")} / 20";

        hudText.text = $"<b>WORKSHOP SAFETY SIM</b>\n" +
            $"--------------------------------------\n" +
            $"PPE Status: {ppeStatus}\n" +
            $"Inspection: {inspectStatus}\n" +
            $"Grinder: {engineStatus}\n" +
            $"--------------------------------------\n" +
            $"{ppeScore}\n{inspectScore}\n{powerOnScore}\n{heatScore}\n{coolScore}\n" +
            $"<b>TOTAL SCORE: {totalScore} / 100</b>";
    }


}
