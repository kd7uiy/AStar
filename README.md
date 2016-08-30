This is a general implementation of the A* Algorithm. This can be used with any system, all that is required is that each tile implement the IAstarNode interface. 

The interface IAstarNode requires implementing four functions. They include:
* GetNeighborsAstarDistance- Returns a set of nodes, along with their associated distances, expressed as integers.
* GetAstarHeuristic- This is the heuristic distance between the tile and the destination.
* AStarIndex- The index of the current tile
* MaxAStarIndex- The maximum A* index. This is likely the number of nodes, if the previous function was done correctly.

To run, first implement the IAstarNode for each node. The neighbors can include 4, hex, 8, or any other arbitrary definition that you have for such. 

The AStarPathfinder is a singleton, to find a path use AStarPathfinder.Instance.FindPath(current,destination); The current and destination must be of the same type, which implements the IAstarNode interface, as specified above.
