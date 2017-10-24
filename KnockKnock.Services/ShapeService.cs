using System;
using System.Runtime.Caching;

namespace KnockKnock.Services
{
  /// <summary>
  /// Represents the geometric shape service. 
  /// For more details about Trials see at https://en.wikipedia.org/wiki/Triangle.
  /// </summary>
  public class ShapeService
  {
    /// <summary>
    /// Classifies the type of the specified triangle.
    /// </summary>
    /// <param name="a">The length of side 'a'.</param>
    /// <param name="b">The length of side 'b'.</param>
    /// <param name="c">The length of side 'c'.</param>
    /// <returns>The <see cref="TriangleType"/> type.</returns>
    public TriangleType ClassifyTriangleType(int a, int b, int c)
    {
      var key = string.Format("ClassifyTriangleType{0},{1},{2}", a, b, c);
      var cacheItem = MemoryCache.Default.GetCacheItem(key);

      var result = TriangleType.Error;

      if (cacheItem != null)
      {
        result = (TriangleType)cacheItem.Value;
      }
      else
      {
        if (!this.IsExistentTriangle(a, b, c))
        {
          return TriangleType.Error;
        }
        else if (this.IsEquilateralType(a, b, c))
        {
          result = TriangleType.Equilateral;
        }
        else if (this.IsScaleneType(a, b, c))
        {
          result = TriangleType.Scalene;
        }
        else if (this.IsIsoscelesType(a, b, c))
        {
          result = TriangleType.Isosceles;
        }

        MemoryCache.Default.Add(new CacheItem(key, result), new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(6) });
      }

      return result;
    }

    #region Protected Methods

    protected bool IsExistentTriangle(int a, int b, int c)
    {           
      bool triangleExist = true;

      // All sides must have positive length.
      if (a <= 0 || b <= 0 || c <= 0)
      {
        triangleExist = false;
      }
      // The sum of the lengths of any two sides of a triangle must be greater than the length of the third side for non-degenerate triangle.
      else if (((long) a + b) <= c)
      {
        triangleExist = false;
      }
      else if (((long) a + c) <= b)
      {
        triangleExist = false;
      }
      else if (((long) b + c) <= a)
      {
        triangleExist = false;
      }

      return triangleExist;
    }

    // An equilateral triangle has all sides the same length. 
    // An equilateral triangle is also a regular polygon with all angles measuring 60°.
    protected bool IsEquilateralType(int a, int b, int c)
    {
      return (a == b && a == c);
    }

    // An isosceles triangle has two sides of equal length.
    // An isosceles triangle also has two angles of the same measure.
    protected bool IsIsoscelesType(int a, int b, int c)
    {
      return (a == b || a == c || b == c);
    }

    // A scalene triangle has all sides different lengths, or equivalently all angles are unequal.
    protected bool IsScaleneType(int a, int b, int c)
    {
      return (a != b && a != c && b != c);
    }

    #endregion
  }
}