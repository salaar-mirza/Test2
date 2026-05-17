using UnityEngine;

namespace ServiceArchitecture.World.Data
{
    [CreateAssetMenu(fileName = "PhysicsPropConfig", menuName = "ServiceArchitecture/PhysicsPropConfig")]
    public class PhysicsPropConfig : ScriptableObject
    {
        public float mass = 1f;
        public float drag = 0.5f;
    }
}