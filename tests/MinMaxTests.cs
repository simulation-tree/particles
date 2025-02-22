using System;
using System.Numerics;

namespace Particles.Tests
{
    public class MinMaxTests
    {
        [Test]
        public void FloatRange()
        {
            MinMax<float> range = new(5f, 10f);
            Assert.That(range.min, Is.EqualTo(5f));
            Assert.That(range.max, Is.EqualTo(10f));
            Assert.That(range.Center, Is.EqualTo(7.5f));
            Assert.That(range.Size, Is.EqualTo(5f));
        }

        [Test]
        public void Vector3Range()
        {
            MinMax<Vector3> range = new(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            Assert.That(range.min, Is.EqualTo(new Vector3(1, 2, 3)));
            Assert.That(range.max, Is.EqualTo(new Vector3(4, 5, 6)));
            Assert.That(range.Center, Is.EqualTo(new Vector3(2.5f, 3.5f, 4.5f)));
            Assert.That(range.Size, Is.EqualTo(new Vector3(3, 3, 3)));
        }

        [Test]
        public void UIntRange()
        {
            MinMax<uint> range = new(5, 10);
            Assert.That(range.min, Is.EqualTo(5));
            Assert.That(range.max, Is.EqualTo(10));
            Assert.That(range.Center, Is.EqualTo(8));
            Assert.That(range.Size, Is.EqualTo(5));

            MinMax<uint> count = new(1);
            Assert.That(count.min, Is.EqualTo(1));
            Assert.That(count.max, Is.EqualTo(1));
            Assert.That(count.Center, Is.EqualTo(1));
            Assert.That(count.Size, Is.EqualTo(0));
        }

        [Test]
        public void ModifyCenterAndSize()
        {
            MinMax<Vector3> range = new(new Vector3(0f, 0f, 0f), new Vector3(5f, 5f, 5f));
            range.Center = new Vector3(2.5f, 2.5f, 2.5f);
            Assert.That(range.min, Is.EqualTo(new Vector3(0f, 0f, 0f)));
            Assert.That(range.max, Is.EqualTo(new Vector3(5f, 5f, 5f)));

            range.Size = new Vector3(1f, 1f, 1f);
            Assert.That(range.min, Is.EqualTo(new Vector3(2f, 2f, 2f)));
            Assert.That(range.max, Is.EqualTo(new Vector3(3f, 3f, 3f)));

            range.Center = new Vector3(0.5f, 0.5f, 0.5f);
            Assert.That(range.min, Is.EqualTo(new Vector3(0f, 0f, 0f)));
            Assert.That(range.max, Is.EqualTo(new Vector3(1f, 1f, 1f)));
        }

        [Test]
        public void CreateFromCenterAndSize()
        {
            Vector2 center = new(2.5f, 2.5f);
            Vector2 size = new(1f, 1f);
            MinMax<Vector2> range = MinMax<Vector2>.CreateFromCenterAndSize(center, size);
            Assert.That(range.min, Is.EqualTo(new Vector2(2f, 2f)));
            Assert.That(range.max, Is.EqualTo(new Vector2(3f, 3f)));
            Assert.That(range.Center, Is.EqualTo(center));
            Assert.That(range.Size, Is.EqualTo(size));
        }

#if DEBUG
        [Test]
        public void ThrowIfTypeNotCompatible()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                MinMax<byte>.CreateFromCenterAndSize(3, 5);
            });
        }
#endif
    }
}