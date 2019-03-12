using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts.Generators
{
    /// <summary> Represents a pseudo-random number generator, which is a device that produces
    /// a sequence of numbers that meet certain statistical requirements for randomness.</summary>
    public static class ThreadsafeRandom
    {
        /// <summary> The random number generator used on this thread.</summary>
        /// <remarks> Each instance generates a GUID to use as seed, preventing collision.</remarks>
        private static ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random(new Guid().GetHashCode()));
        private static ThreadLocal<short> _operationWheel = new ThreadLocal<short>(() => 0);

        /// <summary> Generates a non-negative random interger.</summary>
        /// <returns> Generated non-negative random interger.</returns>
        public static int Next() => _random.Value.Next();

        /// <summary> Returns a non-negative random integer that is less than the specified maximum.</summary>
        /// <param name="maxValue"> The exclusive upper bound of the random number to be generated. maxValue must
        /// be greater than or equal to 0.</param>
        /// <returns> A 32-bit signed integer that is greater than or equal to 0, and less than maxValue
        /// <para> that is, the range of return values ordinarily includes 0 but not maxValue. However,
        /// if maxValue equals 0, maxValue is returned.</para></returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static int Next(int maxValue) => _random.Value.Next(maxValue);

        /// <summary> Returns a random integer that is within a specified range.</summary>
        /// <param name="minValue"> The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue"> The exclusive upper bound of the random number returned. maxValue must be greater
        /// than or equal to minValue.</param>
        /// <returns> A 32-bit signed integer greater than or equal to minValue and less than maxValue
        /// <para>that is, the range of return values includes minValue but not maxValue. If minValue
        /// equals maxValue, minValue is returned.</para></returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static int Next(int minValue, int maxValue) => _random.Value.Next(minValue, maxValue);


        /// <summary> Fills the elements of a specified array of bytes with random numbers.</summary>
        /// <param name="buffer"> An array of bytes to contain random numbers.</param>
        public static void NextBytes(byte[] buffer) => _random.Value.NextBytes(buffer);

        /// <summary> Fills the elements of a span of bytes with random numbers.</summary>
        /// <param name="buffer"> A span of bytes to contain random numbers.</param>
        public static void NextBytes(Span<byte> buffer) => _random.Value.NextBytes(buffer);


        /// <summary> Returns a random floating-point number that is greater than or equal to 0.0,
        /// and less than 1.0.</summary>
        /// <returns> A double-precision floating point number that is greater than or equal to 0.0,
        /// and less than 1.0.</returns>
        public static double NextDouble() => _random.Value.NextDouble();

        /// <summary> Returns a random floating-point number that is greater than or equal to 0,
        /// and less than the specified maximum value.</summary>
        /// <param name="maxValue"> The exclusive upper bound of the random number returned.</param>
        /// <returns> A double-precision floating point number that is greater than or equal to 0.0,
        /// and less than the given maximum value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static double NextDouble(double maxValue) => maxValue < 0 ?
            throw new ArgumentOutOfRangeException() :
            _random.Value.NextDouble() * maxValue;

        /// <summary> Returns a random floating-point number that is greater than or equal to 0,
        /// and less than the specified maximum value.</summary>
        /// <param name="minValue"> The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue"> The exclusive upper bound of the random number returned.</param>
        /// <returns> A double-precision floating point number that is greater than or equal to the given minimum value,
        /// and less than the given maximum value.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public static double NextDouble(double minValue, double maxValue) => minValue > maxValue ?
            throw new ArgumentOutOfRangeException() :
            (maxValue - minValue) * _random.Value.NextDouble() + minValue;

        private static ushort _longIdWheel = 0;
        private static object _longIdLock = new object();
        private static readonly DateTime _epoch = new DateTime(2019, 3, 1);
        public static ulong GenerateUlongId()
        {
            long id;

            lock (_longIdLock)
            {
                id = _longIdWheel++;
            }

            byte[] randomBytes = new byte[2];
            NextBytes(randomBytes);

            id |= (long)randomBytes[0] << 16;
            id |= (long)randomBytes[0] << 24;

            id = ((DateTime.UtcNow - _epoch).Ticks / TimeSpan.TicksPerMillisecond) << 32;

            return (ulong)id;
        }

        /// <summary> Returns a guaranteed unique 128-bit ID based on the current time, thread, randomness and operation on this tick. </summary>
        /// <returns> A random 128-bit ID based on the current time, thread randomness and operation on this tick.</returns>
        public static Guid GenerateGuid()
        {
            byte[] guidBytes = new byte[16];

            int threadId = Thread.CurrentThread.ManagedThreadId;
            short operation = _operationWheel.Value++;
            int randomValue = _random.Value.Next(ushort.MaxValue);
            long ticksPassed = DateTime.UtcNow.Ticks;

            guidBytes[0] = (byte)(threadId >> 24);
            guidBytes[1] = (byte)(threadId >> 16);
            guidBytes[2] = (byte)(threadId >> 8);
            guidBytes[3] = (byte)threadId;
            guidBytes[4] = (byte)(operation >> 8);
            guidBytes[5] = (byte)operation;
            guidBytes[6] = (byte)(randomValue >> 8);
            guidBytes[7] = (byte)randomValue;
            guidBytes[8] = (byte)(ticksPassed >> 56);
            guidBytes[9] = (byte)(ticksPassed >> 48);
            guidBytes[10] = (byte)(ticksPassed >> 40);
            guidBytes[11] = (byte)(ticksPassed >> 32);
            guidBytes[12] = (byte)(ticksPassed >> 24);
            guidBytes[13] = (byte)(ticksPassed >> 16);
            guidBytes[14] = (byte)(ticksPassed >> 8);
            guidBytes[15] = (byte)ticksPassed;

            return new Guid(guidBytes);
        }
    }
}
