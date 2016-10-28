using System;

namespace Farmhand.Helpers
{
    /// <summary>
    /// The Mersenne Twister is a pseudorandom number generator.
    /// </summary>
    internal class MersenneTwister : IDisposable
    {
        #region Fields

        private const short N = 624;
        private const short M = 397;
        private const uint MatrixA = 0x9908b0df;
        private const uint UpperMask = 0x80000000;
        private const uint LowerMask = 0x7fffffff;
        private uint[] _mt;
        private ushort _mti;
        private uint[] _mag01;
        private bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class with given seed value.
        /// </summary>
        /// <param name="seed"></param>
        public MersenneTwister(uint seed)
        {
            Init();
            InitGenRand(seed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class.
        /// </summary>
        public MersenneTwister()
        {
            Init();

            var seedKey = new uint[6];
            var rnseed = new byte[8];

            seedKey[0] = (uint)DateTime.Now.Millisecond;
            seedKey[1] = (uint)DateTime.Now.Second;
            seedKey[2] = (uint)DateTime.Now.DayOfYear;
            seedKey[3] = (uint)DateTime.Now.Year;
            seedKey[4] = ((uint)rnseed[0] << 24) | ((uint)rnseed[1] << 16) | ((uint)rnseed[2] << 8) | rnseed[3];
            seedKey[5] = ((uint)rnseed[4] << 24) | ((uint)rnseed[5] << 16) | ((uint)rnseed[6] << 8) | rnseed[7];

            InitByArray(seedKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class with given seed array.
        /// </summary>
        /// <param name="initKey">An array for initializing keys.</param>
        public MersenneTwister(uint[] initKey)
        {
            Init();

            InitByArray(initKey);
        }

        /// <summary>
        /// 
        /// </summary>
        ~MersenneTwister()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases all resources used by the current instance of <see cref="MersenneTwister"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance of <see cref="MersenneTwister"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _mt = null;
                _mag01 = null;
            }

            _disposed = true;
        }

        private void Init()
        {
            _mt = new uint[N];
            _mag01 = new uint[] { 0, MatrixA };
            _mti = N + 1;
        }

        /// <summary>
        /// Initializes mt[N] with a seed.
        /// </summary>
        /// <param name="seed">Seed value.</param>
        private void InitGenRand(uint seed)
        {
            _mt[0] = seed;

            for (_mti = 1; _mti < N; _mti++)
            {
                _mt[_mti] = 1812433253 * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + _mti;
            }
        }

        /// <summary>
        /// Initialize by an array with array-length.
        /// </summary>
        /// <param name="initKey">An array for initializing keys.</param>
        private void InitByArray(uint[] initKey)
        {
            uint i, j;
            int k;
            var keyLength = initKey.Length;

            InitGenRand(19650218);

            i = 1;
            j = 0;
            k = N > keyLength ? N : keyLength;

            for (; k > 0; k--)
            {
                _mt[i] = (_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30)) * 1664525)) + initKey[j] + j;
                i++;
                j++;

                if (i >= N)
                {
                    _mt[0] = _mt[N - 1];
                    i = 1;
                }

                if (j >= keyLength)
                {
                    j = 0;
                }
            }
            for (k = N - 1; k > 0; k--)
            {
                _mt[i] = (_mt[i] ^ ((_mt[i - 1] ^ (_mt[i - 1] >> 30)) * 1566083941)) - i;
                i++;

                if (i >= N)
                {
                    _mt[0] = _mt[N - 1];
                    i = 1;
                }
            }

            _mt[0] = 0x80000000;
        }

        /// <summary>
        /// Generates a random number on [0,0xffffffff]-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public uint GenRandInt32()
        {
            uint y;

            if (_mti >= N)
            {
                short kk;

                if (_mti == N + 1)
                {
                    InitGenRand(5489);
                }

                for (kk = 0; kk < N - M; kk++)
                {
                    y = ((_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask)) >> 1;
                    _mt[kk] = _mt[kk + M] ^ _mag01[_mt[kk + 1] & 1] ^ y;
                }

                for (; kk < N - 1; kk++)
                {
                    y = ((_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask)) >> 1;
                    _mt[kk] = _mt[kk + (M - N)] ^ _mag01[_mt[kk + 1] & 1] ^ y;
                }

                y = ((_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask)) >> 1;
                _mt[N - 1] = _mt[M - 1] ^ _mag01[_mt[0] & 1] ^ y;
                _mti = 0;
            }

            y = _mt[_mti++];
            y ^= y >> 11;
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= y >> 18;

            return y;
        }

        /// <summary>
        /// Generates a random number on [0,0x7fffffff]-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public uint GenRandInt31() => GenRandInt32() >> 1;

        /// <summary>
        /// Generates a random number on [0,1]-real-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal1() => GenRandInt32() * (1.0 / 4294967295.0);

        /// <summary>
        /// Generates a random number on [0,1)-real-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal2() => GenRandInt32() * (1.0 / 4294967296.0);

        /// <summary>
        /// Generates a random number on (0,1)-real-Interval
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal3() => (GenRandInt32() + 0.5) * (1.0 / 4294967296.0);

        /// <summary>
        /// Generates a random number on [0,1) with 53-bit resolution.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandRes53()
        {
            uint a = GenRandInt32() >> 5, b = GenRandInt32() >> 6;

            return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
        }

        #endregion
    }
}
