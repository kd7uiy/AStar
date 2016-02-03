using System.Collections.Generic;

public class AStarPathfinder<T> where T : class, TilePathfinderInterface<T>
{

    private MinHeap<DataSet<T>> neighborHeap;
    private static AStarPathfinder<T> _instance;
    private List<T> visited;
    private List<T> tested;
    private Dictionary<T,DataSet<T>> fullDataTraveled;

    private IComparer<DataSet<T>> Comparer;

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

    public List<T> FindPath(T cur, T dest)
    {
        neighborHeap.Clear();
        visited.Clear();
        tested.Clear();
        fullDataTraveled.Clear();

        List<DataSet<T>> set;
        DataSet<T> curTest = new DataSet<T>(cur,null,0, 0);
        fullDataTraveled.Add(cur, curTest);

        while (curTest.current != dest)
        {
            visited.Add(curTest.current);
            set = curTest.current.GetNeighborsAstarDistance(dest,curTest.distTraveled);
            foreach (DataSet<T> ds in set)
            {
 //               if (!visited.Contains(ds.current) && !tested.Contains(ds.current))
 //               {
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
