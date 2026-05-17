using System;
using System.Collections.Generic;

namespace ServiceArchitecture.Core
{
    /// <summary>
    /// A simple service locator to register and retrieve game services.
    /// </summary>
    public static class GameService
    {
        private static readonly Dictionary<Type, IService> Services = new Dictionary<Type, IService>();

        public static void Register<T>(T service) where T : IService
        {
            Services[typeof(T)] = service;
        }

        public static T Get<T>() where T : IService
        {
            if (Services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            return default;
        }
    }
}