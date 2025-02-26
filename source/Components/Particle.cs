using System.Numerics;

namespace Particles.Components
{
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 drag;
        public Vector3 extents;
        public float lifetime;
        public bool free;
    }
}