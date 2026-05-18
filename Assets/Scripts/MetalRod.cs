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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache all necessary components on Start for performance and reliability.
        _rb = GetComponent<Rigidbody>();
        _rodMaterial =GetComponent<Renderer>().material;
        _organicColor = _rodMaterial.color;
        
        if(sparkParticles != null) sparkParticles.Stop();

        // Set the initial visual state.
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
        // Instantly heat the rod for clear, immediate feedback.
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
        // The visual state is determined by a clear priority: Hot > Hover > Idle.
        if (_currentHeat >= 1f)
        {
            // Priority 1: If the rod is hot, it glows red.
            _rodMaterial.SetColor("_EmissionColor", hotColor * _organicColor);
        }
        else if (_isHovering)
        {
            // Priority 2: If not hot and being hovered, show the hover glow.
            _rodMaterial.SetColor("_EmissionColor", hoverColor * 0.5f);
        }
        else
        {
            // Priority 3: Otherwise, return to the default non-glowing state.
            _rodMaterial.SetColor("_EmissionColor", Color.black);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Only cool the rod if it's actually hot.
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
        // Implements IIntractable: Logic for being picked up.
        _rb.isKinematic = true;
        transform.SetParent(interactor);
        transform.localPosition = new Vector3(0, 0, 1.5f); // A default hold position.
    }

    public void OnRelease(Vector3 releaseVelocity)
    {
        // Implements IIntractable: Logic for being dropped.
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.linearVelocity = releaseVelocity;
    }

    public void OnHoverEnter()
    {
        // Implements IHovarabel: Set hover state and update visuals.
        _isHovering = true;
        UpdateVisuals();
    }

    public void OnHoverExit()
    {
        // Implements IHovarabel: Clear hover state and update visuals.
        _isHovering = false;
        UpdateVisuals();
    }
}
