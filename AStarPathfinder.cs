using System.Collections.Generic;

public class AStarPathfinder<T> where T : class, TilePathfinderInterface<T>
{

    private MinHeap<DataSet<T>> neighborHeap;
    private static AStarPathfinder<T> _instance;
    private List<T> visited;
    private List<T> tested;
    private Dictionary<T,DataSet<T>> fullDataTraveled;

    private IComparer<DataSet<T>> Comparer;

    /// <summary>
    /// Get an instance of the pathfinder. The tile should be passed as a function
    /// </summary>
    public static AStarPathfinder<T> Instance
    {
        get{
            if (_instance== null)
            {
                _instance = new AStarPathfinder<T>();
            }
            return _instance;
        }
    }

    private AStarPathfinder()
    {
        neighborHeap = new MinHeap<DataSet<T>>();
        fullDataTraveled = new Dictionary<T, DataSet<T>>();
        visited = new List<T>();
        tested = new List<T>();
        Comparer = Comparer<DataSet<T>>.Default;
    }


    /// <summary>
    /// Find a path from the current tile to the destination. Assumes there is a path to the destination
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns></returns>
    public List<T> FindPath(T cur, T dest)
    {
        neighborHeap.Clear();
        visited.Clear();
        tested.Clear();
        fullDataTraveled.Clear();

        Dictionary<T,int> set;
        Dictionary<T, int> heuristics=new Dictionary<T, int>();
        DataSet<T> curTest = new DataSet<T>(cur,null,0, 0);
        fullDataTraveled.Add(cur, curTest);

        while (curTest.current != dest)
        {
            visited.Add(curTest.current);
            set = curTest.current.GetNeighborsAstarDistance();
            foreach (T neighbor in set.Keys)
            {
                int distanceTo = curTest.distTraveled + set[neighbor];
                int heuristic = 0;
                if (heuristics.ContainsKey(neighbor))
                {
                    heuristic = heuristics[neighbor];
                } else
                {
                    heuristic = neighbor.GetAstarHeuristic(dest);
                    heuristics.Add(neighbor, heuristic);
                }

                DataSet<T> ds = new DataSet<T>(neighbor, curTest.current, distanceTo, distanceTo+heuristic);
                tested.Add(ds.current);
                if (fullDataTraveled.ContainsKey(ds.current))
                {
                    //A quicker path was found to the tile
                    if ( Comparer.Compare(ds, fullDataTraveled[ds.current]) < 0) {
                        neighborHeap.Remove(fullDataTraveled[ds.current]);
                        fullDataTraveled[ds.current] = ds;
                        neighborHeap.Insert(ds);
                    }
                }
                else
                {
                    fullDataTraveled.Add(ds.current, ds);
                    neighborHeap.Insert(ds);
                }
 //               }
            }
            curTest = neighborHeap.PopRoot();

        }
        List<T> ret = new List<T>();
        while (curTest.prev!=null)
        {
            ret.Insert(0,curTest.current);
            curTest = fullDataTraveled[curTest.prev];
        }
        return ret;
    }
}
