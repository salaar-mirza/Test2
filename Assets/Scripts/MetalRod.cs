using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MetalRod : MonoBehaviour, IIntractable, IHovarabel
{
    [Header("Grinding Settings")]
    public Color hotColor = new Color(1f,.3f,0f);
    public Color hoverColor = Color.yellow;
    
    [Header("References")]
    public ParticleSystem sparkParticles;
    
    private Material _rodMaterial;
    private Color _organicColor;
    private float _currentHeat = 0f;
    private Rigidbody _rb;
    private bool _isHovering = false;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rodMaterial =GetComponent<Renderer>().material;
        _organicColor = _rodMaterial.color;
        
        _rodMaterial.EnableKeyword("_EMISSION");
        
        if(sparkParticles != null) sparkParticles.Stop();

        UpdateVisuals();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GrinderWheel"))
        {
            var manager = SimulationManager.instance;

            if (manager.isFullSpeed && manager.hasGoggles && manager.hasGloves)
            {
                ApplyGrinding();
            }
            else
            {
                Debug.LogWarning("DANGER: Grinding without Goggles or Gloves");
            }
        }
        
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrinderWheel"))
        {
            if(sparkParticles != null) sparkParticles.Stop();
        }
    }



    void ApplyGrinding()
    {
        if (_currentHeat < 1f)
        {
            _currentHeat = 1f;
            SimulationManager.instance.ScoreHeat();
            UpdateVisuals();
        }
        
        if (sparkParticles != null && !sparkParticles.isPlaying)
        {
            sparkParticles.Play();
        }
    }

    void UpdateVisuals()
    {
        if (_currentHeat >= 1f)
        {
            _rodMaterial.SetColor("_EmissionColor", hotColor * _organicColor);
        }
        else if (_isHovering)
        {
            _rodMaterial.SetColor("_EmissionColor", hoverColor * 0.5f);
        }
        else
        {
            _rodMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WaterBucket") && _currentHeat >= 1f)
        {
            _currentHeat = 0f;
            UpdateVisuals();
            SimulationManager.instance.ScoreCool();
            Debug.Log("Sizzle ! Rod cooled");
        }
    }

    // --- Interface Implementations ---

    public void OnGrab(Transform interactor)
    {
        _rb.isKinematic = true;
        transform.SetParent(interactor);
        transform.localPosition = new Vector3(0, 0, 1.5f);
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.linearVelocity = releaseVelocity;
    }

    public void OnHoverEnter()
    {
        _isHovering = true;
        UpdateVisuals();
    }

    public void OnHoverExit()
    {
        _isHovering = false;
        UpdateVisuals();
    }
}
