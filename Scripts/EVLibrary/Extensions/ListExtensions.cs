using System;
using System.Collections.Generic;
using System.Linq;

namespace EVLibrary.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Merge<T>(this IList<T> listTo, IList<T> listFrom)
        {
            listFrom.ToList().ForEach(item => listTo.Add(item));
            return listTo;
        }
    }
}
