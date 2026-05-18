using UnityEngine;

// Represents a piece of Personal Protective Equipment that can be "equipped" by grabbing it.
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
        _material = GetComponent<Renderer>().material;
        _material.EnableKeyword("_EMISSION");
        _originalEmissionColor = _material.GetColor("_EmissionColor");
    }

    public void OnGrab(Transform interactor)
    {
        if (type == PpeType.Goggles)
        {
            SimulationManager.instance.EquipGoggles();
        }
        else if (type == PpeType.Gloves)
        {
            SimulationManager.instance.EquipGloves();
        }
        Destroy(gameObject);
    }

    public void OnRelease(Vector3 releaseVelocity) { }

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

    private void UpdateVisuals()
    {
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