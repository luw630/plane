using System;

/// <summary>
/// Random number generator
///     Algorithm: Mother of all
///     
///  This is a multiply-with-carry type of random number generator      
///  invented by George Marsaglia.  The algorithm is:                   
///     S = 2111111111*X[n-4] + 1492*X[n-3] + 1776*X[n-2] + 5115*X[n-1] + C
///     X[n] = S modulo 2^32                                               
///     C = floor(S / 2^32)
///  
/// </summary>
public class RandomMOA
{
    protected UInt32[] x = new UInt32[5];

    private int uint2int(UInt32 v)
    {
        byte[] buf = BitConverter.GetBytes(v);
        return BitConverter.ToInt32(buf, 0);
    }
    private UInt32 int2uint(int v)
    {
        byte[] buf = BitConverter.GetBytes(v);
        return BitConverter.ToUInt32(buf, 0);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="seed">random seed</param>
    public void RandomInit(int seed)
    {
        int i;
        UInt32 s = int2uint(seed);
        // make random numbers and put them into the buffer
        for (i = 0; i < 5; i++)
        {
            s = s * 29943829 - 1;
            x[i] = s;
        }
        // randomize some more
        for (i = 0; i < 19; i++) BRandom();
    
    }

    /// <summary>
    /// Get integer random number in desired interval
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int IRandom(int min, int max)
    {
        // Output random integer in the interval min <= x <= max
        // Relative error on frequencies < 2^-32
        if (max <= min)
        {
            return min;
        }

        // Assume 64 bit integers supported. Use multiply and shift method
        UInt32 interval;                  // Length of interval
        UInt64 longran;                   // Random bits * interval
        UInt32 iran;                      // Longran / 2^32

        interval = (UInt32)(max - min + 1);
        longran = (UInt64)BRandom() * interval;
        iran = (UInt32)(longran >> 32);
        // Convert back to signed and return result
        return (Int32)iran + min;
    }

    /// <summary>
    /// Get floating point random number
    /// </summary>
    /// <returns></returns>
    public double Random()
    {
        return (double)BRandom() * (1.0 / 4294967296.0);
    }

    /// <summary>
    /// Output random bits
    /// </summary>
    /// <returns></returns>
    public UInt32 BRandom()
    {
        UInt64 sum;
        sum = (UInt64)2111111111UL * (UInt64)x[3] +
            (UInt64)1492 * (UInt64)(x[2]) +
            (UInt64)1776 * (UInt64)(x[1]) +
            (UInt64)5115 * (UInt64)(x[0]) +
            (UInt64)x[4];
        x[3] = x[2]; x[2] = x[1]; x[1] = x[0];
        x[4] = (UInt32)(sum >> 32);                  // Carry
        x[0] = (UInt32)sum;                          // Low 32 bits of sum
        return x[0];    
    }

    public RandomMOA(int seed)
	{
        RandomInit(seed);
	}
}







