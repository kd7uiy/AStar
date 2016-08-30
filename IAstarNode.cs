using System.Collections.Generic;

public interface IAstarNode<T>
{
    /// <summary>
    /// This function should return a dictionary of all accessible neighbor nodes, with their associated distances.
    /// </summary>
    /// <param name="tweak">An optional parameter to tweak the output</param>
    /// <returns></returns>
    Tuple<T,int>[] GetNeighborsAstarDistance(float tweak);

    /// <summary>
    /// This should return the A* heuristic for a tile. The heuristic is the estimated minimum distance from the tile to the destination. A* will provide the shortest path if the heuristic is minimal, but will provide a solution faster with a higher heuristic.
    /// </summary>
    /// <param name="dest">The destination</param>
    /// <param name="tweak">An optional parameter to tweak the output</param>
    /// <returns></returns>
    int GetAstarHeuristic(T dest, float tweak);

    /// <summary>
    /// The index of the tile
    /// </summary>
    int AStarIndex();

    /// <summary>
    /// The maxindex of the tile
    /// </summary>
    int MaxAStarIndex();

}
