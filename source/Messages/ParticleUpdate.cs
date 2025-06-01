namespace Particles.Messages
{
    public readonly struct ParticleUpdate
    {
        public readonly float deltaTime;

        public ParticleUpdate(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }
    }
}
