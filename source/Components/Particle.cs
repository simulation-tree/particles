using System.Numerics;

namespace Particles.Components
{
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 extents;
        public Vector4 color;
        public float lifetime;
    }
}