using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager instance;
    
    [Header("Saftey States")]
    
    public bool hasGoggles = false;
    public bool hasGloves = false;
    public bool isinspected = false;
    public bool isPowerOn = false;
    public bool isFullSpeed = false;
        
    [Header("Scoring")]
    
    public int totalScore = 0;
    private bool _techniqueScoreAwarded = false;
    
    [Header("UI Refaeance")]
    public TextMeshProUGUI hudText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); 
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

    public void AwardTechniqueScore()
    {
        if (!_techniqueScoreAwarded)
        {
            UpdateScore(10, "Safe Grinding Technique");
            _techniqueScoreAwarded = true;
        }
    }

    void UpdateHUD()
    {
        if (hudText == null) return;

        string ppeStatus = (hasGoggles && hasGloves)
            ? "<color=green>EQUIPPED</color>"
            : "<color=red>MISSING</color>";
        string inspectStatus = isinspected ? "<color=green>DONE</color>" : "<color=yellow>Pending</color>";
        string engineStatus = isPowerOn
            ? (isFullSpeed ? "<color=green>READY</color>" : "<color=yellow>WARMING UP</color>")
            : "<color=white>OFF</color>";

        hudText.text = $"<b>WORKSHOP SAFETY SIM</b>\n" +
            $"--------------------------------------\n" +
            $"PPE:{ppeStatus}\n" +
            $"Inspection: {inspectStatus}\n" +
            $"Grinder: {engineStatus}\n" +
            $"<b>SCORE:{totalScore}</b>";
    }


}
