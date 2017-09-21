using UnityEngine;
using System.Collections.Generic;

public class PathInfo<T> {
    public T[] path { get; protected set; }
    public int[] distance { get; protected set; }

    public PathInfo (List<T> path, int[] distance) {
        this.path=path.ToArray();
        this.distance=distance;
        }
}
