using Particles.Components;
using System;
using Worlds;

namespace Particles
{
    public readonly partial struct ParticleEmitter : IEntity
    {
        public readonly ref IsParticleEmitter.Emission Emission => ref GetComponent<IsParticleEmitter>().emission;
        public readonly ref IsParticleEmitter.InitialParticleState InitialParticleState => ref GetComponent<IsParticleEmitter>().initialParticleState;
        public readonly Span<Particle> AllParticles => GetArray<Particle>();

        public readonly int AliveParticles
        {
            get
            {
                Span<Particle> particles = GetArray<Particle>();
                int count = 0;
                for (int i = 0; i < particles.Length; i++)
                {
                    if (!particles[i].free)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public readonly int FreeParticles
        {
            get
            {
                Span<Particle> particles = GetArray<Particle>();
                int count = 0;
                for (int i = 0; i < particles.Length; i++)
                {
                    if (particles[i].free)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public ParticleEmitter(World world)
        {
            this.world = world;
            this.value = world.CreateEntity(IsParticleEmitter.Default);
            Values<Particle> particles = CreateArray<Particle>(256);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].free = true;
            }
        }

        public ParticleEmitter(World world, IsParticleEmitter emitter)
        {
            this.world = world;
            this.value = world.CreateEntity(emitter);
            Values<Particle> particles = CreateArray<Particle>(256);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].free = true;
            }
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsParticleEmitter>();
            archetype.AddArrayType<Particle>();
        }

        public readonly ref Particle GetAliveParticle(uint index)
        {
            Values<Particle> particles = GetArray<Particle>();
            int count = 0;
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].free)
                {
                    if (count == index)
                    {
                        return ref particles[i];
                    }

                    count++;
                }
            }

            throw new IndexOutOfRangeException();
        }
    }
}