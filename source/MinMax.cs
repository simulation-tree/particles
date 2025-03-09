using System;
using System.Diagnostics;

namespace Particles
{
    public struct MinMax<T> : IEquatable<MinMax<T>> where T : unmanaged
    {
        private unsafe static readonly int PartCount = sizeof(T) / sizeof(float);

        public T min;
        public T max;

        public unsafe T Center
        {
            readonly get
            {
                fixed (T* pMin = &min, pMax = &max)
                {
                    float* pCenter = stackalloc float[PartCount];
                    for (int i = 0; i < PartCount; i++)
                    {
                        pCenter[i] = (((float*)pMin)[i] + ((float*)pMax)[i]) * 0.5f;
                    }

                    return *(T*)pCenter;
                }
            }
            set
            {
                T size = Size;
                fixed (T* pMin = &min, pMax = &max)
                {
                    T* pSize = &size;
                    T* pValue = &value;
                    for (int i = 0; i < PartCount; i++)
                    {
                        ((float*)pMin)[i] = ((float*)pValue)[i] - ((float*)pSize)[i] * 0.5f;
                        ((float*)pMax)[i] = ((float*)pValue)[i] + ((float*)pSize)[i] * 0.5f;
                    }
                }
            }
        }

        public unsafe T Size
        {
            readonly get
            {
                fixed (T* pMin = &min, pMax = &max)
                {
                    float* pSize = stackalloc float[PartCount];
                    for (int i = 0; i < PartCount; i++)
                    {
                        pSize[i] = (((float*)pMax)[i] - ((float*)pMin)[i]);
                    }

                    return *(T*)pSize;
                }
            }
            set
            {
                T center = Center;
                fixed (T* pMin = &min, pMax = &max)
                {
                    T* pCenter = &center;
                    T* pValue = &value;
                    for (int i = 0; i < PartCount; i++)
                    {
                        ((float*)pMin)[i] = ((float*)pCenter)[i] - ((float*)pValue)[i] * 0.5f;
                        ((float*)pMax)[i] = ((float*)pCenter)[i] + ((float*)pValue)[i] * 0.5f;
                    }
                }
            }
        }

#if NET
        public MinMax()
        {
            ThrowIfSizeIsNotCompatible();
        }
#endif

        public MinMax(T min, T max)
        {
            ThrowIfSizeIsNotCompatible();

            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Creates a min-max value with the given <paramref name="center"/> and size of 0.
        /// </summary>
        public MinMax(T center)
        {
            ThrowIfSizeIsNotCompatible();

            min = center;
            max = center;
        }

        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[128];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public unsafe readonly int ToString(Span<char> destination)
        {
            fixed (T* pMin = &min, pMax = &max)
            {
                int length = 0;
                destination[length++] = 'M';
                destination[length++] = 'i';
                destination[length++] = 'n';
                destination[length++] = ':';
                for (int i = 0; i < PartCount; i++)
                {
                    float v = ((float*)pMin)[i];
                    length += v.ToString(destination.Slice(length));
                    if (i != PartCount - 1)
                    {
                        destination[length++] = ',';
                        destination[length++] = ' ';
                    }
                }

                destination[length++] = ' ';
                destination[length++] = 'M';
                destination[length++] = 'a';
                destination[length++] = 'x';
                destination[length++] = ':';
                for (int i = 0; i < PartCount; i++)
                {
                    float v = ((float*)pMax)[i];
                    length += v.ToString(destination.Slice(length));
                    if (i != PartCount - 1)
                    {
                        destination[length++] = ',';
                        destination[length++] = ' ';
                    }
                }

                return length;
            }
        }

        public unsafe readonly T Evaluate(T time)
        {
            fixed (T* pMin = &min, pMax = &max)
            {
                T* pTime = &time;
                float* pValue = stackalloc float[PartCount];
                for (int i = 0; i < PartCount; i++)
                {
                    float minValue = ((float*)pMin)[i];
                    float maxValue = ((float*)pMax)[i];
                    float t = ((float*)pTime)[i];
                    pValue[i] = minValue + (maxValue - minValue) * t;
                }

                return *(T*)pValue;
            }
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is MinMax<T> max && Equals(max);
        }

        public unsafe readonly bool Equals(MinMax<T> other)
        {
            fixed (T* pMin = &min, pMax = &max)
            {
                T* pOtherMin = &other.min;
                T* pOtherMax = &other.max;
                for (int i = 0; i < PartCount; i++)
                {
                    if (((float*)pMin)[i] != ((float*)pOtherMin)[i] || ((float*)pMax)[i] != ((float*)pOtherMax)[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public unsafe readonly override int GetHashCode()
        {
            fixed (T* pMin = &min, pMax = &max)
            {
                int hash = 17;
                for (int i = 0; i < PartCount; i++)
                {
                    hash = hash * 31 + ((float*)pMin)[i].GetHashCode();
                    hash = hash * 31 + ((float*)pMax)[i].GetHashCode();
                }

                return hash;
            }
        }

        public unsafe static MinMax<T> CreateFromCenterAndSize(T center, T size)
        {
            ThrowIfSizeIsNotCompatible();

            float* min = stackalloc float[PartCount];
            float* max = stackalloc float[PartCount];
            T* pCenter = &center;
            T* pSize = &size;
            for (uint i = 0; i < PartCount; i++)
            {
                min[i] = ((float*)pCenter)[i] - ((float*)pSize)[i] * 0.5f;
                max[i] = ((float*)pCenter)[i] + ((float*)pSize)[i] * 0.5f;
            }

            return new MinMax<T>(*(T*)min, *(T*)max);
        }

        [Conditional("DEBUG")]
        private unsafe static void ThrowIfSizeIsNotCompatible()
        {
            if (sizeof(T) % sizeof(float) != 0)
            {
                throw new InvalidOperationException("MinMax<T> only supports types that are a multiple of 4 bytes in size");
            }
        }

        public static bool operator ==(MinMax<T> left, MinMax<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MinMax<T> left, MinMax<T> right)
        {
            return !(left == right);
        }
    }
}