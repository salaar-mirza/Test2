using System;
using UnityEngine;

public class MetalRod : MonoBehaviour
{
    [Header("Grinding Settings")]
    [Header("References")]
    public ParticleSystem sparkParticles;
    
    private bool _grindingScoreAwarded = false;
    private bool _coolingScoreAwarded = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(sparkParticles != null) sparkParticles.Stop();
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
        if (sparkParticles != null && !sparkParticles.isPlaying)
        {
            sparkParticles.Play();
        }

        // If grinding hasn't been scored yet, award points.
        if (!_grindingScoreAwarded)
        {
            SimulationManager.instance.ScoreHeat();
            _grindingScoreAwarded = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WaterBucket") && !_coolingScoreAwarded)
        {
            SimulationManager.instance.ScoreCool();
            _coolingScoreAwarded = true;
            Debug.Log("Sizzle ! Rod cooled");
        }

    }
}
