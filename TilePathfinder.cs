using System.Collections.Generic;

public interface TilePathfinder<T>
{
    /// <summary>
    /// This function should return a dictionary of all accessible neighbor nodes, with their associated distances.
    /// </summary>
    /// <returns></returns>
    Dictionary<T,int> GetNeighborsAstarDistance();

    /// <summary>
    /// This should return the A* heuristic for a tile. The heuristic is the estimated minimum distance from the tile to the destination. A* will provide the shortest path if the heuristic is minimal, but will provide a solution faster with a higher heuristic.
    /// </summary>
    /// <param name="dest"></param>
    /// <returns></returns>
    int GetAstarHeuristic(T dest);
}
