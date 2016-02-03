This is a general implementation of the A* Algorithm. This can be used with any system, all that is required is that each tile implement the TilePathfinderInterface interface. This interface simply asks for a list of neighbors, along with their distance away. This distance function allows for asymetric distance from two points.

To run, first implement the TilePathfinderInterface in your tile setup. The neighbors can include 4, hex, 8, or any other arbitrary definition that you have for such. 

The AStarPathfinder is a singleton, to find a path use AStarPathfinder.Instance.FindPath(current,destination); The current and destination must both implement the TilePathfinderInterface, as specified above.