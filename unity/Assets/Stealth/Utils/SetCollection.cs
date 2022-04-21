using System;
using System.Collections.Generic;
using UnityEngine;
using Util.DataStructures.BST;

namespace Stealth.Utils
{
    public class SetCollection<T> : IBST<T>
        where T : IComparable<T>, IEquatable<T>
    {
        private SortedSet<T> set;

        public int Count => set.Count;

        public SetCollection()
        {
            set = new SortedSet<T>();
        }

        public void Clear()
        {
            set.Clear();
        }

        public bool Contains(T data)
        {
            return set.Contains(data);
        }

        public bool Delete(T data)
        {
            bool success = set.Remove(data);
            if (!success)
            {
                Debug.Log($"Failed to delete {data}");
            }
            return success;
        }

        public T DeleteMax()
        {
            T data = set.Max;
            set.Remove(data);
            return data;
        }

        public T DeleteMin()
        {
            T data = set.Min;
            set.Remove(data);
            return data;
        }

        public bool FindMax(out T out_MaxValue)
        {
            T data = set.Max;
            out_MaxValue = data;
            return data == null;
        }

        public bool FindMin(out T out_MinValue)
        {
            T data = set.Min;
            out_MinValue = data;
            return data == null;
        }

        public bool FindNextBiggest(T data, out T out_NextBiggest)
        {
            throw new NotImplementedException();
        }

        public bool FindNextSmallest(T data, out T out_NextSmallest)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T data)
        {
            bool success = set.Add(data);
            if (!success)
            {
                Debug.Log($"Failed to insert {data}");
            }
            return success;
        }
    }
}
