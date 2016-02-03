/* The MIT License (MIT)

Copyright (c) 2015 Benjamin Ward

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;

public class MinHeap<T> : IEnumerable<T> where T : class, IComparable<T>
{
    #region Fields

    private IComparer<T> Comparer;
    private List<T> Items = new List<T>();

    #endregion

    #region Properties

    public int Count
    {
        get { return Items.Count; }
    }

    #endregion

    #region Constructors

    public MinHeap()
        : this(Comparer<T>.Default)
    {
    }
    public MinHeap(IComparer<T> comp)
    {
        Comparer = comp;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    public void Clear()
    {
        Items.Clear();
    }

    /// <summary>
    /// Determines if provided item is already included.
    /// </summary>
    public bool Contains(T item)
    {
        return Items.Contains(item);
    }

    /// <summary>
    /// Removes item if provided item is already included.
    /// </summary>
    public void  Remove(T item)
    {
        Items.Remove(item);
    }

    /// <summary>
    /// Sets the capacity to the actual number of elements in the BinaryHeap,
    /// if that number is less than a threshold value.
    /// </summary>
    /// <remarks>
    /// The current threshold value is 90% (.NET 3.5), but might change in a future release.
    /// </remarks>
    public void TrimExcess()
    {
        Items.TrimExcess();
    }

    /// <summary>
    /// Inserts an item onto the heap.
    /// </summary>
    /// <param name="newItem">The item to be inserted.</param>
    public void Insert(T newItem)
    {
        int i = Count;
        Items.Add(newItem);
        while (i > 0 && Comparer.Compare(Items[(i - 1) / 2], newItem) > 0)
        {
            Items[i] = Items[(i - 1) / 2];
            i = (i - 1) / 2;
        }
        Items[i] = newItem;
    }

    /// <summary>
    /// Return the root item from the collection, without removing it.
    /// </summary>
    /// <returns>Returns the item at the root of the heap.</returns>
    public T Peek()
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("The heap is empty.");
        }
        return Items[0];
    }

    /// <summary>
    /// Removes and returns the root item from the collection.
    /// </summary>
    /// <returns>Returns the item at the root of the heap.</returns>
    public T PopRoot()
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("The heap is empty.");
        }
        // Get the first item
        T rslt = Items[0];
        // Get the last item and bubble it down.
        T tmp = Items[Items.Count - 1];
        Items.RemoveAt(Items.Count - 1);
        if (Items.Count > 0)
        {
            int i = 0;
            while (i < Items.Count / 2)
            {
                int j = (2 * i) + 1;
                if ((j < Items.Count - 1) && (Comparer.Compare(Items[j], Items[j + 1]) > 0))
                {
                    ++j;
                }
                if (Comparer.Compare(Items[j], tmp) >= 0)
                {
                    break;
                }
                Items[i] = Items[j];
                i = j;
            }
            Items[i] = tmp;
        }
        return rslt;
    }

    #endregion

    #region IEnumerable implementation

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        foreach (var i in Items)
        {
            yield return i;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    #endregion
}
