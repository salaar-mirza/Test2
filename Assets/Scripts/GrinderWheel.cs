using UnityEngine;
using UnityEngine.UI;

public class GrinderWheel : MonoBehaviour,IIntractable,IHovarabel
{
    [Header("Settinges")] 
    public float maxRPM = 1500f;

    public float acceleration = 400f;
    private float currentRPM = 0f;

    [Header("Visuals")]
    public Color hoverColor = Color.yellow;
    public Color powerOnColor = Color.red;
    private Material _mat;
    private Color _organicColor;
    private bool _isHovering = false;

    private void Awake()
    {
        _mat = GetComponent<MeshRenderer>().material;
        _organicColor = _mat.color;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (SimulationManager.instance.isPowerOn)
        {
            currentRPM = Mathf.MoveTowards(currentRPM,maxRPM,acceleration * Time.deltaTime);
            
        }
        else
        {
            currentRPM = Mathf.MoveTowards(currentRPM,0,acceleration * Time.deltaTime);
        }
        
        transform.Rotate(Vector3.forward,currentRPM*Time.deltaTime);

        // Handle state-based visuals every frame for reliability.
        // This ensures the color always reflects the most important state.
        if (SimulationManager.instance.isPowerOn)
        {
            // Priority 1: If power is on, it's always the "danger" color, overriding any hover effect.
            _mat.color = powerOnColor;
        }
        else if (_isHovering)
        {
            // Priority 2: If power is off and we are hovering, show the hover color.
            _mat.color = hoverColor;
        }
        else
        {
            // Priority 3: Otherwise, it's off and not hovered, so use its original color.
            _mat.color = _organicColor;
        }

        SimulationManager.instance.isFullSpeed = (currentRPM >= maxRPM * .9f);
    }

    public void OnGrab(Transform interactor)
    {
        // First, check if the machine has been inspected.
        if (!SimulationManager.instance.isinspected)
        {
            // If not, this action is the inspection.
            SimulationManager.instance.ScoreInspection();
        }
        else
        {
            // If it has already been inspected, this action toggles the power.
            SimulationManager.instance.isPowerOn = !SimulationManager.instance.isPowerOn;

            // If the power was just turned on, award points.
            if (SimulationManager.instance.isPowerOn)
                SimulationManager.instance.ScorePowerOn();
        }
    }

    public void OnHoverEnter()
    {
        _isHovering = true;
    }

    public void OnHoverExit()
    {
        _isHovering = false;
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        
    }
}
