using System;
using UnityEngine;

public class MetalRod : MonoBehaviour
{
    [Header("Grinding Settings")]
    public  float heatTransferRate = 0.15f;

    public float coolingRate = .1f;
    public Color hotColor = new Color(1f,.3f,0f);
    
    [Header("References")]
    public ParticleSystem sparkParticles;
    
    private Material _rodMaterial;
    private Color _organicColor;
    private float _currentHeat = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rodMaterial =GetComponent<Renderer>().material;
        _organicColor = _rodMaterial.color;
        
        if(sparkParticles != null) sparkParticles.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHeat > 0)
        {
            _currentHeat -= coolingRate * Time.deltaTime;
            UpdateVisuals();
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GrinderWheel"))
        {
            var manager = SimulationManager.instance;

            if (manager.isFullSpeed && manager.hasGoggles && manager.hasGloves)
            {
                ApplyGrinding();
                manager.AwardTechniqueScore();
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
        _currentHeat = Mathf.Clamp01(_currentHeat+heatTransferRate * Time.deltaTime);
        
        if (sparkParticles != null && !sparkParticles.isPlaying)
        {
            sparkParticles.Play();
        }
    }

    void UpdateVisuals()
    {
        if (_currentHeat > .8f)
        {
            _rodMaterial.SetColor("_EmissionColor", hotColor * _organicColor);
        }
        else
        {
            _rodMaterial.SetColor("_EmissionColor", Color.black);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WaterBucket"))
        {
            _currentHeat = 0f;
            UpdateVisuals();
            SimulationManager.instance.UpdateScore(5, "Cooled Work Piece");
            Debug.Log("Sizzle ! Rod cooled");
        }

    }
}
