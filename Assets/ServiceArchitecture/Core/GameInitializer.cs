using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The single MonoBehaviour entry point for the entire application.
/// It creates, initializes, and ticks all game services.
/// </summary>
public class GameInitializer : MonoBehaviour
{
    private readonly List<IService> _services = new List<IService>();
    private readonly List<ITickable> _tickables = new List<ITickable>();

    private void Awake()
    {
        // --- Service Creation ---
        // In the future, we will create all our services here.
        // Example: CreateAndRegister(new SOP_Service());

        Debug.Log("Game Initializer: All services created and registered.");
    }

    private void Update()
    {
        // Centralized tick loop for high performance.
        for (int i = 0; i < _tickables.Count; i++)
        {
            _tickables[i].OnTick();
        }
    }
}