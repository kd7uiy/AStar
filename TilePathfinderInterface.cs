using System.Collections.Generic;

public interface TilePathfinderInterface<T>  {
    List<DataSet<T>> GetNeighborsAstarDistance(T dest, int distAlreadyTraveled);
}
