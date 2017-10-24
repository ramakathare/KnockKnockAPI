using System;
using System.Collections;
using System.Runtime.Caching;

namespace KnockKnock.Services
{
  /// <summary>
  /// Represents the Fibonacci Number service. See details at https://en.wikipedia.org/wiki/Fibonacci_number.  
  /// </summary>
  public class FibonacciNumberService
  {
    /// <summary>
    /// The threshold for Fibonacci number if using long (Int64) type for calculating the result.
    /// </summary>
    protected readonly long threshold = 92;

    /// <summary>
    /// Calculates the negative Fibonacci sequence.
    /// </summary>
    /// <param name="n">The index in the sequence.</param>
    /// <returns>The Fibonacci number at specified position.</returns>
    public long Calculate(long n)
    {
      if (n > threshold)
      {
        throw new ArgumentOutOfRangeException(nameof(n), $"Value cannot be greater than {threshold}, since the result will cause a 64-bit integer overflow.");
      }

      if (n < -threshold)
      {
        throw new ArgumentOutOfRangeException(nameof(n), $"Value cannot be less than {-threshold}, since the result will cause a 64-bit integer overflow.");
      }

      var key = string.Format("FibonacciNumber{0}", n);
      var cacheItem = MemoryCache.Default.GetCacheItem(key);

      long result;

      if (cacheItem != null)
      {
        result = (long) cacheItem.Value;
      }
      else
      {
        result = CalculateBinetFormula(n);
        MemoryCache.Default.Add(new CacheItem(key, result), new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(6) });
      }

      return result;
    }

    #region Protected Methods
    
    // Calculates the Fibonacci number using the Binet's formula.
    // http://www.wikihow.com/Calculate-the-Fibonacci-Sequence
    protected long CalculateBinetFormula(long n)
    {
      var numerator = Math.Pow((1.0 + Math.Sqrt(5.0)), n) - Math.Pow((1.0 - Math.Sqrt(5.0)), n);
      var denominator = Math.Pow(2.0, n) * Math.Sqrt(5.0);
      var result = numerator / denominator;

      var roundedResult = Math.Round(result);

      return (long)roundedResult;
    }

    // Calculates the Fibonacci number using a sequence of "negafibonacci" numbers.
    protected long CalculateNega(long n)
    {
      long result = CalculateSequence(Math.Abs(n));

      // If n is negative and even, invert the sign.
      if (n < 0 && (n % 2 == 0))
      {
        result = -result;
      }

      return result;
    }

    // Calculates the Fibonacci number using a sequence.
    protected long CalculateSequence(long n)
    {
      if (n <= 1)
      {
        return n;
      }

      return CalculateSequence(n - 1) + CalculateSequence(n - 2);
    }

    #endregion
  }
}