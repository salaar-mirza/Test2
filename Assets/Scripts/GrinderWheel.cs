using UnityEngine;
using UnityEngine.UI;

public class GrinderWheel : MonoBehaviour,IIntractable,IHovarabel
{
    [Header("Settinges")] 
    public float maxRPM = 1500f;

    public float acceleration = 400f;
    private float currentRPM = 0f;

    [Header("Materials")] 
    private Material _mat;
    private Color _organicColor;

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

            // Award points based on the new power state.
            if (SimulationManager.instance.isPowerOn)
                SimulationManager.instance.ScorePowerOn();
        }
    }

    public void OnHoverEnter()
    {
        _mat.color = _organicColor;
    }

    public void OnHoverExit()
    {
        _mat.color = _organicColor;
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        
    }
}
