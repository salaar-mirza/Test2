using System.Collections.Generic;
using UnityEngine;
using ServiceArchitecture.Input;
using ServiceArchitecture.Interaction;
using ServiceArchitecture.Simulation;
using ServiceArchitecture.Scoring;
using ServiceArchitecture.UI;
using ServiceArchitecture.World.Data;
using ServiceArchitecture.Player;
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
        public GameObject playerPrefab;
        public WorkstationView workstationPrefab;
        public PhysicsPropView physicsPropPrefab;
        public PhysicsPropConfig defaultPropConfig;
        public TextMeshProUGUI statusText;

        private readonly List<IService> _services = new List<IService>();
        private readonly List<ITickable> _tickables = new List<ITickable>();

        private void Awake()
        {
            // --- Player Creation ---
            var playerInstance = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            var playerCharacterController = playerInstance.GetComponent<CharacterController>();
            var playerCamera = playerInstance.GetComponentInChildren<Camera>();


            
            // --- Service Creation ---
            CreateAndRegister(new InputService());
            CreateAndRegister(new PlayerMovementService(playerCharacterController, 7.5f, 100f, -9.81f));
            CreateAndRegister(new InteractionService(playerCamera, 10f));
            CreateAndRegister(new SOP_Service());
            CreateAndRegister(new ScoringService());
            CreateAndRegister(new UIService(statusText));
            var worldCreator = CreateAndRegister(new WorldCreationService());

            Debug.Log("Game Initializer: All services created and registered.");
              
            // --- World Creation ---
            // In a real project, this would be driven by a level loading service.
            Object.Instantiate(workstationPrefab, new Vector3(3, 0.5f, 3), Quaternion.identity);
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