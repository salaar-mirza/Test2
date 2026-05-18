using UnityEngine;

/// <summary>
/// Represents a piece of Personal Protective Equipment that can be "equipped" by grabbing it.
/// This script calls the SimulationManager to update the game state and score.
/// </summary>
public class PPE : MonoBehaviour, IIntractable
{
    public enum PpeType { Goggles, Gloves }

    [Tooltip("The type of PPE this object represents.")]
    public PpeType type;

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
}