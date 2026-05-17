using UnityEngine;

/// <summary>
/// Represents a piece of Personal Protective Equipment that can be "equipped" by grabbing it.
/// </summary>
public class PPE : MonoBehaviour, IIntractable
{
    public enum PpeType { Goggles, Gloves }

    [Tooltip("The type of PPE this object represents.")]
    public PpeType type;

    public void OnGrab(Transform interactor)
    {
        var manager = SimulationManager.instance;
        if (type == PpeType.Goggles)
        {
            manager.ScoreGoggles();
        }
        else if (type == PpeType.Gloves)
        {
            manager.ScoreGloves();
        }
        Destroy(gameObject);
    }

    public void OnRelease(Vector3 releaseVelocity) { } // Not needed, but required by the interface.
}