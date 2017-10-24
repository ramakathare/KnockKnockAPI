using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace KnockKnock.Services
{
  /// <summary>
  /// Represent the reverse words service.
  /// </summary>
  public class StringReverseService
  {
    // The separator between words.
    // Note: Add dot, comma, question mark, exclamation mark etc. to make it more intelligence.
    protected char[] separator = { ' ' };

    /// <summary>
    /// Reverses words in the specified string.
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns>The string with reversed words.</returns>
    public string ReverseWords(string s)
    {
      if (s == null)
      {
        throw new ArgumentNullException(nameof(s), "Value cannot be null.");
      }

      var key = string.Format("ReverseWords{0}", s.GetHashCode());
      var cacheItem = MemoryCache.Default.GetCacheItem(key);

      string result = string.Empty;

      if (cacheItem != null)
      {
        result = (string) cacheItem.Value;
      }
      else
      {
        var words = this.Split(s, separator);
        var reversedWords = new StringBuilder();

        foreach (var word in words)
        {
          reversedWords.Append(ReverseWord(word));
        }

        result = reversedWords.ToString();

        MemoryCache.Default.Add(new CacheItem(key, result), new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(6) });
      }

      return result;
    }

    /// <summary>
    /// Reverses the word.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <returns>The reversed word.</returns>
    public string ReverseWord(string word)
    {
      char[] charArray = word.ToCharArray();
      Array.Reverse(charArray);

      return new string(charArray);
    }

    #region Protected Methods

    protected string[] Split(string value, char[] separator)
    {
      var result = new List<string>();
      string[] temp;

      do
      {
        temp = this.InnerSplit(value, separator);
        result.Add(temp.First());
        value = temp.Last();
      }
      while (temp.Length > 1);

      return result.ToArray();
    }

    protected string[] InnerSplit(string value, char[] separator)
    {
      string[] result = new string[2];

      if (value.Length > 1)
      {
        for (int index = 0; index < value.Length; index++)
        {
          if (this.IsDelimiterChar(value[index], separator))
          //if (value[index].Equals(' '))
          {
            int endIndex = index == 0 ? index + 1 : index;

            result[0] = value.Substring(0, endIndex);
            result[1] = value.Substring(endIndex);

            return result;
          }
        }
      }

      return new string[] { value };
    }

    protected bool IsDelimiterChar(char character, char[] delimiterCharacters)
    {
      bool result = false;

      for (int index = 0; index < delimiterCharacters.Length; index++)
      {
        if (delimiterCharacters[index] == character)
        {
          result = true;
        }
      }

      return result;
    }

    #endregion
  }
}