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
        if (SimulationManager.instance.isPowerOn && !SimulationManager.instance.isinspected)
        {
            SimulationManager.instance.isinspected = true;
            SimulationManager.instance.UpdateScore(20,"Machine Inspected");
        }

        SimulationManager.instance.isPowerOn = !SimulationManager.instance.isPowerOn;
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
