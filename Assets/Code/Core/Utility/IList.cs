using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Core.Utility
{
    public interface IList<T> : IEnumerable, ICollection<T>, IEnumerable<T>
    {
        T this[int index] { get; set; }
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);
    }
}
