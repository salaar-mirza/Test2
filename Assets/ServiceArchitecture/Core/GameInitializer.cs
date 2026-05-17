using System.Collections.Generic;
using UnityEngine;
using ServiceArchitecture.Input;
using ServiceArchitecture.Interaction;
using ServiceArchitecture.Simulation;

namespace ServiceArchitecture.Core
{
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
            // --- Dependencies ---
            // In a real project, these might come from a config file.
            Camera mainCamera = Camera.main;
            float interactionDistance = 10f;

            
            // --- Service Creation ---
            CreateAndRegister(new InputService());
            CreateAndRegister(new InteractionService(mainCamera, interactionDistance));
            CreateAndRegister(new SOP_Service());
            
            Debug.Log("Game Initializer: All services created and registered.");
        }
        
         
        private void CreateAndRegister<T>(T service) where T : IService
        {
            _services.Add(service);
            GameService.Register(service);
            if (service is ITickable tickable)
            {
                _tickables.Add(tickable);
            }
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
}