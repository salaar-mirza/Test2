using ServiceArchitecture.Core;
using ServiceArchitecture.World.Controller;
using ServiceArchitecture.World.Data;
using ServiceArchitecture.World.View;
using UnityEngine;

namespace ServiceArchitecture.World
{
    public class WorldCreationService : IService
    {
        public PhysicsPropController CreatePhysicsProp(PhysicsPropConfig config, PhysicsPropView prefab, Vector3 position)
        {
            // 1. Instantiate the View (the prefab)
            var viewInstance = Object.Instantiate(prefab, position, Quaternion.identity);
            
            // 2. Create the Controller (the brain), passing it the data and the view
            var controller = new PhysicsPropController(config, viewInstance);
            
            // 3. Link the View to its Controller
            viewInstance.Initialize(controller);
            return controller;
        }
    }
}