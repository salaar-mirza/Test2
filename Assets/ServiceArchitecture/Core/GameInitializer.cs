using System.Collections.Generic;
using UnityEngine;
using ServiceArchitecture.Input;
using ServiceArchitecture.Interaction;
using ServiceArchitecture.Simulation;
using ServiceArchitecture.Scoring;
using ServiceArchitecture.UI;
using ServiceArchitecture.World.Data;
using ServiceArchitecture.World;
using ServiceArchitecture.World.View;
using TMPro;

namespace ServiceArchitecture.Core
{
    /// <summary>
    /// The single MonoBehaviour entry point for the entire application.
    /// It creates, initializes, and ticks all game services.
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Scene Dependencies")]
        public PhysicsPropView physicsPropPrefab;
        public PhysicsPropConfig defaultPropConfig;
        public TextMeshProUGUI statusText;

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
            CreateAndRegister(new ScoringService());
            CreateAndRegister(new UIService(statusText));
            var worldCreator = CreateAndRegister(new WorldCreationService());

            Debug.Log("Game Initializer: All services created and registered.");
              
            // --- World Creation ---
            worldCreator.CreatePhysicsProp(defaultPropConfig, physicsPropPrefab, new Vector3(0, 1, 3));
        
        }
        
         
        private T CreateAndRegister<T>(T service) where T : IService
        {
            _services.Add(service);
            GameService.Register(service);
            if (service is ITickable tickable)
            {
                _tickables.Add(tickable);
            }
            return service;
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