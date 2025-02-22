using Shapes;
using System.Numerics;

namespace Particles.Components
{
    public struct ParticleMachine
    {
        public Emission emission;
        public InitialParticleState initialParticleState;

        public struct Emission
        {
            public bool paused;
            public MinMax<float> interval;
            public MinMax<uint> count;
        }

        public struct InitialParticleState
        {
            public Vector3 position;
            public Shape bounds;
            public bool surfaceOnly;
            public MinMax<float> lifetime;
            public MinMax<Vector3> velocity;
            public MinMax<Vector3> drag;
            public MinMax<float> extents;
        }
    }
}