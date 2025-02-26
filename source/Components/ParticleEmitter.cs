using Shapes;
using Shapes.Types;
using System.Numerics;

namespace Particles.Components
{
    public struct IsParticleEmitter
    {
        public static readonly IsParticleEmitter Default;

        static IsParticleEmitter()
        {
            Default = new();
            Default.emission.interval = new(0.1f);
            Default.emission.count = new(1);
            Default.initialParticleState.bounds = new SphereShape(0f);
            Default.initialParticleState.lifetime = new(0.8f, 1.2f);
            Default.initialParticleState.velocity = new(Vector3.UnitX);
            Default.initialParticleState.velocity.Size = new(0.25f);
            Default.initialParticleState.drag = new(Vector3.Zero);
            Default.initialParticleState.Size = new(new Vector3(0.5f));
        }

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
            public MinMax<Vector3> Size;
        }
    }
}