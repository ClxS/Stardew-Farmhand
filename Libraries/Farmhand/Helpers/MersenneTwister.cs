namespace Farmhand.Helpers
{
    using System;

    /// <summary>
    ///     The Mersenne Twister is a pseudo random number generator.
    /// </summary>
    internal class MersenneTwister : IDisposable
    {
        #region Fields

        private const short N = 624;

        private const short M = 397;

        private const uint MatrixA = 0x9908b0df;

        private const uint UpperMask = 0x80000000;

        private const uint LowerMask = 0x7fffffff;

        private uint[] mt;

        private ushort mti;

        private uint[] mag01;

        private bool disposed;

        #endregion

        #region Constructors
        
        public MersenneTwister(uint seed)
        {
            this.Init();
            this.InitGenRand(seed);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MersenneTwister" /> class.
        /// </summary>
        public MersenneTwister()
        {
            this.Init();

            var seedKey = new uint[6];
            var rnseed = new byte[8];

            seedKey[0] = (uint)DateTime.Now.Millisecond;
            seedKey[1] = (uint)DateTime.Now.Second;
            seedKey[2] = (uint)DateTime.Now.DayOfYear;
            seedKey[3] = (uint)DateTime.Now.Year;
            seedKey[4] = ((uint)rnseed[0] << 24) | ((uint)rnseed[1] << 16) | ((uint)rnseed[2] << 8) | rnseed[3];
            seedKey[5] = ((uint)rnseed[4] << 24) | ((uint)rnseed[5] << 16) | ((uint)rnseed[6] << 8) | rnseed[7];

            this.InitByArray(seedKey);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MersenneTwister" /> class with given seed array.
        /// </summary>
        /// <param name="initKey">An array for initializing keys.</param>
        public MersenneTwister(uint[] initKey)
        {
            this.Init();

            this.InitByArray(initKey);
        }
        
        ~MersenneTwister()
        {
            this.Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Releases all resources used by the current instance of <see cref="MersenneTwister" />.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.mt = null;
                this.mag01 = null;
            }

            this.disposed = true;
        }

        private void Init()
        {
            this.mt = new uint[N];
            this.mag01 = new uint[] { 0, MatrixA };
            this.mti = N + 1;
        }

        /// <summary>
        ///     Initializes mt[N] with a seed.
        /// </summary>
        /// <param name="seed">Seed value.</param>
        private void InitGenRand(uint seed)
        {
            this.mt[0] = seed;

            for (this.mti = 1; this.mti < N; this.mti++)
            {
                this.mt[this.mti] = 1812433253 * (this.mt[this.mti - 1] ^ (this.mt[this.mti - 1] >> 30))
                                      + this.mti;
            }
        }

        /// <summary>
        ///     Initialize by an array with array-length.
        /// </summary>
        /// <param name="initKey">An array for initializing keys.</param>
        private void InitByArray(uint[] initKey)
        {
            var keyLength = initKey.Length;

            this.InitGenRand(19650218);

            uint i = 1;
            uint j = 0;
            var k = N > keyLength ? N : keyLength;

            for (; k > 0; k--)
            {
                this.mt[i] = (this.mt[i] ^ ((this.mt[i - 1] ^ (this.mt[i - 1] >> 30)) * 1664525)) + initKey[j] + j;
                i++;
                j++;

                if (i >= N)
                {
                    this.mt[0] = this.mt[N - 1];
                    i = 1;
                }

                if (j >= keyLength)
                {
                    j = 0;
                }
            }

            for (k = N - 1; k > 0; k--)
            {
                this.mt[i] = (this.mt[i] ^ ((this.mt[i - 1] ^ (this.mt[i - 1] >> 30)) * 1566083941)) - i;
                i++;

                if (i >= N)
                {
                    this.mt[0] = this.mt[N - 1];
                    i = 1;
                }
            }

            this.mt[0] = 0x80000000;
        }
        
        public uint GenRandInt32()
        {
            uint y;

            if (this.mti >= N)
            {
                short kk;

                if (this.mti == N + 1)
                {
                    this.InitGenRand(5489);
                }

                for (kk = 0; kk < N - M; kk++)
                {
                    y = ((this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask)) >> 1;
                    this.mt[kk] = this.mt[kk + M] ^ this.mag01[this.mt[kk + 1] & 1] ^ y;
                }

                for (; kk < N - 1; kk++)
                {
                    y = ((this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask)) >> 1;
                    this.mt[kk] = this.mt[kk + (M - N)] ^ this.mag01[this.mt[kk + 1] & 1] ^ y;
                }

                y = ((this.mt[N - 1] & UpperMask) | (this.mt[0] & LowerMask)) >> 1;
                this.mt[N - 1] = this.mt[M - 1] ^ this.mag01[this.mt[0] & 1] ^ y;
                this.mti = 0;
            }

            y = this.mt[this.mti++];
            y ^= y >> 11;
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= y >> 18;

            return y;
        }
        
        public uint GenRandInt31() => this.GenRandInt32() >> 1;

        /// <summary>
        ///     Generates a random number on [0,1]-real-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal1() => this.GenRandInt32() * (1.0 / 4294967295.0);

        /// <summary>
        ///     Generates a random number on [0,1)-real-Interval.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal2() => this.GenRandInt32() * (1.0 / 4294967296.0);

        /// <summary>
        ///     Generates a random number on (0,1)-real-Interval
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandReal3() => (this.GenRandInt32() + 0.5) * (1.0 / 4294967296.0);

        /// <summary>
        ///     Generates a random number on [0,1) with 53-bit resolution.
        /// </summary>
        /// <returns>Returns generated number.</returns>
        public double GenRandRes53()
        {
            uint a = this.GenRandInt32() >> 5, b = this.GenRandInt32() >> 6;

            return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
        }

        #endregion
    }
}