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
    public Color powerOnColor = Color.green;
    private Material _mat;
    private Color _organicColor;
    private bool _isHovering = false;

    private void Awake()
    {
        _mat = GetComponent<MeshRenderer>().material;
        _organicColor = _mat.color;
    }
    
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

        if (SimulationManager.instance.isPowerOn)
        {
            _mat.color = powerOnColor;
        }
        else if (_isHovering)
        {
            _mat.color = hoverColor;
        }
        else
        {
            _mat.color = _organicColor;
        }

        SimulationManager.instance.isFullSpeed = (currentRPM >= maxRPM * .9f);
    }

    public void OnGrab(Transform interactor)
    {
        if (!SimulationManager.instance.isinspected)
        {
            SimulationManager.instance.ScoreInspection();
        }
        else
        {
            SimulationManager.instance.isPowerOn = !SimulationManager.instance.isPowerOn;

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
