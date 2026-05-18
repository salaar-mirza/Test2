using UnityEngine;

/// <summary>
/// Represents a piece of Personal Protective Equipment that can be "equipped" by grabbing it.
/// This script calls the SimulationManager to update the game state and score.
/// </summary>
public class PPE : MonoBehaviour, IIntractable, IHovarabel
{
    public enum PpeType { Goggles, Gloves }

    [Tooltip("The type of PPE this object represents.")]
    public PpeType type;

    [Header("Visuals")]
    [Tooltip("The color the object will glow when hovered over.")]
    public Color hoverColor = Color.yellow;

    private Material _material;
    private Color _originalEmissionColor;
    private bool _isHovering = false;

    private void Awake()
    {
        // Cache material properties and ensure emission is enabled for the hover effect.
        _material = GetComponent<Renderer>().material;
        _material.EnableKeyword("_EMISSION");
        _originalEmissionColor = _material.GetColor("_EmissionColor");
    }

    public void OnGrab(Transform interactor)
    {
        // Call the appropriate method on the SimulationManager based on the PPE type.
        if (type == PpeType.Goggles)
        {
            SimulationManager.instance.EquipGoggles();
        }
        else if (type == PpeType.Gloves)
        {
            SimulationManager.instance.EquipGloves();
        }
        // Destroy the object to simulate equipping it.
        Destroy(gameObject);
    }

    // This object is consumed on grab, so OnRelease is not needed, but the interface requires it.
    public void OnRelease(Vector3 releaseVelocity) { }

    public void OnHoverEnter()
    {
        // Set the hover state and update the object's appearance.
        _isHovering = true;
        UpdateVisuals();
    }

    public void OnHoverExit()
    {
        // Clear the hover state and restore the object's original appearance.
        _isHovering = false;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // Set the emission color based on the current hover state.
        if (_isHovering)
        {
            _material.SetColor("_EmissionColor", hoverColor * 0.5f);
        }
        else
        {
            _material.SetColor("_EmissionColor", _originalEmissionColor);
        }
    }
}