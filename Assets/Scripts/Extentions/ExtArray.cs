using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtArray
{
    /// <summary>
    /// Check if the element is in the array
    /// </summary>
    /// <param name="elementToCheck"></param>
    /// <returns></returns>
    public static bool Contains<T>(this T[] array, T elementToCheck)
    {
        foreach (T element in array)
        {
            if (element.Equals(elementToCheck))
            {
                return true;
            }
        }
        return false;
    }
}
