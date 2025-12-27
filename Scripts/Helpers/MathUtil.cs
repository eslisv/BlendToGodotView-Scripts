// Original Authors - Wyatt Senalik

namespace AM.Helpers
{
    public static class MathUtil
    {
        /// <summary>
        /// See https://en.wikipedia.org/wiki/Van_der_Corput_sequence.
        /// For generating evenly spaced spectrum values
        /// </summary>
        public static float VanDerCorput(uint n, uint baseNum = 2)
        {
            float result = 0;
            float denom = 1;
            while (n > 0)
            {
                denom *= baseNum;
                result += (n % baseNum) / denom;
                n /= baseNum;
            }
            return result;
        }
    }
}